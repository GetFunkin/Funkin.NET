using System;
using System.IO;

namespace GetFunkin.Logging
{
    public class FileLogger : IFileLogger
    {
        public StreamWriter FileStream { get; }

        public string FilePath { get; }

        public FileLogger(string filePath)
        {
            FilePath = filePath;

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath) ?? throw new NullReferenceException("Directory provided for logging was null!"));

            FileStream = new StreamWriter(FilePath);
        }

        public void Write(object o) => FileStream.Write(o);

        public void WriteLine(object o) => FileStream.Write($"{o}\n");
        
        public void Dispose()
        {
            // TODO: needed?
            GC.SuppressFinalize(this);

            FileStream.Dispose();
        }
    }
}