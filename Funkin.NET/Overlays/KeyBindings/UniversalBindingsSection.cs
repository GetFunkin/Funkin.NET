using Funkin.NET.Input.Bindings;
using Funkin.NET.Overlays.Settings;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Overlays.KeyBindings
{
    public class UniversalBindingsSection : SettingsSection
    {
        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.UniversalAccess
        };

        public override string Header => "Universal KeyBinds";

        // todo: split sections up
        public UniversalBindingsSection(UniversalActionContainer bindings)
        { 
            Add(new UniversalBindingsSubsection(bindings));
        }

        public class UniversalBindingsSubsection : KeyBindingsSubsection
        {
            protected override LocalisableString Header => string.Empty;

            public UniversalBindingsSubsection(UniversalActionContainer bindings)
            {
                Defaults = bindings.FallbackKeyBindings;
            }
        }
    }
}