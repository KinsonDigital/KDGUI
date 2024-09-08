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
        this.button.Text = "This is a long piece of text";

        var button2 = this.ctrlFactory.CreateButton();
        button2.Text = "R1C2";

        var button3 = this.ctrlFactory.CreateButton();
        button3.Text = "R2C1";

        var button4 = this.ctrlFactory.CreateButton();
        button4.Text = "R2C2";

        this.ctrlGroup = this.ctrlFactory.CreateControlGroup();
        this.ctrlGroup.TotalRows = 2;
        this.ctrlGroup.TotalColumns = 2;
        this.ctrlGroup.Title = "Button Group";
        this.ctrlGroup.Width = 200;
        this.ctrlGroup.Height = 200;
        this.ctrlGroup.Position = new Point(
            ((int)WindowSize.Width / 2) - (this.ctrlGroup.Width / 2),
            ((int)WindowSize.Height / 2) - (this.ctrlGroup.Height / 2));
        this.ctrlGroup.Add(this.button, 0, 0);
        this.ctrlGroup.Add(button2, 0, 1);
        this.ctrlGroup.Add(button3, 1, 0);
        this.ctrlGroup.Add(button4, 1, 1);

        base.LoadContent();
    }

    public override void UnloadContent()
    {
        this.ctrlGroup.Dispose();

        base.UnloadContent();
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
