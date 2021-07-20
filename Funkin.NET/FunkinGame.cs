using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Funkin.NET.Configuration;
using Funkin.NET.Graphics.Containers;
using Funkin.NET.Graphics.Cursor;
using Funkin.NET.Resources;
using Funkin.NET.Screens;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data game data. Different platforms likely build upon it, if only slightly.
    /// </summary>
    public class FunkinGame : Game
    {
        public const string ProgramName = "Funkin.NET";

        public static List<string[]> FunnyTextList { get; protected set; }

        public static string[] FunnyText { get; protected set; }

        public virtual Version AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version();

        public string VersionHash { get; set; }

        protected FunkinConfigManager LocalConfig { get; set; }

        protected FunkinCursorContainer FunkinCursorContainer { get; set; }

        protected Storage Storage { get; set; }

        protected override Container<Drawable> Content => _content;

        private Container _content;
        private DependencyContainer _dependencies;
        private Bindable<bool> _showFpsDisplay;
        private Container _overlayContent;
        private Container _rightFloatingOverlayContent;
        private Container _leftFloatingOverlayContent;
        private Container _topMostOverlayContent;
        private ScalingContainer _screenContainer;
        private Container _screenOffsetContainer;

        private FunkinScreenStack _screenStack;

        // TODO: private SettingsOverlay _settings;
        private readonly List<OverlayContainer> _overlays = new();
        private readonly List<OverlayContainer> _visibleBlockingOverlays = new();

        public MenuScreen MenuScreen { get; private set; }

        public FunnyTextScreen IntroScreen { get; private set; }

        public ScreenChangedDelegate OnScreenPushed;

        public FunkinGame()
        {
            Name = ProgramName;

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonSerializer.Deserialize<List<string[]>>(text);
            FunnyText = FunnyTextList?[new Random().Next(0, FunnyTextList.Count)];
        }

        protected void UpdateBlockingOverlayFade() => _screenContainer.FadeColour(
            _visibleBlockingOverlays.Any() ? new Colour4(0.5f, 0.5f, 0.5f, 1f) : Colour4.White, 500D, Easing.OutQuint);

        public void AddBlockingOverlay(OverlayContainer overlay)
        {
            if (!_visibleBlockingOverlays.Contains(overlay))
                _visibleBlockingOverlays.Add(overlay);

            UpdateBlockingOverlayFade();
        }

        public void RemoveBlockingOverlay(OverlayContainer overlay) => Schedule(() =>
        {
            _visibleBlockingOverlays.Remove(overlay);
            UpdateBlockingOverlayFade();
        });

        public void CloseAllOverlays()
        {
            foreach (OverlayContainer overlay in _overlays)
                overlay.Hide();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            _showFpsDisplay = LocalConfig.GetBindable<bool>(FunkinConfigManager.FunkinSetting.ShowFpsDisplay);
            _showFpsDisplay.ValueChanged += x => FrameStatistics.Value = x.NewValue
                ? FrameStatisticsMode.Full
                : FrameStatisticsMode.None;
            _showFpsDisplay.TriggerChange();

            FrameStatistics.ValueChanged += x => _showFpsDisplay.Value = x.NewValue != FrameStatisticsMode.None;

            // todo: localization

            FunkinCursorContainer.CanShowCursor = MenuScreen?.CursorVisible ?? false;

            // todo: implement osu!'s volume control receptor
            AddRange(new Drawable[]
            {
                _screenOffsetContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,

                    Children = new Drawable[]
                    {
                        _screenContainer = new ScalingContainer(FunkinConfigManager.ScalingMode.ExcludeOverlays)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,

                            Children = new Drawable[]
                            {
                                _screenStack = new FunkinScreenStack
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                },

                _overlayContent = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },

                _rightFloatingOverlayContent = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },

                _leftFloatingOverlayContent = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },

                _topMostOverlayContent = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            _screenStack.ScreenPushed += ScreenPushed;
            _screenStack.ScreenExited += ScreenExited;

            _screenStack.Push(new FunnyTextScreen(FunnyTextScreen.TextDisplayType.Intro));

            /*_dependencies.CacheAs(_settings = new SettingsOverlay());
            LoadComponentAsync(_settings, _leftFloatingOverlayContent.Add, CancellationToken.None);

            OverlayContainer[] singleDisplaySideOverlays =
            {
                _settings
            };

            foreach (OverlayContainer overlay in singleDisplaySideOverlays)
            {
                overlay.State.ValueChanged += x =>
                {
                    if (x.NewValue == Visibility.Hidden)
                        return;

                    singleDisplaySideOverlays.Where(y => y != x).ForEach(y => y.Hide());
                };
            }*/
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            _dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage = host.Storage;
            LocalConfig = new FunkinConfigManager(Storage);
        }

        protected virtual Container CreateScalingContainer() =>
            new ScalingContainer(FunkinConfigManager.ScalingMode.Everything);

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            LocalConfig?.Dispose();
        }

        protected override IDictionary<FrameworkSetting, object> GetFrameworkConfigDefaults() =>
            new Dictionary<FrameworkSetting, object>
            {
                {FrameworkSetting.WindowMode, WindowMode.Fullscreen}
            };

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            FunkinCursorContainer.CanShowCursor = (_screenStack.CurrentScreen as IFunkinScreen)?.CursorVisible ?? false;
        }

        protected virtual void ScreenChanged(IScreen current, IScreen newScreen)
        {
            OnScreenPushed?.Invoke(current, newScreen);

            switch (newScreen)
            {
                case FunnyTextScreen funny:
                    if (funny.DisplayType == FunnyTextScreen.TextDisplayType.Intro)
                        IntroScreen = funny;
                    break;

                case MenuScreen menu:
                    MenuScreen = menu;
                    break;
            }
        }

        private void ScreenPushed(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen changed → {newScreen}");
        }

        private void ScreenExited(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen changed ← {newScreen}");

            // todo: set to FunnyTextScreen exit edition
            if (newScreen == null)
                Exit();
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            // don't care about android lol!
            using (FileStream hash = File.OpenRead(typeof(FunkinGame).Assembly.Location))
                VersionHash = hash.ComputeMD5Hash();

            Resources.AddStore(new DllResourceStore(ResourcesAssembly.Assembly));

            _dependencies.CacheAs(Storage);

            _dependencies.CacheAs(this);
            _dependencies.CacheAs(LocalConfig);

            AddFont(Resources, "Fonts/VCR");
            AddFont(Resources, "Fonts/Funkin");
            AddFont(Resources, @"Fonts/Torus/Torus-Regular");
            AddFont(Resources, @"Fonts/Torus/Torus-Light");
            AddFont(Resources, @"Fonts/Torus/Torus-SemiBold");
            AddFont(Resources, @"Fonts/Torus/Torus-Bold");

            Drawable[] mainContent =
            {
                FunkinCursorContainer = new FunkinCursorContainer
                {
                    RelativeSizeAxes = Axes.Both
                }
            };

            FunkinCursorContainer.Child = _content = new FunkinTooltipContainer(FunkinCursorContainer.Cursor)
            {
                RelativeSizeAxes = Axes.Both
            };

            base.Content.Add(CreateScalingContainer().WithChildren(mainContent));
        }
    }
}