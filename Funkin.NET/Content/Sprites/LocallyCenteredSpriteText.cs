using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Funkin.NET.Content.Sprites
{
    public class LocallyCenteredSpriteText : SpriteText
    {
        public Vector2 OffCenterPosition { get; set; }

        protected override void Update()
        {
            base.Update();

            Position = OffCenterPosition - new Vector2((Width / 2f) - Padding.Left - Padding.Right, 0f);
        }
    }
}