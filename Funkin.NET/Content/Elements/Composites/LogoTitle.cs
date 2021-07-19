using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;

namespace Funkin.NET.Content.Elements.Composites
{
    public class LogoTitle : FunkinCompositeDrawable, IBackgroundDependencyLoadable
    {
        public TextureAnimation Anim;

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            Anim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            for (int i = 0; i < 15; i++)
                Anim.AddFrame(ResolvedTextureStore.Get($"Title/logo bumpin{i}"), 1D / 24D * 1000D);

            AddInternal(Anim);
        }
    }
}