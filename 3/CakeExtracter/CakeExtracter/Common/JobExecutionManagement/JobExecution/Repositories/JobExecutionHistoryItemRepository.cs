using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public class JobExecutionHistoryItemRepository : IJobExecutionHistoryItemRepository
    {
        private static object ExecutionHistoryLocker = new object();

        public void UpdateItem(JobRequestExecution itemToUpdate)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                var historyItem = dbContext.JobRequestExecutions.SingleOrDefault(item => item.Id == itemToUpdate.Id);
                if (historyItem != null)
                {
                    dbContext.Entry(historyItem).CurrentValues.SetValues(itemToUpdate);
                    dbContext.SaveChanges();
                }
            }, ExecutionHistoryLocker, "Updating History Item");
        }

        public JobRequestExecution AddItem(JobRequestExecution jobRequestExecution)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
               dbContext.JobRequestExecutions.Add(jobRequestExecution);
                dbContext.SaveChanges();
            }, ExecutionHistoryLocker, "Inserting History item.");
            return jobRequestExecution;
        }
    }
}
