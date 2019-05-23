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
    /// <inheritdoc />
    /// <summary>
    /// The service for work with job request items.
    /// </summary>
    public class JobExecutionRequestScheduler : IJobExecutionRequestScheduler
    {
        private readonly IBaseRepository<JobRequest> requestRepository;
        private readonly Queue<CommandWithSchedule> scheduledCommands;

        /// <summary>
        /// Initializes a new instance of <see cref="JobExecutionRequestScheduler"/>
        /// </summary>
        /// <param name="jobRequestRepository">The repository for job requests.</param>
        public JobExecutionRequestScheduler(IBaseRepository<JobRequest> jobRequestRepository)
        {
            scheduledCommands = new Queue<CommandWithSchedule>();
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
            UpdateRequest(request, JobRequestStatus.Processing);
        }

        /// <inheritdoc />
        public void ScheduleCommandLaunch(ConsoleCommand command)
        {
            if (command.NoNeedToCreateRepeatRequests)
            {
                return;
            }

            var scheduledCommand = new CommandWithSchedule
            {
                Command = command,
                ScheduledTime = GetScheduledTime(command),
            };
            scheduledCommands.Enqueue(scheduledCommand);
        }

        /// <inheritdoc />
        public void RescheduleRequest(JobRequest request, ConsoleCommand sourceCommand)
        {
            var scheduledTime = GetScheduledTime(sourceCommand);
            RescheduleRequest(request, scheduledTime);
        }

        /// <inheritdoc />
        public void CreateRequestsForScheduledCommands(ConsoleCommand sourceCommand, JobRequest sourceRequest)
        {
            var broadCommands = sourceCommand.GetUniqueBroadCommands(scheduledCommands);
            var jobRequests = broadCommands.Select(x => CreateJobRequest(x.Command, x.ScheduledTime, sourceRequest.Id)).ToList();
            ProcessEndOfRequest(jobRequests, sourceRequest);
            requestRepository.AddItems(jobRequests);
            jobRequests.ForEach(x => JobRequestsLogger.LogInfo("Schedule the new request", x));
        }

        /// <inheritdoc />
        public void EndRequest(JobRequest request)
        {
            if (request.Status != JobRequestStatus.Processing)
            {
                return;
            }
            UpdateRequest(request, JobRequestStatus.Aborted);
            JobRequestsLogger.LogError("The request is aborted", request);
        }

        private void ProcessEndOfRequest(List<JobRequest> newRequests, JobRequest endedRequest)
        {
            var childRequestAsParent = newRequests.FirstOrDefault(x =>
                x.CommandName == endedRequest.CommandName &&
                x.CommandExecutionArguments == endedRequest.CommandExecutionArguments);
            if (childRequestAsParent == null)
            {
                UpdateRequest(endedRequest, JobRequestStatus.Completed);
                return;
            }

            RescheduleRequest(endedRequest, childRequestAsParent.ScheduledTime);
            newRequests.Remove(childRequestAsParent);
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
                ParentJobRequestId = parentRequestId
            };
        }

        private void RescheduleRequest(JobRequest request, DateTime? scheduledTime)
        {
            UpdateRequest(request, JobRequestStatus.Scheduled, scheduledTime);
            JobRequestsLogger.LogInfo("Reschedule the current request", request);
        }

        private void UpdateRequest(JobRequest request, JobRequestStatus status, DateTime? scheduledTime)
        {
            request.ScheduledTime = scheduledTime;
            UpdateRequest(request, status);
        }

        private void UpdateRequest(JobRequest request, JobRequestStatus status)
        {
            UpdateRequestProperties(request, status);
            requestRepository.UpdateItem(request);
        }

        private void UpdateRequestProperties(JobRequest request, JobRequestStatus status)
        {
            if (status == JobRequestStatus.Processing)
            {
                request.AttemptNumber++;
            }

            request.Status = status;
        }

        private DateTime GetScheduledTime(ConsoleCommand command)
        {
            return DateTime.Now.AddMinutes(command.IntervalBetweenUnsuccessfulAndNewRequestInMinutes);
        }
    }
}
