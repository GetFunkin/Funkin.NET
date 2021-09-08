using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK;

namespace Funkin.NET.osuImpl.Graphics.Cursor
{
    /// <summary>
    ///     See: osu!'s OsuTooltipContainer.
    /// </summary>
    public class DefaultTooltipContainer : TooltipContainer
    {
        public DefaultTooltipContainer(CursorContainer cursor) : base(cursor)
        {
        }

        protected override ITooltip CreateTooltip() => new FunkinTooltip();

        // reduce appear delay is tooltip is alr partly visible
        protected override double AppearDelay => (1f - CurrentTooltip.Alpha) * base.AppearDelay;

        /// <summary>
        ///     See: osu!'s OsuTooltipContainer.OsuTooltip.
        /// </summary>
        public class FunkinTooltip : Tooltip
        {
            private readonly Box _background;
            private readonly SpriteText _text;
            private bool _instantMovement = true;

            public FunkinTooltip()
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
                    _background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.9f
                    },

                    _text = new SpriteText
                    {
                        Padding = new MarginPadding(5f)
                    }
                };
            }

            public override bool SetContent(object content)
            {
                if (content is not LocalisableString contentString)
                    return false;

                if (contentString == _text.Text)
                    return true;

                if (IsPresent)
                {
                    AutoSizeDuration = 250f;
                    _background.FlashColour(new Colour4(0.4f, 0.4f, 0.4f, 1f), 1000D, Easing.OutQuint);
                }
                else
                    AutoSizeDuration = 0;

                return true;
            }

            protected override void PopIn()
            {
                _instantMovement |= !IsPresent;

                this.FadeIn(500D, Easing.OutQuint);
            }

            public override void Move(Vector2 pos)
            {
                if (_instantMovement)
                {
                    Position = pos;
                    _instantMovement = false;
                }
                else
                    this.MoveTo(pos, 200D, Easing.OutQuint);
            }

            [BackgroundDependencyLoader]
            private void Load()
            {
                _background.Colour = Color4Extensions.FromHex("333");
            }
        }
    }
}