using Funkin.NET.Intermediary.Screens.Backgrounds;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Intermediary.Screens
{
    public abstract class DefaultScreen : Screen, IBackgroundProvider
    {
        /// <summary>
        ///     Whether this <see cref="DefaultScreen"/> allows the cursor to be displayed.
        /// </summary>
        public virtual bool CursorVisible { get; set; } = true;

        /// <summary>
        ///     Whether all overlays should be hidden when this screen is entered or resumed.
        /// </summary>
        public virtual bool HideOverlaysOnEnter { get; set; } = false;

        /// <summary>
        ///     The amount of parallax to be applied while this screen is displayed.
        /// </summary>
        public virtual float BackgroundParallaxAmount { get; set; } = 1f;

        public IBackgroundScreen? OwnedBackground { get; set; }

        public IBackgroundScreen? Background { get; set; }

        [Resolved(CanBeNull = true)] public BackgroundScreenStack? BackgroundStack { get; private set; }

        protected DefaultScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public override void OnEntering(IScreen last)
        {
            BackgroundStack?.Push(OwnedBackground = CreateBackground());

            Background = BackgroundStack?.CurrentScreen as IBackgroundScreen;

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

        public abstract IBackgroundScreen CreateBackground();
    }
}