using osu.Framework;
using osu.Framework.Platform;

namespace Funkin.NET.Game.Tests
{
    public static class Program
    {
        public const string HOST_NAME = "visual-tests";

        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost(HOST_NAME);
            using FunkinTestBrowser game = new FunkinTestBrowser();
            host.Run(game);
        }
    }
}
