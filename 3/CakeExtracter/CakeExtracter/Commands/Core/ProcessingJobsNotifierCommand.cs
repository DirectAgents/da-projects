using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;

namespace CakeExtracter.Commands.Core
{
    /// <summary>
    /// Command for notifications about processing jobs. Use info from Job Execution history.
    /// </summary>
    /// <seealso cref="ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class ProcessingJobsNotifierCommand : ConsoleCommand
    {
        /// <summary>
        /// The command name.
        /// </summary>
        public const string CommandName = "ProcessingJobsNotifierCommand";



        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingJobsNotifierCommand"/> class.
        /// </summary>
        public ProcessingJobsNotifierCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand(CommandName, "Notifies about processing jobs.");
            //HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
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
            var jobExecutionNotificationService = DIKernel.Get<IJobExecutionNotificationService>();

            // Notifies about jobs that are still processing.
            jobExecutionNotificationService.NotifyAboutProcessingJobs();
            return 0;
        }
    }
}
