using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestManagers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestManagers.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement
{
    /// <summary>
    /// The context of the command being executed.
    /// </summary>
    public class CommandExecutionContext
    {
        public static CommandExecutionContext Current;
        
        private readonly IJobExecutionItemService jobExecutionItemService;
        private readonly IJobExecutionRequestService jobExecutionRequestService;

        private ConsoleCommand currentCommand;
        private JobRequest currentJobRequest;
        private JobRequestExecution currentJobRequestExecution;

        private CommandExecutionContext(ConsoleCommand command)
        {
            var executionItemRepository = new JobExecutionItemRepository();
            jobExecutionItemService = new JobExecutionItemService(executionItemRepository);
            var requestRepository = new JobRequestRepository();
            jobExecutionRequestService = new JobExecutionRequestService(requestRepository);
            InitCurrentJobRequest(command);
        }

        /// <summary>
        /// Resets the current context for a new command.
        /// </summary>
        /// <param name="command">The command for which the context is updated.</param>
        public static void ResetContext(ConsoleCommand command)
        {
            Current = new CommandExecutionContext(command);
        }

        /// <summary>
        /// Updates all services according to the start of the main command.
        /// </summary>
        public void StartRequestExecution()
        {
            jobExecutionRequestService.SetJobRequestAsProcessing(currentJobRequest);
            currentJobRequestExecution = jobExecutionItemService.CreateJobExecutionItem(currentJobRequest);
        }

        /// <summary>
        /// Updates all services according to the successful end of the main command.
        /// </summary>
        public void CompleteRequestExecution()
        {
            jobExecutionItemService.SetJobExecutionItemFinishedState(currentJobRequestExecution);
            jobExecutionRequestService.CreateRequestsForScheduledCommands(currentCommand, currentJobRequest);
        }

        /// <summary>
        /// Updates all services according to the failed end of the main command.
        /// </summary>
        public void FailedRequestExecution()
        {
            jobExecutionItemService.SetJobExecutionItemFailedState(currentJobRequestExecution);
            jobExecutionRequestService.RescheduleRequest(currentJobRequest, currentCommand);
        }

        /// <summary>
        /// Adds a command to the commands that should become scheduled job requests.
        /// </summary>
        /// <param name="command">The command for a new job request.</param>
        public void ScheduleCommandLaunch(ConsoleCommand command)
        {
            if (!command.NoNeedToCreateRepeatRequests)
            {
                jobExecutionRequestService.ScheduleCommandLaunch(command);
            }
        }

        /// <summary>
        /// Logs the error in job execution history.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="accountId"></param>
        public void LogErrorInHistory(string message, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.AddErrorToJobExecutionItem(currentJobRequestExecution, message, accountId);
            }
        }

        /// <summary>
        /// Logs the warning in job execution history history.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="accountId"></param>
        public void LogWarningInHistory(string message, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.AddWarningToJobExecutionItem(currentJobRequestExecution, message, accountId);
            }
        }

        /// <summary>
        /// Sets the state in job execution history history.
        /// </summary>
        /// <param name="stateMessage">The state message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void SetJobExecutionStateInHistory(string stateMessage, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.AddStateMessage(currentJobRequestExecution, stateMessage, accountId);
            }
        }

        private void InitCurrentJobRequest(ConsoleCommand command)
        {
            currentCommand = command;
            currentJobRequest = jobExecutionRequestService.GetJobRequest(currentCommand);
        }
    }
}
