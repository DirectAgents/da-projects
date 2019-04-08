using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestManagers;

namespace CakeExtracter.Commands
{
    /// <summary>
    /// The class represents a command that is used to run scheduled job requests.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    internal class ScheduledRequestsLauncherCommand : ConsoleCommand
    {
        /// <summary>
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public ScheduledRequestsLauncherCommand()
        {
            IsCommand("ScheduledRequestsLauncherCommand", "Runs Job Requests that have been scheduled");
        }

        /// <inheritdoc />
        public override void ResetProperties()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Runs all the scheduled in the past not started earlier jobs in parallel.
        /// </summary>
        public override int Execute(string[] remainingArguments)
        {
            var maxNumberOfJobRequests = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfRequestsToRunWithUniqueArguments"]);
            var requestRepository = new JobRequestRepository();
            var requestService = new JobExecutionRequestService(requestRepository);
            requestService.ExecuteScheduledInPastJobRequests(maxNumberOfJobRequests);
            return 0;
        }
    }
}
