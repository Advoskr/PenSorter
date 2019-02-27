using System;
using System.Collections.Generic;
using System.Text;

namespace PenSorter.Core
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);

        void Warn(string message);
        void Debug(string message);
        void Info(string message);
        void Error(string message);
        void Fatal(string message);
        void Trace(string message);

        void Warn<T>(string message);
        void Debug<T>(string message);
        void Info<T>(string message);
        void Error<T>(string message);
        void Fatal<T>(string message);
        void Trace<T>(string message);

        void Error(Exception exception);
        void Fatal<T>(Exception exception);
        void Error(string message, Exception exception);
        void Flush();
    }
}
