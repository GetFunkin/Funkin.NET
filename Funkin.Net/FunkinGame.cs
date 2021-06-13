using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Funkin.NET.Resources;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;

[assembly: InternalsVisibleTo("Funkin.NET.Desktop")]

namespace Funkin.NET
{
    /// <summary>
    ///     Main Funkin' game. Ran by a <see cref="Host"/>.
    /// </summary>
    public class FunkinGame : Game
    {
        /// <summary>
        ///     Launched executable location.
        /// </summary>
        public static string ExecutablePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public const string ProgramName = "Funkin'";
        
        public TextureStore TextureStore { get; private set; }

        public DependencyContainer DependencyContainer { get; private set; }

        /* Sample game code. Temporary. */
        private Sprite _test;

        public FunkinGame()
        {
            Name = ProgramName;
        }

        [BackgroundDependencyLoader]
        private void DepLoad()
        {
            Host.Window.Title = ProgramName;

            Resources.AddStore(new DllResourceStore(ResourcesAssembly.Assembly));

            TextureStore = new TextureStore(Textures);
            DependencyContainer.Cache(TextureStore);

            Add(_test = new Sprite
            {
                Texture = TextureStore.Get("Logo")
            });
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            DependencyContainer = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}