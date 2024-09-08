// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui.Core;

using System.Drawing;
using System.Numerics;

/// <summary>
/// Provides extension methods for various types.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Maps the given <paramref name="value"/> from one range to another.
    /// </summary>
    /// <param name="value">The value to map.</param>
    /// <param name="fromStart">The starting range value of where the result will map from.</param>
    /// <param name="fromStop">The from ending range value.</param>
    /// <param name="toStart">The starting range value of where the result will map to.</param>
    /// <param name="toStop">The to ending range value.</param>
    /// <returns>A value that has been mapped to a range between <paramref name="toStart"/> and <paramref name="toStop"/>.</returns>
    public static float MapValue(this byte value, float fromStart, float fromStop, float toStart, float toStop)
        => toStart + ((toStop - toStart) * ((value - fromStart) / (fromStop - fromStart)));

    /// <summary>
    /// Converts a <see cref="Vector2"/> to a <see cref="Size"/>.
    /// </summary>
    /// <param name="value">The vector to convert.</param>
    /// <returns>The converted result.</returns>
    public static Size ToSize(this Vector2 value) => new ((int)value.X, (int)value.Y);

    /// <summary>
    /// Converts a <see cref="Point"/> to a <see cref="Vector2"/>.
    /// </summary>
    /// <param name="value">The point to convert.</param>
    /// <returns>The converted result.</returns>
    public static Vector2 ToVector2(this Point value) => new (value.X, value.Y);

    /// <summary>
    /// Converts a <see cref="Size"/> to a <see cref="Vector2"/>.
    /// </summary>
    /// <param name="value">The sie to convert.</param>
    /// <returns>The converted result.</returns>
    public static Vector2 ToVector2(this Size value) => new (value.Width, value.Height);

    /// <summary>
    /// Converts a <see cref="Vector2"/> to a <see cref="Point"/>.
    /// </summary>
    /// <param name="value">The vector to convert.</param>
    /// <returns>The converted result.</returns>
    public static Point ToPoint(this Vector2 value) => new ((int)value.X, (int)value.Y);

    public static Vector4 ToVector4(this Color value) => new (value.R, value.G, value.B, value.A);
}
