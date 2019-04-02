using CakeExtracter.Common.ExecutionHistory.Constants;
using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;
using System;
using System.Linq;

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

        public void AddErrorToHistoryitem(JobExecutionHistoryItem executionHistoryItem)
        {
            throw new NotImplementedException();
        }

        public void AddWarningToHistoryItem(JobExecutionHistoryItem executionHistoryItem)
        {
            throw new NotImplementedException();
        }

        public JobExecutionHistoryItem CreateJobExecutionHistoryItem(string commandName, string[] arguments)
        {
            var jobExecutionHistoryitem = new JobExecutionHistoryItem
            {
                Status = JobStatuses.Processing,
                ExecutionArguments = GetCommandArgumentsString(arguments),
                CommandName = commandName,
                StartDate = DateTime.UtcNow
            };
            jobExecutionHistoryRepository.AddItem(jobExecutionHistoryitem);
            return jobExecutionHistoryitem;
        }

        public void SetExecutionHistoryItemFailedState(JobExecutionHistoryItem executionHistoryItem)
        {
            executionHistoryItem.Status = JobStatuses.Failed;
            executionHistoryItem.EndDate = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public void SetExecutionHistoryItemFinishedState(JobExecutionHistoryItem executionHistoryItem)
        {
            executionHistoryItem.Status = JobStatuses.Completed;
            executionHistoryItem.EndDate = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        private string GetCommandArgumentsString(string[] arguments)
        {
            arguments = arguments.Skip(1).ToArray(); // first item is .exe file path
            return string.Join(" ", arguments);
        }
    }
}
