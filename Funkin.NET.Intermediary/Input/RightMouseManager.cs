using osu.Framework.Input;
using osuTK.Input;

namespace Funkin.NET.Intermediary.Input
{
    /// <summary>
    ///     Simplistic <see cref="MouseButtonEventManager"/> for allowing right-click dragging.
    /// </summary>
    public class RightMouseManager : MouseButtonEventManager
    {
        public RightMouseManager(MouseButton button) : base(button)
        {
        }
        
        public override bool EnableDrag => true;

        public override bool EnableClick => false;

        public override bool ChangeFocusOnClick => false;
    }
}