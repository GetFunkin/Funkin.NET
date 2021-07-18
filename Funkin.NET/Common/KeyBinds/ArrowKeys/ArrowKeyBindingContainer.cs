using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Common.KeyBinds.ArrowKeys
{
    /// <summary>
    ///     Container for an arrow key action.
    /// </summary>
    public class ArrowKeyBindingContainer : KeyBindingContainer<ArrowKeyAction>
    {
        public ArrowKeyBindingContainer() : base(SimultaneousBindingMode.All)
        {
        }

        /// <inheritdoc cref="KeyBindingContainer.DefaultKeyBindings"/>
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] {InputKey.Left}, ArrowKeyAction.Left),
            new KeyBinding(new[] {InputKey.Down}, ArrowKeyAction.Down),
            new KeyBinding(new[] {InputKey.Up}, ArrowKeyAction.Up),
            new KeyBinding(new[] {InputKey.Down}, ArrowKeyAction.Down),
            new KeyBinding(new[] {InputKey.D}, ArrowKeyAction.Left),
            new KeyBinding(new[] {InputKey.F}, ArrowKeyAction.Down),
            new KeyBinding(new[] {InputKey.J}, ArrowKeyAction.Up),
            new KeyBinding(new[] {InputKey.K}, ArrowKeyAction.Down)
        };
    }
}