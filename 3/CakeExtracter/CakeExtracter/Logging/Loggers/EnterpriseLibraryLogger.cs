﻿using System;
using System.Diagnostics;
using CakeExtracter.Helpers;
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
                Message = string.Format(format, args),
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
            var innerExceptionMessages = exception.GetAllMessages();
            Write(LogEntry(TraceEventType.Error, "{0}{1}", exception.Message, innerExceptionMessages));
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
            var innerExceptionMessages = exception.GetAllMessages();
            var entry = LogEntry(TraceEventType.Error, "{0}{1}", exception.Message, innerExceptionMessages);
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