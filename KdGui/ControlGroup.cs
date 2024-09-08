// <copyright file="ControlGroup.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using Carbonate.OneWay;
using ImGuiNET;

/// <inheritdoc/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class ControlGroup : IControlGroup
{
    private const float CollapseButtonWidth = 28f;
    private readonly List<IControl> controls = [];
    private readonly IImGuiInvoker imGuiInvoker;
    private readonly IPushReactable<GridData> renderReactable;
    private readonly string pushId;
    private Point position;
    private Size size = new (32, 32);
    private Size prevSize;
    private bool isDisposed;
    private bool isBeingDragged;
    private bool isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlGroup"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public ControlGroup(IImGuiInvoker imGuiInvoker, IPushReactable<GridData> renderReactable)
    {
        ArgumentNullException.ThrowIfNull(imGuiInvoker);
        ArgumentNullException.ThrowIfNull(renderReactable);

        this.imGuiInvoker = imGuiInvoker;
        Id = Guid.NewGuid();
        this.pushId = Id.ToString();
        this.renderReactable = renderReactable;
    }

    /// <inheritdoc/>
    public event EventHandler? Initialized;

    /// <inheritdoc/>
    public event EventHandler<Size>? SizeChanged;

    /// <inheritdoc/>
    public string Title { get; set; } = "ControlGroup";

    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    public Point Position
    {
        get => this.position;
        set => this.position = value;
    }

    /// <inheritdoc/>
    public int Width
    {
        get => this.size.Width;
        set => this.size = this.size with { Width = value };
    }

    /// <inheritdoc/>
    public int Height
    {
        get => this.size.Height;
        set => this.size = this.size with { Height = value };
    }

    /// <inheritdoc/>
    public int HalfWidth => this.size.Width / 2;

    /// <inheritdoc/>
    public int HalfHeight => this.size.Height / 2;

    /// <inheritdoc/>
    public int Left => this.position.X;

    /// <inheritdoc/>
    public int Top => this.position.Y;

    /// <inheritdoc/>
    public int Right => this.position.X + this.size.Width;

    /// <inheritdoc/>
    public int Bottom => this.position.Y + this.size.Height;

    /// <inheritdoc/>
    public bool GridVisible { get; set; }

    /// <inheritdoc/>
    public bool TitleBarVisible { get; set; } = true;

    /// <inheritdoc/>
    public bool AutoSizeToFitContent { get; set; }

    /// <inheritdoc/>
    public bool NoResize { get; set; }

    /// <inheritdoc/>
    public int TotalRows { get; set; } = 1;

    /// <inheritdoc/>
    public int TotalColumns { get; set; } = 1;

    /// <inheritdoc/>
    public bool Visible { get; set; } = true;

    /// <inheritdoc/>
    public void Add(IControl control)
    {
        control.WindowOwnerId = Id;
        this.controls.Add(control);
    }

    /// <inheritdoc/>
    public void Add(IControl control, int row, int column)
    {
        control.WindowOwnerId = Id;
        control.Row = row;
        control.Column = column;
        this.controls.Add(control);
    }

    /// <inheritdoc/>
    public T? GetControl<T>(string name)
        where T : IControl
    {
        foreach (var ctrl in this.controls)
        {
            if (ctrl.Name == name)
            {
                return (T)ctrl;
            }
        }

        return default;
    }

    /// <inheritdoc/>
    public void Render()
    {
        var flags = GetWindowFlags();

        this.imGuiInvoker.PushID(this.pushId);

        PushWindowStyles();

        if (!AutoSizeToFitContent)
        {
            this.imGuiInvoker.SetNextWindowSize(AutoSizeToFitContent ? Vector2.Zero : this.size.ToVector2());
        }

        if (!this.isBeingDragged)
        {
            this.imGuiInvoker.SetNextWindowPos(this.position.ToVector2());
        }

        this.imGuiInvoker.Begin(Title, flags);

        this.size = this.imGuiInvoker.GetWindowSize().ToSize();

        if (AutoSizeToFitContent && TitleBarVisible)
        {
            var titleWidth = this.imGuiInvoker.CalcTextSize(Title).X + CollapseButtonWidth;

            this.imGuiInvoker.InvisibleButton($"##title_width {Title}", new Vector2(titleWidth, 0));
        }

        PopWindowStyles();

        this.imGuiInvoker.PopID();

        if (Visible && this.controls.Count > 0)
        {
            if (this.imGuiInvoker.BeginTable(Id.ToString(), TotalColumns))
            {
                for (var row = 0; row < TotalRows; row++)
                {
                    this.imGuiInvoker.TableNextRow(ImGuiTableRowFlags.None, 25);
                    for (var col = 0; col < TotalColumns; col++)
                    {
                        this.imGuiInvoker.TableSetColumnIndex(col);

                        if (GridVisible)
                        {
                            var isEvenCell = (row + col) % 2 == 0;
                            var bgColor = isEvenCell ? new Vector4(0.5f, 0.5f, 0.5f, 1) : new Vector4(0.7f, 0.7f, 0.7f, 1);
                            this.imGuiInvoker.TableSetBgColor(ImGuiTableBgTarget.CellBg, bgColor);
                        }

                        this.renderReactable.Push(Id, new GridData { Row = row, Column = col });
                    }
                }

                this.imGuiInvoker.EndTable();
            }
        }

        if (this.size != this.prevSize)
        {
            this.SizeChanged?.Invoke(this, this.size);
        }

        // Check if the window is being dragged
        this.isBeingDragged = this.imGuiInvoker.IsWindowFocused() && this.imGuiInvoker.IsMouseDragging(ImGuiMouseButton.Left);

        // Update the position if the window is being dragged
        if (this.isBeingDragged)
        {
            this.position = this.imGuiInvoker.GetWindowPos().ToPoint();
        }

        if (!this.isInitialized)
        {
            this.Initialized?.Invoke(this, EventArgs.Empty);
            this.isInitialized = true;
        }

        this.imGuiInvoker.End();

        this.prevSize = this.size;
    }

    /// <inheritdoc/>
    public void Dispose() => Dispose(true);

    /// <summary>
    /// Gets the window flags.
    /// </summary>
    /// <returns>The window flags.</returns>
    private ImGuiWindowFlags GetWindowFlags()
    {
        var flags = ImGuiWindowFlags.None;

        flags = TitleBarVisible ? flags : flags | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove;
        flags = AutoSizeToFitContent ? flags | ImGuiWindowFlags.AlwaysAutoResize : flags;
        flags = NoResize ? flags | ImGuiWindowFlags.NoResize : flags;
        flags = Visible ? flags : flags | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground;

        return flags;
    }

    /// <summary>
    /// Pushes the styles for the window.
    /// </summary>
    private void PushWindowStyles()
    {
        var styleAlpha = Visible ? 1f : 0f;
        this.imGuiInvoker.PushStyleVar(ImGuiStyleVar.Alpha, styleAlpha);
        this.imGuiInvoker.PushStyleColor(ImGuiCol.Text, Color.White);

        if (!NoResize)
        {
            return;
        }

        this.imGuiInvoker.PushStyleColor(ImGuiCol.ResizeGrip, Color.Transparent);
        this.imGuiInvoker.PushStyleColor(ImGuiCol.ResizeGripHovered, Color.Transparent);
        this.imGuiInvoker.PushStyleColor(ImGuiCol.ResizeGripActive, Color.Transparent);
    }

    /// <summary>
    /// Pops the current window styles.
    /// </summary>
    private void PopWindowStyles()
    {
        this.imGuiInvoker.PopStyleColor(NoResize ? 4 : 1);
        this.imGuiInvoker.PopStyleVar(1);
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    /// <param name="disposing">True to dispose of managed resources.</param>
    private void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.Initialized = null;

            foreach (var ctrl in this.controls)
            {
                ctrl.Dispose();
            }

            this.controls.Clear();
        }

        this.isDisposed = true;
    }
}
