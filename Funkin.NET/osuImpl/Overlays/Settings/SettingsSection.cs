using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.osuImpl.Overlays.Settings
{
    /// <summary>
    ///     See: osu!'s SettingsSection.
    /// </summary>
    public abstract class SettingsSection : Container, IHasFilterableChildren
    {
        protected FillFlowContainer FlowContent;
        protected override Container<Drawable> Content => FlowContent;

        public abstract Drawable CreateIcon();
        public abstract string Header { get; }

        public IEnumerable<IFilterable> FilterableChildren => Children.OfType<IFilterable>();
        public virtual IEnumerable<string> FilterTerms => new[] {Header};

        public const int HeaderSize = 26;
        public const int ConstantMargin = 20;
        public const int BorderSize = 2;

        public bool MatchingFilter
        {
            set => this.FadeTo(value ? 1 : 0);
        }

        public bool FilteringActive { get; set; }

        protected SettingsSection()
        {
            Margin = new MarginPadding {Top = ConstantMargin};
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;

            FlowContent = new FillFlowContainer
            {
                Margin = new MarginPadding
                {
                    Top = HeaderSize
                },
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
            };
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    Colour = new Color4(0, 0, 0, 255),
                    RelativeSizeAxes = Axes.X,
                    Height = BorderSize,
                },
                new Container
                {
                    Padding = new MarginPadding
                    {
                        Top = ConstantMargin + BorderSize,
                        Bottom = 10,
                    },
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Font = new FontUsage("Torus-Regular", HeaderSize),
                            Text = Header,
                            Colour = Color4Extensions.FromHex(@"ffcc22"),
                            Margin = new MarginPadding
                            {
                                Left = SettingsPanel.ContentMargins,
                                Right = SettingsPanel.ContentMargins
                            }
                        },
                        FlowContent
                    }
                },
            });
        }
    }
}