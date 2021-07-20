using Funkin.NET.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Screens
{
    /// <summary>
    ///     See: osu!'s OsuScreenStack.
    /// </summary>
    public class FunkinScreenStack : ScreenStack
    {
        [Cached] private BackgroundScreenStack _backgroundScreenStack;
        private readonly ParallaxContainer _parallaxContainer;

        protected float ParallaxAmount => _parallaxContainer.ParallaxAmount;

        public FunkinScreenStack()
        {
            InternalChild = _parallaxContainer = new ParallaxContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = _backgroundScreenStack = new BackgroundScreenStack {RelativeSizeAxes = Axes.Both},
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
            _parallaxContainer.ParallaxAmount = ParallaxContainer.DefaultParallaxAmount *
                ((IFunkinScreen) next)?.BackgroundParallaxAmount ?? 1.0f;
    }
}