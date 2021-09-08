using Funkin.NET.Core.Graphics.Textures;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Game.Graphics.Composites
{
    /// <summary>
    ///     Animation for the girlfriend as she appears on the title screen.
    /// </summary>
    public class GirlfriendDanceTitle : CompositeDrawable
    {
        public static readonly int[] LeftFrames = {0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14};
        public static readonly int[] RightFrames = {15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29};
        public TextureAnimation LeftAnim;
        public TextureAnimation RightAnim;
        public bool IsDancingLeft;

        [BackgroundDependencyLoader]
        private void Load(SparrowAtlasStore textures)
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

            Texture Get(string name) => textures.GetTexture("Textures/Shared/gfDanceTitle.xml", name);

            foreach (int frame in LeftFrames)
                LeftAnim.AddFrame(Get($"gfDance{PathHelper.FrameAsString(frame)}"), 1D / 24D * 1000D);

            foreach (int frame in RightFrames)
                RightAnim.AddFrame(Get($"gfDance{PathHelper.FrameAsString(frame)}"), 1D / 24D * 1000D);
        }

        public void SwapAnimation()
        {
            if (LeftAnim is null || RightAnim is null)
                return;

            ClearInternal(false);

            IsDancingLeft = !IsDancingLeft;

            LeftAnim.GotoFrame(0);
            RightAnim.GotoFrame(0);

            AddInternal(IsDancingLeft ? LeftAnim : RightAnim);
        }
    }
}