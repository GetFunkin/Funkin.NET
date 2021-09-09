using osu.Framework.Input;
using osuTK.Input;

namespace Funkin.NET.Intermediary.Input
{
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