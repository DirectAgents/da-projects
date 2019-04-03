using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    public interface IJobExecutionItemService
    {
        JobRequestExecution CreateJobExecutionItem(JobRequest jobRequest);

        void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem);

        void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem);

        void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem);

        void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem);
    }
}
