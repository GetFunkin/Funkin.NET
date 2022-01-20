using System;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET.Game.Screens
{
    public abstract class BackgroundScreen : Screen, IEquatable<BackgroundScreen>
    {
        public const double TRANSITION_LENGTH = 500D;
        public const float X_MOVEMENT_AMOUNT = 50f;

        protected readonly bool animateOnEnter;

        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        protected BackgroundScreen(bool animateOnEnter = true)
        {
            this.animateOnEnter = animateOnEnter;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public virtual bool Equals(BackgroundScreen? other) => other?.GetType() == GetType();

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            // Refuse to ever handle keys.
            return false;
        }

        /// <summary>
        ///     Apply arbitrary changes to this background in a thread-safe manner.
        /// </summary>
        /// <param name="action">The operation to perform.</param>
        public virtual void ApplyToBackground(Action<BackgroundScreen> action) => Schedule(() => action.Invoke(this));

        protected override void Update()
        {
            base.Update();

            Scale = new Vector2(1f + X_MOVEMENT_AMOUNT / DrawSize.X * 2f);
        }

        public override void OnEntering(IScreen last)
        {
            if (animateOnEnter)
            {
                this.FadeOut();
                this.MoveToX(X_MOVEMENT_AMOUNT);

                this.FadeIn(TRANSITION_LENGTH, Easing.InOutQuart);
                this.MoveToX(0f, TRANSITION_LENGTH, Easing.InOutQuart);
            }

            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            this.MoveToX(-X_MOVEMENT_AMOUNT, TRANSITION_LENGTH, Easing.InOutQuart);
            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            if (!IsLoaded)
                return base.OnExiting(next);

            this.FadeOut(TRANSITION_LENGTH, Easing.OutExpo);
            this.MoveToX(X_MOVEMENT_AMOUNT, TRANSITION_LENGTH, Easing.OutExpo);

            return base.OnExiting(next);
        }

        public override void OnResuming(IScreen last)
        {
            if (IsLoaded)
                this.MoveToX(0f, TRANSITION_LENGTH, Easing.OutExpo);

            base.OnResuming(last);
        }
    }
}
