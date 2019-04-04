using System;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Extensions;
using CakeExtracter.Logging.Loggers;
using CakeExtracter.Logging.Utils;

namespace CakeExtracter
{
    /// <summary>
    /// Logger entry point.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static ILogger Instance = new NullLogger();

        /// <summary>
        /// // Set this to true to force logging to the main log file even when accountId is passed in.
        // e.g. for ETL commands that use TDStrategySummaryLoader - until they're converted to processing accounts in parallel
        /// </summary>
        public static bool LogToOneFile = false;

        /// <summary>
        /// Log with info severity level.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Info(string format, params object[] args)
        {
            Instance.Info(format, args);
        }

        /// <summary>
        /// Log with warn severity level.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Warn(string format, params object[] args)
        {
            Instance.Warn(format, args);
            CommandExecutionContext.Current?.JobDataWriter?.LogWarningInHistory(LoggerUtils.GetLogMessage(format, args));
        }

        /// <summary>
        /// Log exception with error severity level.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Error(Exception exception)
        {
            Instance.Error(exception);
            CommandExecutionContext.Current?.JobDataWriter?.LogErrorInHistory(exception.GetAllExceptionMessages());
        }

        /// <summary>
        /// Log with info severity level with account id specification.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Info(int accountId, string format, params object[] args)
        {
            if (LogToOneFile || accountId < 0)
            {
                Instance.Info(format, args);
            }
            else
            {
                Instance.Info(accountId, format, args);
            }
        }

        /// <summary>
        /// Log with warn severity level with account specification.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void Warn(int accountId, string format, params object[] args)
        {
            if (LogToOneFile || accountId < 0)
            {
                Instance.Warn(format, args);
                CommandExecutionContext.Current?.JobDataWriter?.LogWarningInHistory(LoggerUtils.GetLogMessage(format, args));
            }
            else
            {
                Instance.Warn(accountId, format, args);
                CommandExecutionContext.Current?.JobDataWriter?.LogWarningInHistory(LoggerUtils.GetLogMessage(format, args), accountId);
            }
        }

        /// <summary>
        ///  Log exception with error severity level with account specification..
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(int accountId, Exception exception)
        {
            if (LogToOneFile || accountId < 0)
            {
                Instance.Error(exception);
                CommandExecutionContext.Current?.JobDataWriter?.LogErrorInHistory(exception.GetAllExceptionMessages());
            }
            else
            {
                Instance.Error(accountId, exception);
                CommandExecutionContext.Current?.JobDataWriter?.LogErrorInHistory(exception.GetAllExceptionMessages(), accountId);
            }
        }
    }
}
