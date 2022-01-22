using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Funkin.Game.Graphics.Containers
{
    public class RelativeScalingRatioPreservingContainer : Container
    {
        private readonly Container content;

        protected override Container<Drawable> Content => content;

        public Vector2 TargetDrawSize = new(1920f, 1080f);

        public RelativeScalingRatioPreservingContainer()
        {
            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.Both,
            });

            RelativeSizeAxes = Axes.Both;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
