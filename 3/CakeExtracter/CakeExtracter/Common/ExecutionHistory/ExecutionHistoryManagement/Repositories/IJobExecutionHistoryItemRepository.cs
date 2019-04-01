using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionHistoryItemRepository
    {
        void UpdateItem(JobExecutionHistoryItem jobExecutionHistoryItem);

        JobExecutionHistoryItem AddItem(JobExecutionHistoryItem JobExecutionHistoryItem);
    }
}
