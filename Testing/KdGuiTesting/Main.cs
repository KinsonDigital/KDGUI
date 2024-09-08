// <copyright file="Main.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTesting;

using System.Drawing;
using KdGui;
using KdGui.Factories;
using Scenes;
using Velaptor;
using Velaptor.UI;

public class Main : Window
{
    private readonly IControlFactory ctrlFactory;
    private IControlGroup? ctrlGroup;
    private INextPrevious? nextPrevious;

    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
        Width = 800;
        Height = 800;

        this.ctrlFactory = new ControlFactory();
    }

    protected override void OnLoad()
    {
        this.nextPrevious = this.ctrlFactory.CreateNextPrevious();
        this.nextPrevious.Next += NextPreviousOnNext;
        this.nextPrevious.Previous += NextPreviousOnPrevious;

        this.ctrlGroup = this.ctrlFactory.CreateControlGroup();
        this.ctrlGroup.TotalRows = 1;
        this.ctrlGroup.TotalColumns = 1;
        this.ctrlGroup.Add(this.nextPrevious);
        this.ctrlGroup.NoResize = true;
        this.ctrlGroup.TitleBarVisible = false;
        this.ctrlGroup.AutoSizeToFitContent = true;

        var buttonScene = new ButtonScene();
        var relativePosScene = new RelativePositioningScene();
        var allControlsScene = new AllControlsScene();

        SceneManager.AddScene(buttonScene, true);
        SceneManager.AddScene(relativePosScene);
        SceneManager.AddScene(allControlsScene);

        base.OnLoad();
    }

    protected override void OnUnload()
    {
        this.nextPrevious.Next -= NextPreviousOnNext;
        this.nextPrevious.Previous -= NextPreviousOnPrevious;

        base.OnUnload();
    }

    protected override void OnUpdate(FrameTime frameTime)
    {
        Title = SceneManager.CurrentScene.Name;

        this.ctrlGroup.Position = new Point(
            (int)Width - (this.ctrlGroup.Width + 10),
            (int)Height - (this.ctrlGroup.Height + 10));

        base.OnUpdate(frameTime);
    }

    protected override void OnDraw(FrameTime frameTime)
    {
        this.ctrlGroup.Render();

        base.OnDraw(frameTime);
    }

    private void NextPreviousOnPrevious(object? sender, EventArgs e) => SceneManager.PreviousScene();

    private void NextPreviousOnNext(object? sender, EventArgs e) => SceneManager.NextScene();
}
