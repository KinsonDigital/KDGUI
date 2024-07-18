// <copyright file="AllControlsScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTesting.Scenes;

using System.Drawing;
using KdGui;
using KdGui.Factories;
using Velaptor.Scene;

public class AllControlsScene : SceneBase
{
    private readonly IControlFactory ctrlFactory;
    private IControlGroup? ctrlGroup;
    private IButton? button;
    private ILabel? label;
    private IArrowButton? arrowButton;
    private ICheckBox? checkBox;
    private IComboBox? comboBox;
    private IRadioButton? radioButton;
    private ISlider? slider;
    private IUpDown? upDown;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllControlsScene"/> class.
    /// </summary>
    public AllControlsScene()
    {
        Name = "All Controls Scene";
        this.ctrlFactory = new ControlFactory();
    }

    /// <summary>
    /// Loads the content for the scene.
    /// </summary>
    public override void LoadContent()
    {
        this.ctrlGroup = this.ctrlFactory.CreateControlGroup();
        this.ctrlGroup.Title = "Controls";
        this.ctrlGroup.Position = new Point(10, 10);
        this.ctrlGroup.Width = (int)WindowSize.Width - 20;
        this.ctrlGroup.Height = (int)WindowSize.Height - 100;

        this.button = this.ctrlFactory.CreateButton();
        this.button.Text = "My Button";

        this.label = this.ctrlFactory.CreateLabel();
        this.label.Text = "My Label";

        this.arrowButton = this.ctrlFactory.CreateArrowButton();
        this.checkBox = this.ctrlFactory.CreateCheckbox();

        this.comboBox = this.ctrlFactory.CreateComboBox();
        this.comboBox.Items.Add("Item 1");
        this.comboBox.Items.Add("Item 2");

        this.radioButton = this.ctrlFactory.CreateRadioButton();

        this.slider = this.ctrlFactory.CreateSlider();
        this.slider.Min = 0;
        this.slider.Max = 100;

        this.upDown = this.ctrlFactory.CreateUpDown();

        this.ctrlGroup.Add(this.button);
        this.ctrlGroup.Add(this.label);
        this.ctrlGroup.Add(this.arrowButton);
        this.ctrlGroup.Add(this.checkBox);
        this.ctrlGroup.Add(this.comboBox);
        this.ctrlGroup.Add(this.radioButton);
        this.ctrlGroup.Add(this.slider);
        this.ctrlGroup.Add(this.upDown);

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
