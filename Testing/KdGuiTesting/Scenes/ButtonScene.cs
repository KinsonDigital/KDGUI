// <copyright file="ButtonScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTesting.Scenes;

using System.Drawing;
using KdGui;
using KdGui.Factories;
using Velaptor.Scene;

public class ButtonScene : SceneBase
{
    private readonly IControlFactory ctrlFactory;
    private IControlGroup? ctrlGroup;
    private IButton? button;

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonScene"/> class.
    /// </summary>
    public ButtonScene()
    {
        Name = "Button Scene";
        this.ctrlFactory = new ControlFactory();
    }

    /// <summary>
    /// Loads the content for the scene.
    /// </summary>
    public override void LoadContent()
    {
        this.button = this.ctrlFactory.CreateButton();

        this.ctrlGroup = this.ctrlFactory.CreateControlGroup();
        this.ctrlGroup.Title = "Button Group";
        this.ctrlGroup.Width = 200;
        this.ctrlGroup.Height = 200;
        this.ctrlGroup.Position = new Point(
            ((int)WindowSize.Width / 2) - (this.ctrlGroup.Width / 2),
            ((int)WindowSize.Height / 2) - (this.ctrlGroup.Height / 2));
        this.ctrlGroup.Add(this.button);

        base.LoadContent();
    }

    /// <summary>
    /// Renders the scene.
    /// </summary>
    public override void Render()
    {
        this.ctrlGroup.Render();

        base.Render();
    }
}
