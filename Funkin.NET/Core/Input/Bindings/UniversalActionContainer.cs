using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;

namespace Funkin.NET.Core.Input.Bindings
{
    public class UniversalActionContainer : StoredKeyBindingContainer<UniversalAction>, IHandleGlobalKeyboardInput
    {
        private readonly Drawable Handler;
        private InputManager ParentInputManager;

        public UniversalActionContainer(Storage storage, Drawable game) : base(storage, SimultaneousBindingMode.All,
            KeyCombinationMatchingMode.Modifiers)
        {
            if (game is IKeyBindingHandler<UniversalAction>)
                Handler = game;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ParentInputManager = GetContainingInputManager();
        }

        public override IEnumerable<IKeyBinding> FallbackKeyBindings => ArrowKeyBinds.Concat(SelectionKeyBinds);

        protected override bool SendRepeats => true;

        public static IEnumerable<KeyBinding> ArrowKeyBinds => new[]
        {
            new KeyBinding(new[] {InputKey.Left}, UniversalAction.Left),
            new KeyBinding(new[] {InputKey.Down}, UniversalAction.Down),
            new KeyBinding(new[] {InputKey.Up}, UniversalAction.Up),
            new KeyBinding(new[] {InputKey.Right}, UniversalAction.Right),
            new KeyBinding(new[] {InputKey.D}, UniversalAction.Left),
            new KeyBinding(new[] {InputKey.F}, UniversalAction.Down),
            new KeyBinding(new[] {InputKey.J}, UniversalAction.Up),
            new KeyBinding(new[] {InputKey.K}, UniversalAction.Right)
        };

        public static IEnumerable<KeyBinding> SelectionKeyBinds => new[]
        {
            new KeyBinding(new[] {InputKey.Enter}, UniversalAction.Select),
            new KeyBinding(new[] {InputKey.Space}, UniversalAction.Select)
        };

        protected override IEnumerable<Drawable> KeyBindingInputQueue
        {
            get
            {
                IEnumerable<Drawable> queue = ParentInputManager?.NonPositionalInputQueue ?? base.KeyBindingInputQueue;

                return Handler != null ? queue.Prepend(Handler) : queue;
            }
        }
    }
}