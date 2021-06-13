using System;

namespace GetFunkin.Logging
{
    public abstract class Logger : ILogger
    {
        public virtual IFileLogger FileLogger { get; }

        public virtual string LoggerName => "System";

        protected Logger(IFileLogger fileLogger)
        {
            FileLogger = fileLogger;
        }

        public void Log(object o, ILogLevel level)
        {
            string value = $"{DateTime.Now:[hh:mm:ss]} [{level.LevelName}] [{LoggerName}] {o}";
            Console.WriteLine(value);

            FileLogger.WriteLine(value);
        }
    }
}