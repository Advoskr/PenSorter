using System;

namespace PenSorter.Core
{
    public class DebugLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(level, message));
        }

        public void Warn(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Warn, message));
        }

        public void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Debug, message));
        }

        public void Info(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Info, message));
        }

        public void Error(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Error, message));
        }

        public void Fatal(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Fatal, message));
        }

        public void Trace(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Trace, message));
        }

        public void Warn<T>(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage<T>(LogLevel.Warn, message));
        }

        public void Debug<T>(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage<T>(LogLevel.Debug, message));
        }

        public void Info<T>(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage<T>(LogLevel.Info, message));
        }

        public void Error<T>(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage<T>(LogLevel.Error, message));
        }

        public void Fatal<T>(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage<T>(LogLevel.Fatal, message));
        }

        public void Trace<T>(string message)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage<T>(LogLevel.Trace, message));
        }

        public void Error(Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(exception));
        }

        public void Fatal<T>(Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(exception));
        }

        public void Error(string message, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(FormMessage(LogLevel.Error, message));
            System.Diagnostics.Debug.WriteLine(FormMessage(exception));
        }

        public void Flush()
        {
            //Output.Flush();
        }

        private string FormMessage(LogLevel level, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            //StackTrace stackTrace = new StackTrace();
            //var callingMethod = stackTrace.GetFrame(2).GetMethod().Name;
            return string.Format($"{memberName};{DateTime.Now} {level}:{message}");
        }

        private string FormMessage<T>(LogLevel level, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return string.Format($"{memberName};{DateTime.Now} {level}({typeof(T).FullName}):{message}");
        }
        private string FormMessage(Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return string.Format($"{memberName};{DateTime.Now} {LogLevel.Error}" +
                                 $":{exception.GetFullExceptionMessage()} {Environment.NewLine} {exception.StackTrace}");
        }
        private string FormFatalMessage<T>(Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return string.Format($"{memberName};{DateTime.Now} {LogLevel.Fatal}" +
                                 $"({typeof(T).FullName})" +
                                 $":{exception.GetFullExceptionMessage()} {Environment.NewLine} {exception.StackTrace}");
        }
    }
}