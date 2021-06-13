namespace GetFunkin.Logging.Loggers
{
    public class ConsoleLogger : Logger
    {
        public override string LoggerName { get; }

        public ConsoleLogger(IFileLogger fileLogger, string loggerName) : base(fileLogger)
        {
            LoggerName = loggerName;
        }
    }
}