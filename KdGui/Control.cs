// <copyright file="Control.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Core;
using System;
using System.Drawing;
using Carbonate;
using Carbonate.NonDirectional;

/// <inheritdoc/>
internal abstract class Control : IControl
{
    private readonly IPushReactable renderReactable;
    private IDisposable? unsubscriber;
    private Guid windowOwnerId;

    /// <summary>
    /// Initializes a new instance of the <see cref="Control"/> class.
    /// </summary>
    /// <param name="imGuiInvoker">Invokes ImGui functions.</param>
    /// <param name="renderReactable">Manages render notifications.</param>
    protected Control(IImGuiInvoker imGuiInvoker, IPushReactable renderReactable)
    {
        ArgumentNullException.ThrowIfNull(imGuiInvoker);
        ArgumentNullException.ThrowIfNull(renderReactable);

        ImGuiInvoker = imGuiInvoker;
        this.renderReactable = renderReactable;
    }

    /// <inheritdoc/>
    public string Name { get; set; } = "Control";

    /// <inheritdoc/>
    public Guid WindowOwnerId
    {
        get => this.windowOwnerId;
        set
        {
            if (this.windowOwnerId == value)
            {
                return;
            }

            // If the subscription is no already set, then we need to unsubscribe
            // the already subscribed id before resubscribing
            if (this.windowOwnerId != Guid.Empty)
            {
                this.unsubscriber?.Dispose();
            }

            this.windowOwnerId = value;

            this.unsubscriber = this.renderReactable.CreateNonReceiveOrRespond(
                this.windowOwnerId,
                Render,
                () => this.unsubscriber?.Dispose(),
                callerMemberName: $"{Name}|{this.windowOwnerId}");
        }
    }

    /// <inheritdoc/>
    public Point Position { get; set; }

    /// <inheritdoc/>
    public virtual int Width { get; set; }

    /// <inheritdoc/>
    public virtual int Height { get; protected set; }

    /// <inheritdoc/>
    public bool Enabled { get; set; } = true;

    /// <inheritdoc/>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether if the control has been disposed of.
    /// </summary>
    protected bool IsDisposed { get; private set; }

    /// <summary>
    /// Gets the ImGui invoker.
    /// </summary>
    protected IImGuiInvoker ImGuiInvoker { get; private set; }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Renders the control.
    /// </summary>
    protected abstract void Render();

    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// <param name="disposing">True to dispose of managed resources.</param>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;
    }
}
