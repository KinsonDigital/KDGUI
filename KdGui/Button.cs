// <copyright file="Button.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Carbonate.OneWay;
using ImGuiNET;

/// <inheritdoc cref="IButton"/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class Button : Control, IButton
{
    private readonly string id = Guid.NewGuid().ToString();
    private bool isMousePressedInvoked;

    /// <summary>
    /// Initializes a new instance of the <see cref="Button"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public Button(IImGuiInvoker imGuiInvoker, IPushReactable<GridData> renderReactable)
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

    /// <inheritdoc cref="Control"/>
    protected override void Render()
    {
        if (!Visible)
        {
            return;
        }

        ImGuiInvoker.PushID(this.id);

        ImGuiInvoker.PushStyleColor(ImGuiCol.Text, Color.White);
        ImGuiInvoker.Button(Text);

        Height = (int)ImGuiInvoker.GetFrameHeightWithSpacing();
        Width = (int)GetWidth(Text);

        ImGuiInvoker.PopStyleColor(2);

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

        if (!ImGuiInvoker.IsMouseReleased(ImGuiMouseButton.Left))
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
