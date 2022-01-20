using System;
using Funkin.NET.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Funkin.NET.Game.Screens
{
    public abstract class FunkinScreen : Screen, IHasDescription
    {
        /// <summary>
        ///     The amount of negative padding that should be applied to the game background content which touches both the left and right sides of the screen. <br />
        ///     This allows for the game content to be pushed by the options/notification overlays without causing black areas to appear.
        /// </summary>
        public const float HORIZONTAL_OVERFLOW_PADDING = 50f;

        /// <summary>
        ///     A user-facing title for this screen.
        /// </summary>
        public virtual string Title => GetType().Name;

        string IHasDescription.Description => Title;

        public virtual bool AllowBackButton => true;

        public virtual bool AllowExternalScreenChange => false;

        /// <summary>
        ///     Whether all overlays should be hidden when this screen is entered or resumed.
        /// </summary>
        public virtual bool HideOverlaysOnEnter => false;

        /// <summary>
        ///     The initial overlay activation mod eto use when this screen is entered for the first time.
        /// </summary>
        protected virtual OverlayActivation InitialOverlayAcivationMode => OverlayActivation.All;

        protected readonly Bindable<OverlayActivation> OverlayActivationMode;

        public virtual bool CursorVisibile => true;

        protected new FunkinGameBase Game => base.Game as FunkinGameBase ?? throw new InvalidOperationException();

        public virtual float BackgroundParallaxAmount => 1;

        /// <summary>
        ///     The background created and owned by this screen. May be <see langword="null"/> if the background didn't change.
        /// </summary>
        protected BackgroundScreen? OwnedBackground;

        protected BackgroundScreen? Background;

        [Resolved(canBeNull: true)]
        protected BackgroundScreenStack? BackgroundStack { get; private set; }

        protected FunkinScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            OverlayActivationMode = new Bindable<OverlayActivation>(InitialOverlayAcivationMode);
        }

        /// <summary>
        ///     Apply arbitrary changes to the current background screen in a thread-safe manner.
        /// </summary>
        /// <param name="action">The operation to perform.</param>
        /// <exception cref="InvalidOperationException">Invocation prior to the screen being pushed or while there is no provided background stack.</exception>
        public virtual void ApplyToBackground(Action<BackgroundScreen> action)
        {
            if (BackgroundStack is null)
                throw new InvalidOperationException("Attempted to apply to background without a background stack being available.");

            if (Background is null)
                throw new InvalidOperationException("Attempted to apply to background before screen is pushed.");

            Background.ApplyToBackground(action);
        }

        public override void OnEntering(IScreen last)
        {
            if (BackgroundStack?.Push(OwnedBackground = CreateBackground()) != true)
            {
                // If the constructed instance was not actually pushed to the background stack, we don't want to track it unnecessarily.
                OwnedBackground?.Dispose();
                OwnedBackground = null;
            }

            Background = BackgroundStack?.CurrentScreen as BackgroundScreen;

            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            if (base.OnExiting(next))
                return true;

            if (OwnedBackground is not null && BackgroundStack?.CurrentScreen == OwnedBackground)
                BackgroundStack?.Exit();

            return false;
        }

        protected virtual BackgroundScreen? CreateBackground() => null;

        protected virtual bool OnBackButton() => false;
    }
}
