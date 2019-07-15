using System.Collections.Concurrent;
using System.Collections.Generic;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services
{
    /// <summary>
    /// Retry Requests Holder.
    /// </summary>
    public class RetryRequestsHolder
    {
        private readonly ConcurrentQueue<CommandWithSchedule> commandsToSchedule;

        private ConsoleCommand parentCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryRequestsHolder" /> class.
        /// </summary>
        /// <param name="parentCommand">The parent command.</param>
        public RetryRequestsHolder(ConsoleCommand parentCommand)
        {
            commandsToSchedule = new ConcurrentQueue<CommandWithSchedule>();
            this.parentCommand = parentCommand;
        }

        /// <summary>
        /// Adds the retry request command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void EnqueueRetryRequestCommand(ConsoleCommand command)
        {
            var commandToSchedule = new CommandWithSchedule
            {
                Command = command,
                ScheduledTime = CommandSchedulingUtils.GetCommandRetryScheduledTime(command),
            };
            commandsToSchedule.Enqueue(commandToSchedule);
        }

        /// <summary>
        /// Gets the unique broad commands.
        /// </summary>
        /// <returns>Collection of unique broad commands.</returns>
        public IEnumerable<CommandWithSchedule> GetUniqueBroadCommands()
        {
            return parentCommand.GetUniqueBroadCommands(commandsToSchedule);
        }
    }
}
