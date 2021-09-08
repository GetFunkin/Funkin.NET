using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Funkin.NET.Common.Configuration;
using Funkin.NET.Common.Input;
using Funkin.NET.Common.Screens;
using Funkin.NET.Core.Input;
using Funkin.NET.Game.Screens.Gameplay;
using Funkin.NET.Intermediary.ResourceStores;
using Funkin.NET.osuImpl.Graphics.Containers;
using Funkin.NET.osuImpl.Graphics.Cursor;
using Funkin.NET.osuImpl.Overlays;
using Funkin.NET.Resources;
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
    public class FunkinGame : osu.Framework.Game
    {
        public const string ProgramName = "Funkin.NET";

        public static List<string[]> FunnyTextList { get; protected set; }

        public static string[] FunnyText { get; protected set; }

        public virtual Version AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new Version();

        public virtual string Version => AssemblyVersion.ToString();

        public string VersionHash { get; set; }

        protected FunkinConfigManager LocalConfig { get; set; }

        protected DefaultCursorContainer DefaultCursorContainer { get; set; }

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
        protected DefaultScreenStack ScreenStack;

        private readonly List<OverlayContainer> VisibleBlockingOverlays = new();

        public StartupIntroductionScreen MenuScreen { get; private set; }

        public EnterScreen IntroScreen { get; private set; }

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

            DefaultCursorContainer.CanShowCursor = MenuScreen?.CursorVisible ?? false;

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
                                ScreenStack = new DefaultScreenStack
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
                new StartupIntroductionScreen(new EnterScreen()));

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

        protected override UserInputManager CreateUserInputManager() => new RightMouseSpecializedInputManager();

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            DefaultCursorContainer.CanShowCursor = (ScreenStack.CurrentScreen as IDefaultScreen)?.CursorVisible ?? false;
        }

        protected virtual void ScreenChanged(IScreen current, IScreen newScreen)
        {
            switch (newScreen)
            {
                case EnterScreen enter: 
                    IntroScreen = enter;
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
            using (FileStream hash = File.OpenRead(typeof(FunkinGame).Assembly.Location))
                VersionHash = hash.ComputeMD5Hash();

            Resources.AddStore(new DllResourceStore(PathHelper.Assembly));

            ProtectedDependencies.CacheAs(Storage);

            ProtectedDependencies.CacheAs(this);
            ProtectedDependencies.CacheAs(LocalConfig);

            SparrowAtlasStore sparrowAtlas = new(Resources);
            ProtectedDependencies.CacheAs(sparrowAtlas);

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
                DefaultCursorContainer = new DefaultCursorContainer
                {
                    RelativeSizeAxes = Axes.Both
                },

                ActionContainer = new UniversalActionContainer(Storage, this)
            };

            DefaultCursorContainer.Child = ProtectedContent = new DefaultTooltipContainer(DefaultCursorContainer.Cursor)
            {
                RelativeSizeAxes = Axes.Both
            };

            ProtectedDependencies.Cache(ActionContainer);

            base.Content.Add(CreateScalingContainer().WithChildren(mainContent));
        }

        protected void UpdateBlockingOverlayFade() => ScreenContainer.FadeColour(VisibleBlockingOverlays.Any()
            ? new Colour4(0.5f, 0.5f, 0.5f, 1f)
            : Colour4.White, 500D, Easing.OutQuint);

        public void AddBlockingOverlay(OverlayContainer overlay)
        {
            if (!VisibleBlockingOverlays.Contains(overlay))
                VisibleBlockingOverlays.Add(overlay);

            UpdateBlockingOverlayFade();
        }

        public void RemoveBlockingOverlay(OverlayContainer overlay) => Schedule(() =>
        {
            VisibleBlockingOverlays.Remove(overlay);
            UpdateBlockingOverlayFade();
        });
    }
}