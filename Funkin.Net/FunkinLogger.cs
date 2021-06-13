using System;
using System.IO;
using System.Reflection;
using GetFunkin.Logging;
using GetFunkin.Logging.Loggers;
using GetFunkin.Logging.LogLevels;

namespace Funkin.NET
{
    /// <summary>
    ///     Funkin logging manager.
    /// </summary>
    public static class FunkinLogger
    {
        public static IFileLogger FileLogger { get; internal set; } = null!;

        public static ILogger Logger { get; private set; } = null!;

        internal static void Initialize()
        {
            Logger = new ConsoleLogger(FileLogger, "Funkin'");
            Logger.Log("Initialized logging!", Levels.Info);
        }

        internal static void SaveAndExit()
        {
            File.Copy(FileLogger.FilePath,
                Path.Combine(Path.GetDirectoryName(FileLogger.FilePath)!,
                    $"log_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
        }
    }
}