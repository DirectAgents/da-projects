using System;
using CakeExtracter.Common.JobExecutionManagement;
using System.Collections.Generic;
using System.Reflection;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;

namespace CakeExtracter.Common
{
    public abstract class ConsoleCommand : ManyConsole.ConsoleCommand
    {
        public const string RequestIdArgumentName = "jobRequestId";

        private readonly List<ConsoleCommand> commandsToRunBeforeThisCommand = new List<ConsoleCommand>();

        public virtual int IntervalBetweenUnsuccessfulAndNewRequestInMinutes { get; set; } = 0;

        public int? RequestId { get; set; }

        protected ConsoleCommand()
        {
            HasOption<int>($"{RequestIdArgumentName}=", "Job Request Id (default = null)", c => RequestId = c);
        }

        public int Run()
        {
            return Run(null);
        }

        public override int Run(string[] remainingArguments)
        {
            var commandName = GetType().Name;
            if (commandsToRunBeforeThisCommand.Count > 0)
            {
                Logger.Info("{0} has prerequisites..", commandName);
                foreach (var consoleCommand in this.commandsToRunBeforeThisCommand)
                {
                    consoleCommand.Run(null);
                }
                Logger.Info("Completed prerequisites for {0}", commandName);
            }
            Logger.Info("Executing command: {0}", commandName);
            using (new LogElapsedTime("for " + commandName))
            {
                return ExecuteJobWithContext(remainingArguments);
            }
        }

        //Note: I believe this is only called by the LineCommander before a command is run (because the Command object is not re-instantiated).
        //      When a Command is first instantiated, we rely on default property values (e.g. 0 for int).
        //TODO?: Call this from the Run() method - so it always sets the default properties
        public abstract void ResetProperties();

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

        public virtual IEnumerable<CommandWithSchedule> GetUniqueBroadCommands( IEnumerable<CommandWithSchedule> commands)
        {
            return commands;
        }

        protected void ScheduleNewJobRequest<T>(Action<T> changeCurrentCommand)
            where T : ConsoleCommand
        {
            var command = (T) MemberwiseClone();
            changeCurrentCommand(command);
            var scheduledCommand = new CommandWithSchedule
            {
                Command = command,
                ScheduledTime = DateTime.Now.AddMinutes(IntervalBetweenUnsuccessfulAndNewRequestInMinutes)
            };
            CommandExecutionContext.Current.JobRequestManager.ScheduleCommand(scheduledCommand);
        }

        protected void RunBefore(ConsoleCommand consoleCommand)
        {
            commandsToRunBeforeThisCommand.Add(consoleCommand);
        }

        private int ExecuteJobWithContext(string[] remainingArguments)
        {
            CommandExecutionContext.InitContext(this);
            CommandExecutionContext.Current.StartRequestExecution();
            try
            {
                var retCode = Execute(remainingArguments);
                CommandExecutionContext.Current.CompleteRequestExecution();
                return retCode;
            }
            catch
            {
                CommandExecutionContext.Current.FailRequestExecution();
                return 1;
            }
        }
    }
}
