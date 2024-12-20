﻿using System;
using System.Diagnostics;
using CakeExtracter.Extensions;
using CakeExtracter.Logging.Utils;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CakeExtracter.Logging.Loggers
{
    public class EnterpriseLibraryLogger : ILogger
    {
        private string jobName = string.Empty;

        public EnterpriseLibraryLogger(string jobName = "")
        {
            this.jobName = jobName;
        }

        private void Write(LogEntry logEntry)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);
        }

        private LogEntry LogEntry(TraceEventType severity, string format, params object[] args)
        {
            var logEntry = new LogEntry
            {
                Message = LoggerUtils.GetLogMessage(format, args),
                Severity = severity,
            };
            logEntry.ExtendedProperties["JobName"] = jobName;
            return logEntry;
        }

        public void Info(string format, params object[] args)
        {
            Write(LogEntry(TraceEventType.Information, format, args));
        }

        public void Warn(string format, params object[] args)
        {
            Write(LogEntry(TraceEventType.Warning, format, args));
        }

        public void Error(Exception exception)
        {
            Write(LogEntry(TraceEventType.Error, "{0}", exception.GetAllExceptionMessages()));
        }

        public void Trace(string format, params object[] args)
        {
            Write(LogEntry(TraceEventType.Verbose, format, args));
        }

        public void Info(int accountId, string format, params object[] args)
        {
            var entry = LogEntry(TraceEventType.Information, format, args);
            entry.ExtendedProperties.Add("AccountId", accountId);
            Write(entry);
        }

        public void Warn(int accountId, string format, params object[] args)
        {
            var entry = LogEntry(TraceEventType.Warning, format, args);
            entry.ExtendedProperties.Add("AccountId", accountId);
            Write(entry);
        }

        public void Error(int accountId, Exception exception)
        {
            var entry = LogEntry(TraceEventType.Error, "{0}", exception.GetAllExceptionMessages());
            entry.ExtendedProperties.Add("AccountId", accountId);
            Write(entry);
        }

        public void Trace(int accountId, string format, params object[] args)
        {
            var entry = LogEntry(TraceEventType.Verbose, format, args);
            entry.ExtendedProperties.Add("AccountId", accountId);
            Write(entry);
        }
    }
}