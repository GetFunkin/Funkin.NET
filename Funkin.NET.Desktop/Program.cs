using osu.Framework.Platform;
using osu.Framework;

namespace Funkin.NET.Desktop
{
    public static class Program
    {
        public const string HOST_NAME = "funkin.net";

        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost(HOST_NAME);
            using osu.Framework.Game game = new FunkinGameDesktop();
            host.Run(game);
        }
    }
}
