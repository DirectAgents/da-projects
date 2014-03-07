using System;

namespace DirectAgents.Common.Logging.Loggers
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

        public void Trace(string format, params object[] args)
        {
        }
    }
}
