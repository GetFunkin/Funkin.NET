using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Funkin.NET.Common.KeyBinds.SelectionKey;
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

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data shared between the test browser and game implementation.
    /// </summary>
    public class FunkinGame : Game, IBackgroundDependencyLoadable
    {
        public const string ProgramName = "Funkin.NET";


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

        public ScreenStack ScreenStack { get; private set; }

        public TextureStore TextureStore { get; private set; }

        public DependencyContainer DependencyContainer { get; private set; }

        public IntroScreen IntroScreen { get; private set; }

        public List<string[]> FunnyTextList { get; }

        public string[] FunnyText { get; }


        protected override Container<Drawable> Content { get; }

        public FunkinGame()
        {
            Name = ProgramName;

            /*
            Content = new SafeAreaContainer
            {
                RelativeSizeAxes = Axes.Both,

                Child = new DrawSizePreservingFillContainer
                {
                    RelativeSizeAxes = Axes.Both,

                    Children = new Drawable[]
                    {
                        new SelectionKeyBindingContainer(this)
                    }
                }
            }
             */
            base.Content.Add(null);

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonConvert.DeserializeObject<List<string[]>>(text);
            FunnyText = FunnyTextList?[new Random().Next(0, FunnyTextList.Count)];
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Window.WindowMode.Value = WindowMode.Fullscreen;
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            DependencyContainer = new DependencyContainer(base.CreateChildDependencies(parent));


        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            foreach (Assembly store in ResourceMarket)
                Resources.AddStore(new DllResourceStore(store));

            foreach (string font in FontMarket)
                AddFont(Resources, font);

            TextureStore = new TextureStore(Textures);
            DependencyContainer.Cache(TextureStore);

            Add(ScreenStack = new ScreenStack());
            ScreenStack.Push(IntroScreen = new IntroScreen());
        }
    }
}