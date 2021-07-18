using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Content.Elements.Composites
{
    public class GirlfriendDanceTitle : CompositeDrawable, IBackgroundDependencyLoadable
    {
        [Resolved] private TextureStore Textures { get; set; }

        private readonly int[] _leftFrames = {0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14};
        private readonly int[] _rightFrames = {15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29};
        private TextureAnimation _leftAnim;
        private TextureAnimation _rightAnim;
        private bool _danceLeft;

        public void SwapAnimation()
        {
            if (Textures is null)
                return;

            ClearInternal(false);

            _danceLeft = !_danceLeft;

            _leftAnim.GotoFrame(0);
            _rightAnim.GotoFrame(0);

            AddInternal(_danceLeft ? _leftAnim : _rightAnim);
        }

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            _leftAnim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            _rightAnim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            foreach (int frame in _leftFrames)
                _leftAnim.AddFrame(Textures.Get($"Title/gfDance{frame}"), 1D / 24D * 1000D);

            foreach (int frame in _rightFrames)
                _rightAnim.AddFrame(Textures.Get($"Title/gfDance{frame}"), 1D / 24D * 1000D);
        }
    }
}