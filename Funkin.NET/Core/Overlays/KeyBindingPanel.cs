using Funkin.NET.Core.Input.Bindings;
using Funkin.NET.Core.Overlays.KeyBindings;
using Funkin.NET.Core.Overlays.Settings;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace Funkin.NET.Core.Overlays
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