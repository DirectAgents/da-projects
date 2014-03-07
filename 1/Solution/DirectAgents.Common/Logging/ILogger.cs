using System;

namespace DirectAgents.Common.Logging
{
    public interface ILogger
    {
        void Info(string format, params object[] args);

        void Warn(string format, params object[] args);

        void Error(Exception exception);

        void Trace(string format, params object[] args);
    }
}
