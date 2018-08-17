using System;

namespace CakeExtracter
{
    public interface ILogger
    {
        void Info(string format, params object[] args);

        void Warn(string format, params object[] args);

        void Error(Exception exception);

        void Trace(string format, params object[] args);

        void Info(int accountId, string format, params object[] args);

        void Warn(int accountId, string format, params object[] args);

        void Error(int accountId, Exception exception);

        void Trace(int accountId, string format, params object[] args);

    }
}