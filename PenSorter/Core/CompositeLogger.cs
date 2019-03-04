using System;
using System.Collections.Generic;

namespace PenSorter.Core
{
    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(List<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public void Log(LogLevel level, string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(level,message);
            }
        }

        public void Warn(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Warn(message);
            }
        }

        public void Debug(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Debug(message);
            }
        }

        public void Info(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Info(message);
            }
        }

        public void Error(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(message);
            }
        }

        public void Fatal(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal(message);
            }
        }

        public void Trace(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Trace(message);
            }
        }

        public void Warn<T>(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Warn<T>(message);
            }
        }

        public void Debug<T>(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Debug<T>(message);
            }
        }

        public void Info<T>(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Info<T>(message);
            }
        }

        public void Error<T>(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Error<T>(message);
            }
        }

        public void Fatal<T>(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal<T>(message);
            }
        }

        public void Trace<T>(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Trace<T>(message);
            }
        }

        public void Error(Exception exception)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(exception);
            }
        }

        public void Fatal<T>(Exception exception)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal<T>(exception);
            }
        }

        public void Error(string message, Exception exception)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(message, exception);
            }
        }

        public void Flush()
        {
            foreach (var logger in _loggers)
            {
                logger.Flush();
            }
        }
    }
}