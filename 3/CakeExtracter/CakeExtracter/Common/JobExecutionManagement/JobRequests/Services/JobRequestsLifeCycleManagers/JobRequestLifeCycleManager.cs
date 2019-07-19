using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers
{
    /// <inheritdoc />
    /// <summary>
    /// Job request life cycle manager.
    /// </summary>
    /// <seealso cref="IJobRequestLifeCycleManager" />
    public class JobRequestLifeCycleManager : IJobRequestLifeCycleManager
    {
        private readonly IJobRequestsRepository requestRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobRequestLifeCycleManager"/> class.
        /// </summary>
        /// <param name="jobRequestRepository">The repository for job requests.</param>
        public JobRequestLifeCycleManager(IJobRequestsRepository jobRequestRepository)
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
        public void SetJobRequestsAsAborted(int[] ids)
        {
            var requests = requestRepository.GetItems(x => ids.Contains(x.Id));
            requests.ForEach(x => x.Status = JobRequestStatus.Aborted);
            requestRepository.UpdateItems(requests);
        }

        /// <inheritdoc />
        public void ScheduleJobRequest(JobRequest jobRequest)
        {
            jobRequest.ScheduledTime = jobRequest.ScheduledTime.HasValue
                ? CommandSchedulingUtils.GetTimeInCorrectTimeZone(jobRequest.ScheduledTime.Value)
                : CommandSchedulingUtils.DefaultScheduledTime;
            CommandVerificationUtil.VerifyJobRequest(jobRequest);
            AddNotInheritedRequest(jobRequest);
        }

        /// <inheritdoc />
        public void ProcessFailedRequest(JobRequest request, ConsoleCommand command)
        {
            if (command.NoNeedToCreateRepeatRequests)
            {
                ProcessNotRescheduledFailedRequest(request, command);
            }
            else
            {
                var scheduledTime = CommandSchedulingUtils.GetCommandRetryScheduledTime(command);
                RescheduleRequest(request, scheduledTime);
            }
        }

        /// <inheritdoc />
        public void ProcessNotRescheduledFailedRequest(JobRequest request, ConsoleCommand command)
        {
            request.Status = JobRequestStatus.Failed;
            requestRepository.UpdateItem(request);
            LogNotScheduledCommand(command.Command, CommandArgumentsConverter.GetCommandArgumentsAsLine(command));
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
            var jobRetryRequests = GetUniqueJobRetryRequests(request, retryRequestsHolder);
            if (jobRetryRequests.Count > 0)
            {
                ProcessRequestWithRetriesCompletion(command, request, jobRetryRequests);
            }
            else
            {
                ProcessRequestWithoutRetriesCompletion(request);
            }
        }

        /// <inheritdoc />
        public void ActualizeStatusOfRetryPendingJobs()
        {
            var retryPendingJobRequests = GetRetryPendingsJobRequests();
            retryPendingJobRequests.ForEach(jobRequest =>
            {
                var actualStatus = GetActualStatusOfJobRequestBasedOnChildren(jobRequest);
                if (jobRequest.Status != actualStatus)
                {
                    jobRequest.Status = actualStatus;
                    requestRepository.UpdateItem(jobRequest);
                    Logger.Info($"Retry pending job with id {jobRequest.Id} status updated to {jobRequest.Status}");
                }
            });
        }

        private JobRequestStatus GetActualStatusOfJobRequestBasedOnChildren(JobRequest jobRequest)
        {
            var childrenRequests = requestRepository.GetAllChildrenRequests(jobRequest);
            if (childrenRequests.All(req => req.Status == JobRequestStatus.Completed))
            {
                return JobRequestStatus.Completed;
            }

            return childrenRequests.Any(childReq =>
                childReq.Status == JobRequestStatus.Processing ||
                childReq.Status == JobRequestStatus.PendingRetries ||
                childReq.Status == JobRequestStatus.Scheduled)
                ? JobRequestStatus.PendingRetries
                : JobRequestStatus.Failed;
        }

        private List<JobRequest> GetRetryPendingsJobRequests()
        {
            return requestRepository.GetItems(request => request.Status == JobRequestStatus.PendingRetries).ToList();
        }

        private void ProcessRequestWithRetriesCompletion(ConsoleCommand command, JobRequest request, List<JobRequest> jobRetryRequests)
        {
            if (command.NoNeedToCreateRepeatRequests)
            {
                ProcessRequestsCompletionWithoutRetriesCreation(request, jobRetryRequests);
            }
            else
            {
                ProcessRequestCompletionWithRetriesCreation(request, jobRetryRequests);
            }
        }

        private void ProcessRequestsCompletionWithoutRetriesCreation(JobRequest request, List<JobRequest> jobRetryRequests)
        {
            request.Status = JobRequestStatus.Failed;
            requestRepository.UpdateItem(request);
            jobRetryRequests.ForEach(x => LogNotScheduledCommand(x.CommandName, x.CommandExecutionArguments));
        }

        private void ProcessRequestCompletionWithRetriesCreation(JobRequest request, List<JobRequest> jobRetryRequests)
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

        private JobRequest AddNotInheritedRequest(JobRequest jobRequest)
        {
            SetJobRequestAsNewScheduled(jobRequest);
            requestRepository.AddItem(jobRequest);
            return jobRequest;
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

        private void SetJobRequestAsNewScheduled(JobRequest jobRequest)
        {
            jobRequest.AttemptNumber = 0;
            jobRequest.Status = JobRequestStatus.Scheduled;
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
