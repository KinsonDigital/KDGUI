// <copyright file="TestHelpers.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTests.Helpers;

using System.Reflection;
using FluentAssertions;

/// <summary>
/// Provides testing helpers.
/// </summary>
public static class TestHelpers
{
    /// <summary>
    /// Sets a field with a name that matches the given <paramref name="fieldName"/> to the value of null.
    /// </summary>
    /// <param name="fieldContainer">The object that contains the field.</param>
    /// <param name="fieldName">The name of the field.</param>
    /// <param name="value">The value to set the field to.</param>
    /// <typeparam name="T">The type of parameter.</typeparam>
    public static void SetFieldValue<T>(this object fieldContainer, string fieldName, T value)
    {
        fieldContainer.Should().NotBeNull("setting the field value of a null object is not possible.");
        fieldName.Should().NotBeNullOrEmpty("setting an field value requires a non-empty or null field name.");

        var fields = fieldContainer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        fields.Should().HaveCountGreaterThan(0, $"no fields exist in the object.");

        var foundField = Array.Find(fields, f => f.Name == fieldName);

        foundField.Should().NotBeNull($"a field with the name '{fieldName}' does not exist in the object.");
        foundField.FieldType.Should().Be(typeof(T), "the generic type should match the actual field type.");

        foundField.SetValue(fieldContainer, value);
    }

    /// <summary>
    /// Sets a field of type <see cref="List{T}"/> with a name that matches the given <paramref name="eventName"/> to the value of null.
    /// </summary>
    /// <param name="fieldContainer">The object that contains the field.</param>
    /// <param name="eventName">The name of the field.</param>
    /// <typeparam name="TElements">The type of the field array's elements.</typeparam>
    /// <returns>The <see cref="List{T}"/> of items.</returns>
    public static EventHandler<EventArgs>? GetEvent<TElements>(this object fieldContainer, string eventName)
    {
        fieldContainer.Should().NotBeNull("setting the enum field value of a null object is not possible.");
        eventName.Should().NotBeNullOrEmpty("setting an enum field value requires a non-empty or null field name.");

        var targetType = fieldContainer.GetType();
        var eventInfo = targetType.GetEvent(eventName);
        eventInfo.Should().NotBeNull($"a field with the name '{eventName}' does not exist in the object.");

        var eventFieldInfo = targetType.GetField(eventInfo.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        eventFieldInfo.Should().NotBeNull("event backing field must exist for test");

        var eventField = eventFieldInfo.GetValue(fieldContainer) as EventHandler<EventArgs>;

        return eventField;
    }

    /// <summary>
    /// Sets a field of type <see cref="List{T}"/> with a name that matches the given <paramref name="fieldName"/> to the value of null.
    /// </summary>
    /// <param name="fieldContainer">The object that contains the field.</param>
    /// <param name="fieldName">The name of the field.</param>
    /// <typeparam name="TElements">The type of the field array's elements.</typeparam>
    /// <returns>The <see cref="List{T}"/> of items.</returns>
    public static TElements GetListField<TElements>(this object fieldContainer, string fieldName)
        where TElements : class
    {
        fieldContainer.Should().NotBeNull("setting the enum field value of a null object is not possible.");
        fieldName.Should().NotBeNullOrEmpty("setting an enum field value requires a non-empty or null field name.");

        var allFields = fieldContainer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        allFields.Should().HaveCountGreaterThan(0, $"no enum fields exist in the object.");

        var foundField = Array.Find(allFields, f => f.Name == fieldName);

        foundField.Should().NotBeNull($"a field with the name '{fieldName}' does not exist in the object.");

        var isCorrectType = foundField.FieldType.IsGenericType &&
                            foundField.FieldType.GetGenericTypeDefinition() == typeof(List<>);

        isCorrectType.Should().BeTrue("the field must be of type 'List<T>'.");

        return (TElements)foundField.GetValue(fieldContainer) !;
    }
}
