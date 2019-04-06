using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories;
using CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests
{
    public class JobExecutionRequestManager
    {
        private readonly Queue<CommandWithSchedule> scheduledCommands;
        private readonly IBasicRepository<JobRequest> requestRepository;

        public JobExecutionRequestManager()
        {
            scheduledCommands = new Queue<CommandWithSchedule>();
            requestRepository = RepositoriesContainer.GetJobRequestRepository();
        }

        public JobRequest GetJobRequest(ConsoleCommand command)
        {
            return command.RequestId.HasValue
                ? requestRepository.GetItem(command.RequestId.Value)
                : AddNotInheritedRequest(command);
        }

        public void ScheduleCommand(CommandWithSchedule commandWithSchedule)
        {
            scheduledCommands.Enqueue(commandWithSchedule);
        }

        public void MarkJobRequestAsProcessing(JobRequest request)
        {
            UpdateRequest(request, JobRequestStatus.Processing);
        }

        public void RescheduleRequest(JobRequest request, DateTime scheduledTime)
        {
            RescheduleRequest(request, (DateTime?) scheduledTime);
        }

        public void CreateRequestsForScheduledCommands(ConsoleCommand command, JobRequest parentRequest)
        {
            var broadCommands = command.GetUniqueBroadCommands(scheduledCommands);
            var jobRequests = broadCommands.Select(x => CreateJobRequest(x.Command, x.ScheduledTime, parentRequest.Id)).ToList();
            ProcessEndOfRequest(jobRequests, parentRequest);
            requestRepository.AddItems(jobRequests);
            jobRequests.ForEach(x => LogInfoAboutRequest("Schedule the new request", x));
        }

        public void ExecuteScheduledInPastJobRequests(int maxNumberOfJobRequests)
        {
            var now = DateTime.Now;
            var requestsToRun = requestRepository.GetItems(x => x.Status == JobRequestStatus.Scheduled && x.ScheduledTime <= now);
            var failedRequests = requestsToRun.Where(x => x.AttemptNumber == maxNumberOfJobRequests).ToList();
            failedRequests.ForEach(x => UpdateRequest(x, JobRequestStatus.Failed));
            requestsToRun.Where(x => x.Status == JobRequestStatus.Scheduled).ForEach(RunRequestInNewProcess);
        }

        private void RunRequestInNewProcess(JobRequest request)
        {
            LogInfoAboutRequest("Run the job request", request);
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            ProcessManager.RestartApplicationInNewProcess(arguments);
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
            LogInfoAboutRequest("Reschedule the current request", request);
        }

        private void UpdateRequest(JobRequest request, JobRequestStatus status, DateTime? scheduledTime)
        {
            request.ScheduledTime = scheduledTime;
            UpdateRequest(request, status);
        }

        private void UpdateRequest(JobRequest request, JobRequestStatus status)
        {
            if (status == JobRequestStatus.Processing)
            {
                request.AttemptNumber++;
            }

            request.Status = status;
            requestRepository.UpdateItem(request);
        }

        private void LogInfoAboutRequest(string message, JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            var info = $"{message} ({request.Id}, {request.ScheduledTime}): \"{request.CommandName} {arguments}\"";
            Logger.Info(info);
        }
    }
}
