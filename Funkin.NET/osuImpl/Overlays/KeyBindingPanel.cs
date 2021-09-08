using Funkin.NET.Common.Input;
using Funkin.NET.osuImpl.Overlays.KeyBindings;
using Funkin.NET.osuImpl.Overlays.Settings;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace Funkin.NET.osuImpl.Overlays
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