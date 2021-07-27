using System;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Layout;
using osu.Framework.Utils;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Core.Graphics.Containers
{
    /// <summary>
    ///     See: osu!'s SectionsContainer. <br />
    ///     A container that can scroll to each section inside it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SectionsContainer<T> : Container<T> where T : Drawable
    {
        /// <summary>
        ///     The percentage of the container to consider the center-point for deciding the active section (and scrolling to a requested section).
        /// </summary>
        public const float ScrollYCenter = 0.1f;

        public Bindable<T> SelectedSection { get; } = new();

        public Drawable LastClickedSection { get; protected set; }

        public Drawable ExpandableHeader
        {
            get => _expandableHeader;

            set
            {
                if (value == _expandableHeader)
                    return;

                if (_expandableHeader != null)
                    RemoveInternal(_expandableHeader);

                _expandableHeader = value;

                if (value == null)
                    return;

                AddInternal(_expandableHeader);
                _lastKnownScroll = null;
            }
        }

        public Drawable FixedHeader
        {
            get => _fixedHeader;
            set
            {
                if (value == _fixedHeader)
                    return;

                _fixedHeader?.Expire();
                _fixedHeader = value;

                if (value == null)
                    return;

                AddInternal(_fixedHeader);
                _lastKnownScroll = null;
            }
        }

        public Drawable Footer
        {
            get => _footer;
            set
            {
                if (value == _footer)
                    return;

                if (_footer != null)
                    ScrollContainer.Remove(_footer);

                _footer = value;

                if (value == null)
                    return;

                _footer.Anchor |= Anchor.y2;
                _footer.Origin |= Anchor.y2;

                ScrollContainer.Add(_footer);
                _lastKnownScroll = null;
            }
        }

        public Drawable HeaderBackground
        {
            get => _headerBackground;
            set
            {
                if (value == _headerBackground)
                    return;

                _headerBackgroundContainer.Clear();
                _headerBackground = value;

                if (value == null)
                    return;

                _headerBackgroundContainer.Add(_headerBackground);
                _lastKnownScroll = null;
            }
        }

        protected override Container<T> Content => _scrollContentContainer;

        public readonly UserTrackingScrollContainer ScrollContainer;
        private readonly Container _headerBackgroundContainer;
        private readonly MarginPadding _originalSectionsMargin;
        private Drawable _expandableHeader;
        private Drawable _fixedHeader;
        private Drawable _footer;
        private Drawable _headerBackground;
        private FlowContainer<T> _scrollContentContainer;
        private float? _headerHeight;
        private float? _footerHeight;
        private float? _lastKnownScroll;

        public SectionsContainer()
        {
            AddRangeInternal(new Drawable[]
            {
                ScrollContainer = CreateScrollContainer().With(x =>
                {
                    x.RelativeSizeAxes = Axes.Both;
                    x.Masking = true;
                    x.ScrollbarVisible = false;
                    x.Child = _scrollContentContainer = CreateScrollContentContainer();
                }),
                _headerBackgroundContainer = new Container
                {
                    RelativeSizeAxes = Axes.X
                }
            });

            _originalSectionsMargin = _scrollContentContainer.Margin;
        }

        public override void Add(T drawable)
        {
            base.Add(drawable);

            _lastKnownScroll = _headerHeight = _footerHeight = null;
        }

        public void ScrollTo(Drawable section)
        {
            LastClickedSection = section;
            ScrollContainer.ScrollTo(ScrollContainer.GetChildPosInContent(section) -
                                      ScrollContainer.DisplayableContent * ScrollYCenter -
                                      (FixedHeader?.BoundingBox.Height ?? 0));
        }

        public void ScrollToTop() => ScrollContainer.ScrollTo(0);

        [NotNull]
        protected virtual UserTrackingScrollContainer CreateScrollContainer() => new();

        [NotNull]
        protected virtual FlowContainer<T> CreateScrollContentContainer() => new FillFlowContainer<T>
        {
            Direction = FillDirection.Vertical,
            AutoSizeAxes = Axes.Y,
            RelativeSizeAxes = Axes.X
        };

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            bool result = base.OnInvalidate(invalidation, source);

            if (source != InvalidationSource.Child || (invalidation & Invalidation.DrawSize) == 0)
                return result;

            _lastKnownScroll = null;
            return true;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            float fixedHeaderSize = FixedHeader?.LayoutSize.Y ?? 0;
            float expandableHeaderSize = ExpandableHeader?.LayoutSize.Y ?? 0;

            float headerH = expandableHeaderSize + fixedHeaderSize;
            float footerH = Footer?.LayoutSize.Y ?? 0;

            if (headerH != _headerHeight || footerH != _footerHeight)
            {
                _headerHeight = headerH;
                _footerHeight = footerH;
                UpdateSectionsMargin();
            }

            float currentScroll = ScrollContainer.Current;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (currentScroll == _lastKnownScroll)
                return;

            _lastKnownScroll = currentScroll;

            // reset last clicked section because user started scrolling themselves
            if (ScrollContainer.UserScrolling)
                LastClickedSection = null;

            if (ExpandableHeader != null && FixedHeader != null)
            {
                float offset = Math.Min(expandableHeaderSize, currentScroll);

                ExpandableHeader.Y = -offset;
                FixedHeader.Y = -offset + expandableHeaderSize;
            }

            _headerBackgroundContainer.Height = expandableHeaderSize + fixedHeaderSize;
            _headerBackgroundContainer.Y = ExpandableHeader?.Y ?? 0;

            float smallestSectionHeight = Children.Count > 0 ? Children.Min(d => d.Height) : 0;

            // scroll offset is our fixed header height if we have it plus 10% of content height
            // plus 5% to fix floating point errors and to not have a section instantly unselect when scrolling upwards
            // but the 5% can't be bigger than our smallest section height, otherwise it won't get selected correctly
            float selectionLenienceAboveSection = Math.Min(smallestSectionHeight / 2.0f,
                ScrollContainer.DisplayableContent * 0.05f);

            float scrollCentre = fixedHeaderSize + ScrollContainer.DisplayableContent * ScrollYCenter +
                                 selectionLenienceAboveSection;

            if (Precision.AlmostBigger(0, ScrollContainer.Current))
                SelectedSection.Value = LastClickedSection as T ?? Children.FirstOrDefault();
            else if (Precision.AlmostBigger(ScrollContainer.Current, ScrollContainer.ScrollableExtent))
                SelectedSection.Value = LastClickedSection as T ?? Children.LastOrDefault();
            else
            {
                SelectedSection.Value = Children
                    .TakeWhile(section =>
                        ScrollContainer.GetChildPosInContent(section) - currentScroll - scrollCentre <= 0)
                    .LastOrDefault() ?? Children.FirstOrDefault();
            }
        }

        private void UpdateSectionsMargin()
        {
            if (!Children.Any())
                return;

            MarginPadding newMargin = _originalSectionsMargin;

            newMargin.Top += _headerHeight ?? 0;
            newMargin.Bottom += _footerHeight ?? 0;

            _scrollContentContainer.Margin = newMargin;
        }
    }
}