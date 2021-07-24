using osu.Framework.Input;
using osuTK.Input;

namespace Funkin.NET.Input
{
    public class FunkinUserInputManager : UserInputManager
    {
        protected override MouseButtonEventManager CreateButtonEventManagerFor(MouseButton button)
        {
            return button == MouseButton.Right
                ? new RightMouseManager(button)
                : base.CreateButtonEventManagerFor(button);
        }

        public class RightMouseManager : MouseButtonEventManager
        {
            public RightMouseManager(MouseButton button) : base(button)
            {
            }

            // absolute scroll like in osu soon
            public override bool EnableDrag => true;

            public override bool EnableClick => false;

            public override bool ChangeFocusOnClick => false;
        }
    }
}