namespace GetFunkin.Logging
{
    public interface ILogger
    {
        IFileLogger FileLogger { get; }

        string LoggerName { get; }

        void Log(object o, ILogLevel level);
    }
}