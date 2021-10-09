using System;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Intermediary.Screens.Backgrounds
{
    public abstract class BaseBackgroundScreen : Screen, IBackgroundScreen
    {
        public const float DefaultTransitionLength = 500f;
        public const float DefaultXMovementAmount = 50f;

        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        public virtual float TransitionLength { get; set; } = DefaultTransitionLength;

        public virtual float XMovementAmount { get; set; } = DefaultXMovementAmount;

        public virtual bool AnimateOnEnter { get; set; }

        protected BaseBackgroundScreen(bool animateOnEnter = true)
        {
            AnimateOnEnter = animateOnEnter;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override bool OnKeyDown(KeyDownEvent e) => false;

        protected override void Update()
        {
            base.Update();

            Scale = new Vector2(1f + XMovementAmount / DrawSize.X * 2f);
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
                this.MoveToX(0f, TransitionLength, Easing.OutExpo);

            base.OnResuming(last);
        }
        
        public void ApplyToBackground(Action<IBackgroundScreen> action) => Schedule(() => action.Invoke(this));

        public virtual bool Equals(IBackgroundScreen other) => other?.GetType() == GetType();

    }
}