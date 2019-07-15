using System;
using System.Configuration;
using System.Threading;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces;

namespace CakeExtracter.Common.JobExecutionManagement
{
    /// <inheritdoc />
    /// <summary>
    /// Command automated stop watcher(singleton).
    /// </summary>
    public sealed class CommandAutoStopWatcher : IDisposable
    {
        private static CommandAutoStopWatcher current;

        private readonly IProcessManager processManager;
        private Timer watcherTimer;

        private static readonly object WatcherLock = new object();

        private CommandAutoStopWatcher()
        {
            processManager = DIKernel.Get<IProcessManager>();
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
                    return current ?? (current = new CommandAutoStopWatcher());
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
                ? (int)TimeSpan.FromHours(valueFromConfigInHours).TotalMilliseconds
                : Timeout.Infinite; // if -1 defined in configuration don't need convert to milliseconds
        }

        private int GetMaxExecutionTimeInHoursConfigValue()
        {
            const string commandMaxExecutionTimeConfigurationPrefix = "CommandMaxExecutionTimeInHours_";
            var maxExecutionTimeConfigName = $"{commandMaxExecutionTimeConfigurationPrefix}{CommandExecutionContext.Current.CommandName}";
            var maxExecutionTimeConfigValue = ConfigurationManager.AppSettings[maxExecutionTimeConfigName];
            if (maxExecutionTimeConfigValue != null)
            {
                return int.TryParse(maxExecutionTimeConfigValue, out var result)
                    ? result
                    : Timeout.Infinite;
            }

            return GetDefaultMaxExecutionTimeForAllCommands();
        }

        private int GetDefaultMaxExecutionTimeForAllCommands()
        {
            const string defaultMaxExecutionTimeConfigurationKey = "CommandMaxExecutionTimeInHours_Default";
            const int defaultMaxExecutionTimeInHours = 10;
            return int.TryParse(ConfigurationManager.AppSettings[defaultMaxExecutionTimeConfigurationKey], out var result)
                   ? result
                   : defaultMaxExecutionTimeInHours;
        }

        private void ExecutionTimeoutCallback(object o)
        {
            Logger.Error(new ApplicationException("Execution timeout expired. Process will be automatically stopped"));
            CommandExecutionContext.Current.SetAsAbortedByTimeoutRequestExecution();
            watcherTimer?.Dispose();
            processManager.EndCurrentProcess();
        }
    }
}
