using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Funkin.NET.Content.Screens;
using Funkin.NET.Core.BackgroundDependencyLoading;
using Funkin.NET.Resources;
using Newtonsoft.Json;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data shared between the test browser and game implementation.
    /// </summary>
    public class FunkinGame : Game, IBackgroundDependencyLoadable
    {
        #region Constants

        public const string ProgramName = "Funkin.NET";

        #endregion

        #region Static Fields & Properties

        /// <summary>
        ///     Active <see cref="GameHost"/> instance. Resolved at runtime in <see cref="Main"/>.
        /// </summary>
        public static GameHost RunningHost { get; set; }

        /// <summary>
        ///     Active <see cref="FunkinGame"/> instance. Resolved at runtime in <see cref="Main"/>.
        /// </summary>
        public static FunkinGame RunningGame { get; set; }

        public static readonly Assembly[] ResourceMarket =
        {
            ResourcesAssembly.Assembly
        };

        public static readonly string[] FontMarket =
        {
            @"Fonts/VCR",
            @"Fonts/Funkin"
        };

        #endregion

        #region Instanced Fields & Properties

        public ScreenStack ScreenStack { get; private set; }

        public TextureStore TextureStore { get; private set; }

        public DependencyContainer DependencyContainer { get; private set; }

        public IntroScreen IntroScreen { get; private set; }

        public List<string[]> FunnyTextList { get; }

        public string[] FunnyText { get; }

        #endregion

        #region Overridden Properties

        protected override Container<Drawable> Content { get; }

        #endregion

        #region Constructors

        public FunkinGame()
        {
            Name = ProgramName;

            base.Content.Add(Content = new SafeAreaContainer
            {
                RelativeSizeAxes = Axes.Both,

                Child = new DrawSizePreservingFillContainer
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonConvert.DeserializeObject<List<string[]>>(text);
            FunnyText = FunnyTextList?[new Random().Next(0, FunnyTextList.Count)];
        }

        #endregion

        #region Overridden Methods

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Window.WindowMode.Value = WindowMode.Fullscreen;

            Add(ScreenStack = new ScreenStack());
            ScreenStack.Push(IntroScreen = new IntroScreen());
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            DependencyContainer = new DependencyContainer(base.CreateChildDependencies(parent));

        #endregion

        #region BackgroundDependencyLoader

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            foreach (Assembly store in ResourceMarket)
                Resources.AddStore(new DllResourceStore(store));

            foreach (string font in FontMarket)
                AddFont(Resources, font);

            TextureStore = new TextureStore(Textures);
            DependencyContainer.Cache(TextureStore);
        }

        #endregion
    }
}