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
using Carbonate.NonDirectional;
using ImGuiNET;

/// <inheritdoc/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class ControlGroup : IControlGroup
{
    private const int PreRenderCount = 5;
    private const float CollapseButtonWidth = 28f;
    private readonly List<IControl> controls = [];
    private readonly IImGuiInvoker imGuiInvoker;
    private readonly IPushReactable renderReactable;
    private readonly string pushId;
    private bool shouldSetPos = true;
    private bool shouldSetSize = true;
    private Point position;
    private Size size = new (32, 32);
    private Size prevSize;
    private bool isDisposed;
    private bool isBeingDragged;
    private int invokeCount;
    private bool isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlGroup"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public ControlGroup(IImGuiInvoker imGuiInvoker, IPushReactable renderReactable)
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
    public bool TitleBarVisible { get; set; } = true;

    /// <inheritdoc/>
    public bool AutoSizeToFitContent { get; set; }

    /// <inheritdoc/>
    public bool NoResize { get; set; }

    /// <inheritdoc/>
    public bool Visible { get; set; } = true;

    /// <inheritdoc/>
    public void Add(IControl control)
    {
        control.WindowOwnerId = Id;
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

        if (Visible)
        {
            this.renderReactable.Push(Id);
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

        if (!this.isInitialized && this.invokeCount >= PreRenderCount)
        {
            this.Initialized?.Invoke(this, EventArgs.Empty);
            this.isInitialized = true;
        }

        this.invokeCount += 1;
        this.autoFitSize = this.imGuiInvoker.GetWindowSize().ToSize();

        this.imGuiInvoker.End();

        // NOTE: Then a KdGui control is rendered for the very first time, it is rendered 5 times.
        // This is to ensure that certain ImGui functions and state is established.  Things such as
        // size calculations and more are only done on the first render, so these numbers are not
        // available or accurate until ta few renders are complete.
        if (this.invokeCount < PreRenderCount)
        {
            Render();
        }

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
