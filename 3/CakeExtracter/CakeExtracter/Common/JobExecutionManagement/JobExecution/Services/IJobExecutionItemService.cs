using DirectAgents.Domain.Entities.Administration.JobExecution;
using System.Collections.Generic;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    public interface IJobExecutionItemService
    {
        JobRequestExecution CreateJobExecutionItem(JobRequest jobRequest);

        void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem);

        void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem);

        void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null);

        void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null);

        void AddStateMessage(JobRequestExecution executionHistoryItem, string message, int? accountId = null);

        List<JobRequestExecution> GetJobExecutionHistoryItems();
    }
}
