using Funkin.NET.Overlays.Settings;
using osu.Framework.Graphics;

namespace Funkin.NET.Overlays
{
    public class KeyBindingPanel : SettingsSubPanel
    {
        protected override Drawable CreateHeader() => new SettingsHeader("Key Configuration", "Customize your keys!");

        private void Load()
        {
            // AddSection(new SelectionKeyBindingsSection());
        }
    }
}