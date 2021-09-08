using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK.Graphics;
using osuTK.Input;

namespace Funkin.NET.osuImpl.Graphics.UserInterface
{
    /// <summary>
    ///     See: osu!'s FocusedTextBox. <br />
    ///     A textbox which holds focus eagerly.
    /// </summary>
    public class FocusedTextBox : FunkinTextBox
    {
        public bool Focused;

        public bool AllowImmediateFocus => Host?.OnScreenKeyboardOverlapsGameWindow != true;

        public override bool RequestsFocus => HoldFocus;

        public void TakeFocus()
        {
            if (AllowImmediateFocus)
                GetContainingInputManager().ChangeFocus(this);
        }

        public bool HoldFocus
        {
            get => AllowImmediateFocus && Focused;

            set
            {
                Focused = value;

                if (!Focused && HasFocus)
                    base.KillFocus();
            }
        }

        [Resolved] private GameHost Host { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            BackgroundUnfocused = new Color4(10, 10, 10, 255);
            BackgroundFocused = new Color4(10, 10, 10, 255);
        }

        // We may not be focused yet, but we need to handle keyboard input to be able to request focus
        public override bool HandleNonPositionalInput => HoldFocus || base.HandleNonPositionalInput;

        protected override void OnFocus(FocusEvent e)
        {
            base.OnFocus(e);
            BorderThickness = 0;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!HasFocus)
                return false;

            return e.Key != Key.Escape && base.OnKeyDown(e);
        }
    }
}