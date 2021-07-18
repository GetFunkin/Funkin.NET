using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Common.KeyBinds.WindowModeKeys
{
    /// <summary>
    ///     Container for key binds that cycle window mods.
    /// </summary>
    public class WindowModeKeyBindingContainer : KeyBindingContainer<WindowModeKeyAction>
    {
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new KeyCombination(InputKey.Alt, InputKey.Enter), WindowModeKeyAction.Switch)
        };
    }
}