using DirectAgents.Domain.Entities.Administration.JobExecutionHistory;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionHistoryItemService
    {
        JobExecutionHistoryItem CreateJobExecutionHistoryItem(string comamndName, string[] argumentsString);
    }
}
