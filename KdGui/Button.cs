// <copyright file="Button.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Carbonate.NonDirectional;
using ImGuiNET;

/// <inheritdoc cref="IButton"/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class Button : Control, IButton
{
    private readonly string id = Guid.NewGuid().ToString();
    private readonly Color hoverClr = Color.FromArgb(255, 66, 150, 250);
    private readonly Color activeClr = Color.FromArgb(255, 15, 135, 250);
    private readonly Color disabledGray = Color.FromArgb(
        255,
        Color.DimGray.R - 40,
        Color.DimGray.G - 40,
        Color.DimGray.B - 40);
    private bool isMousePressedInvoked;

    /// <summary>
    /// Initializes a new instance of the <see cref="Button"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public Button(IImGuiInvoker imGuiInvoker, IPushReactable renderReactable)
        : base(imGuiInvoker, renderReactable)
    {
        Text = "Button";
        Name = "Button";
    }

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? Click;

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? MousePressed;

    /// <inheritdoc/>
    public event EventHandler<EventArgs>? MouseReleased;

    /// <inheritdoc/>
    public string Text { get; set; }

    /// <inheritdoc/>
    public Color BackgroundColor { get; set; } = Color.FromArgb(255, 35, 70, 110);

    /// <inheritdoc cref="Control"/>
    protected override void Render()
    {
        if (!Visible)
        {
            return;
        }

        ImGuiInvoker.PushID(this.id);

        ImGuiInvoker.PushStyleColor(ImGuiCol.Text, Enabled ? Color.White : Color.DimGray);
        ImGuiInvoker.PushStyleColor(ImGuiCol.Button, Enabled ? BackgroundColor : this.disabledGray);
        ImGuiInvoker.PushStyleColor(ImGuiCol.ButtonActive, Enabled ? this.activeClr : this.disabledGray);
        ImGuiInvoker.PushStyleColor(ImGuiCol.ButtonHovered, Enabled ? this.hoverClr : this.disabledGray);

        ImGuiInvoker.Button(Text);

        Height = (int)ImGuiInvoker.GetFrameHeightWithSpacing();
        Width = (int)GetWidth(Text);

        ImGuiInvoker.PopStyleColor(4);

        ImGuiInvoker.PopID();

        ProcessMouseEvents();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            this.Click = null;
            this.MousePressed = null;
            this.MouseReleased = null;
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Process the mouse related events.
    /// </summary>
    private void ProcessMouseEvents()
    {
        if (!ImGuiInvoker.IsItemHovered())
        {
            return;
        }

        if (ImGuiInvoker.IsMouseDown(ImGuiMouseButton.Left) && !this.isMousePressedInvoked)
        {
            this.MousePressed?.Invoke(this, EventArgs.Empty);
            this.isMousePressedInvoked = true;
        }

        if (!Enabled || !ImGuiInvoker.IsMouseReleased(ImGuiMouseButton.Left))
        {
            return;
        }

        this.isMousePressedInvoked = false;

        this.MouseReleased?.Invoke(this, EventArgs.Empty);
        this.Click?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets the width of the button.
    /// </summary>
    /// <param name="text">The text of the button.</param>
    /// <returns>The width.</returns>
    private float GetWidth(string text)
    {
        var style = ImGuiInvoker.GetStyle();
        var textSize = ImGuiInvoker.CalcTextSize(text);
        var buttonWidth = textSize.X + (style.FramePadding.X * 2);

        return buttonWidth;
    }
}
