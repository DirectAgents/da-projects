using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers;

namespace CakeExtracter.Commands.Core
{
    /// <summary>
    /// The class represents a command that is used to run scheduled job requests.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class ScheduledRequestsLauncherCommand : ConsoleCommand
    {
        /// <summary>
        /// The command name.
        /// </summary>
        public const string CommandName = "ScheduledRequestsLauncherCommand";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledRequestsLauncherCommand"/> class.
        /// The constructor sets a command name and command arguments names, provides a description for them.
        /// </summary>
        public ScheduledRequestsLauncherCommand()
        {
            NoNeedToCreateRepeatRequests = true;
            IsCommand(CommandName, "Runs Job Requests that have been scheduled");
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
