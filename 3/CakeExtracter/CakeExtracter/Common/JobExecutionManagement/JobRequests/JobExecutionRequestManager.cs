using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.SimpleRepositories;
using CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests
{
    public class JobExecutionRequestManager
    {
        private readonly IBasicRepository<JobRequest> requestRepository;
        private readonly ConsoleCommand sourceCommand;

        public JobExecutionRequestManager(ConsoleCommand command)
        {
            sourceCommand = command;
            requestRepository = RepositoriesContainer.GetJobRequestRepository();
        }

        public JobRequest GetJobRequest(int requestId)
        {
            var request = requestRepository.GetItem(requestId);
            return request;
        }

        public JobRequest AddJobRequest(ConsoleCommand command, DateTime? scheduledTime = null)
        {
            var request = CreateJobRequest(command, scheduledTime);
            requestRepository.AddItem(request);
            return request;
        }

        public void MarkJobRequestAsProcessing(JobRequest request)
        {
            UpdateRequestStatus(request, JobRequestStatus.Processing);
        }

        public T GetSourceCommandCopy<T>()
            where T : ConsoleCommand
        {
            return (T) sourceCommand.Clone();
        }

        public void ExecuteJobRequest(JobRequest request)
        {
            UpdateRequestStatus(request, JobRequestStatus.Processing);
            RunRequestInNewProcess(request);
            UpdateRequestStatus(request, JobRequestStatus.Completed);
        }

        private JobRequest CreateJobRequest(ConsoleCommand command, DateTime? scheduledTime)
        {
            return new JobRequest
            {
                AttemptNumber = 0,
                CommandName = command.Command,
                CommandExecutionArguments = CommandArgumentsConverter.GetCommandArgumentsAsLine(command),
                ScheduledTime = scheduledTime,
                Status = JobRequestStatus.Scheduled
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

        private void RunRequestInNewProcess(JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            ProcessManager.StartConsoleApplicationInNewProcess(arguments);
        }
    }
}
