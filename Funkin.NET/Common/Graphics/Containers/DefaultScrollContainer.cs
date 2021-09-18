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

namespace Funkin.NET.Common.Graphics.Containers
{
    public class DefaultScrollContainer : FunkinScrollContainer<Drawable>
    {
    }

    public class FunkinScrollContainer<T> : ScrollContainer<T> where T : Drawable
    {
        public const float ScrollBarHeight = 10;

        /// <summary>
        ///     Allows controlling the scroll bar from any position in the container using the right mouse button. <br />
        ///     Uses the value of <see cref="DistanceDecayOnRightMouseScrollbar"/> to smoothly scroll to the dragged location.
        /// </summary>
        public bool RightMouseScrollbar;

        /// <summary>
        ///     Controls the rate with which the target position is approached when performing a relative drag. Default is 0.02.
        /// </summary>
        public double DistanceDecayOnRightMouseScrollbar = 0.02;

        protected bool ShouldPerformRightMouseScroll(MouseButtonEvent e) =>
            RightMouseScrollbar && e.Button == MouseButton.Right;

        protected void ScrollFromMouseEvent(UIEvent e) =>
            ScrollTo(
                Clamp(ToLocalSpace(e.ScreenSpaceMousePosition)[ScrollDim] / DrawSize[ScrollDim]) *
                Content.DrawSize[ScrollDim], true, DistanceDecayOnRightMouseScrollbar);

        protected bool RightMouseDragging;

        protected override bool IsDragging => base.IsDragging || RightMouseDragging;

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
            if (RightMouseDragging)
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

            RightMouseDragging = true;
            return true;

        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            if (RightMouseDragging)
            {
                RightMouseDragging = false;
                return;
            }

            base.OnDragEnd(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            // Allow for controlling volume when alt is held.
            // Mostly for compatibility with osu-stable.
            return !e.AltPressed && base.OnScroll(e);
        }

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new DefaultScrollbar(direction);

        protected class DefaultScrollbar : ScrollbarContainer
        {
            protected Color4 HoverColor;
            protected Color4 DefaultColor;
            protected Color4 HighlightColor;

            protected readonly Box ContainingBox;

            public DefaultScrollbar(Direction scrollDir)
                : base(scrollDir)
            {
                Blending = BlendingParameters.Additive;

                CornerRadius = 5;

                // Needs to be set initially for the ResizeTo to respect minimum size.
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
                Child = ContainingBox = new Box {RelativeSizeAxes = Axes.Both};
            }

            [BackgroundDependencyLoader]
            private void Load()
            {
                Colour = DefaultColor = Color4Extensions.FromHex("888");
                HoverColor = Color4Extensions.FromHex("fff");
                HighlightColor = Color4Extensions.FromHex(@"88b300");
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
                this.FadeColour(HoverColor, 100);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e) => this.FadeColour(DefaultColor, 100);

            protected override bool OnMouseDown(MouseDownEvent e)
            {
                if (!base.OnMouseDown(e)) return false;

                // Note that we are changing the color of the box here as to not interfere with the hover effect.
                ContainingBox.FadeColour(HighlightColor, 100);
                return true;
            }

            protected override void OnMouseUp(MouseUpEvent e)
            {
                if (e.Button != MouseButton.Left) return;

                ContainingBox.FadeColour(Color4.White, 100);

                base.OnMouseUp(e);
            }
        }
    }
}