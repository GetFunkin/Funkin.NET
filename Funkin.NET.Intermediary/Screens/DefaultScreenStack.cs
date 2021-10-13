using Funkin.NET.Intermediary.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Intermediary.Screens
{
    /// <summary>
    ///     <see cref="ScreenStack"/> with extended capabilities
    /// </summary>
    public class DefaultScreenStack : ScreenStack
    {
        protected ParallaxContainer ParallaxContainer;
        public BackgroundScreenStack? BackgroundScreenStack;

        public DefaultScreenStack(bool withBackground = true)
        {
            InternalChild = ParallaxContainer = new ParallaxContainer
            {
                RelativeSizeAxes = Axes.Both
            };

            if (withBackground)
                ParallaxContainer.Child = BackgroundScreenStack = new BackgroundScreenStack
                {
                    RelativeSizeAxes = Axes.Both
                };
        }

        public virtual void SetParallax(IScreen? next)
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