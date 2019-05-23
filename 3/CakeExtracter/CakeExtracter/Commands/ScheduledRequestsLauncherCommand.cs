using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers;

namespace CakeExtracter.Commands
{
    /// <summary>
    /// The class represents a command that is used to run scheduled job requests.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    internal class ScheduledRequestsLauncherCommand : ConsoleCommand
    {
        /// <inheritdoc />
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
            var maxNumberOfJobRequests = int.Parse(ConfigurationManager.AppSettings["JEM_MaxNumberOfRequestsToRunWithUniqueArguments"]);
            var maxNumberOfRunningRequests = int.Parse(ConfigurationManager.AppSettings["JEM_MaxNumberOfRunningRequests"]);
            var requestService = CreateRequestService();
            requestService.ExecuteScheduledInPastJobRequests(maxNumberOfJobRequests, maxNumberOfRunningRequests);
            return 0;
        }

        private JobExecutionRequestLauncher CreateRequestService()
        {
            var requestRepository = new JobRequestRepository();
            var requestService = new JobExecutionRequestLauncher(requestRepository);
            return requestService;
        }
    }
}
