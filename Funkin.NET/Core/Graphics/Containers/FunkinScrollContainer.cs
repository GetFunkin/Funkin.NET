using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Core.Graphics.Containers
{
    /// <summary>
    ///     See: osu!'s OsuScrollContainer.
    /// </summary>
    public class FunkinScrollContainer : FunkinScrollContainer<Drawable>
    {
        public FunkinScrollContainer()
        {
        }

        public FunkinScrollContainer(Direction direction)
            : base(direction)
        {
        }
    }

    /// <summary>
    ///     See: osu!'s OsuScrollContainer&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FunkinScrollContainer<T> : ScrollContainer<T> where T : Drawable
    {
        public const float ScrollBarHeight = 10;
        public const float ScrollBarPadding = 3;

        /// <summary>
        ///     Allows controlling the scroll bar from any position in the container using the right mouse button. <br />
        ///     Uses the value of <see cref="DistanceDecayOnRightMouseScrollbar"/> to smoothly scroll to the dragged location.
        /// </summary>
        public bool RightMouseScrollbar;

        /// <summary>
        ///     Controls the rate with which the target position is approached when performing a relative drag. Default is 0.02.
        /// </summary>
        public double DistanceDecayOnRightMouseScrollbar = 0.02;

        private bool ShouldPerformRightMouseScroll(MouseButtonEvent e) =>
            RightMouseScrollbar && e.Button == MouseButton.Right;

        private void ScrollFromMouseEvent(UIEvent e) =>
            ScrollTo(
                Clamp(ToLocalSpace(e.ScreenSpaceMousePosition)[ScrollDim] / DrawSize[ScrollDim]) *
                Content.DrawSize[ScrollDim], true, DistanceDecayOnRightMouseScrollbar);

        private bool _rightMouseDragging;

        protected override bool IsDragging => base.IsDragging || _rightMouseDragging;

        public FunkinScrollContainer(Direction scrollDirection = Direction.Vertical)
            : base(scrollDirection)
        {
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!ShouldPerformRightMouseScroll(e))
                return base.OnMouseDown(e);

            ScrollFromMouseEvent(e);
            return true;

        }

        protected override void OnDrag(DragEvent e)
        {
            if (_rightMouseDragging)
            {
                ScrollFromMouseEvent(e);
                return;
            }

            base.OnDrag(e);
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            if (!ShouldPerformRightMouseScroll(e))
                return base.OnDragStart(e);

            _rightMouseDragging = true;
            return true;

        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            if (_rightMouseDragging)
            {
                _rightMouseDragging = false;
                return;
            }

            base.OnDragEnd(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            // allow for controlling volume when alt is held.
            // mostly for compatibility with osu-stable.
            return !e.AltPressed && base.OnScroll(e);
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new OsuScrollbar(direction);

        protected class OsuScrollbar : ScrollbarContainer
        {
            private Color4 _hoverColor;
            private Color4 _defaultColor;
            private Color4 _highlightColor;

            private readonly Box _box;

            public OsuScrollbar(Direction scrollDir)
                : base(scrollDir)
            {
                Blending = BlendingParameters.Additive;

                CornerRadius = 5;

                // needs to be set initially for the ResizeTo to respect minimum size
                Size = new Vector2(ScrollBarHeight);

                const float margin = 3;

                Margin = new MarginPadding
                {
                    Left = scrollDir == Direction.Vertical ? margin : 0,
                    Right = scrollDir == Direction.Vertical ? margin : 0,
                    Top = scrollDir == Direction.Horizontal ? margin : 0,
                    Bottom = scrollDir == Direction.Horizontal ? margin : 0,
                };

                Masking = true;
                Child = _box = new Box {RelativeSizeAxes = Axes.Both};
            }

            [BackgroundDependencyLoader]
            private void Load()
            {
                Colour = _defaultColor = Color4Extensions.FromHex("888");
                _hoverColor = Color4Extensions.FromHex("fff");
                _highlightColor = Color4Extensions.FromHex(@"88b300");
            }

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                Vector2 size = new(ScrollBarHeight)
                {
                    [(int) ScrollDirection] = val
                };
                this.ResizeTo(size, duration, easing);
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(_hoverColor, 100);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeColour(_defaultColor, 100);
            }

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if (!base.OnMouseDown(e)) return false;

                // note that we are changing the color of the box here as to not interfere with the hover effect.
                _box.FadeColour(_highlightColor, 100);
                return true;
            }

            protected override void OnMouseUp(MouseUpEvent e)
            {
                if (e.Button != MouseButton.Left) return;

                _box.FadeColour(Color4.White, 100);

                base.OnMouseUp(e);
            }
        }
    }
}