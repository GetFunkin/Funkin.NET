using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Containers
{
    public class ArrowKeyBindingContainer : KeyBindingContainer<ArrowKeyDirection>
    {
        public InputKey[] Keys { get; }

        public ArrowKeyDirection ArrowType { get; }

        public ArrowKeyBindingContainer(InputKey[] keys, ArrowKeyDirection arrow)
        {
            Keys = keys;
            ArrowType = arrow;
        }

        public override IEnumerable<IKeyBinding> DefaultKeyBindings =>
            Keys.Select(key => new KeyBinding(key, ArrowType)).ToList();
    }
}