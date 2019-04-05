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
        private readonly ConsoleCommand sourceCommand;
        private readonly Queue<CommandWithSchedule> scheduledCommands;
        private readonly IBasicRepository<JobRequest> requestRepository;

        public JobExecutionRequestManager(ConsoleCommand command)
        {
            sourceCommand = command;
            scheduledCommands = new Queue<CommandWithSchedule>();
            requestRepository = RepositoriesContainer.GetJobRequestRepository();
        }

        public JobRequest GetJobRequest(int requestId)
        {
            var request = requestRepository.GetItem(requestId);
            return request;
        }

        public JobRequest AddJobRequest(ConsoleCommand command)
        {
            var request = CreateJobRequest(command, null, null);
            requestRepository.AddItem(request);
            return request;
        }

        public void ScheduleCommand(CommandWithSchedule commandWithSchedule)
        {
            scheduledCommands.Enqueue(commandWithSchedule);
        }

        public void MarkJobRequestAsProcessing(JobRequest request)
        {
            UpdateRequestStatus(request, JobRequestStatus.Processing);
        }

        public void CreateRequestsForScheduledCommands(ConsoleCommand command, JobRequest parentRequest)
        {
            var broadCommands = command.GetUniqueBroadCommands(scheduledCommands);
            var jobRequests = broadCommands.Select(x => CreateJobRequest(x.Command, x.ScheduledTime, parentRequest.Id)).ToList();
            //TODO: ATTEMPTS NUMBER
            requestRepository.AddItems(jobRequests);
        }

        public void ExecuteJobRequest(JobRequest request)
        {
            UpdateRequestStatus(request, JobRequestStatus.Processing);
            RunRequestInNewProcess(request);
            UpdateRequestStatus(request, JobRequestStatus.Completed);
        }

        private void RunRequestInNewProcess(JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            ProcessManager.RestartApplicationInNewProcess(arguments);
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

        private void UpdateRequestStatus(JobRequest request, JobRequestStatus status)
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
