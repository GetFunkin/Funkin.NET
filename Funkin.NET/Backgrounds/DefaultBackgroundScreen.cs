using Funkin.NET.Intermediary.Screens.Backgrounds;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Backgrounds
{
    public abstract class DefaultBackgroundScreen : BaseBackgroundScreen
    {
        public Background? Background { get; set; }

        public Texture? Texture { get; set; }
    }
}