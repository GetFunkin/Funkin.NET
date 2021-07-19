using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Core.Resolving
{
    public interface IRequestsResolvables
    {
        [Resolved] AudioManager ResolvableAudioManager { get; set; }

        AudioManager ResolvedAudioManager { get; }

        [Resolved] TextureStore ResolvableTextureStore { get; set; }

        TextureStore ResolvedTextureStore { get; }
    }
}