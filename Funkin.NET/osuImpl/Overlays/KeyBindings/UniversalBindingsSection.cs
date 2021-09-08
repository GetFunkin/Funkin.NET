using System;
using Funkin.NET.Common.Input;
using Funkin.NET.osuImpl.Overlays.Settings;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.osuImpl.Overlays.KeyBindings
{
    public class UniversalBindingsSection : SettingsSection
    {
        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.UniversalAccess
        };

        public override string Header => "Universal Bindings";

        protected UniversalBindingsSubsection ProtectedUniversalSubsection;

        // todo: split sections up
        public UniversalBindingsSection(UniversalActionContainer bindings)
        {
            void The(Drawable drawable)
            {
                Remove(ProtectedUniversalSubsection);
                Add(ProtectedUniversalSubsection = new UniversalBindingsSubsection(bindings, The));
            }

            Add(ProtectedUniversalSubsection = new UniversalBindingsSubsection(bindings, The));
        }

        public class UniversalBindingsSubsection : KeyBindingsSubsection
        {
            protected override LocalisableString Header => string.Empty;

            public UniversalBindingsSubsection(UniversalActionContainer bindings, Action<Drawable> cancelAction) : base(
                cancelAction)
            {
                Defaults = bindings.FallbackKeyBindings;
            }
        }
    }
}