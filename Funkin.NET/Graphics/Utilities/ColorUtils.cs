using osuTK.Graphics;

namespace Funkin.NET.Graphics.Utilities
{
    public static class ColorUtils
    {
        public static Color4 Gray(float floatAmount) => new(floatAmount, floatAmount, floatAmount, 1f);

        public static Color4 Gray(byte byteAmount) => new(byteAmount, byteAmount, byteAmount, byte.MaxValue);
    }
}