using System;
using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Core.Input.Bindings;
using Funkin.NET.Core.Overlays.Settings;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Core.Overlays.KeyBindings
{
    public abstract class KeyBindingsSubsection : SettingsSubsection
    {
        protected IEnumerable<IKeyBinding> Defaults;

        protected Action<Drawable> CancelAction;

        protected KeyBindingsSubsection(Action<Drawable> cancelAction = null)
        {
            CancelAction = cancelAction;

            FlowContent.Spacing = new Vector2(0, 1);
            FlowContent.Padding = new MarginPadding
                {Left = SettingsPanel.ContentMargins, Right = SettingsPanel.ContentMargins};
        }

        [BackgroundDependencyLoader]
        private void Load(UniversalActionContainer bindings)
        {
            List<IKeyBinding> collection = bindings.DefaultKeyBindings.ToList();

            foreach (IGrouping<object, IKeyBinding> defaultGroup in Defaults.GroupBy(d => d.Action))
            {
                // one row per valid action.
                Add(new KeyBindingRow(defaultGroup.Key,
                    collection.Where(b => b.Action.Equals(defaultGroup.Key)).ToList())
                {
                    AllowMainMouseButtons = true,
                    Defaults = defaultGroup.Select(d => d.KeyCombination)
                });
            }

            Add(new ResetButton
            {
                Action = () =>
                {
                    bindings.UpdateKeyBindings(bindings.FallbackKeyBindings);
                    CancelAction?.Invoke(this);
                }
            });
        }
    }
}