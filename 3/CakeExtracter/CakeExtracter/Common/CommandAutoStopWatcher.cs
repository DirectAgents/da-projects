using System;
using System.Configuration;
using System.Threading;
using CakeExtracter.Common.JobExecutionManagement;

namespace CakeExtracter.Common
{
    /// <summary>
    /// Command automated stop watcher(singleton).
    /// </summary>
    public sealed class CommandAutoStopWatcher : IDisposable
    {
        private static CommandAutoStopWatcher current = null;

        private Timer watcherTimer;

        private static readonly object WatcherLock = new object();

        private CommandAutoStopWatcher()
        {
        }

        /// <summary>
        /// Gets the singleton instance object.
        /// </summary>
        /// <value>
        /// The singleton object.
        /// </value>
        public static CommandAutoStopWatcher Current
        {
            get
            {
                lock (WatcherLock)
                {
                    if (current == null)
                    {
                        current = new CommandAutoStopWatcher();
                    }
                    return current;
                }
            }
        }

        /// <summary>
        /// Setups the automated shut down.
        /// </summary>
        public void SetupAutomatedShutDown()
        {
            var dueTimeInMilliseconds = GetDueTimeInMilliseconds();
            watcherTimer = new Timer(ExecutionTimeoutCallback, null, dueTimeInMilliseconds, Timeout.Infinite);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            watcherTimer?.Dispose();
        }

        private int GetDueTimeInMilliseconds()
        {
            var valueFromConfigInHours = GetMaxExecutionTimeInHoursConfigValue();
            return valueFromConfigInHours != Timeout.Infinite
                ? valueFromConfigInHours * 60 * 60 * 1000 // converting from hours to milliseconds
                : Timeout.Infinite; // if -1 defined in configuration don't need convert to milliseconds
        }

        private int GetMaxExecutionTimeInHoursConfigValue()
        {
            const string commandMaxExecutionTimeConfigurationPrefix = "CommandMaxExecutionTimeInHours_";
            if (ConfigurationManager.AppSettings[$"{commandMaxExecutionTimeConfigurationPrefix}{CommandExecutionContext.Current.CommandName}"] != null)
            {
                return int.TryParse(ConfigurationManager.AppSettings[$"{commandMaxExecutionTimeConfigurationPrefix}{CommandExecutionContext.Current.CommandName}"], out int result)
                    ? result
                    : Timeout.Infinite;
            }
            else
            {
                return GetDefaultMaxExecutionTimeForAllCommands();
            }
        }

        private int GetDefaultMaxExecutionTimeForAllCommands()
        {
            const string defaultMaxExecutionTimeConfigurationKey = "CommandMaxExecutionTimeInHours_Default";
            const int defaultMaxExecutionTimeInHours = 10;
            return int.TryParse(ConfigurationManager.AppSettings[defaultMaxExecutionTimeConfigurationKey], out int result)
                   ? result
                   : defaultMaxExecutionTimeInHours;
        }

        private void ExecutionTimeoutCallback(object o)
        {
            Logger.Error(new ApplicationException($"Execution timeout expired. Process will be automatically stopped"));
            CommandExecutionContext.Current.SetAsAbortedByTimeoutRequestExecution();
            watcherTimer?.Dispose();
            Environment.Exit(0);
        }
    }
}
