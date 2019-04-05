using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories;
using CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

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
            ReplaceChildRequestsLikeParent(jobRequests, parentRequest);
            requestRepository.AddItems(jobRequests);
            jobRequests.ForEach(x => Logger.Info(
                $"Schedule the new request ({x.Id}, {x.ScheduledTime}): {x.CommandName} {x.CommandExecutionArguments}"));
        }

        public void ExecuteJobRequest(JobRequest request)
        {
            UpdateRequest(request, JobRequestStatus.Processing);
            RunRequestInNewProcess(request);
            UpdateRequest(request, JobRequestStatus.Completed);
        }

        private void RunRequestInNewProcess(JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            ProcessManager.RestartApplicationInNewProcess(arguments);
        }

        private void ReplaceChildRequestsLikeParent(List<JobRequest> jobRequests, JobRequest parentRequest)
        {
            var childRequestAsParent = jobRequests.FirstOrDefault(x =>
                x.CommandName == parentRequest.CommandName &&
                x.CommandExecutionArguments == parentRequest.CommandExecutionArguments);
            if (childRequestAsParent == null)
            {
                return;
            }

            RescheduleRequest(parentRequest, childRequestAsParent.ScheduledTime);
            jobRequests.Remove(childRequestAsParent);
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
            var info = $"Reschedule the current request ({request.Id}, {request.ScheduledTime}): " +
                       $"{request.CommandName} {request.CommandExecutionArguments}";
            Logger.Info(info);
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
    }
}
