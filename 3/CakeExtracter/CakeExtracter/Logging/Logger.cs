using System;
using CakeExtracter.Logging.Loggers;

namespace CakeExtracter
{
    public static class Logger
    {
        public static ILogger Instance = new NullLogger();

        public static bool LogToOneFile = false;
        // Set this to true to force logging to the main log file even when accountId is passed in.
        // e.g. for ETL commands that use TDStrategySummaryLoader - until they're converted to processing accounts in parallel

        public static void Info(string format, params object[] args)
        {
            Instance.Info(format, args);
        }

        public static void Warn(string format, params object[] args)
        {
            Instance.Warn(format, args); 
        }

        public static void Error(Exception exception)
        {
            Instance.Error(exception);
        }

        public static void Info(int accountId, string format, params object[] args)
        {
            if (LogToOneFile || accountId < 0)
                Instance.Info(format, args);
            else
                Instance.Info(accountId, format, args);
        }

        public static void Warn(int accountId, string format, params object[] args)
        {
            if (LogToOneFile || accountId < 0)
                Instance.Warn(format, args);
            else
                Instance.Warn(accountId, format, args);
        }

        public static void Error(int accountId, Exception exception)
        {
            if (LogToOneFile || accountId < 0)
                Instance.Error(exception);
            else
                Instance.Error(accountId, exception);
        }

    }
}
