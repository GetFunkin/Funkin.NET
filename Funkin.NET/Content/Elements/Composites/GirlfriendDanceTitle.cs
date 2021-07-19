using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;

namespace Funkin.NET.Content.Elements.Composites
{
    public class GirlfriendDanceTitle : FunkinCompositeDrawable, IBackgroundDependencyLoadable
    {
        public static readonly int[] LeftFrames = {0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14};
        public static readonly int[] RightFrames = {15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29};
        public TextureAnimation LeftAnim;
        public TextureAnimation RightAnim;
        public bool IsDancingLeft;

        public void SwapAnimation()
        {
            ClearInternal(false);

            IsDancingLeft = !IsDancingLeft;

            LeftAnim.GotoFrame(0);
            RightAnim.GotoFrame(0);

            AddInternal(IsDancingLeft ? LeftAnim : RightAnim);
        }

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            LeftAnim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            RightAnim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            foreach (int frame in LeftFrames)
                LeftAnim.AddFrame(ResolvedTextureStore.Get($"Title/gfDance{frame}"), 1D / 24D * 1000D);

            foreach (int frame in RightFrames)
                RightAnim.AddFrame(ResolvedTextureStore.Get($"Title/gfDance{frame}"), 1D / 24D * 1000D);
        }
    }
}