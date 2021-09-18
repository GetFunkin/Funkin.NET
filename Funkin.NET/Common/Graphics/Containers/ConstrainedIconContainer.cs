using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Funkin.NET.Common.Graphics.Containers
{
    public class ConstrainedIconContainer : CompositeDrawable
    {
        public Drawable Icon
        {
            get => InternalChild;

            set => InternalChild = value;
        }

        public ConstrainedIconContainer()
        {
            Masking = true;
        }

        protected override void Update()
        {
            base.Update();

            if (InternalChildren.Count <= 0 || !(InternalChild.DrawSize.X > 0))
                return;

            // We're modifying scale here for a few reasons
            // - Guarantees correctness if BorderWidth is being used
            // - If we were to use RelativeSize/FillMode, we'd need to set the Icon's RelativeSizeAxes directly.
            //   We can't do this because we would need access to AutoSizeAxes to set it to none.
            //   Other issues come up along the way too, so it's not a good solution.
            float xMin = DrawSize.X / InternalChild.DrawSize.X;
            float yMin = DrawSize.Y / InternalChild.DrawSize.Y;
            float fitScale = Math.Min(xMin, yMin);

            InternalChild.Scale = new Vector2(fitScale);
            InternalChild.Anchor = InternalChild.Origin = Anchor.Centre;
        }
    }
}