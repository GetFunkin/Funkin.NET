using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Transforms;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.osuImpl.Graphics.Backgrounds
{
    public class Background : CompositeDrawable, IEquatable<Background>
    {
        public const float BlurScale = 0.5f;

        public Sprite Sprite { get; }

        public string SpriteName { get; }

        public BufferedContainer BufferedContainer { get; protected set; }

        public Background(string spriteName = "")
        {
            SpriteName = spriteName;
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

        public Vector2 BlurSigma => BufferedContainer?.BlurSigma / BlurScale ?? Vector2.Zero;

        /// <summary>
        ///     Smoothly adjusts <see cref="IBufferedContainer.BlurSigma"/> over time.
        /// </summary>
        /// <returns>A <see cref="TransformSequence{T}"/> to which further transforms can be added.</returns>
        public void BlurTo(Vector2 newBlurSigma, double duration = 0, Easing easing = Easing.None)
        {
            if (BufferedContainer == null && newBlurSigma != Vector2.Zero)
            {
                RemoveInternal(Sprite);

                AddInternal(BufferedContainer = new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    CacheDrawnFrameBuffer = true,
                    RedrawOnScale = false,
                    Child = Sprite
                });
            }

            if (BufferedContainer != null)
                BufferedContainer.FrameBufferScale =
                    newBlurSigma == Vector2.Zero ? Vector2.One : new Vector2(BlurScale);

            BufferedContainer?.BlurTo(newBlurSigma * BlurScale, duration, easing);
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