using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;

namespace CakeExtracter.Commands.Core
{
    /// <summary>
    /// Command for notifications about failed jobs. Use info from Job Execution history.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ConsoleCommand" />
    public class FailedJobsNotifierCommand : ConsoleCommand
    {
        private readonly IJobExecutionNotificationService jobExecutionNotificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedJobsNotifierCommand"/> class.
        /// </summary>
        public FailedJobsNotifierCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand("FailedJobs Notifier Command", "Notifies about failed jobs.");
        }

        public override int Execute(string[] remainingArguments)
        {
            throw new global::System.NotImplementedException();
        }
    }
}
