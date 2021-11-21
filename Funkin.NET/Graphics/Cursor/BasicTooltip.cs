using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Graphics.Utilities;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osuTK;

namespace Funkin.NET.Graphics.Cursor
{
    public class BasicTooltip : TooltipContainer.Tooltip
    {
        public Box Background;
        public BasicSpriteText Text;
        public bool InstantMovement = true;

        public BasicTooltip()
        {
            AutoSizeEasing = Easing.OutQuint;

            CornerRadius = 5;
            Masking = true;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Colour = Colour4.Black.Opacity(40),
                Radius = 5
            };

            Children = new Drawable[]
            {
                Background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.9f,
                    Colour = Color4Extensions.FromHex("333")
                },

                Text = new BasicSpriteText
                {
                    Padding = new MarginPadding(5f),
                    Font = FunkinFont.GetFont(weight: FunkinFont.FontWeight.Regular)
                }
            };
        }

        public override void SetContent(LocalisableString content)
        {
            if (Text.Text == content)
                return;

            Text.Text = content;

            if (IsPresent)
            {
                AutoSizeDuration = 250;
                Background.FlashColour(ColorUtils.Gray(0.4f), 1000D, Easing.OutQuint);
            }
            else
                AutoSizeDuration = 0;
        }

        protected override void PopIn()
        {
            InstantMovement |= !IsPresent;
            this.FadeIn(500D, Easing.OutQuint);
        }

        protected override void PopOut() => this.Delay(150D).FadeOut(500D, Easing.OutQuint);

        public override void Move(Vector2 pos)
        {
            if (InstantMovement)
            {
                Position = pos;
                InstantMovement = false;
            }
            else
            {
                this.MoveTo(pos, 200D, Easing.OutQuint);
            }
        }
    }
}