namespace GetFunkin.AdobeNecromancer
{
    public sealed class SubTexture
    {
        public string Name { get; }

        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        public int FrameX { get; }

        public int FrameY { get; }

        public int FrameWidth { get; }

        public int FrameHeight { get; }

        public SubTexture(string name, int x, int y, int width, int height, int frameX, int frameY, int frameWidth, int frameHeight)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            FrameX = frameX;
            FrameY = frameY;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
        }
    }
}