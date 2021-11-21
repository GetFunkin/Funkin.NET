using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input;
using osu.Framework.Input.StateChanges;

namespace Funkin.NET.Graphics.Cursor
{
    public class BasicCursorContainer : Container, ICursorProvider
    {
        public CursorContainer? Cursor { get; }

        public bool ProvidingUserCursor => true;

        public bool CanShowCursor = true;

        protected override Container<Drawable> Content => ContainerContent;

        protected readonly Container ContainerContent;
        protected InputManager? InputManager;
        protected ICursorProvider? CursorProvider;

        public BasicCursorContainer()
        {
            AddRangeInternal(new Drawable[]
            {
                Cursor = new BasicCursor
                {
                    State =
                    {
                        Value = Visibility.Hidden
                    }
                },

                ContainerContent = new Container
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

            if (InputManager is null)
                return;

            IInput lastMouseSource = InputManager.CurrentState.Mouse.LastSource;
            bool hasValidInput = lastMouseSource != null && lastMouseSource is not ISourcedFromTouch;

            if (!hasValidInput || !CanShowCursor)
            {
                CursorProvider?.Cursor?.Hide();
                CursorProvider = null;
                return;
            }

            ICursorProvider newTarget = this;

            foreach (Drawable drawable in InputManager.HoveredDrawables)
                if (drawable is ICursorProvider {ProvidingUserCursor: true} provider)
                {
                    newTarget = provider;
                    break;
                }

            if (CursorProvider == newTarget)
                return;

            CursorProvider?.Cursor?.Hide();
            newTarget.Cursor?.Show();

            CursorProvider = newTarget;
        }
    }
}