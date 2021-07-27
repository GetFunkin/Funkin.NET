using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Funkin.NET.Core.Configuration;
using Funkin.NET.Core.Graphics.Containers;
using Funkin.NET.Core.Graphics.Cursor;
using Funkin.NET.Core.Input;
using Funkin.NET.Core.Input.Bindings;
using Funkin.NET.Core.Overlays;
using Funkin.NET.Core.Screens;
using Funkin.NET.Game.Screens.Gameplay;
using Funkin.NET.Resources;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.Input;
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

        public virtual string Version => AssemblyVersion.ToString();

        public string VersionHash { get; set; }

        protected FunkinConfigManager LocalConfig { get; set; }

        protected FunkinCursorContainer FunkinCursorContainer { get; set; }

        protected Storage Storage { get; set; }

        protected override Container<Drawable> Content => ProtectedContent;

        protected Container ProtectedContent;
        protected DependencyContainer ProtectedDependencies;
        protected Bindable<bool> ShowFpsDisplay;
        protected Container OverlayContent;
        protected Container RightFloatingOverlayContainer;
        protected Container LeftFloatingOverlayContainer;
        protected Container TopMostOverlayContainer;
        protected ScalingContainer ScreenContainer;
        protected Container ScreenOffsetContainer;
        protected UniversalActionContainer ActionContainer;
        protected FunkinScreenStack ScreenStack;

        private readonly List<OverlayContainer> _visibleBlockingOverlays = new();

        public StartupIntroductionScreen MenuScreen { get; private set; }

        public FunnyTextScreen IntroScreen { get; private set; }

        public SettingsOverlay Settings;

        public FunkinGame()
        {
            Name = ProgramName;

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonSerializer.Deserialize<List<string[]>>(text);
            FunnyText = FunnyTextList?[new Random().Next(0, FunnyTextList.Count)];
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ShowFpsDisplay = LocalConfig.GetBindable<bool>(FunkinConfigManager.FunkinSetting.ShowFpsDisplay);
            ShowFpsDisplay.ValueChanged += x => FrameStatistics.Value = x.NewValue
                ? FrameStatisticsMode.Full
                : FrameStatisticsMode.None;
            ShowFpsDisplay.TriggerChange();

            FrameStatistics.ValueChanged += x => ShowFpsDisplay.Value = x.NewValue != FrameStatisticsMode.None;

            // todo: localization

            FunkinCursorContainer.CanShowCursor = MenuScreen?.CursorVisible ?? false;

            // todo: implement osu!'s volume control receptor
            AddRange(new Drawable[]
            {
                ScreenOffsetContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,

                    Children = new Drawable[]
                    {
                        ScreenContainer = new ScalingContainer(FunkinConfigManager.ScalingMode.ExcludeOverlays)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,

                            Children = new Drawable[]
                            {
                                ScreenStack = new FunkinScreenStack
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                },

                OverlayContent = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },

                RightFloatingOverlayContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },

                LeftFloatingOverlayContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },

                TopMostOverlayContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            ScreenStack.ScreenPushed += ScreenPushed;
            ScreenStack.ScreenExited += ScreenExited;

            ScreenStack.Push(
                new StartupIntroductionScreen(new FunnyTextScreen(FunnyTextScreen.TextDisplayType.Intro)));

            ProtectedDependencies.CacheAs(Settings = new SettingsOverlay());
            LoadComponentAsync(Settings, LeftFloatingOverlayContainer.Add, CancellationToken.None);

            OverlayContainer[] singleDisplaySideOverlays =
            {
                Settings
            };

            foreach (OverlayContainer overlay in singleDisplaySideOverlays)
            {
                overlay.State.ValueChanged += x =>
                {
                    if (x.NewValue == Visibility.Hidden)
                        return;

                    singleDisplaySideOverlays.Where(y => y != overlay).ForEach(y => y.Hide());
                };
            }
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            ProtectedDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

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
                {FrameworkSetting.WindowMode, WindowMode.Windowed}
            };

        protected override UserInputManager CreateUserInputManager() => new FunkinUserInputManager();

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            FunkinCursorContainer.CanShowCursor = (ScreenStack.CurrentScreen as IFunkinScreen)?.CursorVisible ?? false;
        }

        protected virtual void ScreenChanged(IScreen current, IScreen newScreen)
        {
            switch (newScreen)
            {
                case FunnyTextScreen funny:
                    if (funny.DisplayType == FunnyTextScreen.TextDisplayType.Intro)
                        IntroScreen = funny;
                    break;

                case StartupIntroductionScreen menu:
                    MenuScreen = menu;
                    break;
            }
        }

        private void ScreenPushed(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen ({lastScreen}) pushed to → {newScreen}");
        }

        private void ScreenExited(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen ({lastScreen}) exited to → {newScreen}");

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

            Resources.AddStore(new DllResourceStore(PathHelper.Assembly));

            ProtectedDependencies.CacheAs(Storage);

            ProtectedDependencies.CacheAs(this);
            ProtectedDependencies.CacheAs(LocalConfig);

            RegisterFonts();
            InitializeContent();
        }

        protected virtual void RegisterFonts()
        {
            AddFont(Resources, PathHelper.GetFont("VCR/VCR"));
            AddFont(Resources, PathHelper.GetFont("Funkin/Funkin"));
            AddFont(Resources, PathHelper.GetFont("Torus/Torus-Regular"));
            AddFont(Resources, PathHelper.GetFont("Torus/Torus-Light"));
            AddFont(Resources, PathHelper.GetFont("Torus/Torus-SemiBold"));
            AddFont(Resources, PathHelper.GetFont("Torus/Torus-Bold"));
        }

        protected virtual void InitializeContent()
        {
            Drawable[] mainContent =
            {
                FunkinCursorContainer = new FunkinCursorContainer
                {
                    RelativeSizeAxes = Axes.Both
                },

                ActionContainer = new UniversalActionContainer(Storage, this)
            };

            FunkinCursorContainer.Child = ProtectedContent = new FunkinTooltipContainer(FunkinCursorContainer.Cursor)
            {
                RelativeSizeAxes = Axes.Both
            };

            ProtectedDependencies.Cache(ActionContainer);

            base.Content.Add(CreateScalingContainer().WithChildren(mainContent));
        }

        protected void UpdateBlockingOverlayFade() => ScreenContainer.FadeColour(_visibleBlockingOverlays.Any()
            ? new Colour4(0.5f, 0.5f, 0.5f, 1f)
            : Colour4.White, 500D, Easing.OutQuint);

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
    }
}