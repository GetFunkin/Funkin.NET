using System.Collections.Generic;

namespace GetFunkin.Logging.LogLevels
{
    // TODO: do stuff with level floats

    /// <summary>
    ///     Log level container.
    /// </summary>
    public static class Levels
    {
        /// <summary>
        ///     Simplistic log level.
        /// </summary>
        public class EasyLogLevel : LogLevel
        {
            /// <inheritdoc cref="LogLevel.LevelName"/>
            public override string LevelName { get; }

            /// <inheritdoc cref="LogLevel.Level"/>
            public override float Level { get; }

            /// <summary>
            /// </summary>
            /// <param name="levelName"><see cref="LevelName"/></param>
            /// <param name="level"><see cref="Level"/></param>
            public EasyLogLevel(string levelName, float level)
            {
                LevelName = levelName;
                Level = level;
            }
        }

        /// <summary>
        /// </summary>
        public static Dictionary<LogLevel, float> DefaultLogLevelLevels => new()
        {
            {Info, Info.Level}
        };

        /// <summary>
        /// </summary>
        public static LogLevel Verbose => new EasyLogLevel("Verbose", 1f);

        /// <summary>
        /// </summary>
        public static LogLevel Info => new EasyLogLevel("Info", 0.9f);

        /// <summary>
        /// </summary>
        public static LogLevel Debug => new EasyLogLevel("Debug", 0.75f);

        /// <summary>
        /// </summary>
        public static LogLevel Warn => new EasyLogLevel("Warn", 0.5f);

        /// <summary>
        /// </summary>
        public static LogLevel Error => new EasyLogLevel("Error", 0.25f);
    }
}