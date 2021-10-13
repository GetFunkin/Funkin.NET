using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Utils;
using osuTK;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Intermediary.Graphics.Containers
{
    /// <summary>
    ///     Parallax container based on the cursor position.
    /// </summary>
    public class ParallaxContainer : Container, IRequireHighFrequencyMousePosition
    {
        public const float DefaultParallaxAmount = 0.02f;
        public const float DefaultParallaxDuration = 100;

        /// <summary>
        ///     The amount of parallax movement. Negative values are identical to positive values, save for moving in reverse.
        /// </summary>
        public float ParallaxAmount = DefaultParallaxAmount;

        /// <summary>
        ///     Maximum elapsed time.
        /// </summary>
        public float ParallaxDuration = DefaultParallaxDuration;

        protected readonly Container ContainerContent;
        protected InputManager? InputManager;

        protected override Container<Drawable> Content => ContainerContent;

        public ParallaxContainer()
        {
            RelativeSizeAxes = Axes.Both;

            AddInternal(ContainerContent = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            InputManager = GetContainingInputManager();
        }

        protected override void Update()
        {
            base.Update();

            Vector2 offset = Vector2.Zero;

            if (InputManager?.CurrentState.Mouse is not null)
            {
                Vector2 sizeDiv2 = DrawSize / 2f;
                Vector2 relativeAmount = ToLocalSpace(InputManager.CurrentState.Mouse.Position) - sizeDiv2;

                const float baseFactor = 0.999f;
                int xSign = Math.Sign(relativeAmount.X);
                int ySign = Math.Sign(relativeAmount.Y);
                double xDamp = Interpolation.Damp(0, 1, baseFactor, Math.Abs(relativeAmount.X));
                double yDamp = Interpolation.Damp(0, 1, baseFactor, Math.Abs(relativeAmount.Y));

                relativeAmount.X = (float) (xSign * xDamp);
                relativeAmount.Y = (float) (ySign * yDamp);

                offset = relativeAmount * sizeDiv2 * ParallaxAmount;
            }

            double elapsed = Math.Clamp(Clock.ElapsedFrameTime, 0D, ParallaxDuration);

            Vector2 pos = Interpolation.ValueAt(elapsed, ContainerContent.Position,
                offset, 0D, ParallaxDuration, Easing.OutQuint);

            Vector2 scale = Interpolation.ValueAt(elapsed, ContainerContent.Scale,
                new Vector2(1f + Math.Abs(ParallaxAmount)), 0D, 1000D, Easing.OutQuint);

            ContainerContent.Position = pos;
            ContainerContent.Scale = scale;
        }
    }
}