using osu.Framework.Graphics.Sprites;

namespace Funkin.NET.Graphics
{
    public class FunkinFont
    {
        public const float DefaultSize = 20;

        public static FontUsage Default => GetFont();

        public static FontUsage Vcr => GetFont("VCR");

        public static FontUsage Funkin => GetFont("Funkin");

        public static FontUsage GetFont(string typeface = "Torus-Regular", float size = DefaultSize,
            string weightString = null, bool italics = false, bool fixedWidth = false) =>
            new(typeface, size, weightString, italics, fixedWidth);
    };
}