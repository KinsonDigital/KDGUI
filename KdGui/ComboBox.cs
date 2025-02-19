// <copyright file="ComboBox.cs" company="KinsonDigital">
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

/// <inheritdoc cref="IComboBox"/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class ComboBox : Control, IComboBox
{
    private readonly string textId = Guid.NewGuid().ToString();
    private readonly string ctrlId = $"##combo-{Guid.NewGuid()}";
    private readonly Color disabledGray = Color.FromArgb(
        255,
        Color.DimGray.R - 40,
        Color.DimGray.G - 40,
        Color.DimGray.B - 40);
    private int selectedItemIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBox"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public ComboBox(IImGuiInvoker imGuiInvoker, IPushReactable renderReactable)
        : base(imGuiInvoker, renderReactable)
    {
    }

    /// <inheritdoc/>
    public event EventHandler<int>? SelectedItemIndexChanged;

    /// <inheritdoc/>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the value is out of range of the list of items.
    /// </exception>
    public int SelectedItemIndex
    {
        get => this.selectedItemIndex;
        set
        {
            if (value < 0 || value > Items.Count)
            {
                const string exMsg = "The selected item index must be within the bounds of the items collection.";
                throw new ArgumentOutOfRangeException(nameof(SelectedItemIndex), exMsg);
            }

            this.selectedItemIndex = value;
        }
    }

    /// <inheritdoc/>
    public string Label { get; set; } = "ComboBox:";

    /// <inheritdoc cref="IComboBox"/>
    public override int Width { get; set; } = 200;

    /// <summary>
    /// Gets or sets the list of items to display in the combo box.
    /// </summary>
    public List<string> Items { get; set; } = [];

    /// <inheritdoc/>
    [SuppressMessage("csharpsquid", "S3776", Justification = "Do not care about cognitive complexity.")]
    protected override void Render()
    {
        if (!Visible)
        {
            return;
        }

        var indexOutOfRange = this.selectedItemIndex < 0 || this.selectedItemIndex >= Items.Count;
        var previewValue = indexOutOfRange ? string.Empty : Items[this.selectedItemIndex];

        var currentPos = ImGuiInvoker.GetCursorPos();

        // Bump the label down to be vertically centered with the combo box
        ImGuiInvoker.SetCursorPos(currentPos with { Y = currentPos.Y + 5 });

        var textColor = Enabled ? Color.White : Color.DimGray;

        ImGuiInvoker.PushID(this.textId);
        ImGuiInvoker.PushStyleColor(ImGuiCol.Text, textColor);
        ImGuiInvoker.Text(Label);
        ImGuiInvoker.PopStyleColor(1);
        ImGuiInvoker.PopID();

        var spacing = ImGuiInvoker.GetStyle().ItemSpacing.X;
        ImGuiInvoker.SameLine(0, spacing);

        currentPos = ImGuiInvoker.GetCursorPos();

        ImGuiInvoker.SetNextItemWidth(Width);

        // Bump the combo box up to be vertically centered with the label
        ImGuiInvoker.SetCursorPos(currentPos with { Y = currentPos.Y - 3 });

        ImGuiInvoker.PushID(this.ctrlId);
        if (!Enabled)
        {
            ImGuiInvoker.PushStyleColor(ImGuiCol.FrameBg, this.disabledGray);
            ImGuiInvoker.PushStyleColor(ImGuiCol.FrameBgHovered, this.disabledGray);
            ImGuiInvoker.PushStyleColor(ImGuiCol.FrameBgActive, this.disabledGray);
            ImGuiInvoker.PushStyleColor(ImGuiCol.Button, this.disabledGray);
            ImGuiInvoker.PushStyleColor(ImGuiCol.ButtonHovered, this.disabledGray);
        }

        if (ImGuiInvoker.BeginCombo(this.ctrlId, previewValue, Enabled ? ImGuiComboFlags.None : ImGuiComboFlags.NoArrowButton))
        {
            if (Enabled)
            {
                Height = (int)GetComboHeight();

                for (var n = 0; n < Items.Count; n++)
                {
                    var isSelected = this.selectedItemIndex == n;

                    if (ImGuiInvoker.Selectable(Items[n], isSelected))
                    {
                        this.selectedItemIndex = n;
                        this.SelectedItemIndexChanged?.Invoke(this, this.selectedItemIndex);
                    }

                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                    if (isSelected)
                    {
                        ImGuiInvoker.SetItemDefaultFocus();
                    }
                }
            }

            ImGuiInvoker.EndCombo();
        }

        ImGuiInvoker.PopStyleColor(Enabled ? 1 : 6);

        ImGuiInvoker.PopID();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        this.SelectedItemIndexChanged = null;
        base.Dispose(disposing);
    }

    /// <summary>
    /// Gets the height of the combo box.
    /// </summary>
    /// <returns>The height.</returns>
    private float GetComboHeight()
    {
        ImGuiStylePtr style = ImGuiInvoker.GetStyle();
        Vector2 textSize = ImGuiInvoker.CalcTextSize(Label);
        return textSize.Y + (style.FramePadding.Y * 2);
    }
}
