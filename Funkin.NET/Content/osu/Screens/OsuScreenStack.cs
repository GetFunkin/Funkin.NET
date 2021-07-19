using Funkin.NET.Content.osu.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Content.osu.Screens
{
    public class OsuScreenStack : ScreenStack
    {
        [Cached] public BackgroundScreenStack BackgroundScreenStack;

        public readonly ParallaxContainer ParallaxContainer;

        protected float ParallaxAmount => ParallaxContainer.ParallaxAmount;

        public OsuScreenStack()
        {
            InternalChild = ParallaxContainer = new ParallaxContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = BackgroundScreenStack = new BackgroundScreenStack {RelativeSizeAxes = Axes.Both},
            };

            ScreenPushed += screenPushed;
            ScreenExited += ScreenChanged;
        }

        private void screenPushed(IScreen prev, IScreen next)
        {
            if (LoadState < LoadState.Ready)
            {
                // dependencies must be present to stay in a sane state.
                // this is generally only ever hit by test scenes.
                Schedule(() => screenPushed(prev, next));
                return;
            }

            // create dependencies synchronously to ensure leases are in a sane state.
            // ((OsuScreen)next).CreateLeasedDependencies((prev as OsuScreen)?.Dependencies ?? Dependencies);

            ScreenChanged(prev, next);
        }

        protected virtual void ScreenChanged(IScreen prev, IScreen next)
        {
            setParallax(next);
        }

        private void setParallax(IScreen next) =>
            ParallaxContainer.ParallaxAmount =
                ParallaxContainer.DEFAULT_PARALLAX_AMOUNT * ((OsuScreen) next)?.BackgroundParallaxAmount ?? 1.0f;
    }
}