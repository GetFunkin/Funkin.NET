using osu.Framework;
using osu.Framework.Platform;

namespace Funkin.NET.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost("funkin.net.tests");
            using Game game = new FunkinGameTestBrowser();
            host.Run(game);
        }
    }
}