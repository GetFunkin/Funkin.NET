﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Game.Graphics.Composites
{
    /// <summary>
    ///     Animation for the logo as it appears on the title screen.
    /// </summary>
    public class LogoTitle : CompositeDrawable
    {
        public TextureAnimation Anim;
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            Anim = new TextureAnimation
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.Centre,
                IsPlaying = true
            };

            for (int i = 0; i < 15; i++)
                Anim.AddFrame(textures.Get($"Title/logo bumpin{i}"), 1D / 24D * 1000D);

            AddInternal(Anim);
        }
    }
}