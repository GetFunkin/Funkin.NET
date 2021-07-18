using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace Funkin.NET.Content.Screens
{
    public class MainScreen : Screen, IBackgroundDependencyLoadable
    {
        #region BackgroundDependencyLoader

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both
                },

                new SpriteText
                {
                    Y = 20,
                    Text = "MAIN SCREEN",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = new FontUsage("VCR", 40f)
                }
            };
        }

        #endregion
    }
}