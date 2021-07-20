using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Funkin.NET.Configuration;
using Funkin.NET.Graphics.Cursor;
using Funkin.NET.Resources;
using Newtonsoft.Json;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

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
        private SettingsOverlay _settings;
        private readonly List<OverlayContainer> _overlays = new();
        private readonly List<OverlayContainer> _visibleBlockingOverlays = new();

        public FunkinGame()
        {
            Name = ProgramName;

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonConvert.DeserializeObject<List<string[]>>(text);
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
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            _dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage = host.Storage;
            LocalConfig = new FunkinConfigManager(Storage);
        }

        protected virtual Container CreateScalingContainer() => new DrawSizePreservingFillContainer();

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            LocalConfig?.Dispose();
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            // don't care about android lol!
            using (FileStream hash = File.OpenRead(typeof(FunkinGame).Assembly.Location)) 
                VersionHash = hash.ComputeMD5Hash();

            Resources.AddStore(new DllResourceStore(ResourcesAssembly.Assembly));

            _dependencies.CacheAs(Storage);

            LargeTextureStore largeStore = new(Host.CreateTextureLoaderStore(
                new NamespacedResourceStore<byte[]>(Resources, "Textures")));
            largeStore.AddStore(Host.CreateTextureLoaderStore(new DllResourceStore(ResourcesAssembly.Assembly)));
            _dependencies.Cache(largeStore);

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