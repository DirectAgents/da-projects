using System;

namespace CakeExtracter.Logging.Loggers
{
    class NullLogger : ILogger
    {
        void ILogger.Info(string format, params object[] args)
        {
        }

        void ILogger.Warn(string format, params object[] args)
        {
        }

        void ILogger.Error(Exception exception)
        {
        }

        void ILogger.Trace(string format, params object[] args)
        {
        }

        void ILogger.Info(int accountId, string format, params object[] args)
        {
        }

        void ILogger.Warn(int accountId, string format, params object[] args)
        {
        }

        void ILogger.Error(int accountId, Exception exception)
        {
        }

        void ILogger.Trace(int accountId, string format, params object[] args)
        {
        }
    }
}