using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionHistoryItemService
    {
        JobRequestExecution CreateJobExecutionHistoryItem(string comamndName, string[] argumentsString);

        void SetExecutionHistoryItemFinishedState(JobRequestExecution executionHistoryItem);

        void SetExecutionHistoryItemFailedState(JobRequestExecution executionHistoryItem);

        void AddWarningToHistoryItem(JobRequestExecution executionHistoryItem);

        void AddErrorToHistoryitem(JobRequestExecution executionHistoryItem);
    }
}
