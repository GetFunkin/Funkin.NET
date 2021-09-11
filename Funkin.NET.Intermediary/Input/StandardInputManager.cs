using osu.Framework.Input;
using osuTK.Input;

namespace Funkin.NET.Intermediary.Input
{
    /// <summary>
    ///     Standard <see cref="UserInputManager"/> that uses <see cref="RightMouseManager"/>/
    /// </summary>
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