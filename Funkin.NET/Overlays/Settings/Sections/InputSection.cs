using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;

namespace Funkin.NET.Overlays.Settings.Sections
{
    public class InputSection : SettingsSection
    {
        public override Drawable CreateIcon() => new SpriteIcon()
        {
            Icon = FontAwesome.Solid.Keyboard
        };

        public override string Header => "Input";

        [Resolved]
        private GameHost Host { get; set; }
    }
}