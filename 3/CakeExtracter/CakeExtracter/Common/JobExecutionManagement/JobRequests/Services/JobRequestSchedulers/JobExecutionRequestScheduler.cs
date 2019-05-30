﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using ManyConsole.Internal;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers
{
    /// <inheritdoc />
    /// <summary>
    /// The service for work with scheduling of job request items.
    /// </summary>
    public class JobExecutionRequestScheduler : IJobExecutionRequestScheduler
    {
        private readonly IBaseRepository<JobRequest> requestRepository;
        private readonly Queue<CommandWithSchedule> scheduledCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionRequestScheduler"/> class.
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
        public void SetJobRequestsAsAborted(int[] ids)
        {
            var requests = requestRepository.GetItems(x => ids.Contains(x.Id));
            requests.ForEach(x => UpdateRequestProperties(x, JobRequestStatus.Aborted));
            requestRepository.UpdateItems(requests);
        }

        /// <inheritdoc />
        public void VerifyJobRequest(List<ManyConsole.ConsoleCommand> existingCommands, JobRequest jobRequest)
        {
            var command = existingCommands.Find(x => x.Command == jobRequest.CommandName);
            var commandLine = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(jobRequest);
            var arguments = CommandLineParser.Parse(commandLine);
            var remainingArguments = command.GetActualOptions().Parse(arguments.Skip(1));
            ConsoleUtil.VerifyNumberOfArguments(
                remainingArguments.ToArray(),
                command.RemainingArgumentsCountMin,
                command.RemainingArgumentsCountMax);
            command.OverrideAfterHandlingArgumentsBeforeRun(remainingArguments.ToArray());
        }

        /// <inheritdoc />
        public void ScheduleCommandLaunch(ConsoleCommand command)
        {
            var scheduledCommand = new CommandWithSchedule
            {
                Command = command,
                ScheduledTime = GetScheduledTime(command),
            };
            scheduledCommands.Enqueue(scheduledCommand);
        }

        /// <inheritdoc />
        public void ScheduleJobRequest(JobRequest jobRequest)
        {
            AddNotInheritedRequest(jobRequest);
        }

        /// <inheritdoc />
        public void RescheduleRequest(JobRequest request, ConsoleCommand sourceCommand)
        {
            if (sourceCommand.NoNeedToCreateRepeatRequests)
            {
                UpdateRequest(request, JobRequestStatus.Failed);
                LogNotScheduledCommand(sourceCommand.Command, CommandArgumentsConverter.GetCommandArgumentsAsLine(sourceCommand));
                return;
            }

            var scheduledTime = GetScheduledTime(sourceCommand);
            RescheduleRequest(request, scheduledTime);
        }

        /// <inheritdoc />
        public void CreateRequestsForScheduledCommands(ConsoleCommand sourceCommand, JobRequest sourceRequest)
        {
            var jobRequests = GetUniqueJobRequests(sourceCommand, sourceRequest);
            if (sourceCommand.NoNeedToCreateRepeatRequests)
            {
                UpdateRequest(sourceRequest, JobRequestStatus.Failed);
                jobRequests.ForEach(x => LogNotScheduledCommand(x.CommandName, x.CommandExecutionArguments));
                return;
            }

            ScheduleRequests(sourceRequest, jobRequests);
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

        private List<JobRequest> GetUniqueJobRequests(ConsoleCommand sourceCommand, JobRequest sourceRequest)
        {
            var broadCommands = sourceCommand.GetUniqueBroadCommands(scheduledCommands);
            var jobRequests = broadCommands.Select(x => CreateJobRequest(x.Command, x.ScheduledTime, sourceRequest.Id));
            return jobRequests.ToList();
        }

        private void ScheduleRequests(JobRequest sourceRequest, List<JobRequest> jobRequests)
        {
            ProcessEndOfRequest(jobRequests, sourceRequest);
            requestRepository.AddItems(jobRequests);
            jobRequests.ForEach(x => JobRequestsLogger.LogInfo("Schedule the new request", x));
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

        private JobRequest AddNotInheritedRequest(JobRequest jobRequest)
        {
            jobRequest.AttemptNumber = 0;
            jobRequest.Status = JobRequestStatus.Scheduled;
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

        private void LogNotScheduledCommand(string commandName, string commandArguments)
        {
            Logger.Info($"This command should be scheduled but it does not because NoNeedToCreateRepeatRequests = true: {commandName} {commandArguments}");
        }
    }
}
