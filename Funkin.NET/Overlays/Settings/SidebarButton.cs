using Funkin.NET.Graphics.Containers;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Overlays.Settings
{
    /// <summary>
    ///     See: osu!'s SidebarButton.
    /// </summary>
    public class SidebarButton : FunkinButton
    {
        private readonly ConstrainedIconContainer _iconContainer;
        private readonly SpriteText _headerText;
        private readonly Box _selectionIndicator;
        private readonly Container _text;

        private SettingsSection _section;

        public SettingsSection Section
        {
            get => _section;
            set
            {
                _section = value;
                _headerText.Text = value.Header;
                _iconContainer.Icon = value.CreateIcon();
            }
        }

        private bool selected;

        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;

                if (selected)
                {
                    _selectionIndicator.FadeIn(50);
                    _text.FadeColour(Color4.White, 50);
                }
                else
                {
                    _selectionIndicator.FadeOut(50);
                    _text.FadeColour(new Color4(0.6f, 0.6f, 0.6f, 1f), 50);
                }
            }
        }

        public SidebarButton()
        {
            Height = Sidebar.DefaultWidth;
            RelativeSizeAxes = Axes.X;

            BackgroundColor = Color4.Black;

            AddRange(new Drawable[]
            {
                _text = new Container
                {
                    Width = Sidebar.DefaultWidth,
                    RelativeSizeAxes = Axes.Y,
                    Colour = new Color4(0.6f, 0.6f, 0.6f, 1f),
                    Children = new Drawable[]
                    {
                        _headerText = new FunkinSpriteText
                        {
                            Position = new Vector2(Sidebar.DefaultWidth + 10, 0),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                        },
                        _iconContainer = new ConstrainedIconContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(20),
                        },
                    }
                },
                _selectionIndicator = new Box
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.Y,
                    Width = 5,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                }
            });
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            _selectionIndicator.Colour = Color4Extensions.FromHex(@"ffcc22");
        }
    }
}