using osu.Framework;
using osu.Framework.Platform;

namespace Funkin.NET.Desktop
{
    /// <summary>
    ///     Entry-point class.
    /// </summary>
    public static class Launch
    {
        /// <summary>
        ///     Host name used to get a suitable host (<see cref="Host.GetSuitableHost"/>
        /// </summary>
        public const string SuitableHostName = "funkin.net";

        /// <summary>
        ///     Entry-point method, runs the game with a <see cref="Host"/> instance.
        /// </summary>
        public static void Main()
        {
             // TESTING :)
             //LegacySong song = LegacySong.FromJson("Json/Songs/test.json");
             //_ = song;

            using GameHost host = Host.GetSuitableHost(SuitableHostName);
            using FunkinDesktop game = new();
            host.Run(game);
        }
    }
}