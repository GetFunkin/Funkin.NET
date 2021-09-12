using System;
using Funkin.NET.Intermediary.Screens.Backgrounds;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Common.Screens.Backgrounds
{
    public class DefaultBackgroundScreen : BaseBackgroundScreen
    {
        public Background Background;

        public DefaultBackgroundType BackgroundType { get; }

        public DefaultBackgroundScreen(DefaultBackgroundType backgroundType)
        {
            BackgroundType = backgroundType;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            Texture texture = BackgroundType switch
            {
                DefaultBackgroundType.Yellow => textures.Get(PathHelper.Texture.MenuBackground1),
                DefaultBackgroundType.Purple => textures.Get(PathHelper.Texture.MenuBackground2),
                DefaultBackgroundType.Red => textures.Get(PathHelper.Texture.MenuBackground3),
                _ => throw new NullReferenceException()
            };

            string name = BackgroundType switch
            {
                DefaultBackgroundType.Yellow => "Yellow",
                DefaultBackgroundType.Purple => "Purple",
                DefaultBackgroundType.Red => "Red",
                _ => throw new NullReferenceException()
            };

            AddInternal(Background = new Background(name, texture));
        }
    }
}