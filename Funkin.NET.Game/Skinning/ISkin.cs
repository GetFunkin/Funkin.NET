using Funkin.NET.Game.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Game.Skinning
{
    /// <summary>
    ///     Provides access to skinnable elements.
    /// </summary>
    public interface ISkin
    {
        /// <summary>
        ///     Retrieve a <see cref="Drawable"/> component implementation.
        /// </summary>
        /// <param name="component">The requested component.</param>
        /// <returns>
        ///     A drawable representation for the requested component, or null if unavailable.
        /// </returns>
        Drawable? GetDrawableComponent(ISkinComponent component);

        /// <summary>
        ///     Retrieve a <see cref="Texture"/>/
        /// </summary>
        /// <param name="componentName">The requested texture.</param>
        /// <returns>
        ///     A matching texture, or <see langword="null"/> if unavailable.
        /// </returns>
        Texture? GetTexture(string componentName);

        /// <summary>
        ///     Retrieve a <see cref="Texture"/>.
        /// </summary>
        /// <param name="componentName">The requested texture.</param>
        /// <param name="warpModeS">The texture wrap mode in horizontal direction.</param>
        /// <param name="wrapModeT">The texture wrap mode in vertical direction.</param>
        /// <returns>
        ///     A matching texture, or <see langword="null"/> if unavailable.
        /// </returns>
        Texture? GetTexture(string componentName, WrapMode warpModeS, WrapMode wrapModeT);

        /// <summary>
        ///     Retrieve a <see cref="SampleChannel"/>.
        /// </summary>
        /// <param name="sampleInfo">The requested sample.</param>
        /// <returns>
        ///     A requested sample channel, or <see langword="null"/> if unavailable.
        /// </returns>
        ISample? GetSample(ISampleInfo sampleInfo);
    }
}
