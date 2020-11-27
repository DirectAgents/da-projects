using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
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
        /// Gets or sets a value indicating whether job is working for standart health check time or not.
        /// </summary>
        public bool IsStandartTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingJobsNotifierCommand"/> class.
        /// </summary>
        public ProcessingJobsNotifierCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand(CommandName, "Notifies about processing jobs.");
            HasOption<bool>("s|standart=", "Is time of launch standart (default = true)", c => IsStandartTime = c);
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

            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter = (ConfigurationManager.GetSection(IsStandartTime ? "RunningJobsNames/StandartTimeJobs" : "RunningJobsNames/NonStandartTimeJobs")
                as System.Collections.Hashtable)
             .Cast<System.Collections.DictionaryEntry>()
             .ToDictionary(n => n.Key.ToString(), n => n.Value != null ? n.Value.ToString() : "");

            jobExecutionNotificationService.NotifyAboutProcessingJobs(filter);
            return 0;
        }

        /// <summary>Resets command properties.</summary>
        public override void ResetProperties()
        {
            IsStandartTime = true;
        }
    }
}
