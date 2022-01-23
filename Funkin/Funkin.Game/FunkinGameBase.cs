using Funkin.Game.API;
using Funkin.Game.API.DependencyInjection;
using Funkin.Game.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
using Funkin.Resources;
using osu.Framework.Platform;

namespace Funkin.Game
{
    public class FunkinGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        private DependencyContainer dependencies = null!; // Won't be null when this object is accessed.

        protected readonly DependencyInjectionDispatcher InjectionDispatcher = new();

        protected Storage? Storage { get; set; }

        protected ModStore ModStore { get; private set; } = null!;

        protected FunkinGameBase()
        {
            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new RelativeScalingRatioPreservingContainer
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                TargetDrawSize = new Vector2(1280f, 720f),
            });
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage ??= host.Storage;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(FunkinResources).Assembly));

            dependencies.CacheAs(InjectionDispatcher);
            dependencies.CacheAs(ModStore = new ModStore(Storage));
            dependencies.CacheAs(Storage);
        }
    }
}
