using osu.Framework;
using osu.Framework.Platform;

namespace Funkin.NET.Desktop
{
    /// <summary>
    ///     Desktop entry-point class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     Host name used to get a suitable host (<see cref="Host.GetSuitableHost"/>
        /// </summary>
        public const string SuitableHostName = FunkinGameBase.ClientName;

        /// <summary>
        ///     Entry-point method, runs the game with a <see cref="Host"/> instance.
        /// </summary>
        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost(SuitableHostName);
            using FunkinDesktop game = new();
            host.Run(game);
        }
    }
}