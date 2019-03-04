using System;

namespace PenSorter.Core
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine(FormMessage(level, message));
        }

        public void Warn(string message)
        {
            Console.WriteLine(FormMessage(LogLevel.Warn, message));
        }

        public void Debug(string message)
        {
            Console.WriteLine(FormMessage(LogLevel.Debug, message));
        }

        public void Info(string message)
        {
            Console.WriteLine(FormMessage(LogLevel.Info, message));
        }

        public void Error(string message)
        {
            Console.WriteLine(FormMessage(LogLevel.Error, message));
        }

        public void Fatal(string message)
        {
            Console.WriteLine(FormMessage(LogLevel.Fatal, message));
        }

        public void Trace(string message)
        {
            Console.WriteLine(FormMessage(LogLevel.Trace, message));
        }

        public void Warn<T>(string message)
        {
            Console.WriteLine(FormMessage<T>(LogLevel.Warn, message));
        }

        public void Debug<T>(string message)
        {
            Console.WriteLine(FormMessage<T>(LogLevel.Debug, message));
        }

        public void Info<T>(string message)
        {
            Console.WriteLine(FormMessage<T>(LogLevel.Info, message));
        }

        public void Error<T>(string message)
        {
            Console.WriteLine(FormMessage<T>(LogLevel.Error, message));
        }

        public void Fatal<T>(string message)
        {
            Console.WriteLine(FormMessage<T>(LogLevel.Fatal, message));
        }

        public void Trace<T>(string message)
        {
            Console.WriteLine(FormMessage<T>(LogLevel.Trace, message));
        }

        public void Error(Exception exception)
        {
            Console.WriteLine(FormMessage(exception));
        }

        public void Fatal<T>(Exception exception)
        {
            Console.WriteLine(FormMessage(exception));
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine(FormMessage(LogLevel.Error, message));
            Console.WriteLine(FormMessage(exception));
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
            return string.Format($"{message}");
        }

        private string FormMessage<T>(LogLevel level, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return string.Format($"{message}");
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