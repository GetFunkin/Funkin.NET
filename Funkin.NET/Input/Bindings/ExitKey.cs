using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Input.Bindings
{
    public class ExitKey : FunkinBindingContainer<ExitKey.ExitAction>
    {
        public enum ExitAction
        {
            Escape
        }

        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(InputKey.Escape, ExitAction.Escape)
        };

        public ExitKey([NotNull] Drawable handler) : base(handler)
        {
        }
    }
}