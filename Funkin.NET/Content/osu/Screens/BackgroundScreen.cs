using System;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Content.osu.Screens
{
    /// <summary>
    ///     osu!'s BackgroundScreen.
    /// </summary>
    public abstract class BackgroundScreen : Screen, IEquatable<BackgroundScreen>
    {
        public readonly bool AnimateOnEnter;

        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        protected BackgroundScreen(bool animateOnEnter = true)
        {
            AnimateOnEnter = animateOnEnter;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public virtual bool Equals(BackgroundScreen other)
        {
            return other?.GetType() == GetType();
        }

        public const float TransitionLength = 500;
        public const float XMovementAmount = 50;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            // we don't want to handle escape key.
            return false;
        }

        /// <summary>
        ///     Apply arbitrary changes to this background in a thread safe manner.
        /// </summary>
        /// <param name="action">The operation to perform.</param>
        public void ApplyToBackground(Action<BackgroundScreen> action) => Schedule(() => action.Invoke(this));

        protected override void Update()
        {
            base.Update();
            Scale = new Vector2(1 + XMovementAmount / DrawSize.X * 2);
        }

        public override void OnEntering(IScreen last)
        {
            if (AnimateOnEnter)
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