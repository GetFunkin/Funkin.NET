using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input;
using osu.Framework.Input.StateChanges;

namespace Funkin.NET.Graphics.Cursor
{
    /// <summary>
    ///     See: osu!'s MenuCursorContainer. <br />
    ///     A container which provides a <see cref="FunkinCursor"/> which can be overridden by hovered <see cref="Drawable"/><c>s</c>.
    /// </summary>
    public class FunkinCursorContainer : Container, IProvidesCursor
    {
        protected override Container<Drawable> Content => _content;

        private readonly Container _content;

        public InputManager InputManager { get; protected set; }

        public IProvidesCursor CurrentTarget { get; protected set; }

        /// <summary>
        ///     Whether any cursors can be displayed.
        /// </summary>
        public virtual bool CanShowCursor { get; set; } = true;

        public CursorContainer Cursor { get; }

        public bool ProvidingUserCursor => true;

        public FunkinCursorContainer()
        {
            AddRangeInternal(new Drawable[]
            {
                Cursor = new FunkinCursor
                {
                    State =
                    {
                        Value = Visibility.Hidden
                    }
                },
                _content = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            InputManager = GetContainingInputManager();
        }

        protected override void Update()
        {
            base.Update();

            IInput lastSource = InputManager.CurrentState.Mouse.LastSource;
            bool validInput = lastSource is { } and not ISourcedFromTouch;

            if (!validInput || !CanShowCursor)
            {
                CurrentTarget?.Cursor?.Hide();
                CurrentTarget = null;
                return;
            }

            IProvidesCursor newTarget = this;

            foreach (Drawable drawable in InputManager.HoveredDrawables)
            {
                if (drawable is not IProvidesCursor {ProvidingUserCursor: true} cursorProvider)
                    continue;

                newTarget = cursorProvider;
                break;
            }

            if (CurrentTarget == newTarget)
                return;

            CurrentTarget?.Cursor?.Hide();
            newTarget.Cursor?.Show();

            CurrentTarget = newTarget;
        }
    }
}