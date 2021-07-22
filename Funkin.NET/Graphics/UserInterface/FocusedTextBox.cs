using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK.Graphics;
using osuTK.Input;

namespace Funkin.NET.Graphics.UserInterface
{
    /// <summary>
    ///     See: osu!'s FocusedTextBox. <br />
    ///     A textbox which holds focus eagerly.
    /// </summary>
    public class FocusedTextBox : FunkinTextBox
    {
        private bool focus;

        private bool AllowImmediateFocus => host?.OnScreenKeyboardOverlapsGameWindow != true;

        public void TakeFocus()
        {
            if (AllowImmediateFocus)
                GetContainingInputManager().ChangeFocus(this);
        }

        public bool HoldFocus
        {
            get => AllowImmediateFocus && focus;
            set
            {
                focus = value;
                if (!focus && HasFocus)
                    base.KillFocus();
            }
        }

        [Resolved] private GameHost host { get; set; }

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
            if (!HasFocus) return false;

            return e.Key != Key.Escape && base.OnKeyDown(e);
        }

        public override bool RequestsFocus => HoldFocus;
    }
}