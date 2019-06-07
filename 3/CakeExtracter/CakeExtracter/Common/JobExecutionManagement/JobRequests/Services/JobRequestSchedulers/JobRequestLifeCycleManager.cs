using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers
{
    /// <summary>
    /// Job request life cycle manager.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces.IJobRequestLifeCycleManager" />
    public class JobRequestLifeCycleManager : IJobRequestLifeCycleManager
    {
        private readonly IBaseRepository<JobRequest> requestRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobRequestLifeCycleManager"/> class.
        /// </summary>
        /// <param name="jobRequestRepository">The repository for job requests.</param>
        public JobRequestLifeCycleManager(IBaseRepository<JobRequest> jobRequestRepository)
        {
            requestRepository = jobRequestRepository;
        }

        /// <inheritdoc />
        public JobRequest GetJobRequest(ConsoleCommand command)
        {
            return command.RequestId.HasValue
                ? requestRepository.GetItem(command.RequestId.Value)
                : AddNotInheritedRequest(command);
        }

        /// <inheritdoc />
        public void SetJobRequestAsProcessing(JobRequest request)
        {
            request.Status = JobRequestStatus.Processing;
            request.AttemptNumber++;
            requestRepository.UpdateItem(request);
        }

        /// <inheritdoc />
        public void ProcessFailedRequest(JobRequest request, ConsoleCommand command)
        {
            if (command.NoNeedToCreateRepeatRequests)
            {
                request.Status = JobRequestStatus.Failed;
                requestRepository.UpdateItem(request);
                LogNotScheduledCommand(command.Command, CommandArgumentsConverter.GetCommandArgumentsAsLine(command));
            }
            else
            {
                var scheduledTime = CommandSchedulingUtils.GetCommandRetryScheduledTime(command);
                RescheduleRequest(request, scheduledTime);
            }
        }

        /// <inheritdoc />
        public void ProcessAbortedRequest(JobRequest request)
        {
            request.Status = JobRequestStatus.Aborted;
            requestRepository.UpdateItem(request);
            JobRequestsLogger.LogError("The request is aborted", request);
        }

        /// <inheritdoc />
        public void ProcessCompletedRequest(ConsoleCommand command, JobRequest request, RetryRequestsHolder retryRequestsHolder)
        {
            List<JobRequest> jobRetryRequests = GetUniqueJobRetryRequests(request, retryRequestsHolder);
            if (jobRetryRequests.Count > 0)
            {
                ProcessRequestWithRetriesCompletion(command, request, jobRetryRequests);
            }
            else
            {
                ProcessRequestWithoutRetriesCompletion(request);
            }
        }

        private void ProcessRequestWithRetriesCompletion(ConsoleCommand command, JobRequest request, List<JobRequest> jobRetryRequests)
        {
            if (command.NoNeedToCreateRepeatRequests)
            {
                request.Status = JobRequestStatus.Failed;
                requestRepository.UpdateItem(request);
                jobRetryRequests.ForEach(x => LogNotScheduledCommand(x.CommandName, x.CommandExecutionArguments));
            }
            else
            {
                var retryRequestEqualWithParent = TryGetRequestEqualWithParent(request, jobRetryRequests);
                if (retryRequestEqualWithParent == null)
                {
                    request.Status = JobRequestStatus.PendingRetries;
                    requestRepository.UpdateItem(request);
                    ScheduleRetryRequests(jobRetryRequests);
                }
                else
                {
                    RescheduleRequest(request, retryRequestEqualWithParent.ScheduledTime);
                }
            }
        }

        private JobRequest TryGetRequestEqualWithParent(JobRequest parentRequest, List<JobRequest> jobRetryRequests)
        {
            return jobRetryRequests.FirstOrDefault(x =>
                    x.CommandName == parentRequest.CommandName &&
                    x.CommandExecutionArguments == parentRequest.CommandExecutionArguments);
        }

        private void ScheduleRetryRequests(List<JobRequest> jobRetryRequests)
        {
            requestRepository.AddItems(jobRetryRequests);
            jobRetryRequests.ForEach(x => JobRequestsLogger.LogInfo("Schedule the new request", x));
        }

        private void ProcessRequestWithoutRetriesCompletion(JobRequest request)
        {
            request.Status = JobRequestStatus.Completed;
            requestRepository.UpdateItem(request);
        }

        private List<JobRequest> GetUniqueJobRetryRequests(JobRequest sourceRequest, RetryRequestsHolder retryRequestsHolder)
        {
            IEnumerable<CommandWithSchedule> broadCommands = retryRequestsHolder.GetUniqueBroadCommands();
            var jobRequests = broadCommands.Select(x => CreateJobRequest(x.Command, x.ScheduledTime, sourceRequest.Id));
            return jobRequests.ToList();
        }

        private JobRequest AddNotInheritedRequest(ConsoleCommand command)
        {
            var request = CreateJobRequest(command, null, null);
            requestRepository.AddItem(request);
            return request;
        }

        private JobRequest CreateJobRequest(ConsoleCommand command, DateTime? scheduledTime, int? parentRequestId)
        {
            return new JobRequest
            {
                AttemptNumber = 0,
                CommandName = command.Command,
                CommandExecutionArguments = CommandArgumentsConverter.GetCommandArgumentsAsLine(command),
                ScheduledTime = scheduledTime,
                Status = JobRequestStatus.Scheduled,
                ParentJobRequestId = parentRequestId,
            };
        }

        private void RescheduleRequest(JobRequest request, DateTime? scheduledTime)
        {
            request.ScheduledTime = scheduledTime;
            request.Status = JobRequestStatus.Scheduled;
            requestRepository.UpdateItem(request);
            JobRequestsLogger.LogInfo("Reschedule the current request", request);
        }

        private void LogNotScheduledCommand(string commandName, string commandArguments)
        {
            Logger.Info($"This command should be scheduled but it does not because NoNeedToCreateRepeatRequests = true: {commandName} {commandArguments}");
        }
    }
}
