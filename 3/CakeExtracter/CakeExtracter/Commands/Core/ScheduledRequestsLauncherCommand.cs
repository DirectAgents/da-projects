using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers;

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
            LaunchScheduledRequests();
            ActualizeJobRequestsStatuses();
            return 0;
        }

        private void LaunchScheduledRequests()
        {
            var maxNumberOfJobRequests = int.Parse(ConfigurationManager.AppSettings["JEM_MaxNumberOfRequestsToRunWithUniqueArguments"]);
            var maxNumberOfRunningRequests = int.Parse(ConfigurationManager.AppSettings["JEM_MaxNumberOfRunningRequests"]);
            var requestsLauncher = InitRequestsLauncher();
            requestsLauncher.ExecuteScheduledInPastJobRequests(maxNumberOfJobRequests, maxNumberOfRunningRequests);
        }

        private void ActualizeJobRequestsStatuses()
        {
            var jobRequestLifeCycleManager = InitJobRequestLifeCycleManager();
            jobRequestLifeCycleManager.ActualizeStatusOfRetryPendingJobs();
        }

        private IJobExecutionRequestLauncher InitRequestsLauncher()
        {
            var requestRepository = new JobRequestRepository();
            var requestsLauncher = new JobExecutionRequestLauncher(requestRepository);
            return requestsLauncher;
        }

        private JobRequestLifeCycleManager InitJobRequestLifeCycleManager()
        {
            var requestRepository = new JobRequestRepository();
            var requestsLifeCycleManager = new JobRequestLifeCycleManager(requestRepository);
            return requestsLifeCycleManager;
        }
    }
}
