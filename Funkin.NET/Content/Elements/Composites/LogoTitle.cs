using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Content.Elements.Composites
{
    public class LogoTitle : CompositeDrawable
    {
        public TextureAnimation Anim;

        [Resolved] private TextureStore Textures { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Anim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            for (int i = 0; i < 15; i++)
                Anim.AddFrame(Textures.Get($"Title/logo bumpin{i}"), 1D / 24D * 1000D);

            AddInternal(Anim);
        }
    }
}