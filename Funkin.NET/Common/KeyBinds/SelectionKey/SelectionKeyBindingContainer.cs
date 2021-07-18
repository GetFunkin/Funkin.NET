using System.Collections.Generic;
using Funkin.NET.Common.KeyBinds.ArrowKeys;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Common.KeyBinds.SelectionKey
{
    /// <summary>
    ///     Container for an arrow key action.
    /// </summary>
    public class SelectionKeyBindingContainer : KeyBindingContainer<SelectionKeyAction>
    {
        /// <inheritdoc cref="ArrowKeyBindingContainer.DefaultKeyBindings"/>
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] {InputKey.Enter}, SelectionKeyAction.Enter),
            new KeyBinding(new[] {InputKey.Space}, SelectionKeyAction.Enter)
        };
    }
}