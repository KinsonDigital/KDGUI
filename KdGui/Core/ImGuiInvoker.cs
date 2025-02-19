﻿// <copyright file="ImGuiInvoker.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1515
namespace KdGui.Core;

using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

/// <inheritdoc/>
internal sealed class ImGuiInvoker : IImGuiInvoker
{
     /// <inheritdoc/>
    public ImGuiIOPtr GetIO() => ImGui.GetIO();

    /// <inheritdoc/>
    public bool Begin(string name, ImGuiWindowFlags flags) => ImGui.Begin(name, flags);

    /// <inheritdoc/>
    public void End() => ImGui.End();

    /// <inheritdoc/>
    public void Text(string fmt) => ImGui.Text(fmt);

    /// <inheritdoc/>
    // ReSharper disable once InconsistentNaming
    public bool ArrowButton(string str_id, ImGuiDir dir) => ImGui.ArrowButton(str_id, dir);

    /// <inheritdoc/>
    // ReSharper disable InconsistentNaming
    public bool SliderFloat(string label, ref float v, float v_min, float v_max) => ImGui.SliderFloat(label, ref v, v_min, v_max);
    // ReSharper restore InconsistentNaming

    /// <inheritdoc/>
    public void Button(string label) => ImGui.Button(label);

    /// <inheritdoc/>
    public bool Checkbox(string label, ref bool v) => ImGui.Checkbox(label, ref v);

    /// <inheritdoc/>
    // ReSharper disable InconsistentNaming
    public bool InvisibleButton(string str_id, Vector2 size) => ImGui.InvisibleButton(str_id, size);
    // ReSharper restore InconsistentNaming

    /// <inheritdoc/>
    // ReSharper disable once InconsistentNaming
    public bool BeginCombo(string label, string preview_value, ImGuiComboFlags flags) => ImGui.BeginCombo(label, preview_value, flags);

    /// <inheritdoc/>
    public void EndCombo() => ImGui.EndCombo();

    /// <inheritdoc/>
    public void PushButtonRepeat(bool repeat) => ImGui.PushButtonRepeat(repeat);

    /// <inheritdoc/>
    public void PopButtonRepeat() => ImGui.PopButtonRepeat();

    /// <inheritdoc/>
    public ImGuiStylePtr GetStyle() => ImGui.GetStyle();

    /// <inheritdoc/>
    public void SameLine() => ImGui.SameLine();

    /// <inheritdoc/>
    // ReSharper disable once InconsistentNaming
    public void SameLine(float offset_from_start_x, float spacing) => ImGui.SameLine(offset_from_start_x, spacing);

    /// <inheritdoc/>
    public Vector2 GetCursorPos() => ImGui.GetCursorPos();

    /// <inheritdoc/>
    // ReSharper disable once InconsistentNaming
    public void SetCursorPos(Vector2 local_pos) => ImGui.SetCursorPos(local_pos);

    /// <inheritdoc/>
    // ReSharper disable once InconsistentNaming
    public void PushID(string str_id) => ImGui.PushID(str_id);

    /// <inheritdoc/>
    public void PopID() => ImGui.PopID();

    /// <inheritdoc/>
    public float GetFrameHeightWithSpacing() => ImGui.GetFrameHeightWithSpacing();

    /// <inheritdoc/>
    public Vector4 GetStyleColorVec4(ImGuiCol idx)
    {
        Vector4 result;

        unsafe
        {
            var clrPtr = new IntPtr(ImGui.GetStyleColorVec4(idx));

            result = Marshal.PtrToStructure<Vector4>(clrPtr);
        }

        return result;
    }

    /// <inheritdoc/>
    public Color GetStyleColor(ImGuiCol idx)
    {
        var clr = GetStyleColorVec4(idx);

        return Color.FromArgb((byte)clr.X, (byte)clr.Y, (byte)clr.Z, (byte)clr.W);
    }

    /// <inheritdoc/>
    public void PushStyleColor(ImGuiCol idx, Vector4 col) => ImGui.PushStyleColor(idx, col);

    /// <inheritdoc/>
    public void PushStyleColor(ImGuiCol idx, uint col) => ImGui.PushStyleColor(idx, col);

    /// <inheritdoc/>
    public void PushStyleColor(ImGuiCol idx, Color clr) => PushStyleColor(idx, ImGui.ColorConvertFloat4ToU32(clr.ToImGuiColor()));

    /// <inheritdoc/>
    public void PopStyleColor(int count) => ImGui.PopStyleColor(count);

    /// <inheritdoc/>
    public void PopStyleColor() => ImGui.PopStyleColor();

    /// <inheritdoc/>
    public void PushStyleVar(ImGuiStyleVar idx, float val) => ImGui.PushStyleVar(idx, val);

    /// <inheritdoc/>
    public void PopStyleVar(int count) => ImGui.PopStyleVar(count);

    /// <inheritdoc/>
    public Vector2 CalcTextSize(string text) => string.IsNullOrEmpty(text) ? Vector2.Zero : ImGui.CalcTextSize(text);

    /// <inheritdoc/>
    public bool IsItemHovered() => ImGui.IsItemHovered();

    /// <inheritdoc/>
    public bool IsMouseDown(ImGuiMouseButton button) => ImGui.IsMouseDown(button);

    /// <inheritdoc/>
    public bool IsMouseReleased(ImGuiMouseButton button) => ImGui.IsMouseReleased(button);

    /// <inheritdoc/>
    public bool Selectable(string label, bool selected) => ImGui.Selectable(label, selected);

    /// <inheritdoc/>
    public void SetItemDefaultFocus() => ImGui.SetItemDefaultFocus();

    /// <inheritdoc/>
    // ReSharper disable once InconsistentNaming
    public void SetNextItemWidth(float item_width) => ImGui.SetNextItemWidth(item_width);

    /// <inheritdoc/>
    public void PushItemWidth(float width) => ImGui.PushItemWidth(width);

    /// <inheritdoc/>
    public void PopItemWidth() => ImGui.PopItemWidth();

    /// <inheritdoc/>
    public void SetNextWindowSize(Vector2 size) => ImGui.SetNextWindowSize(size);

    /// <inheritdoc/>
    public void SetNextWindowPos(Vector2 pos) => ImGui.SetNextWindowPos(pos);

    /// <inheritdoc/>
    public void SetWindowPos(Vector2 pos) => ImGui.SetWindowPos(pos);

    /// <inheritdoc/>
    public void SetWindowSize(Vector2 size) => ImGui.SetWindowSize(size);

    /// <inheritdoc/>
    public Vector2 GetWindowPos() => ImGui.GetWindowPos();

    /// <inheritdoc/>
    public Vector2 GetWindowSize() => ImGui.GetWindowSize();

    /// <inheritdoc/>
    public bool IsWindowFocused() => ImGui.IsWindowFocused();

    /// <inheritdoc/>
    public bool IsMouseDragging(ImGuiMouseButton button) => ImGui.IsMouseDragging(button);

    /// <inheritdoc/>
    public void Dummy(Vector2 size) => ImGui.Dummy(size);
}
