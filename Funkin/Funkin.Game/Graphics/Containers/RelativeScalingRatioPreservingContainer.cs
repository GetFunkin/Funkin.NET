using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Funkin.Game.Graphics.Containers
{
    public class RelativeScalingRatioPreservingContainer : Container
    {
        private readonly Container content;

        protected override Container<Drawable> Content => content;

        public Vector2 TargetDrawSize = new(1024f, 768f);

        public RelativeScalingRatioPreservingContainer()
        {
            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            });

            RelativeSizeAxes = Axes.Both;
        }

        protected override void Update()
        {
            base.Update();

            float ratio = TargetDrawSize.X / TargetDrawSize.Y;
            float realRatio = Parent.DrawWidth / Parent.DrawHeight;

            bool scaleY = realRatio < ratio;

            Vector2 ratioSize = scaleY ? new Vector2(Parent.DrawWidth, MathF.Floor(Parent.DrawWidth / ratio)) : new Vector2(MathF.Floor(Parent.DrawHeight * ratio), Parent.DrawHeight);
            content.Size = Vector2.Divide(ratioSize, Parent.DrawSize);
        }
    }
}
