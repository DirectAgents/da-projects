using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionHistoryItemRepository
    {
        void UpdateItem(JobRequestExecution jobRequestExecution);

        JobRequestExecution AddItem(JobRequestExecution JobRequestExecution);
    }
}
