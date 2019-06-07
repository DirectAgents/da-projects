﻿using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement
{
    /// <summary>
    /// The context of the command being executed.
    /// </summary>
    public class CommandExecutionContext
    {
        /// <summary>
        /// Gets the current Execution Context.
        /// </summary>
        /// <value>
        /// The current execution context.
        /// </value>
        public static CommandExecutionContext Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the currently running command.
        /// </summary>
        /// <value>
        /// The name of the currently running command.
        /// </value>
        public string CommandName
        {
            get
            {
                return currentCommand.Command;
            }
        }

        private readonly IJobExecutionItemService jobExecutionItemService;
        private readonly IJobRequestLifeCycleManager jobExecutionLifeCycleManager;

        private ConsoleCommand currentCommand;
        private JobRequest currentJobRequest;
        private JobRequestExecution currentJobRequestExecution;
        private RetryRequestsHolder retryRequestsHolder;

        private CommandExecutionContext(ConsoleCommand command)
        {
            var executionItemRepository = new JobExecutionItemRepository();
            var requestRepository = new JobRequestRepository();
            jobExecutionItemService = new JobExecutionItemService(executionItemRepository, requestRepository);
            jobExecutionLifeCycleManager = new JobRequestLifeCycleManager(requestRepository);
            retryRequestsHolder = new RetryRequestsHolder(command);
            InitCurrentJobRequest(command);
        }

        private CommandExecutionContext(
            ConsoleCommand command,
            IJobExecutionItemService jobExecutionItemService,
            IJobRequestLifeCycleManager jobExecutionRequestScheduler)
        {
            this.jobExecutionItemService = jobExecutionItemService;
            this.jobExecutionLifeCycleManager = jobExecutionRequestScheduler;
            retryRequestsHolder = new RetryRequestsHolder(command);
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
        /// Resets the context.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="jobExecutionItemService">The job execution item service.</param>
        /// <param name="jobExecutionRequestScheduler">The job execution request scheduler.</param>
        public static void ResetContext(
            ConsoleCommand command,
            IJobExecutionItemService jobExecutionItemService,
            IJobRequestLifeCycleManager jobExecutionRequestScheduler)
        {
            Current = new CommandExecutionContext(command, jobExecutionItemService, jobExecutionRequestScheduler);
        }

        /// <summary>
        /// Updates all services according to the start of the main command.
        /// </summary>
        public void StartRequestExecution()
        {
            jobExecutionLifeCycleManager.SetJobRequestAsProcessing(currentJobRequest);
            currentJobRequestExecution = jobExecutionItemService.CreateJobExecutionItem(currentJobRequest);
        }

        /// <summary>
        /// Updates all services according to the successful end of the main command.
        /// </summary>
        public void CompleteRequestExecution()
        {
            jobExecutionItemService.SetJobExecutionItemFinishedState(currentJobRequestExecution);
            jobExecutionLifeCycleManager.ProcessCompletedRequest(currentCommand, currentJobRequest, retryRequestsHolder);
        }

        /// <summary>
        /// Updates all services according to the failed end of the main command.
        /// </summary>
        public void SetAsFailedRequestExecution()
        {
            jobExecutionItemService.SetJobExecutionItemFailedState(currentJobRequestExecution);
            jobExecutionLifeCycleManager.ProcessFailedRequest(currentJobRequest, currentCommand);
        }

        /// <summary>
        /// Sets as aborted by timeout request execution.
        /// </summary>
        public void SetAsAbortedByTimeoutRequestExecution()
        {
            jobExecutionItemService.SetJobExecutionItemAbortedByTimeoutState(currentJobRequestExecution);
            CloseContext();
        }

        /// <summary>
        /// Closes the current context.
        /// </summary>
        public void CloseContext()
        {
            if (currentJobRequest.Status == JobRequestStatus.Processing)
            {
                jobExecutionLifeCycleManager.ProcessAbortedRequest(currentJobRequest);
            }
        }

        /// <summary>
        /// Adds a command to the commands that should become scheduled job requests.
        /// </summary>
        /// <param name="command">The command for a new job request.</param>
        public void ScheduleCommandLaunch(ConsoleCommand command)
        {
            retryRequestsHolder.EnqueueRetryRequestCommand(command);
        }

        /// <summary>
        /// Logs the error in job execution history.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="accountId">Account Id.</param>
        public void LogErrorInHistory(string message, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.AddErrorToJobExecutionItem(currentJobRequestExecution, message, accountId);
            }
        }

        /// <summary>
        /// Logs the warning in job execution history.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void LogWarningInHistory(string message, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.AddWarningToJobExecutionItem(currentJobRequestExecution, message, accountId);
            }
        }

        /// <summary>
        /// Sets the state in job execution history.
        /// </summary>
        /// <param name="stateMessage">The state message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void SetJobExecutionStateInHistory(string stateMessage, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.SetStateMessage(currentJobRequestExecution, stateMessage, accountId);
            }
        }

        /// <summary>
        /// Appends the state in job execution history.
        /// </summary>
        /// <param name="stateMessage">The state message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void AppendJobExecutionStateInHistory(string stateMessage, int? accountId = null)
        {
            if (currentJobRequestExecution != null)
            {
                jobExecutionItemService.AddStateMessage(currentJobRequestExecution, stateMessage, accountId);
            }
        }

        private void InitCurrentJobRequest(ConsoleCommand command)
        {
            currentCommand = command;
            currentJobRequest = jobExecutionLifeCycleManager.GetJobRequest(currentCommand);
        }
    }
}
