using System;
using System.Reflection;
using Funkin.NET.Common.KeyBinds.ArrowKeys;
using Funkin.NET.Common.KeyBinds.SelectionKey;
using Funkin.NET.Common.KeyBinds.WindowModeKeys;
using Funkin.NET.Content.Screens;
using Funkin.NET.Core.BackgroundDependencyLoading;
using Funkin.NET.Resources;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data shared between the test browser and game implementation.
    /// </summary>
    public class FunkinGame : Game, IBackgroundDependencyLoadable, IKeyBindingHandler<ArrowKeyAction>,
        IKeyBindingHandler<SelectionKeyAction>, IKeyBindingHandler<WindowModeKeyAction>
    {
        #region Constants

        public const string ProgramName = "Funkin.NET";

        #endregion

        #region Static Fields & Properties

        /// <summary>
        ///     Active <see cref="osu.Framework.Platform.GameHost"/> instance. Resolved at runtime in <see cref="Main"/>.
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

        public TextureStore TextureStore { get; private set; }

        public DependencyContainer DependencyContainer { get; private set; }

        #endregion

        #region Instanced Fields & Properties

        public ScreenStack ScreenStack { get; private set; }

        #endregion

        #region Overridden Properties

        protected override Container<Drawable> Content { get; }

        #endregion

        #region Constructors

        public FunkinGame()
        {
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1920f, 1080f)
            });
        }

        #endregion

        #region Overridden Methods

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ScreenStack.Push(new IntroScreen());

            Window.WindowMode.Value = WindowMode.Fullscreen;
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
            Child = ScreenStack = new ScreenStack {RelativePositionAxes = Axes.Both};
            DependencyContainer.Cache(TextureStore);
        }

        #endregion

        #region Key Binding Handlers

        public bool OnPressed(ArrowKeyAction action) => false;

        public void OnReleased(ArrowKeyAction action)
        {
        }

        public bool OnPressed(SelectionKeyAction action) => false;

        public void OnReleased(SelectionKeyAction action)
        {
        }

        public bool OnPressed(WindowModeKeyAction action) => true;

        public void OnReleased(WindowModeKeyAction action)
        {
            Window.WindowMode.Value = Window.WindowMode.Value switch
            {
                WindowMode.Windowed => WindowMode.Borderless,
                WindowMode.Borderless => WindowMode.Fullscreen,
                WindowMode.Fullscreen => WindowMode.Windowed,
                _ => throw new ArgumentOutOfRangeException(nameof(Window.WindowMode), "Invalid Window type.")
            };
        }

        #endregion
    }
}