using System;
using osu.Framework.Bindables;
using osu.Framework.Configuration;

namespace Funkin.NET.Intermediary.Utilities
{
    public static class BindableExtensions
    {
        public static void SetBindable<TBindable, TLookup>(
            // ReSharper disable once RedundantAssignment - Justification: reference type
            this Bindable<TBindable> bindable,
            ConfigManager<TLookup> config,
            TLookup lookup,
            Action<ValueChangedEvent<TBindable>> action = null
        ) where TLookup : struct, Enum
        {
            bindable = config.GetBindable<TBindable>(lookup);

            if (action is not null)
                bindable.ValueChanged += action;

            bindable.TriggerChange();
        }
    }
}