using Funkin.NET.Content.osu.Screens;
using Funkin.NET.Core.Resolving;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Content.Screens
{
    public abstract class FunkinScreen : OsuScreen, IRequestsResolvables
    {
        [Resolved] AudioManager IRequestsResolvables.ResolvableAudioManager { get; set; }

        public AudioManager ResolvedAudioManager => ((IRequestsResolvables) this).ResolvableAudioManager;

        [Resolved] TextureStore IRequestsResolvables.ResolvableTextureStore { get; set; }

        public TextureStore ResolvedTextureStore => ((IRequestsResolvables) this).ResolvableTextureStore;
    }
}