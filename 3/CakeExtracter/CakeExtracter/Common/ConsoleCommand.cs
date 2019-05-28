using System;
using CakeExtracter.Common.JobExecutionManagement;
using System.Collections.Generic;
using System.Reflection;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;

namespace CakeExtracter.Common
{
    /// <summary>
    /// The base class for a console command.
    /// </summary>
    public abstract class ConsoleCommand : ManyConsole.ConsoleCommand, ICloneable
    {
        public const string RequestIdArgumentName = "jobRequestId";
        public const string NoNeedToCreateRepeatRequestsArgumentName = "noRepeatedRequests";

        /// <summary>
        /// The interval between the unsuccessful and the new request in minutes for the child requests of this command.
        /// </summary>
        public int IntervalBetweenUnsuccessfulAndNewRequestInMinutes= 0;

        /// <summary>
        /// Command argument: The identifier for the job request that initiates this command run.
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>
        /// Command argument: The flag that indicates that there is no need to create repeated requests.
        /// </summary>
        public bool NoNeedToCreateRepeatRequests { get; set; }

        /// <summary>
        /// The constructor sets base command arguments names and provides a description for them.
        /// </summary>
        protected ConsoleCommand()
        {
            HasOption<int>($"{RequestIdArgumentName}=", "Job Request Id (default = null)", c => RequestId = c);
            HasOption<bool>($"{NoNeedToCreateRepeatRequestsArgumentName}=", "No need to create repeated requests. (default = false)", c => NoNeedToCreateRepeatRequests = c);
        }

        public int Run()
        {
            return Run(null);
        }

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
        public abstract void ResetProperties();

        /// <summary>
        /// The method runs the current command based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code</returns>
        public abstract int Execute(string[] remainingArguments);

        public virtual IEnumerable<PropertyInfo> GetArgumentProperties()
        {
            return GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        }

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

        public virtual void ResetCommandExecutionContext()
        {
            CommandExecutionContext.ResetContext(this);
        }

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

        private int ExecuteJobWithContext(string[] remainingArguments)
        {
            ResetCommandExecutionContext();
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
                CommandExecutionContext.Current.FailedRequestExecution();
                return 1;
            }
        }
    }
}
