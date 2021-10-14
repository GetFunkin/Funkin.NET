using Funkin.NET.Intermediary.Screens.Backgrounds;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Backgrounds
{
    public class SimpleBackgroundScreen : DefaultBackgroundScreen
    {
        public string TexturePath { get; }

        public SimpleBackgroundScreen(string texturePath)
        {
            TexturePath = texturePath;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            Texture = textures.Get(TexturePath);

            AddInternal(Background = new Background("SimpleBackground", Texture));
        }
    }
}