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
    public class JobExecutionItemService : IJobExecutionItemService
    {
        IJobExecutionHistoryItemRepository jobExecutionHistoryRepository;

        public JobExecutionItemService(IJobExecutionHistoryItemRepository jobExecutionHistoryRepository)
        {
            this.jobExecutionHistoryRepository = jobExecutionHistoryRepository;
        }

        public JobRequestExecution CreateJobRequestExecutionItem(string commandName, string[] arguments)
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

        public void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Failed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem)
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

        public void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem)
        {
            throw new NotImplementedException();
        }

        public void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem)
        {
            throw new NotImplementedException();
        }
    }
}
