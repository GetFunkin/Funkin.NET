using System;
using System.Linq;
using Funkin.NET.Graphics.Containers;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Threading;
using osuTK;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Overlays.Settings
{
    /// <summary>
    ///     See: osu!'s Sidebar.
    /// </summary>
    public class Sidebar : Container<SidebarButton>, IStateful<ExpandedState>
    {
        private readonly FillFlowContainer<SidebarButton> _content;
        public const float DefaultWidth = 40f * 1.4f;
        public const int ExpandedWidth = 200;

        public event Action<ExpandedState> StateChanged;

        protected override Container<SidebarButton> Content => _content;

        public Sidebar()
        {
            RelativeSizeAxes = Axes.Y;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = new Colour4(0.02f, 0.02f, 0.02f, 1f),
                    RelativeSizeAxes = Axes.Both,
                },
                new SidebarScrollContainer
                {
                    Children = new[]
                    {
                        _content = new FillFlowContainer<SidebarButton>
                        {
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.CentreLeft,
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Direction = FillDirection.Vertical,
                        }
                    }
                },
            };
        }

        private ScheduledDelegate _expandEvent;
        private ExpandedState _state;

        protected override bool OnHover(HoverEvent e)
        {
            QueueExpandIfHovering();
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            _expandEvent?.Cancel();
            _lastHoveredButton = null;
            State = ExpandedState.Contracted;

            base.OnHoverLost(e);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            QueueExpandIfHovering();
            return base.OnMouseMove(e);
        }

        private class SidebarScrollContainer : FunkinScrollContainer
        {
            public SidebarScrollContainer()
            {
                Content.Anchor = Anchor.CentreLeft;
                Content.Origin = Anchor.CentreLeft;
                RelativeSizeAxes = Axes.Both;
                ScrollbarVisible = false;
            }
        }

        public ExpandedState State
        {
            get => _state;
            set
            {
                _expandEvent?.Cancel();

                if (_state == value) return;

                _state = value;

                switch (_state)
                {
                    default:
                        this.ResizeTo(new Vector2(DefaultWidth, Height), 500, Easing.OutQuint);
                        break;

                    case ExpandedState.Expanded:
                        this.ResizeTo(new Vector2(ExpandedWidth, Height), 500, Easing.OutQuint);
                        break;
                }

                StateChanged?.Invoke(State);
            }
        }

        private Drawable _lastHoveredButton;

        private Drawable HoveredButton => _content.Children.FirstOrDefault(c => c.IsHovered);

        private void QueueExpandIfHovering()
        {
            // only expand when we hover a different button.
            if (_lastHoveredButton == HoveredButton) return;

            if (!IsHovered) return;

            if (State != ExpandedState.Expanded)
            {
                _expandEvent?.Cancel();
                _expandEvent = Scheduler.AddDelayed(() => State = ExpandedState.Expanded, 750);
            }

            _lastHoveredButton = HoveredButton;
        }
    }

    public enum ExpandedState
    {
        Contracted,
        Expanded,
    }
}