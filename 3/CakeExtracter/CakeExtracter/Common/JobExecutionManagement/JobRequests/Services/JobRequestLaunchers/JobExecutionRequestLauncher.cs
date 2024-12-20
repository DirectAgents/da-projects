﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers
{
    /// <inheritdoc />
    internal class JobExecutionRequestLauncher : IJobExecutionRequestLauncher
    {
        private readonly IJobRequestsRepository requestRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionRequestLauncher"/> class.
        /// </summary>
        /// <param name="requestRepository">Job request repository</param>
        public JobExecutionRequestLauncher(IJobRequestsRepository requestRepository)
        {
            this.requestRepository = requestRepository;
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
            var requestsToRun = GetRightAmountOfJobRequestsToRun(
                distinctScheduledNotRunningRequests,
                runningRequests,
                maxNumberOfRunningRequests);
            return requestsToRun;
        }

        private void RunRequestInNewProcess(JobRequest request)
        {
            LogStateInfo("Run the job request", request);
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            ProcessManager.RestartApplicationInNewProcess(arguments);
        }

        private List<JobRequest> GetScheduledValidJobRequests(int maxNumberOfJobRequests)
        {
            var now = DateTime.Now;
            var scheduledInPastRequests = requestRepository
                .GetItems(x => x.Status == JobRequestStatus.Scheduled && x.ScheduledTime <= now);
            var failedRequests = FailOverdueRequests(scheduledInPastRequests, maxNumberOfJobRequests);
            var validRequests = scheduledInPastRequests.Except(failedRequests).ToList();
            return validRequests;
        }

        private List<JobRequest> FailOverdueRequests(List<JobRequest> requests, int maxNumberOfJobRequests)
        {
            var failedRequests = requests.Where(x => x.AttemptNumber == maxNumberOfJobRequests).ToList();
            UpdateRequests(failedRequests, JobRequestStatus.Failed);
            failedRequests.ForEach(x => LogStateInfo("Fail the request", x));
            return failedRequests;
        }

        private List<JobRequest> GetDistinctJobRequests(List<JobRequest> requests)
        {
            var distinctLastRequests = requests.GroupBy(GetJobKey)
                .Select(x => x.OrderByDescending(y => y.ScheduledTime).First()).ToList();
            var duplicatedRequests = requests.Except(distinctLastRequests).ToList();
            UpdateRequests(duplicatedRequests, JobRequestStatus.StartedByAnotherRequest);
            duplicatedRequests.ForEach(x => LogStateInfo("Replace the request with another", x));
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

        private string GetJobKey(JobRequest request)
        {
            return request.CommandName + request.CommandExecutionArguments;
        }

        private void UpdateRequests(IEnumerable<JobRequest> requests, JobRequestStatus status)
        {
            requests.ForEach(request => request.Status = status);
            requestRepository.UpdateItems(requests);
        }

        private void LogStateInfo(string message, JobRequest request)
        {
            var requestInfo = JobRequestsLogger.LogInfo(message, request);
            CommandExecutionContext.Current.AppendJobExecutionStateInHistory(requestInfo);
        }
    }
}
