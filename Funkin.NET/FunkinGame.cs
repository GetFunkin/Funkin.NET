using System;
using System.Collections.Generic;
using System.IO;
using Funkin.NET.Content.Configuration;
using Funkin.NET.Content.osu.Configuration;
using Funkin.NET.Content.osu.Graphics.Containers;
using Funkin.NET.Content.osu.Screens;
using Funkin.NET.Content.Screens;
using Funkin.NET.Core.BackgroundDependencyLoading;
using Funkin.NET.Resources;
using Newtonsoft.Json;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data shared between the test browser and game implementation.
    /// </summary>
    public class FunkinGame : Game, IBackgroundDependencyLoadable
    {
        public const string ProgramName = "Funkin.NET";

        public static List<string[]> FunnyTextList { get; private set; }

        public static string[] FunnyText { get; private set; }

        protected Storage Storage { get; private set; }

        protected FunkinConfigManager Config { get; private set; }

        protected OsuScreenStack ScreenStack;

        protected override Container<Drawable> Content => _content;

        private DependencyContainer _dependencies;
        private Container _content;
        private Container _screenOffsetContainer;
        private ScalingContainer _screenContainer;

        public FunkinGame()
        {
            Name = ProgramName;

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonConvert.DeserializeObject<List<string[]>>(text);
            FunnyText = FunnyTextList?[new Random().Next(0, FunnyTextList.Count)];
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Window.WindowMode.BindTarget = Config.GetBindable<WindowMode>(FunkinSetting.DefaultWindowType);

            AddRange(new Drawable[]
            {
                _screenOffsetContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        _screenContainer = new ScalingContainer(ScalingMode.ExcludeOverlays)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Children = new Drawable[]
                            {
                                ScreenStack = new OsuScreenStack
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                }
            });

            ScreenStack.Push(new FunnyTextScreen(FunnyTextScreen.TextDisplayType.Intro));
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage ??= host.Storage;
            Config = new FunkinConfigManager(Storage);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            _dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            
            Config?.Dispose();
        }

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            // Resources.AddStore(new DllResourceStore(ResourcesAssembly.Assembly));

            _dependencies.CacheAs(Storage);

            _dependencies.CacheAs(this);
            _dependencies.CacheAs(Config);

            Resources.AddStore(new DllResourceStore(ResourcesAssembly.Assembly));

            AddFont(Resources, "Fonts/VCR");
            AddFont(Resources, "Fonts/Funkin");

            // global input container would go here

            _content = new Container
            {
                RelativeSizeAxes = Axes.Both
            };

            base.Content.Add(CreateScalingContainer());

            // todo: port over osu!'s key binding store system?
        }

        protected virtual Container CreateScalingContainer() => new ScalingContainer(ScalingMode.Everything);
    }
}