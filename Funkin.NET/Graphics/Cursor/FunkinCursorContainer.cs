﻿using osu.Framework.Graphics;
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
        private InputManager _inputManager;
        private IProvidesCursor _currentTarget;

        /// <summary>
        ///     Whether any cursors can be displayed.
        /// </summary>
        public virtual bool CanShowCursor => true;

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

            _inputManager = GetContainingInputManager();
        }

        protected override void Update()
        {
            base.Update();

            IInput lastSource = _inputManager.CurrentState.Mouse.LastSource;
            bool validInput = lastSource is { } and not ISourcedFromTouch;

            if (!validInput || !CanShowCursor)
            {
                _currentTarget?.Cursor?.Hide();
                _currentTarget = null;
                return;
            }

            IProvidesCursor newTarget = this;

            foreach (Drawable drawable in _inputManager.HoveredDrawables)
            {
                if (drawable is not IProvidesCursor {ProvidingUserCursor: true} cursorProvider)
                    continue;

                newTarget = cursorProvider;
                break;
            }

            if (_currentTarget == newTarget)
                return;

            _currentTarget?.Cursor?.Hide();
            newTarget.Cursor?.Show();

            _currentTarget = newTarget;
        }
    }
}