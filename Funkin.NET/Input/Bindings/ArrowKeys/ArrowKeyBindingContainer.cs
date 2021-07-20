using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Input.Bindings.ArrowKeys
{
    /// <summary>
    ///     Container for an arrow key action.
    /// </summary>
    public class ArrowKeyBindingContainer : FunkinBindingContainer<ArrowKeyAction>
    {
        public ArrowKeyBindingContainer(Drawable handler) : base(handler, SimultaneousBindingMode.All)
        {
        }

        /// <inheritdoc cref="KeyBindingContainer.DefaultKeyBindings"/>
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] {InputKey.Left}, ArrowKeyAction.Left),
            new KeyBinding(new[] {InputKey.Down}, ArrowKeyAction.Down),
            new KeyBinding(new[] {InputKey.Up}, ArrowKeyAction.Up),
            new KeyBinding(new[] {InputKey.Right}, ArrowKeyAction.Right),
            new KeyBinding(new[] {InputKey.D}, ArrowKeyAction.Left),
            new KeyBinding(new[] {InputKey.F}, ArrowKeyAction.Down),
            new KeyBinding(new[] {InputKey.J}, ArrowKeyAction.Up),
            new KeyBinding(new[] {InputKey.K}, ArrowKeyAction.Right)
        };
    }
}