using osu.Framework.Screens;

namespace Funkin.NET.Common.Screens
{
    public interface IDefaultScreen : IScreen
    {
        /// <summary>
        ///     Whether this <see cref="DefaultScreen"/> allows the cursor to be displayed.
        /// </summary>
        bool CursorVisible { get; }

        /// <summary>
        ///     Whether all overlays should be hidden when this screen is entered or resumed.
        /// </summary>
        bool HideOverlaysOnEnter { get; }

        /// <summary>
        ///     The amount of parallax to be applied while this screen is displayed.
        /// </summary>
        float BackgroundParallaxAmount { get; }
    }
}