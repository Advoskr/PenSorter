using System;

namespace PenSorter.Core
{
    public class NoLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {

        }

        public void Warn(string message)
        {

        }

        public void Debug(string message)
        {

        }

        public void Info(string message)
        {

        }

        public void Error(string message)
        {

        }

        public void Fatal(string message)
        {

        }

        public void Trace(string message)
        {

        }

        public void Warn<T>(string message)
        {

        }

        public void Debug<T>(string message)
        {

        }

        public void Info<T>(string message)
        {

        }

        public void Error<T>(string message)
        {

        }

        public void Fatal<T>(string message)
        {

        }

        public void Trace<T>(string message)
        {

        }

        public void Error(Exception exception)
        {

        }

        public void Fatal<T>(Exception exception)
        {

        }

        public void Error(string message, Exception exception)
        {

        }

        public void Flush()
        {

        }
    }
}