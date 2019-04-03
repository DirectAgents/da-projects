using System;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    /// <summary>
    /// Job execution history management service.
    /// </summary>
    /// <seealso cref="IJobExecutionHistoryItemService" />
    public class JobExecutionHistoryItemService : IJobExecutionHistoryItemService
    {
        IJobExecutionHistoryItemRepository jobExecutionHistoryRepository;

        public JobExecutionHistoryItemService(IJobExecutionHistoryItemRepository jobExecutionHistoryRepository)
        {
            this.jobExecutionHistoryRepository = jobExecutionHistoryRepository;
        }

        public JobRequestExecution CreateJobExecutionHistoryItem(string commandName, string[] argumentsString)
        {
            throw new NotImplementedException();
        }

        public void AddWarningToHistoryItem(JobRequestExecution executionHistoryItem)
        {
            throw new NotImplementedException();
        }

        public JobRequestExecution CreateJobExecutionHistoryItem(string commandName, string[] arguments)
        {
            var jobRequestExecutionItem = new JobRequestExecution
            {
                Status = JobExecutionStatus.Processing,
                JobRequest = new JobRequest
                {
                    AttemptNumber = 1,
                    CommandExecutionArguments = "test stuff",
                    CommandName = commandName,
                    Status = JobRequestStatus.Processing
                },
                StartTime = DateTime.UtcNow
            };
            jobExecutionHistoryRepository.AddItem(jobRequestExecutionItem);
            return jobRequestExecutionItem;
        }

        public void SetExecutionHistoryItemFailedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Failed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public void SetExecutionHistoryItemFinishedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Completed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        private string GetCommandArgumentsString(string[] arguments)
        {
            arguments = arguments.Skip(1).ToArray(); // first item is .exe file path
            return string.Join(" ", arguments);
        }
    }
}
