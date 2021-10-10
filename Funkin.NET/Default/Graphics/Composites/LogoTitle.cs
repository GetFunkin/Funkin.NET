using Funkin.NET.Intermediary.ResourceStores;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Default.Graphics.Composites
{
    /// <summary>
    ///     Animation for the logo as it appears on the title screen.
    /// </summary>
    public class LogoTitle : CompositeDrawable
    {
        public TextureAnimation Anim;
        
        [BackgroundDependencyLoader]
        private void Load(SparrowAtlasStore textures)
        {
            Anim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true,
                AlwaysPresent = true
            };

            Texture Get(string name) => textures.GetTexture(PathHelper.Texture.LogoBumpinXml, name);

            for (int i = 0; i < 15; i++)
                Anim.AddFrame(Get($"logo bumpin{PathHelper.Atlas.FrameAsString(i)}"), 1D / 24D * 1000D);

            AddInternal(Anim);
        }
    }
}