using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;

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
                new JobRequestRepository(),
                new EmailNotificationsService());

            // First Level of notifications(Warning). Notifies about errors occurred in job execution. Relaunch mechanism can fix errors.
            jobExecutionNotificationService.NotifyAboutErrorsInJobExecution();

            // Second Level Of Notifications. (Critical) Notifies about failed state of jobs. Relaunch mechanism didn't fix errors.
            jobExecutionNotificationService.NotifyAboutFailedJobs();
            return 0;
        }
    }
}
