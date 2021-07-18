using System.Reflection;
using Funkin.NET.Content.Screens;
using Funkin.NET.Core.BackgroundDependencyLoading;
using Funkin.NET.Resources;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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
                TargetDrawSize = new Vector2(1366f, 768f)
            });
        }

        #endregion

        #region Overridden Methods

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ScreenStack.Push(new MainScreen());
        }

        #endregion

        #region BackgroundDependencyLoader

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            foreach (Assembly store in ResourceMarket)
                Resources.AddStore(new DllResourceStore(store));

            Child = ScreenStack = new ScreenStack { RelativePositionAxes = Axes.Both };
        }

        #endregion
    }
}