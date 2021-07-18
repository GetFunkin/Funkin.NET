using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Common.KeyBinds.ArrowKeys
{
    /// <summary>
    ///     Container for an arrow key action.
    /// </summary>
    public class ArrowKeyBindingContainer : KeyBindingContainer<ArrowKeyAction>
    {
        /// <inheritdoc cref="ArrowKeyBindingContainer.DefaultKeyBindings"/>
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] { InputKey.Left, InputKey.D }, ArrowKeyAction.Left),
            new KeyBinding(new[] { InputKey.Down, InputKey.F }, ArrowKeyAction.Down),
            new KeyBinding(new[] { InputKey.Up, InputKey.J }, ArrowKeyAction.Up),
            new KeyBinding(new[] { InputKey.Down, InputKey.K }, ArrowKeyAction.Down)
        };
    }
}