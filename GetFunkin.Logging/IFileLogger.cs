using System;
using System.IO;

namespace GetFunkin.Logging
{
    public interface IFileLogger : IDisposable
    {
        StreamWriter FileStream { get; }

        string FilePath { get; }

        void Write(object o);

        void WriteLine(object o);
    }
}