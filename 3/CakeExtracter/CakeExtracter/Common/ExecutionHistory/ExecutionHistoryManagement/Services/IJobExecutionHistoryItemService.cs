using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public interface IJobExecutionHistoryItemService
    {
        JobRequestExecution CreateJobExecutionHistoryItem(string comamndName, string[] argumentsString);
    }
}
