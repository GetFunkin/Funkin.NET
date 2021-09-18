using System;
using Funkin.NET.Common.Utilities;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace Funkin.NET.Common.Graphics.Containers
{
    public abstract class DefaultFocusedOverlayContainer : FocusedOverlayContainer
    {
        public Sample OpenSample { get; protected set; }

        public bool CloseOnMouseUp { get; protected set; }

        protected override bool BlockScrollInput => false;

        protected override bool BlockNonPositionalInput => true;

        /// <summary>
        ///     Whether mouse input should be blocked screen-wide while this overlay is visible. <br />
        ///     Performing mouse actions outside of the valid extents will hide the overlay.
        /// </summary>
        public virtual bool BlockScreenWideMouse => BlockPositionalInput;

        [Resolved] private FunkinGame ResolvableGame { get; set; }

        public FunkinGame ResolvedGame => ResolvableGame;

        //Receive input outside our bounds so we can trigger a close event on ourselves.
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) =>
            BlockScreenWideMouse || base.ReceivePositionalInputAt(screenSpacePos);

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            CloseOnMouseUp = !base.ReceivePositionalInputAt(e.ScreenSpaceMousePosition);

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (CloseOnMouseUp && !base.ReceivePositionalInputAt(e.ScreenSpaceMousePosition))
                Hide();

            base.OnMouseUp(e);
        }

        protected override void UpdateState(ValueChangedEvent<Visibility> state)
        {
            bool didChange = state.NewValue != state.OldValue;

            switch (state.NewValue)
            {
                case Visibility.Visible:
                    if (didChange)
                        OpenSample.Play();

                    if (BlockScreenWideMouse)
                        (ResolvedGame as IOverlayContainer).AddBlockingOverlay(this);
                    break;

                case Visibility.Hidden:
                    if (BlockScreenWideMouse)
                        (ResolvedGame as IOverlayContainer).RemoveBlockingOverlay(this);
                    break;

                default:
                    // Feedback that a state was updated, even if it somehow doesn't fall under an existing visibility.
                    OpenSample.Play();
                    break;
            }

            base.UpdateState(state);
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            OpenSample = audio.Samples.Get(PathHelper.Sample.ConfirmEnter);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            (ResolvedGame as IOverlayContainer).RemoveBlockingOverlay(this);
        }
    }
}