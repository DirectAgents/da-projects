using System;
using CakeExtracter.Helpers;

namespace CakeExtracter.Logging.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Console.WriteLine("Warn: " + format, args);
        }

        public void Error(Exception exception)
        {
            Console.WriteLine(GetErrorExceptionMessage(exception));
        }

        public void Trace(string format, params object[] args)
        {
            Console.WriteLine("Trace: " + format, args);
        }

        public void Info(int accountId, string format, params object[] args)
        {
            Console.WriteLine("Info: " + format, args);
        }

        public void Warn(int accountId, string format, params object[] args)
        {
            Console.WriteLine("Warn: " + format, args);
        }

        public void Error(int accountId, Exception exception)
        {
            Console.WriteLine(GetErrorExceptionMessage(exception));
        }

        public void Trace(int accountId, string format, params object[] args)
        {
            Console.WriteLine("Trace: " + format, args);
        }

        private string GetErrorExceptionMessage(Exception exception)
        {
            var innerExceptionMessages = exception.GetAllMessages();
            return $"Exception: {exception.Message}{innerExceptionMessages}";
        }
    }
}