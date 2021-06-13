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
        ///     Active <see cref="osu.Framework.Platform.GameHost"/> instance. Resolved at runtime in <see cref="Main"/>.
        /// </summary>
        public static osu.Framework.Platform.GameHost RunningHost { get; private set; }

        /// <summary>
        ///     Active <see cref="FunkinGame"/> instance. Resolved at runtime in <see cref="Main"/>.
        /// </summary>
        public static FunkinGame RunningGame { get; private set; }

        /// <summary>
        ///     Entry-point method, runs the game with a <see cref="osu.Framework.Host"/> instance.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            using osu.Framework.Platform.GameHost host = osu.Framework.Host.GetSuitableHost(SuitableHostName);
            RunningHost = host;

            using osu.Framework.Game game = new FunkinGame();
            RunningGame = (FunkinGame) game;

            RunningHost.Run(RunningGame);
        }
    }
}