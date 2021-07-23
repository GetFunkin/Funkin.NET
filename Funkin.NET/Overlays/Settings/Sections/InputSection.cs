using Funkin.NET.Overlays.Settings.Sections.Input;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;

namespace Funkin.NET.Overlays.Settings.Sections
{
    public class InputSection : SettingsSection
    {
        public KeyBindingPanel KeyConfig { get; }

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.Keyboard
        };

        public override string Header => "Input";

        [Resolved]
        private GameHost Host { get; set; }

        public InputSection(KeyBindingPanel keyConfig)
        {
            KeyConfig = keyConfig;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            Children = new Drawable[]
            {
                new BindingSettings(KeyConfig)
            };
        }
    }
}