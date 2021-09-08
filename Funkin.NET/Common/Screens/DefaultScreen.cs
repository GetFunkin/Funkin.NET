using System;
using Funkin.NET.Common.Screens.Background;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Common.Screens
{
    /// <summary>
    ///     Base screen used for screens in Funkin.NET. Provides extra features and functionality.
    /// </summary>
    public abstract class DefaultScreen : Screen, IDefaultScreen
    {
        /// <summary>
        /// A user-facing title for this screen.
        /// </summary>
        public virtual string Title => GetType().Name;

        /// <summary>
        ///     Whether all overlays should be hidden when this screen is entered or resumed.
        /// </summary>
        public virtual bool HideOverlaysOnEnter => false;

        public virtual bool CursorVisible => true;

        protected new FunkinGame Game => base.Game as FunkinGame;

        public virtual float BackgroundParallaxAmount => 1;

        /// <summary>
        ///     The background created and owned by this screen. May be null if the background didn't change.
        /// </summary>
        [CanBeNull] private BackgroundScreen OwnedBackground;

        [CanBeNull] private BackgroundScreen Background;

        [Resolved(canBeNull: true)]
        [CanBeNull]
        private BackgroundScreenStack BackgroundStack { get; set; }

        protected DefaultScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        /// <summary>
        ///     Apply arbitrary changes to the current background screen in a thread safe manner.
        /// </summary>
        /// <param name="action">The operation to perform.</param>
        public void ApplyToBackground(Action<BackgroundScreen> action)
        {
            if (BackgroundStack == null)
                throw new InvalidOperationException(
                    "Attempted to apply to background without a background stack being available.");

            if (Background == null)
                throw new InvalidOperationException("Attempted to apply to background before screen is pushed.");

            Background.ApplyToBackground(action);
        }

        public override void OnEntering(IScreen last)
        {
            BackgroundStack?.Push(OwnedBackground = CreateBackground());

            Background = BackgroundStack?.CurrentScreen as BackgroundScreen;

            if (Background != OwnedBackground)
            {
                // background may have not been replaced, at which point we don't want to track the background lifetime.
                OwnedBackground?.Dispose();
                OwnedBackground = null;
            }

            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            if (base.OnExiting(next))
                return true;

            if (OwnedBackground != null && BackgroundStack?.CurrentScreen == OwnedBackground)
                BackgroundStack?.Exit();

            return false;
        }

        public override void OnSuspending(IScreen next)
        {
            this.FadeOut(300D);
            base.OnSuspending(next);
        }

        /// <summary>
        ///     Override to create a BackgroundMode for the current screen. <br />
        ///     Note that the instance created may not be the used instance if it matches the BackgroundMode equality clause.
        /// </summary>
        protected virtual BackgroundScreen CreateBackground() => new BackgroundScreenDefault();
    }
}