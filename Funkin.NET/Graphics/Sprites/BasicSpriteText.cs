using Funkin.NET.Graphics.Utilities;
using osu.Framework.Graphics.Sprites;

namespace Funkin.NET.Graphics.Sprites
{
    public class BasicSpriteText : SpriteText
    {
        public BasicSpriteText()
        {
            Shadow = true;
            Font = FunkinFont.DefaultFont;
        }
    }
}