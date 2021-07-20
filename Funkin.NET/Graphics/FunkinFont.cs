using osu.Framework.Graphics.Sprites;

namespace Funkin.NET.Graphics
{
    /// <summary>
    ///     Basically just osu!'s OsuFont with some changes.
    /// </summary>
    public class FunkinFont
    {
        public const float DefaultSize = 20;

        public static FontUsage Default => GetFont();

        public static FontUsage Vcr => GetFont(Typeface.VCR);

        public static FontUsage Funkin => GetFont(Typeface.Funkin);

        public static FontUsage GetFont(Typeface typeface = Typeface.Torus, float size = DefaultSize,
            string weightString = null, bool italics = false, bool fixedWidth = false) =>
            new(typeface.ToString(), size, weightString, italics, fixedWidth);

        public enum Typeface
        {
            Torus,
            // ReSharper disable once InconsistentNaming
            VCR,
            Funkin
        }
    };
}