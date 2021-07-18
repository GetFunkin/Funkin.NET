namespace Funkin.NET.Desktop
{
    /// <summary>
    ///     Entry-point class.
    /// </summary>
    public static class Launch
    {
        /// <summary>
        ///     Host name used to get a suitable host (<see cref="osu.Framework.Host.GetSuitableHost"/>
        /// </summary>
        public const string SuitableHostName = "funkin.net";

        /// <summary>
        ///     Entry-point method, runs the game with a <see cref="osu.Framework.Host"/> instance.
        /// </summary>
        public static void Main()
        {
            using osu.Framework.Platform.GameHost host = osu.Framework.Host.GetSuitableHost(SuitableHostName);
            FunkinGame.RunningHost = host;

            using FunkinGame game = new();
            FunkinGame.RunningGame = game;
            FunkinGame.RunningHost.Run(FunkinGame.RunningGame);
        }
    }
}