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
                    Colour = Color4.Red,
                    RelativeSizeAxes = Axes.Both
                },

                new SpriteText
                {
                    Y = 20,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                }
            };
        }

        #endregion
    }
}