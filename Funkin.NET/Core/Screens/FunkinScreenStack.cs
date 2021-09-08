using Funkin.NET.Core.Graphics.Containers;
using Funkin.NET.Core.Screens.Background;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Core.Screens
{
    /// <summary>
    ///     See: osu!'s OsuScreenStack.
    /// </summary>
    public class FunkinScreenStack : ScreenStack
    {
        [Cached] private BackgroundScreenStack BackgroundScreenStack;
        private readonly ParallaxContainer ParallaxContainer;

        protected float ParallaxAmount => ParallaxContainer.ParallaxAmount;

        public FunkinScreenStack()
        {
            InternalChild = ParallaxContainer = new ParallaxContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = BackgroundScreenStack = new BackgroundScreenStack {RelativeSizeAxes = Axes.Both},
            };

            ScreenPushed += InternalScreenPushed;
            ScreenExited += ScreenChanged;
        }

        private void InternalScreenPushed(IScreen prev, IScreen next)
        {
            if (LoadState < LoadState.Ready)
            {
                // dependencies must be present to stay in a sane state.
                // this is generally only ever hit by test scenes.
                Schedule(() => InternalScreenPushed(prev, next));
                return;
            }

            ScreenChanged(prev, next);
        }

        protected virtual void ScreenChanged(IScreen prev, IScreen next)
        {
            SetParallax(next);
        }

        private void SetParallax(IScreen next) =>
            ParallaxContainer.ParallaxAmount = ParallaxContainer.DefaultParallaxAmount *
                ((IFunkinScreen) next)?.BackgroundParallaxAmount ?? 1.0f;
    }
}