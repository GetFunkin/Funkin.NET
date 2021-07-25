using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Graphics.Containers;
using Funkin.NET.Graphics.UserInterface;
using Funkin.NET.Overlays.Settings;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Overlays
{
    /// <summary>
    ///     See: osu!'s SettingsPanel. :p
    /// </summary>
    /// TODO: sidebar lol
    public class SettingsPanel : FunkinFocusedOverlayContainer
    {
        public const float ContentMargins = 15;
        public const float TransitionLength = 600;
        public const float SidebarWidth = Sidebar.DefaultWidth;
        public const float ConstantWidth = 400;

        protected Container<Drawable> ContentContainer;

        protected override Container<Drawable> Content => ContentContainer;

        protected Sidebar Sidebar;
        private SidebarButton _selectedSidebarButton;
        protected SettingsSectionsContainer SectionsContainer;
        private SeekLimitedSearchTextBox _searchTextBox;

        private readonly bool _showSidebar;

        protected SettingsPanel(bool showSidebar)
        {
            _showSidebar = showSidebar;
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
        }

        protected virtual IEnumerable<SettingsSection> CreateSections() => null;

        [BackgroundDependencyLoader]
        private void Load()
        {
            InternalChild = ContentContainer = new NonMaskedContent
            {
                Width = ConstantWidth,
                RelativeSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Scale = new Vector2(2, 1), // over-extend to the left for transitions
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Colour4(0.05f, 0.05f, 0.05f, 1f),
                        Alpha = 1,
                    },
                    SectionsContainer = new SettingsSectionsContainer
                    {
                        Masking = true,
                        RelativeSizeAxes = Axes.Both,
                        ExpandableHeader = CreateHeader(),
                        FixedHeader = _searchTextBox = new SeekLimitedSearchTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            Width = 0.95f,
                            Margin = new MarginPadding
                            {
                                Top = 20,
                                Bottom = 20
                            },
                        },
                        Footer = CreateFooter()
                    },
                }
            };

            if (_showSidebar)
            {
                AddInternal(Sidebar = new Sidebar {Width = SidebarWidth});

                SectionsContainer.SelectedSection.ValueChanged += section =>
                {
                    _selectedSidebarButton.Selected = false;
                    _selectedSidebarButton = Sidebar.Children.Single(b => b.Section == section.NewValue);
                    _selectedSidebarButton.Selected = true;
                };
            }

            _searchTextBox.Current.ValueChanged += term => SectionsContainer.SearchContainer.SearchTerm = term.NewValue;

            CreateSections()?.ForEach(AddSection);
        }

        protected void AddSection(SettingsSection section)
        {
            SectionsContainer.Add(section);

            if (Sidebar != null)
            {
                var button = new SidebarButton
                {
                    Section = section,
                    Action = () =>
                    {
                        SectionsContainer.ScrollTo(section);
                        Sidebar.State = ExpandedState.Contracted;
                    },
                };

                Sidebar.Add(button);

                if (_selectedSidebarButton != null)
                    return;

                _selectedSidebarButton = Sidebar.Children.First();
                _selectedSidebarButton.Selected = true;
            }
        }

        protected virtual Drawable CreateHeader() => new Container();

        protected virtual Drawable CreateFooter() => new Container();

        protected override void PopIn()
        {
            base.PopIn();

            ContentContainer.MoveToX(ExpandedPosition, TransitionLength, Easing.OutQuint);

            Sidebar?.MoveToX(0, TransitionLength, Easing.OutQuint);
            this.FadeTo(1, TransitionLength, Easing.OutQuint);

            _searchTextBox.HoldFocus = true;
        }

        protected virtual float ExpandedPosition => 0;

        protected override void PopOut()
        {
            base.PopOut();

            ContentContainer.MoveToX(-ConstantWidth + ExpandedPosition, TransitionLength, Easing.OutQuint);

            Sidebar?.MoveToX(-SidebarWidth, TransitionLength, Easing.OutQuint);
            this.FadeTo(0, TransitionLength, Easing.OutQuint);

            _searchTextBox.HoldFocus = false;
            if (_searchTextBox.HasFocus)
                GetContainingInputManager().ChangeFocus(null);
        }

        public override bool AcceptsFocus => true;

        protected override void OnFocus(FocusEvent e)
        {
            _searchTextBox.TakeFocus();
            base.OnFocus(e);
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            ContentContainer.Margin = new MarginPadding {Left = Sidebar?.DrawWidth ?? 0};
        }

        public class NonMaskedContent : Container<Drawable>
        {
            // masking breaks the pan-out transform with nested sub-settings panels.
            protected override bool ComputeIsMaskedAway(RectangleF maskingBounds) => false;
        }

        public class SettingsSectionsContainer : SectionsContainer<SettingsSection>
        {
            public SearchContainer<SettingsSection> SearchContainer;

            protected override FlowContainer<SettingsSection> CreateScrollContentContainer()
                => SearchContainer = new SearchContainer<SettingsSection>
                {
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                };

            public SettingsSectionsContainer()
            {
                HeaderBackground = new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both
                };
            }

            protected override void UpdateAfterChildren()
            {
                base.UpdateAfterChildren();

                // no null check because the usage of this class is strict
                HeaderBackground.Alpha = -ExpandableHeader.Y / ExpandableHeader.LayoutSize.Y;
            }
        }
    }
}