using System.IO;
using System.Reflection;
using osu.Framework;
using osu.Framework.Platform;

namespace Funkin.NET.Desktop
{
    /// <summary>
    ///     Desktop <see cref="FunkinGame"/> instance. <br />
    ///     Provides window title, cursor settings, and icon.
    /// </summary>
    public class FunkinDesktop : FunkinGame
    {
        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            
            host.Window.Title = "Funkin.NET";

            if (host.Window is not SDL2DesktopWindow desktopWindow)
                return;

            desktopWindow.CursorState |= CursorState.Hidden;

            using Stream icon = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType(), "Icon.ico");
            desktopWindow.SetIconFromStream(icon);
        }
    }
}