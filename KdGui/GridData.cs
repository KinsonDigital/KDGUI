// <copyright file="GridData.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

/// <summary>
/// Presents the row and column location in a grid.
/// </summary>
internal readonly record struct GridData
{
    /// <summary>
    /// Gets the grid row.
    /// </summary>
    public int Row { get; init; }

    /// <summary>
    /// Gets the grid column.
    /// </summary>
    public int Column { get; init; }
}
