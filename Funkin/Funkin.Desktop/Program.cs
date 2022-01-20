using osu.Framework.Platform;
using osu.Framework;
using Funkin.Game;

namespace Funkin.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"Funkin"))
            using (osu.Framework.Game game = new FunkinGame())
                host.Run(game);
        }
    }
}
