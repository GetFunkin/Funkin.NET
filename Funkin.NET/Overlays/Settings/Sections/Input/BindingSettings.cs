using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;

namespace Funkin.NET.Overlays.Settings.Sections.Input
{
    public class BindingSettings : SettingsSubsection
    {
        protected override LocalisableString Header => "Key Bindings";

        public BindingSettings(VisibilityContainer keyConfig)
        {
            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Text = "Configure Bindings",
                    TooltipText = "Change your key binding configuration.",
                    Action = keyConfig.ToggleVisibility
                }
            };
        }
    }
}