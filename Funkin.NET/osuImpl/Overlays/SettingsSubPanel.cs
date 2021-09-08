using Funkin.NET.osuImpl.Graphics.UserInterface;
using Funkin.NET.osuImpl.Overlays.Settings;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace Funkin.NET.osuImpl.Overlays
{
    public class SettingsSubPanel : SettingsPanel
    {
        protected SettingsSubPanel()
            : base(true)
        {
            MarginPadding padding = Margin;
            padding.Left += SidebarWidth;
            Margin = padding;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddInternal(new BackButton
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Action = Hide
            });
        }

        public class BackButton : FunkinButton
        {
            [BackgroundDependencyLoader]
            private void Load()
            {
                Size = new Vector2(Sidebar.DefaultWidth);

                BackgroundColor = Color4.Black;

                AddRange(new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Children = new Drawable[]
                        {
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(15),
                                Shadow = true,
                                Icon = FontAwesome.Solid.ChevronLeft
                            },
                            new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Y = 15,
                                Font = new FontUsage("Torus-Bold", 12f),
                                Text = "back"
                            },
                        }
                    }
                });
            }
        }
    }
}