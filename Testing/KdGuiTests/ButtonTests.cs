// <copyright file="ButtonTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTests;

using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Carbonate.Core.NonDirectional;
using Carbonate.NonDirectional;
using FluentAssertions;
using Helpers;
using ImGuiNET;
using KdGui;
using KdGui.Core;
using NSubstitute;

/// <summary>
/// Tests the <see cref="Button"/>.
/// </summary>
public class ButtonTests
{
    private readonly IImGuiInvoker mockImGuiInvoker;
    private readonly IPushReactable mockRenderReactable;

    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonTests"/> class.
    /// </summary>
    public ButtonTests()
    {
        this.mockImGuiInvoker = Substitute.For<IImGuiInvoker>();
        this.mockRenderReactable = Substitute.For<IPushReactable>();
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullImGuiInvokerParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Button(null, this.mockRenderReactable);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'imGuiInvoker')");
    }

    [Fact]
    public void Ctor_WithNullRenderReactableParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new Button(this.mockImGuiInvoker, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'renderReactable')");
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsDefaultPropertyValues()
    {
        // Arrange & Act
        var sut = CreateSystemUnderTest();

        // Assert
        sut.Text.Should().Be("Button");
        sut.Name.Should().Be("Button");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void Text_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Text = "test-value";

        // Assert
        sut.Text.Should().Be("test-value");
    }
    #endregion

    #region Method Tests
    [Fact]
    public unsafe void Render_NotVisibleDoesNotRenderButton()
    {
        // Arrange
        var guid = new Guid("ea033421-803e-45b8-aea3-b6ff0a0a70cc");

        var style = new ImGuiStyle { FramePadding = new Vector2(10, 0) };
        ImGuiStyle* stylePtr = &style;
        this.mockImGuiInvoker.GetStyle().Returns(new ImGuiStylePtr(stylePtr));
        this.mockImGuiInvoker.IsItemHovered().Returns(false);

        IReceiveSubscription? subscription = null;

        this.mockRenderReactable.When(x => x.Subscribe(Arg.Any<IReceiveSubscription>()))
            .Do(callInfo =>
            {
                subscription = callInfo.Arg<IReceiveSubscription>();
            });

        var sut = CreateSystemUnderTest();
        sut.Visible = false;
        sut.WindowOwnerId = guid;

        // Act
        subscription?.OnReceive();

        // Assert
        this.mockImGuiInvoker.DidNotReceive().PushID(Arg.Any<string>());
        this.mockImGuiInvoker.DidNotReceive().PushStyleColor(Arg.Any<ImGuiCol>(), Arg.Any<Color>());
        this.mockImGuiInvoker.DidNotReceive().Button(Arg.Any<string>());
        this.mockImGuiInvoker.DidNotReceive().GetFrameHeightWithSpacing();
        this.mockImGuiInvoker.DidNotReceive().PopStyleColor(Arg.Any<int>());
        this.mockImGuiInvoker.DidNotReceive().PopID();
        this.mockImGuiInvoker.DidNotReceive().IsItemHovered();
        this.mockImGuiInvoker.DidNotReceive().IsMouseDown(Arg.Any<ImGuiMouseButton>());
        this.mockImGuiInvoker.DidNotReceive().IsMouseReleased(Arg.Any<ImGuiMouseButton>());
    }

    [Fact]
    public unsafe void Render_WhenInvoked_RendersButton()
    {
        // Arrange
        var guid = new Guid("ea033421-803e-45b8-aea3-b6ff0a0a70cc");

        var style = new ImGuiStyle { FramePadding = new Vector2(10, 0), };
        ImGuiStyle* stylePtr = &style;
        this.mockImGuiInvoker.GetStyle().Returns(new ImGuiStylePtr(stylePtr));
        this.mockImGuiInvoker.IsItemHovered().Returns(true);

        IReceiveSubscription? subscription = null;

        this.mockRenderReactable.When(x => x.Subscribe(Arg.Any<IReceiveSubscription>()))
            .Do(callInfo =>
            {
                subscription = callInfo.Arg<IReceiveSubscription>();
            });

        var sut = CreateSystemUnderTest();
        sut.Text = "test-value";
        sut.WindowOwnerId = guid;

        // Act
        subscription?.OnReceive();

        // Assert
        this.mockImGuiInvoker.Received(1).PushID(Arg.Any<string>());
        this.mockImGuiInvoker.Received(1).PushStyleColor(ImGuiCol.Text, Color.White);
        this.mockImGuiInvoker.Received(1).Button("test-value");
        this.mockImGuiInvoker.Received(1).GetFrameHeightWithSpacing();
        this.mockImGuiInvoker.Received(1).PopStyleColor(4);
        this.mockImGuiInvoker.Received(1).PopID();
        this.mockImGuiInvoker.Received(1).IsItemHovered();
        this.mockImGuiInvoker.Received(1).IsMouseDown(ImGuiMouseButton.Left);
        this.mockImGuiInvoker.Received(1).IsMouseReleased(ImGuiMouseButton.Left);
    }

    [Fact]
    public unsafe void Render_WhenMouseIsNotHoveringOverButton_DoesCheckMouseButtonState()
    {
        // Arrange
        var guid = new Guid("ea033421-803e-45b8-aea3-b6ff0a0a70cc");

        var style = new ImGuiStyle { FramePadding = new Vector2(10, 0) };
        ImGuiStyle* stylePtr = &style;
        this.mockImGuiInvoker.GetStyle().Returns(new ImGuiStylePtr(stylePtr));
        this.mockImGuiInvoker.IsItemHovered().Returns(false);

        IReceiveSubscription? subscription = null;

        this.mockRenderReactable.When(x => x.Subscribe(Arg.Any<IReceiveSubscription>()))
            .Do(callInfo =>
            {
                subscription = callInfo.Arg<IReceiveSubscription>();
            });

        var sut = CreateSystemUnderTest();
        sut.WindowOwnerId = guid;

        // Act
        subscription?.OnReceive();

        // Assert
        this.mockImGuiInvoker.DidNotReceive().IsMouseDown(Arg.Any<ImGuiMouseButton>());
        this.mockImGuiInvoker.DidNotReceive().IsMouseReleased(Arg.Any<ImGuiMouseButton>());
    }

    [Fact]
    public unsafe void Render_WhenMouseIsDown_InvokesMousePressedEvent()
    {
        // Arrange
        var guid = new Guid("ea033421-803e-45b8-aea3-b6ff0a0a70cc");
        var mousePressedInvoked = false;
        var mouseReleasedInvoked = false;

        var style = new ImGuiStyle { FramePadding = new Vector2(10, 0) };
        ImGuiStyle* stylePtr = &style;
        this.mockImGuiInvoker.GetStyle().Returns(new ImGuiStylePtr(stylePtr));
        this.mockImGuiInvoker.IsItemHovered().Returns(true);
        this.mockImGuiInvoker.IsMouseDown(Arg.Any<ImGuiMouseButton>()).Returns(true);
        this.mockImGuiInvoker.IsMouseReleased(Arg.Any<ImGuiMouseButton>()).Returns(false);

        IReceiveSubscription? subscription = null;

        this.mockRenderReactable.When(x => x.Subscribe(Arg.Any<IReceiveSubscription>()))
            .Do(callInfo =>
            {
                subscription = callInfo.Arg<IReceiveSubscription>();
            });

        var sut = CreateSystemUnderTest();
        sut.WindowOwnerId = guid;
        sut.MousePressed += (_, _) => mousePressedInvoked = true;
        sut.MouseReleased += (_, _) => mouseReleasedInvoked = true;

        // Act
        subscription?.OnReceive();

        // Assert
        mousePressedInvoked.Should().BeTrue();
        mouseReleasedInvoked.Should().BeFalse();
    }

    [Fact]
    public unsafe void Render_WhenMouseIsReleased_InvokesMousePressedEvent()
    {
        // Arrange
        var guid = new Guid("ea033421-803e-45b8-aea3-b6ff0a0a70cc");
        var mouseReleasedInvoked = false;
        var clickInvoked = false;

        var style = new ImGuiStyle { FramePadding = new Vector2(10, 0) };
        ImGuiStyle* stylePtr = &style;
        this.mockImGuiInvoker.GetStyle().Returns(new ImGuiStylePtr(stylePtr));
        this.mockImGuiInvoker.IsItemHovered().Returns(true);
        // this.mockImGuiInvoker.IsMouseDown(Arg.Any<ImGuiMouseButton>()).Returns(true);
        this.mockImGuiInvoker.IsMouseReleased(Arg.Any<ImGuiMouseButton>()).Returns(true);

        IReceiveSubscription? subscription = null;

        this.mockRenderReactable.When(x => x.Subscribe(Arg.Any<IReceiveSubscription>()))
            .Do(callInfo =>
            {
                subscription = callInfo.Arg<IReceiveSubscription>();
            });

        var sut = CreateSystemUnderTest();
        sut.WindowOwnerId = guid;
        sut.MouseReleased += (_, _) => mouseReleasedInvoked = true;
        sut.Click += (_, _) => clickInvoked = true;

        // Act
        subscription?.OnReceive();

        // Assert
        mouseReleasedInvoked.Should().BeTrue();
        clickInvoked.Should().BeTrue();
    }

    [Fact]
    public void Dispose_WhenInvoked_DisposesOfButton()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.Click += (_, _) => { };
        sut.MousePressed += (_, _) => { };
        sut.MouseReleased += (_, _) => { };

        // Act
        sut.Dispose();
        sut.Dispose();

        var clickEvent = sut.GetEvent<EventHandler<EventArgs>>("Click");
        var mousePressedEvent = sut.GetEvent<EventHandler<EventArgs>>("MousePressed");
        var mouseReleasedEvent = sut.GetEvent<EventHandler<EventArgs>>("MouseReleased");

        // Assert
        clickEvent.Should().BeNull();
        mousePressedEvent.Should().BeNull();
        mouseReleasedEvent.Should().BeNull();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="Button"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private Button CreateSystemUnderTest()
        => new (this.mockImGuiInvoker, this.mockRenderReactable);
}
