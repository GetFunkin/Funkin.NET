using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Framework.Testing;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Core.Overlays.Settings
{
    [ExcludeFromDynamicCompile]
    public abstract class SettingsSubsection : FillFlowContainer, IHasFilterableChildren
    {
        protected override Container<Drawable> Content => FlowContent;

        protected readonly FillFlowContainer FlowContent;

        protected abstract LocalisableString Header { get; }

        public IEnumerable<IFilterable> FilterableChildren => Children.OfType<IFilterable>();
        public virtual IEnumerable<string> FilterTerms => new[] { Header.ToString() };

        public bool MatchingFilter
        {
            set => this.FadeTo(value ? 1 : 0);
        }

        public bool FilteringActive { get; set; }

        protected SettingsSubsection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Vertical;

            FlowContent = new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 8),
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            };
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddRangeInternal(new Drawable[]
            {
                new SpriteText
                {
                    Text = Header.ToString().ToUpper(),
                    Margin = new MarginPadding { Vertical = 30, Left = SettingsPanel.ContentMargins, Right = SettingsPanel.ContentMargins },
                    Font = new FontUsage("Torus-Bold")
                },

                FlowContent
            });
        }
    }
}