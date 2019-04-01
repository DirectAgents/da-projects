using CakeExtracter.Common.ExecutionHistory.Constants;
using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;
using System;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    /// <summary>
    /// Job execution history management service.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement.IJobExecutionHistoryItemService" />
    public class JobExecutionHistoryItemService : IJobExecutionHistoryItemService
    {
        IJobExecutionHistoryItemRepository jobExecutionHistoryRepository;

        public JobExecutionHistoryItemService(IJobExecutionHistoryItemRepository jobExecutionHistoryRepository)
        {
            this.jobExecutionHistoryRepository = jobExecutionHistoryRepository;
        }

        public JobExecutionHistoryItem CreateJobExecutionHistoryItem(string commandName, string[] argumentsString)
        {
            var jobExecutionHistoryitem = new JobExecutionHistoryItem
            {
                Status = JobStatuses.Processing,
                ExecutionArguments = "test stuff",
                CommandName = commandName,
                StartDate = DateTime.UtcNow
            };
            jobExecutionHistoryRepository.AddItem(jobExecutionHistoryitem);
            return jobExecutionHistoryitem;
        }
    }
}
