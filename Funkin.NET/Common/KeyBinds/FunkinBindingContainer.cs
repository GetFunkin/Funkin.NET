#nullable enable
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;

namespace Funkin.NET.Common.KeyBinds
{
    public abstract class FunkinBindingContainer<T> : KeyBindingContainer<T> where T : struct
    {
        public Drawable? Handler { get; }

        protected FunkinBindingContainer(Drawable handler,
            SimultaneousBindingMode simultaneousMode = SimultaneousBindingMode.None,
            KeyCombinationMatchingMode matchingMode = KeyCombinationMatchingMode.Any) : base(simultaneousMode,
            matchingMode)
        {
            if (handler is IKeyBindingHandler<T>)
                Handler = handler;
        }

        protected override IEnumerable<Drawable> KeyBindingInputQueue
        {
            get
            {
                IEnumerable<Drawable>? inputQueue = GetContainingInputManager()?.NonPositionalInputQueue;
                inputQueue ??= base.KeyBindingInputQueue;

                return Handler is not null ? inputQueue.Prepend(Handler) : inputQueue;
            }
        }
    }
}