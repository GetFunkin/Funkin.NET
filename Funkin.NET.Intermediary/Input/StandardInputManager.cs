using osu.Framework.Input;
using osuTK.Input;

namespace Funkin.NET.Intermediary.Input
{
    public class StandardInputManager : UserInputManager
    {
        protected override MouseButtonEventManager CreateButtonEventManagerFor(MouseButton button)
        {
            return button == MouseButton.Right
                ? new RightMouseManager(button)
                : base.CreateButtonEventManagerFor(button);
        }
    }
}