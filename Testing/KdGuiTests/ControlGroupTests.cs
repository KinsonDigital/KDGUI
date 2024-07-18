// <copyright file="ControlGroupTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KdGuiTests;

using System.Drawing;
using System.Numerics;
using Carbonate.NonDirectional;
using FluentAssertions;
using Helpers;
using ImGuiNET;
using KdGui;
using KdGui.Core;
using NSubstitute;

/// <summary>
/// Tests the <see cref="ControlGroup"/> class.
/// </summary>
public class ControlGroupTests
{
    private readonly IImGuiInvoker mockImGuiInvoker;
    private readonly IPushReactable mockPushReactable;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlGroupTests"/> class.
    /// </summary>
    public ControlGroupTests()
    {
        this.mockImGuiInvoker = Substitute.For<IImGuiInvoker>();
        this.mockPushReactable = Substitute.For<IPushReactable>();
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullImGuiInvokerParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ControlGroup(null, this.mockPushReactable);
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
            _ = new ControlGroup(this.mockImGuiInvoker, null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'renderReactable')");
    }

    [Fact]
    public void Ctor_WhenInvoked_SetsIdProperty()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        var actual = sut.Id;

        // Assert
        actual.Should().NotBeEmpty();
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void Title_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Title = "test";

        // Assert
        sut.Title.Should().Be("test");
    }

    [Fact]
    public void Position_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Position = new Point(10, 20);

        // Assert
        sut.Position.Should().Be(new Point(10, 20));
    }

    [Fact]
    public void Width_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Width = 100;

        // Assert
        sut.Width.Should().Be(100);
    }

    [Fact]
    public void Height_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Height = 100;

        // Assert
        sut.Height.Should().Be(100);
    }

    [Fact]
    public void HalfWidth_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Width = 100;

        // Assert
        sut.HalfWidth.Should().Be(50);
    }

    [Fact]
    public void HalfHeight_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Height = 100;

        // Assert
        sut.HalfHeight.Should().Be(50);
    }

    [Fact]
    public void Left_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Position = new Point(10, 20);

        // Assert
        sut.Left.Should().Be(10);
    }

    [Fact]
    public void Top_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Position = new Point(10, 20);

        // Assert
        sut.Top.Should().Be(20);
    }

    [Fact]
    public void Right_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Position = new Point(10, 20);
        sut.Width = 100;

        // Assert
        sut.Right.Should().Be(110);
    }

    [Fact]
    public void Bottom_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();

        // Act
        sut.Position = new Point(10, 20);
        sut.Height = 100;

        // Assert
        sut.Bottom.Should().Be(120);
    }

    [Fact]
    public void TitleBarVisible_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        var expected = !sut.TitleBarVisible;

        // Act
        sut.TitleBarVisible = !sut.TitleBarVisible;

        // Assert
        sut.TitleBarVisible.Should().Be(expected);
    }

    [Fact]
    public void AutoSizeToFitContent_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        var expected = !sut.AutoSizeToFitContent;

        // Act
        sut.AutoSizeToFitContent = !sut.AutoSizeToFitContent;

        // Assert
        sut.AutoSizeToFitContent.Should().Be(expected);
    }

    [Fact]
    public void NoResize_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        var expected = !sut.NoResize;

        // Act
        sut.NoResize = !sut.NoResize;

        // Assert
        sut.NoResize.Should().Be(expected);
    }

    [Fact]
    public void Visible_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        var expected = !sut.Visible;

        // Act
        sut.Visible = !sut.Visible;

        // Assert
        sut.Visible.Should().Be(expected);
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Add_WhenAddingControl_AddsControl()
    {
        // Arrange
        var fakeBtn = new FakeButton();
        fakeBtn.Name = "fake-button";

        var fakeLabel = new FakeLabel();
        fakeLabel.Name = "fake-label";

        var sut = CreateSystemUnderTest();
        sut.Add(fakeBtn);
        sut.Add(fakeLabel);

        // Act
        var actualFakeBtn = sut.GetControl<IControl>("fake-button");
        var actualFakeLabel = sut.GetControl<IControl>("fake-label");

        // Assert
        actualFakeBtn.Should().NotBeNull();
        actualFakeBtn.Should().BeOfType<FakeButton>();
        actualFakeBtn.Name.Should().Be("fake-button");

        actualFakeLabel.Should().NotBeNull();
        actualFakeLabel.Should().BeOfType<FakeLabel>();
        actualFakeLabel.Name.Should().Be("fake-label");
    }

    [Fact]
    public void GetControl_WithControlIsNotFound_ReturnsDefault()
    {
        // Arrange
        var fakeBtn = Substitute.For<IControl>();

        var sut = CreateSystemUnderTest();
        sut.Add(fakeBtn);

        // Act
        var actual = sut.GetControl<IControl>("fake-button");

        // Assert
        actual.Should().BeNull();
    }

    [Theory]
    // Title Bar Visible, No Auto Size
    [InlineData(true, false, false, false, false)]
    // Title Bar Hidden, Auto Size To Fit Content
    [InlineData(false, true, true, true, true)]
    public void Render_WhenInvoked_UsesCorrectWindowFlags(
        bool titleBarVisible,
        bool autoSizeToFitContent,
        bool expectedNoTitleBar,
        bool expectedAlwaysAutoResize,
        bool expectedNoMove)
    {
        // Arrange
        var windowFlags = ImGuiWindowFlags.None;

        this.mockImGuiInvoker.When(x => x.Begin(Arg.Any<string>(), Arg.Any<ImGuiWindowFlags>()))
            .Do((callInfo) =>
            {
                windowFlags = callInfo.Arg<ImGuiWindowFlags>();
            });

        var sut = CreateSystemUnderTest();
        sut.TitleBarVisible = titleBarVisible;
        sut.AutoSizeToFitContent = autoSizeToFitContent;

        // Act
        sut.Render();
        var actualNoTitleBar = (windowFlags & ImGuiWindowFlags.NoTitleBar) != 0;
        var actualAlwaysAutoResize = (windowFlags & ImGuiWindowFlags.AlwaysAutoResize) != 0;
        var actualNoMove = (windowFlags & ImGuiWindowFlags.NoMove) != 0;

        // Assert
        actualNoTitleBar.Should().Be(expectedNoTitleBar);
        actualAlwaysAutoResize.Should().Be(expectedAlwaysAutoResize);
        actualNoMove.Should().Be(expectedNoMove);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void Render_WhenSettingNoResize_SetsCorrectWindowFlags(bool noResize, bool expected)
    {
        // Arrange
        var windowFlags = ImGuiWindowFlags.None;
        var sut = CreateSystemUnderTest();
        sut.NoResize = noResize;
        this.mockImGuiInvoker.When(x => x.Begin(Arg.Any<string>(), Arg.Any<ImGuiWindowFlags>()))
            .Do((callInfo) =>
            {
                windowFlags = callInfo.Arg<ImGuiWindowFlags>();
            });

        // Act
        sut.Render();
        var actual = (windowFlags & ImGuiWindowFlags.NoResize) != 0;

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, true)]
    public void Render_WhenSettingVisibility_SetsCorrectWindowFlags(bool isVisible, bool expectedNoTitleBar, bool expectedNoBackground)
    {
        // Arrange
        var windowFlags = ImGuiWindowFlags.None;
        var sut = CreateSystemUnderTest();
        sut.TitleBarVisible = true;
        sut.Visible = isVisible;
        this.mockImGuiInvoker.When(x => x.Begin(Arg.Any<string>(), Arg.Any<ImGuiWindowFlags>()))
            .Do((callInfo) =>
            {
                windowFlags = callInfo.Arg<ImGuiWindowFlags>();
            });

        // Act
        sut.Render();
        var actualNoTitleBar = (windowFlags & ImGuiWindowFlags.NoTitleBar) != 0;
        var actualNoBackground = (windowFlags & ImGuiWindowFlags.NoBackground) != 0;

        // Assert
        actualNoTitleBar.Should().Be(expectedNoTitleBar);
        actualNoBackground.Should().Be(expectedNoBackground);
    }

    [Theory]
    [InlineData(true, true, 1f, 1, 4)]
    [InlineData(false, false, 0f, 0, 1)]
    public void Render_WhenInvoked_SetsCorrectWindowStyles(
        bool isVisible,
        bool noResize,
        float expectedAlpha,
        int expectedInvokedCount,
        int expectedPopClrCount)
    {
        // Arrange
        var actualAlpha = float.MaxValue;
        var actualTextClr = default(Color);

        this.mockImGuiInvoker.When(x => x.PushStyleVar(ImGuiStyleVar.Alpha, Arg.Any<float>()))
            .Do(callInfo =>
            {
                actualAlpha = callInfo.Arg<float>();
            });

        this.mockImGuiInvoker.When(x => x.PushStyleColor(ImGuiCol.Text, Arg.Any<Color>()))
            .Do(callInfo =>
            {
                actualTextClr = callInfo.Arg<Color>();
            });

        var sut = CreateSystemUnderTest();
        sut.Visible = isVisible;
        sut.NoResize = noResize;

        // Act
        sut.Render();

        // Assert
        actualAlpha.Should().Be(expectedAlpha);
        actualTextClr.Should().Be(Color.White);

        this.mockImGuiInvoker.Received(expectedInvokedCount).PushStyleColor(ImGuiCol.ResizeGrip, Color.Transparent);
        this.mockImGuiInvoker.Received(expectedInvokedCount).PushStyleColor(ImGuiCol.ResizeGripHovered, Color.Transparent);
        this.mockImGuiInvoker.Received(expectedInvokedCount).PushStyleColor(ImGuiCol.ResizeGripActive, Color.Transparent);

        this.mockImGuiInvoker.Received(1).PopStyleColor(expectedPopClrCount);
        this.mockImGuiInvoker.Received(1).PopStyleVar(1);
    }

    [Fact]
    public void Render_WhenAutoSizingToContentWithVisibleTitleBar_AutoSizesToTitle()
    {
        // Arrange
        var actualBtnSize = Vector2.Zero;

        var sut = CreateSystemUnderTest();
        sut.AutoSizeToFitContent = true;
        sut.TitleBarVisible = true;
        sut.Title = "Test Title";

        this.mockImGuiInvoker.CalcTextSize(Arg.Any<string>()).Returns(new Vector2(100, 0));
        this.mockImGuiInvoker.When(x => x.InvisibleButton(Arg.Any<string>(), Arg.Any<Vector2>()))
            .Do(callInfo => actualBtnSize = callInfo.Arg<Vector2>());

        // Act
        sut.Render();

        // Assert
        this.mockImGuiInvoker.Received(1).CalcTextSize("Test Title");
        this.mockImGuiInvoker.Received(1).InvisibleButton("##title_width Test Title", new Vector2(128, 0));

        // NOTE: The width of the title is always the width of the text plus 28 pixels to take
        // into account the ImGui window collapse button to the left of the title.
        actualBtnSize.Should().Be(new Vector2(128, 0));
    }

    [Theory]
    [InlineData(true, 10, 0, 0f, 0f, 0)]
    [InlineData(true, 0, 20, 0f, 0f, 0)]
    [InlineData(false, 10, 0, 10f, 32f, 1)]
    [InlineData(false, 0, 20, 32f, 20f, 1)]
    public void Render_WhenResizingControlGroup_UpdatesPosition(
        bool autoSieToFitContent,
        int width,
        int height,
        float expectedWidth,
        float expectedHeight,
        int expectedInvokeCount)
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        sut.AutoSizeToFitContent = autoSieToFitContent;

        if (width != 0)
        {
            sut.Width = width;
        }

        if (height != 0)
        {
            sut.Height = height;
        }

        // Act
        sut.Render();

        // Assert
        this.mockImGuiInvoker.Received(expectedInvokeCount).SetNextWindowSize(new Vector2(expectedWidth, expectedHeight));
    }

    [Fact]
    public void Render_WhenSizeHasChanged_InvokesSizeChangedEvent()
    {
        // Arrange
        var sut = CreateSystemUnderTest();
        this.mockImGuiInvoker.GetWindowSize().Returns(new Vector2(100, 0));
        var sizeChangedInvoked = false;

        sut.SizeChanged += (_, size) =>
        {
            sizeChangedInvoked = true;
            size.Should().Be(new Size(100, 0));
        };

        // Act
        sut.Render();

        // Assert
        sizeChangedInvoked.Should().BeTrue();
    }

    [Fact]
    public void Render_WhenWindowIsNotBeingDragged_SetsImGuiWindowPosition()
    {
        // Arrange
        // Guarantee that the window is not being dragged
        this.mockImGuiInvoker.IsWindowFocused().Returns(false);
        this.mockImGuiInvoker.IsMouseDragging(Arg.Any<ImGuiMouseButton>()).Returns(false);

        var sut = CreateSystemUnderTest();
        sut.Position = new Point(10, 20);

        // Act
        sut.Render();

        // Assert
        this.mockImGuiInvoker.Received(1).SetNextWindowPos(new Vector2(10, 20));
    }

    [Fact]
    public void Render_WhenInvoked_InvokesInitializedEventAfterPreRender()
    {
        // Arrange
        var initInvoked = false;
        var sut = CreateSystemUnderTest();
        sut.Initialized += (_, _) => initInvoked = true;

        // Act
        sut.Render();

        // Assert
        initInvoked.Should().BeTrue();
    }

    [Fact]
    public void Render_WhenDraggingControlGroup_UpdatesPosition()
    {
        // Arrange
        this.mockImGuiInvoker.IsWindowFocused().Returns(true);
        this.mockImGuiInvoker.IsMouseDragging(Arg.Any<ImGuiMouseButton>()).Returns(true);

        var sut = CreateSystemUnderTest();

        // Act
        sut.Render();

        // Assert
        this.mockImGuiInvoker.Received(1).GetWindowPos();
    }

    [Fact]
    public void Render_WithDefaultSettings_RendersGroup()
    {
        // Arrange
        this.mockImGuiInvoker.IsWindowFocused().Returns(true);

        var sut = CreateSystemUnderTest();
        sut.Title = "Test Title";

        // Act
        sut.Render();

        // Assert
        this.mockImGuiInvoker.Received(1).PushID(Arg.Any<string>());
        this.mockImGuiInvoker.Received(1).Begin("Test Title", Arg.Any<ImGuiWindowFlags>());
        this.mockImGuiInvoker.Received(1).PopID();
        this.mockImGuiInvoker.Received(1).IsWindowFocused();
        this.mockImGuiInvoker.Received(1).IsMouseDragging(ImGuiMouseButton.Left);
        this.mockImGuiInvoker.Received(1).End();
    }

    [Fact]
    public void Dispose_WhenInvoked_DisposesOfGroup()
    {
        // Arrange
        var mockCtrlA = Substitute.For<IControl>();
        var mockCtrlB = Substitute.For<IControl>();

        var sut = CreateSystemUnderTest();
        sut.Add(mockCtrlA);
        sut.Add(mockCtrlB);

        // Act
        sut.Dispose();
        sut.Dispose();
        var controls = sut.GetListField<List<IControl>>("controls");

        // Assert
        mockCtrlA.Received(1).Dispose();
        mockCtrlB.Received(1).Dispose();
        controls.Should().BeEmpty();
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ControlGroup"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ControlGroup CreateSystemUnderTest()
        => new (this.mockImGuiInvoker, this.mockPushReactable);
}
