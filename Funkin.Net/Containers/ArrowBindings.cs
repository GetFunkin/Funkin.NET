using osu.Framework.Input.Bindings;

namespace Funkin.NET.Containers
{
    public static class ArrowBindings
    {
        public static ArrowKeyBindingContainer Up = new(new[] {InputKey.W, InputKey.Up}, ArrowKeyDirection.Up);

        public static ArrowKeyBindingContainer Down = new(new[] {InputKey.S, InputKey.Down}, ArrowKeyDirection.Down);

        public static ArrowKeyBindingContainer Left = new(new[] {InputKey.A, InputKey.Left}, ArrowKeyDirection.Left);

        public static ArrowKeyBindingContainer Right = new(new[] {InputKey.D, InputKey.Right}, ArrowKeyDirection.Right);
    }
}