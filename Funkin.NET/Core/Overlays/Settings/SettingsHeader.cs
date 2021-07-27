using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;

namespace Funkin.NET.Core.Overlays.Settings
{
    public class SettingsHeader : Container
    {
        private readonly LocalisableString _heading;
        private readonly LocalisableString _subheading;

        public SettingsHeader(LocalisableString heading, LocalisableString subheading)
        {
            _heading = heading;
            _subheading = subheading;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = _heading,
                            Font = new FontUsage("Torus-Regular", 40f),
                            Margin = new MarginPadding
                            {
                                Left = SettingsPanel.ContentMargins
                            },
                        },
                        new SpriteText
                        {
                            Colour = Color4Extensions.FromHex(@"ff66aa"),
                            Text = _subheading,
                            Font = new FontUsage("Torus-Regular", 18f),
                            Margin = new MarginPadding
                            {
                                Left = SettingsPanel.ContentMargins,
                                Bottom = 30
                            },
                        },
                    }
                }
            };
        }
    }
}