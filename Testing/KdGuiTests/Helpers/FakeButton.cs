// <copyright file="FakeButton.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTests.Helpers;

using System.Drawing;
using KdGui;

/// <summary>
/// Used for the purpose of testing.
/// </summary>
public class FakeButton : IControl
{
    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Guid WindowOwnerId { get; set; }

    /// <inheritdoc/>
    public Point Position { get; set; }

    /// <inheritdoc/>
    public int Width { get; } = 0;

    /// <inheritdoc/>
    public int Height { get; } = 0;

    /// <inheritdoc/>
    public bool Enabled { get; set; }

    /// <inheritdoc/>
    public bool Visible { get; set; }

    /// <inheritdoc/>
    public int Row { get; set; }

    /// <inheritdoc/>
    public int Column { get; set; }

    /// <inheritdoc/>
    public void Dispose() => throw new NotImplementedException();
}
