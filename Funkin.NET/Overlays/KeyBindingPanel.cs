using Funkin.NET.Input.Bindings;
using Funkin.NET.Overlays.KeyBindings;
using Funkin.NET.Overlays.Settings;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace Funkin.NET.Overlays
{
    public class KeyBindingPanel : SettingsSubPanel
    {
        protected override Drawable CreateHeader() => new SettingsHeader("Key Configuration", "Customize your keys!");

        [BackgroundDependencyLoader]
        private void Load(UniversalActionContainer actions)
        {
            AddSection(new UniversalBindingsSection(actions));
        }
    }
}