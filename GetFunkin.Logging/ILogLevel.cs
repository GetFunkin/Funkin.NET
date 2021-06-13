namespace GetFunkin.Logging
{
    /// <summary>
    ///     Log level implementation.
    /// </summary>
    public interface ILogLevel
    {
        string LevelName { get; }

        /// <summary>
        ///     Level value ranging from <c>0f</c> to <c>1f</c>. <br />
        ///     Anything above or below defaults to the maximum or minimum value contextually.
        /// </summary>
        float Level { get; }
    }
}