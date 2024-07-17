// <copyright file="ButtonTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTests;

using Carbonate.NonDirectional;
using FluentAssertions;
using KdGui;
using KdGui.Core;
using NSubstitute;

/// <summary>
/// Tests the <see cref="Button"/>.
/// </summary>
public class ButtonTests
{
    private readonly IImGuiInvoker mockImGuiInvoker;
    private readonly IPushReactable mockRenderReactable;

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonTests"/> class.
    /// </summary>
    public ButtonTests()
    {
        this.mockImGuiInvoker = Substitute.For<IImGuiInvoker>();
        this.mockRenderReactable = Substitute.For<IPushReactable>();
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullImGuiInvokerParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Button(null, this.mockRenderReactable);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'imGuiInvoker')");
    }

    [Fact]
    public void Ctor_WithNullRenderReactableParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Button(this.mockImGuiInvoker, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'renderReactable')");
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsDefaultPropertyValues()
    {
        // Arrange & Act
        var sut = CreateSystemUnderTest();

        // Assert
        sut.Text.Should().Be("Button");
        sut.Name.Should().Be("Button");
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="Button"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private Button CreateSystemUnderTest()
        => new (this.mockImGuiInvoker, this.mockRenderReactable);
}
