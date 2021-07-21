using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Transforms;
using osuTK;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Graphics.Backgrounds
{
    /// <summary>
    ///     See: osu!'s Background. <br />
    ///     A background which offers blurring via a <see cref="BufferedContainer"/> on demand.
    /// </summary>
    public class Background : CompositeDrawable, IEquatable<Background>
    {
        private const float BlurScale = 0.5f;

        public Sprite Sprite;
        public readonly string SpriteName;

        private BufferedContainer _bufferedContainer;

        public Background(string textureName = @"")
        {
            SpriteName = textureName;
            RelativeSizeAxes = Axes.Both;

            AddInternal(Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill
            });
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            Sprite.Texture = textures.Get(SpriteName);
        }

        public Vector2 BlurSigma => _bufferedContainer?.BlurSigma / BlurScale ?? Vector2.Zero;

        /// <summary>
        ///     Smoothly adjusts <see cref="IBufferedContainer.BlurSigma"/> over time.
        /// </summary>
        /// <returns>A <see cref="TransformSequence{T}"/> to which further transforms can be added.</returns>
        public void BlurTo(Vector2 newBlurSigma, double duration = 0, Easing easing = Easing.None)
        {
            if (_bufferedContainer == null && newBlurSigma != Vector2.Zero)
            {
                RemoveInternal(Sprite);

                AddInternal(_bufferedContainer = new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    CacheDrawnFrameBuffer = true,
                    RedrawOnScale = false,
                    Child = Sprite
                });
            }

            if (_bufferedContainer != null)
                _bufferedContainer.FrameBufferScale =
                    newBlurSigma == Vector2.Zero ? Vector2.One : new Vector2(BlurScale);

            _bufferedContainer?.BlurTo(newBlurSigma * BlurScale, duration, easing);
        }

        public virtual bool Equals(Background other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return other.GetType() == GetType()
                   && other.SpriteName == SpriteName;
        }
    }
}