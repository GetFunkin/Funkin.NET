using osu.Framework.Platform;
using osu.Framework;
using Funkin.NET.Game;

namespace Funkin.NET.Desktop
{
    public static class Program
    {
        public const string HOST_NAME = "funkin.net";

        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost(HOST_NAME);
            using osu.Framework.Game game = new FunkinGame();
            host.Run(game);
        }
    }
}
