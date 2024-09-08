// <copyright file="IoC.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGui;

using Carbonate.OneWay;
using Core;
using SimpleInjector;

/// <summary>
/// Provides the inversion of control container for the library.
/// </summary>
internal static class IoC
{
    private static readonly Container IoCContainer = new ();
    private static bool isInitialized;

    /// <summary>
    /// Gets the inversion of control container used to get instances of objects.
    /// </summary>
    public static Container Container
    {
        get
        {
            if (!isInitialized)
            {
                SetupContainer();
            }

            return IoCContainer;
        }
    }

    /// <summary>
    /// Sets up the IoC container.
    /// </summary>
    private static void SetupContainer()
    {
        IoCContainer.Options.EnableAutoVerification = false;
        IoCContainer.Register<IPushReactable<GridData>, PushReactable<GridData>>(Lifestyle.Singleton);
        IoCContainer.Register<IImGuiInvoker, ImGuiInvoker>(Lifestyle.Singleton);

        RegisterControls();

        isInitialized = true;
    }

    /// <summary>
    /// Registers all the ImGui controls.
    /// </summary>
    private static void RegisterControls()
    {
        IoCContainer.Register<ILabel, Label>(Lifestyle.Transient, true);
        IoCContainer.Register<IControlGroup, ControlGroup>(Lifestyle.Transient, true);
        IoCContainer.Register<INextPrevious, NextPrevious>(Lifestyle.Transient, true);
        IoCContainer.Register<IUpDown, UpDown>(Lifestyle.Transient, true);
        IoCContainer.Register<IArrowButton, ArrowButton>(Lifestyle.Transient, true);
        IoCContainer.Register<ICheckBox, CheckBox>(Lifestyle.Transient, true);
        IoCContainer.Register<IRadioButton, RadioButton>(Lifestyle.Transient, true);
        IoCContainer.Register<IComboBox, ComboBox>(Lifestyle.Transient, true);
        IoCContainer.Register<IButton, Button>(Lifestyle.Transient, true);
        IoCContainer.Register<ISlider, Slider>(Lifestyle.Transient, true);
    }
}
