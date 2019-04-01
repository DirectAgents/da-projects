using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;
using System.Linq;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public class JobExecutionHistoryItemRepository : IJobExecutionHistoryItemRepository
    {
        private static object ExecutionHistoryLocker = new object();

        public void UpdateItem(JobExecutionHistoryItem itemToUpdate)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
                var historyItem = dbContext.JobExecutionHistoryItems.SingleOrDefault(item => item.Id == itemToUpdate.Id);
                if (historyItem != null)
                {
                    dbContext.Entry(historyItem).CurrentValues.SetValues(itemToUpdate);
                    dbContext.SaveChanges();
                }
            }, ExecutionHistoryLocker, "Updating History Item");
        }

        public JobExecutionHistoryItem AddItem(JobExecutionHistoryItem jobExecutionHistoryItem)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<ClientPortalProgContext>(dbContext =>
            {
               dbContext.JobExecutionHistoryItems.Add(jobExecutionHistoryItem);
                dbContext.SaveChanges();
            }, ExecutionHistoryLocker, "Inserting History item.");
            return jobExecutionHistoryItem;
        }
    }
}
