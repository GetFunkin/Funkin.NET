using System.IO;
using System.Reflection;
using osu.Framework.Platform;

namespace Funkin.NET.Desktop
{
    public class FunkinDesktop : FunkinGame
    {
        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            
            host.Window.Title = ProgramName;

            if (host.Window is not SDL2DesktopWindow desktopWindow)
                return;

            desktopWindow.CursorState |= CursorState.Hidden;

            using Stream icon = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType(), "Icon.ico");
            desktopWindow.SetIconFromStream(icon);
        }
    }
}