// <copyright file="CheckBox.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Carbonate.OneWay;
using ImGuiNET;

/// <inheritdoc cref="ICheckBox"/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class CheckBox : Control, ICheckBox
{
    private readonly string id = Guid.NewGuid().ToString();
    private bool isChecked;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBox"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public CheckBox(IImGuiInvoker imGuiInvoker, IPushReactable<GridData> renderReactable)
        : base(imGuiInvoker, renderReactable)
    {
    }

    /// <inheritdoc/>
    public event EventHandler<bool>? CheckedChanged;

    /// <inheritdoc/>
    public string LabelWhenChecked { get; set; } = "Checked";

    /// <inheritdoc/>
    public string LabelWhenUnchecked { get; set; } = "Unchecked";

    /// <inheritdoc/>
    public bool IsChecked
    {
        get => this.isChecked;
        set => this.isChecked = value;
    }

    /// <inheritdoc/>
    protected override void Render()
    {
        if (!Visible)
        {
            return;
        }

        ImGuiInvoker.PushID(this.id);
        ImGuiInvoker.PushStyleColor(ImGuiCol.Text, Color.White);

        if (ImGuiInvoker.Checkbox(this.isChecked ? LabelWhenChecked : LabelWhenUnchecked, ref this.isChecked))
        {
            this.CheckedChanged?.Invoke(this, this.isChecked);
        }

        ImGuiInvoker.PopStyleColor(1);
        ImGuiInvoker.PopID();

        Width = (int)GetWidth(this.isChecked ? LabelWhenChecked : LabelWhenUnchecked);
        Height = (int)ImGuiInvoker.GetFrameHeightWithSpacing();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        this.CheckedChanged = null;
        base.Dispose(disposing);
    }

    /// <summary>
    /// Gets the width of the control.
    /// </summary>
    /// <param name="text">The text of the control.</param>
    /// <returns>The width.</returns>
    private float GetWidth(string text)
    {
        var style = ImGuiInvoker.GetStyle();
        var textSize = ImGuiInvoker.CalcTextSize(text);
        var buttonWidth = textSize.X + (style.FramePadding.X * 2);

        return buttonWidth;
    }
}
