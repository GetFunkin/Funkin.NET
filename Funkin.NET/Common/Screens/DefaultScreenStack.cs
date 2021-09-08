using Funkin.NET.Common.Screens.Background;
using Funkin.NET.osuImpl.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Common.Screens
{
    public class DefaultScreenStack : ScreenStack
    {
        [Cached] private BackgroundScreenStack BackgroundScreenStack;
        private readonly ParallaxContainer ParallaxContainer;

        protected float ParallaxAmount => ParallaxContainer.ParallaxAmount;

        public DefaultScreenStack()
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
                ((IDefaultScreen) next)?.BackgroundParallaxAmount ?? 1.0f;
    }
}