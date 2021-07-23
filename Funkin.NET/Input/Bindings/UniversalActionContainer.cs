using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;

namespace Funkin.NET.Input.Bindings
{
    public class UniversalActionContainer : StoredKeyBindingContainer<UniversalAction>, IHandleGlobalKeyboardInput
    {
        private readonly Drawable _handler;
        private InputManager _parentInputManager;

        public UniversalActionContainer(Storage storage, Drawable game) : base(storage, SimultaneousBindingMode.All,
            KeyCombinationMatchingMode.Modifiers)
        {
            if (game is IKeyBindingHandler<UniversalAction>)
                _handler = game;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            _parentInputManager = GetContainingInputManager();
        }

        public override IEnumerable<IKeyBinding> FallbackKeyBindings => ArrowKeyBinds.Concat(SelectionKeyBinds);

        public IEnumerable<KeyBinding> ArrowKeyBinds => new[]
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

        public IEnumerable<KeyBinding> SelectionKeyBinds => new[]
        {
            new KeyBinding(new[] {InputKey.Enter}, UniversalAction.Select),
            new KeyBinding(new[] {InputKey.Space}, UniversalAction.Select)
        };

        protected override IEnumerable<Drawable> KeyBindingInputQueue
        {
            get
            {
                IEnumerable<Drawable> queue = _parentInputManager?.NonPositionalInputQueue ?? base.KeyBindingInputQueue;

                return _handler != null ? queue.Prepend(_handler) : queue;
            }
        }
    }

    public enum UniversalAction
    {
        [Description("Left arrow key.")] Left = 0,

        [Description("Down arrow key.")] Down = 1,

        [Description("Up arrow key.")] Up = 2,

        [Description("Right arrow key.")] Right = 3,

        [Description("Menu confirmation button.")]
        Select
    }
}