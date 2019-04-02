using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionHistoryItemService
    {
        JobExecutionHistoryItem CreateJobExecutionHistoryItem(string comamndName, string[] arguments);

        void SetExecutionHistoryItemFinishedState(JobExecutionHistoryItem executionHistoryItem);

        void SetExecutionHistoryItemFailedState(JobExecutionHistoryItem executionHistoryItem);

        void AddWarningToHistoryItem(JobExecutionHistoryItem executionHistoryItem);

        void AddErrorToHistoryitem(JobExecutionHistoryItem executionHistoryItem);
    }
}
