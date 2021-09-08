using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Utils;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.osuImpl.Graphics.Containers
{
    /// <summary>
    ///     See: osu!'s ParallaxContainer
    /// </summary>
    public class ParallaxContainer : Container, IRequireHighFrequencyMousePosition
    {
        public const float DefaultParallaxAmount = 0.02f;

        /// <summary>
        ///     The amount of parallax movement. Negative values will reverse the direction of parallax relative to user input.
        /// </summary>
        public float ParallaxAmount = DefaultParallaxAmount;
        
        private const float ParallaxDuration = 100;
        private readonly Container _content;
        private InputManager _input;

        public ParallaxContainer()
        {
            RelativeSizeAxes = Axes.Both;

            AddInternal(_content = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        protected override Container<Drawable> Content => _content;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            _input = GetContainingInputManager();
        }

        protected override void Update()
        {
            base.Update();

            Vector2 offset = Vector2.Zero;

            if (_input.CurrentState.Mouse != null)
            {
                Vector2 sizeDiv2 = DrawSize / 2f;

                Vector2 relativeAmount = ToLocalSpace(_input.CurrentState.Mouse.Position) - sizeDiv2;

                const float baseFactor = 0.999f;

                relativeAmount.X = (float)(Math.Sign(relativeAmount.X) *
                                           Interpolation.Damp(0, 1, baseFactor, Math.Abs(relativeAmount.X)));
                relativeAmount.Y = (float)(Math.Sign(relativeAmount.Y) *
                                           Interpolation.Damp(0, 1, baseFactor, Math.Abs(relativeAmount.Y)));

                offset = relativeAmount * sizeDiv2 * ParallaxAmount;
            }

            double elapsed = Math.Clamp(Clock.ElapsedFrameTime, 0, ParallaxDuration);

            _content.Position = Interpolation.ValueAt(elapsed, _content.Position, offset, 0, ParallaxDuration,
                Easing.OutQuint);
            _content.Scale = Interpolation.ValueAt(elapsed, _content.Scale,
                new Vector2(1 + Math.Abs(ParallaxAmount)), 0, 1000, Easing.OutQuint);
        }
    }
}