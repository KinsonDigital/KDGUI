// <copyright file="RelativePositioningScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTesting.Scenes;

using System.Drawing;
using KdGui;
using KdGui.Factories;
using Velaptor;
using Velaptor.Scene;

/// <summary>
/// Creates a scene to test out relative positioning.
/// </summary>
public class RelativePositioningScene : SceneBase
{
    private const int Spacing = 10;
    private IControlGroup? groupA;
    private IControlGroup? groupB;
    private IControlGroup? groupC;
    private IControlGroup? groupD;
    private IControlGroup? groupE;
    private IControlGroup? groupF;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelativePositioningScene"/> class.
    /// </summary>
    public RelativePositioningScene() => Name = "Relative Positioning";

    /// <summary>
    /// Loads the content for the scene.
    /// </summary>
    public override void LoadContent()
    {
        var ctrlFactory = new ControlFactory();

        // Vertical Aligned Groups
        var groupALabel = ctrlFactory.CreateLabel();
        groupALabel.Text = "Drag Me Around";

        this.groupA = ctrlFactory.CreateControlGroup();
        this.groupA.Title = "Group A (Vertical)";
        this.groupA.NoResize = true;
        this.groupA.Width = 200;
        this.groupA.Height = 100;
        this.groupA.Position = new Point(Spacing, Spacing);
        this.groupA.Add(groupALabel);

        this.groupB = ctrlFactory.CreateControlGroup();
        this.groupB.Title = "Group B (Vertical)";
        this.groupB.NoResize = true;
        this.groupB.Width = 200;
        this.groupB.Height = 100;
        this.groupB.Position = new Point(Spacing, this.groupA.Bottom + Spacing);

        this.groupC = ctrlFactory.CreateControlGroup();
        this.groupC.Title = "Group C (Vertical)";
        this.groupC.NoResize = true;
        this.groupC.Width = 200;
        this.groupC.Height = 100;
        this.groupC.Position = new Point(Spacing, this.groupB.Bottom + Spacing);

        // Horizontal Aligned Groups
        var groupDLabel = ctrlFactory.CreateLabel();
        groupDLabel.Text = "Drag Me Around";

        this.groupD = ctrlFactory.CreateControlGroup();
        this.groupD.Title = "Group D (Horizontal)";
        this.groupD.NoResize = true;
        this.groupD.Width = 215;
        this.groupD.Height = 100;
        this.groupD.Position = new Point(Spacing, this.groupC.Bottom + Spacing);
        this.groupD.Add(groupDLabel);

        this.groupE = ctrlFactory.CreateControlGroup();
        this.groupE.Title = "Group E (Horizontal)";
        this.groupE.NoResize = true;
        this.groupE.Width = 215;
        this.groupE.Height = 100;
        this.groupE.Position = new Point(this.groupD.Right + Spacing, this.groupD.Top);

        this.groupF = ctrlFactory.CreateControlGroup();
        this.groupF.Title = "Group F (Horizontal)";
        this.groupF.NoResize = true;
        this.groupF.Width = 215;
        this.groupF.Height = 100;
        this.groupF.Position = new Point(this.groupE.Right + Spacing, this.groupF.Top);

        base.LoadContent();
    }

    /// <summary>
    /// Updates the scene.
    /// </summary>
    /// <param name="frameTime">The time for the current frame.</param>
    public override void Update(FrameTime frameTime)
    {
        this.groupB.Position = new Point(this.groupA.Position.X, this.groupA.Bottom + Spacing);
        this.groupC.Position = new Point(this.groupB.Position.X, this.groupB.Bottom + Spacing);

        this.groupE.Position = new Point(this.groupD.Right + Spacing, this.groupD.Top);
        this.groupF.Position = new Point(this.groupE.Right + Spacing, this.groupE.Top);

        base.Update(frameTime);
    }

    /// <summary>
    /// Renders the scene.
    /// </summary>
    public override void Render()
    {
        this.groupA.Render();
        this.groupB.Render();
        this.groupC.Render();

        this.groupD.Render();
        this.groupE.Render();
        this.groupF.Render();

        base.Render();
    }
}
