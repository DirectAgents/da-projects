using System;
using System.Collections.Generic;
using System.Reflection;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;

namespace CakeExtracter.Common
{
    /// <summary>
    /// The base class for a console command.
    /// </summary>
    public abstract class ConsoleCommand : ManyConsole.ConsoleCommand, ICloneable
    {
        /// <summary>
        /// The request identifier argument name.
        /// </summary>
        public const string RequestIdArgumentName = "jobRequestId";

        /// <summary>
        /// The no need to create repeat requests argument name.
        /// </summary>
        public const string NoNeedToCreateRepeatRequestsArgumentName = "noRepeatedRequests";

        /// <summary>
        /// Gets or sets the interval between the unsuccessful and the new request in minutes for the child requests of this command.
        /// </summary>
        public int IntervalBetweenUnsuccessfulAndNewRequestInMinutes { get; set; } = 0;

        /// <summary>
        /// Gets or sets command argument: The identifier for the job request that initiates this command run.
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether command argument: The flag that indicates that there is no need to create repeated requests.
        /// </summary>
        public bool NoNeedToCreateRepeatRequests { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic shut down mechanism enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is automatic shut down mechanism enabled; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAutoShutDownMechanismEnabled { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCommand"/> class.
        /// The constructor sets base command arguments names and provides a description for them.
        /// </summary>
        protected ConsoleCommand()
        {
            HasOption<int>($"{RequestIdArgumentName}=", "Job Request Id (default = null)", c => RequestId = c);
            HasOption<bool>($"{NoNeedToCreateRepeatRequestsArgumentName}=", "No need to create repeated requests. (default = false)", c => NoNeedToCreateRepeatRequests = c);
        }

        /// <summary>
        /// Runs the command actions.
        /// </summary>
        /// <returns>Execution code.</returns>
        public int Run()
        {
            return Run(null);
        }

        /// <summary>
        /// Runs the command actions with the specified remaining arguments.
        /// </summary>
        /// <param name="remainingArguments">The remaining arguments.</param>
        /// <returns><Execution code.</returns>
        public override int Run(string[] remainingArguments)
        {
            var commandName = GetType().Name;
            Logger.Info("Executing command: {0}", commandName);
            using (new LogElapsedTime("for " + commandName))
            {
                return ExecuteJobWithContext(remainingArguments);
            }
        }

        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public virtual void ResetProperties()
        {
        }

        /// <summary>
        /// The method runs the current command based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments">The remaining arguments.</param>
        /// <returns>Execution code</returns>
        public abstract int Execute(string[] remainingArguments);

        /// <summary>
        /// Gets the argument properties.
        /// </summary>
        /// <returns>Collection of argument properties.</returns>
        public virtual IEnumerable<PropertyInfo> GetArgumentProperties()
        {
            return GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Tries to set the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns>Flag indicating whether property set was successful.</returns>
        public virtual bool TrySetProperty(string propertyName, object propertyValue)
        {
            var property = GetType().GetProperty(propertyName);
            if (property == null)
            {
                return false;
            }

            property.SetValue(this, propertyValue);
            return true;
        }

        /// <summary>
        /// Filters scheduled commands and returns only those commands that will not return duplicate data.
        /// </summary>
        /// <param name="commands">The source scheduled commands.</param>
        /// <returns>The broad commands to extract unique data.</returns>
        public virtual IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(IEnumerable<CommandWithSchedule> commands)
        {
            return commands;
        }

        /// <summary>
        /// Resets the command execution context.
        /// </summary>
        public virtual void PrepareCommandExecutionContext()
        {
            CommandExecutionContext.ResetContext(this);
            if (IsAutoShutDownMechanismEnabled)
            {
                CommandAutoStopWatcher.Current.SetupAutomatedShutDown();
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Schedules a new command that should become scheduled job requests.
        /// </summary>
        /// <typeparam name="T">The type of the current command.</typeparam>
        /// <param name="changeCurrentCommand">The action that updates a clone of the current command to save the updated clone as a job request.</param>
        protected void ScheduleNewCommandLaunch<T>(Action<T> changeCurrentCommand)
            where T : ConsoleCommand
        {
            var command = (T)Clone();
            changeCurrentCommand(command);
            CommandExecutionContext.Current.ScheduleCommandLaunch(command);
        }

        private void CleanUpExecutionContext()
        {
            if (IsAutoShutDownMechanismEnabled)
            {
                CommandAutoStopWatcher.Current.Dispose();
            }
        }

        private int ExecuteJobWithContext(string[] remainingArguments)
        {
            PrepareCommandExecutionContext();
            CommandExecutionContext.Current.StartRequestExecution();
            try
            {
                var retCode = Execute(remainingArguments);
                CommandExecutionContext.Current.CompleteRequestExecution();
                return retCode;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                CommandExecutionContext.Current.SetAsFailedRequestExecution();
                return 1;
            }
            finally
            {
                CleanUpExecutionContext();
            }
        }
    }
}
