using System.IO;
using Funkin.NET.Game;
using osu.Framework.Platform;

namespace Funkin.NET.Desktop
{
    public class FunkinGameDesktop : FunkinGame
    {
        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Stream? iconStream = GetType().Assembly.GetManifestResourceStream(GetType(), "icon.ico");
            SDL2DesktopWindow desktopWindow = (SDL2DesktopWindow)host.Window;

            desktopWindow.CursorState |= CursorState.Hidden;
            desktopWindow.SetIconFromStream(iconStream);
            desktopWindow.Title = Name;
        }
    }
}
