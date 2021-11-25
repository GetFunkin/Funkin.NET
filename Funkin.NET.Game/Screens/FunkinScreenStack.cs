using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;

namespace Funkin.NET.Game.Screens
{
    public class FunkinScreenStack : ScreenStack
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new Box
            {
                Colour = Colour4.Black
            });
        }
    }
}
