using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;

namespace CakeExtracter.Commands.Core
{
    /// <summary>
    /// Command for notifications about failed jobs. Use info from Job Execution history.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class FailedJobsNotifierCommand : ConsoleCommand
    {
        /// <summary>
        /// The command name.
        /// </summary>
        public const string CommandName = "FailedJobsNotifierCommand";

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedJobsNotifierCommand"/> class.
        /// </summary>
        public FailedJobsNotifierCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand(CommandName, "Notifies about failed jobs.");
        }

        /// <summary>
        /// The method runs the current command based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments">The remaining arguments.</param>
        /// <returns>
        /// Execution result code.
        /// </returns>
        public override int Execute(string[] remainingArguments)
        {
            IJobExecutionNotificationService jobExecutionNotificationService = new JobExecutionNotificationService(
                new JobExecutionItemRepository(),
                new EmailNotificationsService());
            jobExecutionNotificationService.NotifyAboutFailedJobs();
            return 0;
        }
    }
}
