using System.Collections.Generic;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Helpers;

namespace CakeExtracter.Commands.Core
{
    /// <summary>
    /// Command for notifications about processing jobs. Use info from Job Execution history.
    /// </summary>
    /// <seealso cref="ConsoleCommand" />
    [Export(typeof(ConsoleCommand))]
    public class HealthCheckNotifierCommand : ConsoleCommand
    {
        /// <summary>
        /// The command name.
        /// </summary>
        private const string CommandName = "HealthCheckNotifierCommand";

        /// <summary>
        /// Gets or sets a value indicating whether job is working for standard health check time or not.
        /// </summary>
        public bool IsStandardTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckNotifierCommand"/> class.
        /// </summary>
        public HealthCheckNotifierCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand(CommandName, "Notifies about issues.");
            HasOption<bool>("s|standard=", "Is time of launch standard (default = true)", c => IsStandardTime = c);
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
            Dictionary<string, string> filter;

            if (IsStandardTime)
            {
                filter = GetStandardTimeJobs();
            }
            else
            {
                filter = GetNonStandardTimeJobs();
                jobExecutionNotificationService.NotifyAboutSynchAnalyticIssues();
            }

            jobExecutionNotificationService.NotifyAboutProcessingJobs(filter);
            return 0;
        }

        /// <inheritdoc/>
        public override void ResetProperties()
        {
            IsStandardTime = true;
        }

        private Dictionary<string, string> GetStandardTimeJobs()
        {
            return ConfigurationHelper
                .ExtractDictionaryFromConfigValue(
                    "JobProcessing_StandardTimeJobs_Names",
                    "JobProcessing_StandardTimeJobs_Parameters",
                    false);
        }

        private Dictionary<string, string> GetNonStandardTimeJobs()
        {
            return ConfigurationHelper.ExtractDictionaryFromConfigValue(
                "JobProcessing_NonStandardTimeJobs_Names",
                "JobProcessing_NonStandardTimeJobs_Parameters",
                false);
        }
    }
}
