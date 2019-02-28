using System;

namespace PenSorter.Core
{
    public class NLogger : ILogger
    {
        public NLogger()
        {
            //get calling class name somehow...
            Name = "";
        }

        public string Name { get; set; }

        public NLog.Logger Logger { get { return NLog.LogManager.GetLogger(Name); } }

        public void Log(LogLevel level, string message)
        {
            NLog.LogLevel nlogLevel = NLog.LogLevel.Off;
            switch (level)
            {
                case LogLevel.Debug:
                    nlogLevel = NLog.LogLevel.Debug;
                    break;
                case LogLevel.Info:
                    nlogLevel = NLog.LogLevel.Info;
                    break;
                case LogLevel.Warn:
                    nlogLevel = NLog.LogLevel.Warn;
                    break;
                case LogLevel.Fatal:
                    nlogLevel = NLog.LogLevel.Fatal;
                    break;
                case LogLevel.Trace:
                    nlogLevel = NLog.LogLevel.Trace;
                    break;
                case LogLevel.Error:
                    nlogLevel = NLog.LogLevel.Error;
                    break;
            }
            Logger.Log(nlogLevel, message);
        }

        public void Warn(string message)
        {
            Logger.Log(NLog.LogLevel.Warn, message);
        }

        public void Debug(string message)
        {
            Logger.Log(NLog.LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Logger.Log(NLog.LogLevel.Info, message);
        }

        public void Error(string message)
        {
            Logger.Log(NLog.LogLevel.Error, message);
        }

        public void Fatal(string message)
        {
            Logger.Log(NLog.LogLevel.Fatal, message);
        }

        public void Trace(string message)
        {
            Logger.Log(NLog.LogLevel.Trace, message);
        }

        public void Warn<T>(string message)
        {
            Logger.Log(NLog.LogLevel.Warn, message);
        }

        public void Debug<T>(string message)
        {
            Logger.Log(NLog.LogLevel.Debug, message);
        }

        public void Info<T>(string message)
        {
            Logger.Log(NLog.LogLevel.Info, message);
        }

        public void Error<T>(string message)
        {
            Logger.Log(NLog.LogLevel.Error, message);
        }

        public void Fatal<T>(string message)
        {
            Logger.Log(NLog.LogLevel.Fatal, message);
        }

        public void Trace<T>(string message)
        {
            Logger.Log(NLog.LogLevel.Trace, message);
        }

        public void Error(Exception exception)
        {
            Logger.Log(NLog.LogLevel.Error, exception.GetFullExceptionMessage()
                                            + Environment.NewLine + exception.StackTrace);
        }

        public void Fatal<T>(Exception exception)
        {
            Logger.Log(NLog.LogLevel.Fatal, exception.GetFullExceptionMessage()
                                            + Environment.NewLine + exception.StackTrace);
        }

        public void Error(string message, Exception exception)
        {
            Logger.Log(NLog.LogLevel.Error, message + Environment.NewLine +
                                            exception.GetFullExceptionMessage()
                                            + Environment.NewLine + exception.StackTrace);
        }

        public void Flush()
        {
            Logger.Factory.Flush();
        }
    }
}