using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionItemService
    {
        JobRequestExecution CreateJobRequestExecutionItem(string comamndName, string[] argumentsString);

        void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem);

        void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem);

        void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem);

        void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem);
    }
}
