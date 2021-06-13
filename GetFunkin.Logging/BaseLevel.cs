namespace GetFunkin.Logging
{
    /// <summary>
    ///     Basic working log level.
    /// </summary>
    public abstract class LogLevel : ILogLevel
    {
        /// <inheritdoc cref="ILogLevel.LevelName"/>
        public abstract string LevelName { get; }

        /// <inheritdoc cref="ILogLevel.Level"/>
        public abstract float Level { get; }
    }
}