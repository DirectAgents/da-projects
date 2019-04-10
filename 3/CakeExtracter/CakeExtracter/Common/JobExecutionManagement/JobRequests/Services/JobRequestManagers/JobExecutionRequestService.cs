using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestManagers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestManagers
{
    /// <inheritdoc />
    /// <summary>
    /// The service for work with job request items.
    /// </summary>
    public class JobExecutionRequestService : IJobExecutionRequestService
    {
        private readonly IBaseRepository<JobRequest> requestRepository;
        private readonly Queue<CommandWithSchedule> scheduledCommands;

        /// <summary>
        /// Initializes a new instance of <see cref="JobExecutionRequestService"/>
        /// </summary>
        /// <param name="jobRequestRepository">The repository for job requests.</param>
        public JobExecutionRequestService(IBaseRepository<JobRequest> jobRequestRepository)
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
                ScheduledTime = GetScheduledTime(command)
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
            jobRequests.ForEach(x => LogInfoAboutRequest("Schedule the new request", x));
        }

        /// <inheritdoc />
        public void ExecuteScheduledInPastJobRequests(int maxNumberOfJobRequests, int maxNumberOfRunningRequests)
        {
            var requestsToRun = GetRequestsToRun(maxNumberOfJobRequests, maxNumberOfRunningRequests);
            Logger.Info($"{requestsToRun.Count} requests will be launched.");
            requestsToRun.ForEach(RunRequestInNewProcess);
        }

        private List<JobRequest> GetRequestsToRun(int maxNumberOfJobRequests, int maxNumberOfRunningRequests)
        {
            var scheduledValidRequests = GetScheduledValidJobRequests(maxNumberOfJobRequests);
            var distinctScheduledValidRequests = GetDistinctJobRequests(scheduledValidRequests);
            var runningRequests = GetRunningJobRequests();
            var distinctScheduledNotRunningRequests =
                GetJobRequestsOfNotRunningJobs(distinctScheduledValidRequests, runningRequests);
            var requestsToRun = GetRightAmountOfJobRequestsToRun(distinctScheduledNotRunningRequests, runningRequests,
                maxNumberOfRunningRequests);
            return requestsToRun;
        }

        private List<JobRequest> GetScheduledValidJobRequests(int maxNumberOfJobRequests)
        {
            var now = DateTime.Now;
            var scheduledInPastRequests = requestRepository.GetItems(x => x.Status == JobRequestStatus.Scheduled && x.ScheduledTime <= now);
            var failedRequests = FailOverdueRequests(scheduledInPastRequests, maxNumberOfJobRequests);
            var validRequests = scheduledInPastRequests.Except(failedRequests).ToList();
            return validRequests;
        }

        private List<JobRequest> FailOverdueRequests(List<JobRequest> requests, int maxNumberOfJobRequests)
        {
            var failedRequests = requests.Where(x => x.AttemptNumber == maxNumberOfJobRequests).ToList();
            UpdateRequests(failedRequests, JobRequestStatus.Failed);
            failedRequests.ForEach(x => LogInfoAboutRequest("Fail the request", x));
            return failedRequests;
        }

        private List<JobRequest> GetDistinctJobRequests(List<JobRequest> requests)
        {
            var distinctLastRequests = requests.GroupBy(GetJobKey)
                .Select(x => x.OrderByDescending(y => y.ScheduledTime).First()).ToList();
            var duplicatedRequests = requests.Except(distinctLastRequests).ToList();
            UpdateRequests(duplicatedRequests, JobRequestStatus.StartedByAnotherRequest);
            duplicatedRequests.ForEach(x => LogInfoAboutRequest("Replace the request with another", x));
            return distinctLastRequests;
        }

        private List<JobRequest> GetRunningJobRequests()
        {
            var runningRequests = requestRepository.GetItems(x => x.Status == JobRequestStatus.Processing);
            Logger.Info($"Currently {runningRequests.Count} job requests are processing.");
            return runningRequests;
        }

        private List<JobRequest> GetJobRequestsOfNotRunningJobs(List<JobRequest> requests, List<JobRequest> runningRequests)
        {
            var runningJobs = runningRequests.Select(GetJobKey).ToList();
            var requestsOfNotRunningJobs = requests.Where(x => !runningJobs.Contains(GetJobKey(x))).ToList();
            return requestsOfNotRunningJobs;
        }

        private List<JobRequest> GetRightAmountOfJobRequestsToRun(List<JobRequest> requests,
            List<JobRequest> runningRequests, int maxNumberOfRunningRequests)
        {
            var maxNumberOfRequestsToRun = maxNumberOfRunningRequests - runningRequests.Count;
            var requestsToRun = new List<JobRequest>();
            var groupedByCommandRequests = requests.GroupBy(x => x.CommandName).ToList();
            for (var i = 0; requestsToRun.Count < maxNumberOfRequestsToRun && requestsToRun.Count < requests.Count;
                i = i == groupedByCommandRequests.Count - 1 ? 0 : i + 1)
            {
                var request = groupedByCommandRequests[i].FirstOrDefault(x => !requestsToRun.Contains(x));
                if (request != null)
                {
                    requestsToRun.Add(request);
                }
            }

            return requestsToRun;
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

        private void RunRequestInNewProcess(JobRequest request)
        {
            LogInfoAboutRequest("Run the job request", request);
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            ProcessManager.RestartApplicationInNewProcess(arguments);
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
            LogInfoAboutRequest("Reschedule the current request", request);
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

        private void UpdateRequests(IEnumerable<JobRequest> requests, JobRequestStatus status)
        {
            requests.ForEach(x => UpdateRequestProperties(x, status));
            requestRepository.UpdateItems(requests);
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

        private string GetJobKey(JobRequest request)
        {
            return request.CommandName + request.CommandExecutionArguments;
        }

        private void LogInfoAboutRequest(string message, JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            var info = $"{message} ({request.Id}, {request.ScheduledTime}): \"{request.CommandName} {arguments}\"";
            Logger.Info(info);
        }
    }
}
