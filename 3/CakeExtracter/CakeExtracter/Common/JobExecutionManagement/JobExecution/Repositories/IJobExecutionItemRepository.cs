using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    public interface IJobExecutionItemRepository
    {
        void UpdateItem(JobRequestExecution jobRequestExecution);

        JobRequestExecution AddItem(JobRequestExecution JobRequestExecution);
    }
}
