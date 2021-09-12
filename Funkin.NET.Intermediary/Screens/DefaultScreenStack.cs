using Funkin.NET.Intermediary.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary.Screens
{
    /// <summary>
    ///     <see cref="ScreenStack"/> with extended capabilities
    /// </summary>
    public class DefaultScreenStack : ScreenStack
    {
        protected readonly ParallaxContainer ParallaxContainer;
        protected readonly BackgroundScreenStack BackgroundScreenStack;

        public DefaultScreenStack()
        {
            InternalChild = ParallaxContainer = new ParallaxContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = BackgroundScreenStack = new BackgroundScreenStack
                {
                    RelativeSizeAxes = Axes.Both
                }
            };
        }

        protected virtual void SetParallax(IScreen next)
        {
            if (next is not DefaultScreen screen)
            {
                ParallaxContainer.ParallaxAmount = ParallaxContainer.DefaultParallaxAmount;
                return;
            }

            float parallaxAmount = ParallaxContainer.DefaultParallaxAmount * screen.BackgroundParallaxAmount;
            ParallaxContainer.ParallaxAmount = parallaxAmount;
        }
    }
}