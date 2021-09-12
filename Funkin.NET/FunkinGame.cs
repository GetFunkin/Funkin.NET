using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Funkin.NET.Common.Configuration;
using Funkin.NET.Common.Input;
using Funkin.NET.Common.Utilities;
using Funkin.NET.Game.Screens.Gameplay;
using Funkin.NET.Intermediary;
using Funkin.NET.Intermediary.Input;
using Funkin.NET.Intermediary.ResourceStores;
using Funkin.NET.Intermediary.Screens;
using Funkin.NET.Intermediary.Utilities;
using Funkin.NET.osuImpl.Graphics.Containers;
using Funkin.NET.osuImpl.Graphics.Cursor;
using Funkin.NET.osuImpl.Overlays;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.Input;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data game data. Different platforms will build upon it, if only slightly.
    /// </summary>
    public abstract class FunkinGame : IntermediaryGame, IOverlayContainer
    {
        public const string ProgramName = "Funkin.NET";

        public override Assembly Assembly => typeof(FunkinGame).Assembly;

        public override IEnumerable<IResourceStore<byte[]>> ResourceStores => new IResourceStore<byte[]>[]
        {
            new DllResourceStore(PathHelper.Assembly)
        };

        public override IEnumerable<(ResourceStore<byte[]>, string)> FontStore => new[]
        {
            (Resources, PathHelper.Font.Vcr),
            (Resources, PathHelper.Font.Funkin),
            (Resources, PathHelper.Font.TorusRegular),
            (Resources, PathHelper.Font.TorusLight),
            (Resources, PathHelper.Font.TorusSemiBold),
            (Resources, PathHelper.Font.TorusBold)
        };

        protected override Container<Drawable> Content => Containers[FunkinContainers.Content];

        public List<OverlayContainer> VisibleBlockingOverlays { get; } = new();

        protected FunkinConfigManager Configuration;
        protected DependencyContainer DependencyContainer;
        protected Bindable<bool> ShowFpsDisplay;
        protected Dictionary<OverlayContainerType, Container> OverlayContainers;
        protected Container ScreenOffsetContainer;
        protected UniversalActionContainer ActionContainer;

        protected FunkinGame()
        {
            Name = ProgramName;
        }

        protected override void LoadComplete()
        {
            ShowFpsDisplay.SetBindable(Configuration,
                FunkinConfigManager.FunkinSetting.ShowFpsDisplay,
                x => FrameStatistics.Value = x.NewValue
                    ? FrameStatisticsMode.Full
                    : FrameStatisticsMode.None);

            FrameStatistics.ValueChanged += x => ShowFpsDisplay.Value = x.NewValue != FrameStatisticsMode.None;
            Containers.As<DefaultCursorContainer>(FunkinContainers.Cursor).CanShowCursor = false;
            OverlayContainers = new Dictionary<OverlayContainerType, Container>();

            static Container QuickContainer() => new()
            {
                RelativeSizeAxes = Axes.Both
            };

            AddRange(new Drawable[]
            {
                ScreenOffsetContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,

                    Children = new Drawable[]
                    {
                        Containers[FunkinContainers.Screen] = new Container
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

                OverlayContainers[OverlayContainerType.Content] = QuickContainer(),
                OverlayContainers[OverlayContainerType.RightFloating] = QuickContainer(),
                OverlayContainers[OverlayContainerType.LeftFloating] = QuickContainer(),
                OverlayContainers[OverlayContainerType.TopMost] = QuickContainer(),
            });

            base.LoadComplete();

            ScreenStack.Push(new StartupIntroductionScreen(new EnterScreen()));

            DependencyContainer.CacheAs(Containers[FunkinContainers.Settings] = new SettingsOverlay());
            LoadComponentAsync(Containers[FunkinContainers.Settings],
                OverlayContainers[OverlayContainerType.LeftFloating].Add,
                CancellationToken.None);

            OverlayContainer[] singleDisplaySideOverlays =
            {
                Containers.As<SettingsOverlay>(FunkinContainers.Settings)
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
            DependencyContainer = new DependencyContainer(base.CreateChildDependencies(parent));

        public override Container CreateScalingContainer() =>
            Containers[FunkinContainers.ScalingContainer] = new ScalingContainer(FunkinConfigManager.ScalingMode.On);

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage = host.Storage;
            Configuration = new FunkinConfigManager(Storage);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            Configuration?.Dispose();
        }

        protected override IDictionary<FrameworkSetting, object> GetFrameworkConfigDefaults() =>
            new Dictionary<FrameworkSetting, object>
            {
                {FrameworkSetting.WindowMode, WindowMode.Windowed}
            };

        protected override UserInputManager CreateUserInputManager() => new StandardInputManager();

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Containers.As<DefaultCursorContainer>(FunkinContainers.Cursor).CanShowCursor =
                (ScreenStack.CurrentScreen as DefaultScreen)?.CursorVisible ?? false;
        }

        public override void OnBackgroundDependencyLoad()
        {
            DependencyContainer.CacheAs(Storage);
            DependencyContainer.CacheAs(this);
            DependencyContainer.CacheAs(Configuration);

            SparrowAtlasStore sparrowAtlas = new(Resources);
            DependencyContainer.CacheAs(sparrowAtlas);
        }

        public override void InitializeContent()
        {
            Drawable[] mainContent =
            {
                Containers[FunkinContainers.Cursor] = new DefaultCursorContainer
                {
                    RelativeSizeAxes = Axes.Both
                },

                ActionContainer = new UniversalActionContainer(Storage, this)
            };

            Containers[FunkinContainers.Cursor].Child = Containers[FunkinContainers.Content] =
                new DefaultTooltipContainer(Containers.As<DefaultCursorContainer>(FunkinContainers.Cursor).Cursor)
                {
                    RelativeSizeAxes = Axes.Both
                };

            DependencyContainer.Cache(ActionContainer);

            base.Content.Add(CreateScalingContainer().WithChildren(mainContent));
        }

        public override void ScreenChanged(IScreen current, IScreen newScreen)
        {
            base.ScreenChanged(current, newScreen);

            if (newScreen is not DefaultScreen backgroundProvider 
                || Containers[FunkinContainers.ScalingContainer] is null) 
                return;
            
            Containers.As<ScalingContainer>(FunkinContainers.ScalingContainer).BackgroundStack?.Push(backgroundProvider.CreateBackground());
            ScreenStack.SetParallax(backgroundProvider);
            ScreenStack.BackgroundScreenStack.Push(backgroundProvider.CreateBackground());
        }

        public void UpdateBlockingOverlayFade() => Containers[FunkinContainers.Screen].FadeColour(
            VisibleBlockingOverlays.Any()
                ? new Colour4(0.5f, 0.5f, 0.5f, 1f)
                : Colour4.White, 500D, Easing.OutQuint);
    }
}