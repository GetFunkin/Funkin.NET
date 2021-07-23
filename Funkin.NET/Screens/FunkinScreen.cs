using System;
using Funkin.NET.Screens.Background;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Screens;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Screens
{
    public abstract class FunkinScreen : Screen, IFunkinScreen, IHasDescription
    {
        /// <summary>
        ///     The amount of negative padding that should be applied to game background content which touches both the left and right sides of the screen. <br />
        ///     This allows for the game content to be pushed by the options/notification overlays without causing black areas to appear.
        /// </summary>
        public const float HorizontalOverflowPadding = 50;

        /// <summary>
        /// A user-facing title for this screen.
        /// </summary>
        public virtual string Title => GetType().Name;

        public string Description => Title;

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
        [CanBeNull] private BackgroundScreen _ownedBackground;

        [CanBeNull] private BackgroundScreen _background;

        [Resolved(canBeNull: true)]
        [CanBeNull]
        private BackgroundScreenStack BackgroundStack { get; set; }

        protected FunkinScreen()
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

            if (_background == null)
                throw new InvalidOperationException("Attempted to apply to background before screen is pushed.");

            _background.ApplyToBackground(action);
        }

        public override void OnEntering(IScreen last)
        {
            BackgroundStack?.Push(_ownedBackground = CreateBackground());

            _background = BackgroundStack?.CurrentScreen as BackgroundScreen;

            if (_background != _ownedBackground)
            {
                // background may have not been replaced, at which point we don't want to track the background lifetime.
                _ownedBackground?.Dispose();
                _ownedBackground = null;
            }

            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            if (base.OnExiting(next))
                return true;

            if (_ownedBackground != null && BackgroundStack?.CurrentScreen == _ownedBackground)
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