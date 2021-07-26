using System;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Screens.Background
{
    /// <summary>
    ///     See: osu!'s BackgroundScreen.
    /// </summary>
    public abstract class BackgroundScreen : Screen, IEquatable<BackgroundScreen>
    {
        private readonly bool _animateOnEnter;

        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        protected BackgroundScreen(bool animateOnEnter = true)
        {
            _animateOnEnter = animateOnEnter;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public virtual bool Equals(BackgroundScreen other) => other?.GetType() == GetType();

        private const float TransitionLength = 500f;
        private const float XMovementAmount = 50f;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            // we don't want to handle escape key.
            return false;
        }

        /// <summary>
        /// Apply arbitrary changes to this background in a thread safe manner.
        /// </summary>
        /// <param name="action">The operation to perform.</param>
        public void ApplyToBackground(Action<BackgroundScreen> action) => Schedule(() => action.Invoke(this));

        protected override void Update()
        {
            base.Update();

            Scale = new Vector2(1f + XMovementAmount / DrawSize.X * 2f);
        }

        public override void OnEntering(IScreen last)
        {
            if (_animateOnEnter)
            {
                this.FadeOut();
                this.MoveToX(XMovementAmount);

                this.FadeIn(TransitionLength, Easing.InOutQuart);
                this.MoveToX(0, TransitionLength, Easing.InOutQuart);
            }

            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            this.MoveToX(-XMovementAmount, TransitionLength, Easing.InOutQuart);
            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            if (!IsLoaded)
                return base.OnExiting(next);

            this.FadeOut(TransitionLength, Easing.OutExpo);
            this.MoveToX(XMovementAmount, TransitionLength, Easing.OutExpo);

            return base.OnExiting(next);
        }

        public override void OnResuming(IScreen last)
        {
            if (IsLoaded)
                this.MoveToX(0, TransitionLength, Easing.OutExpo);

            base.OnResuming(last);
        }
    }
}