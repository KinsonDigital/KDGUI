// <copyright file="NextPrevious.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Core;
using System;
using System.Diagnostics.CodeAnalysis;
using Carbonate.OneWay;
using ImGuiNET;

/// <inheritdoc cref="INextPrevious"/>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via IoC container.")]
internal sealed class NextPrevious : Control, INextPrevious
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NextPrevious"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    public NextPrevious(IImGuiInvoker imGuiInvoker, IPushReactable<GridData> renderReactable)
        : base(imGuiInvoker, renderReactable)
    {
    }

    /// <inheritdoc/>
    public event EventHandler? Next;

    /// <inheritdoc/>
    public event EventHandler? Previous;

    /// <inheritdoc/>
    protected override void Render()
    {
        if (!Visible)
        {
            return;
        }

        if (ImGuiInvoker.ArrowButton("##left", ImGuiDir.Left))
        {
            this.Previous?.Invoke(this, EventArgs.Empty);
        }

        var spacing = ImGuiInvoker.GetStyle().ItemInnerSpacing.X;
        ImGuiInvoker.SameLine(0.0f, spacing);

        if (ImGuiInvoker.ArrowButton("##right", ImGuiDir.Right))
        {
            this.Next?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        this.Next = null;
        this.Previous = null;
        base.Dispose(disposing);
    }
}
