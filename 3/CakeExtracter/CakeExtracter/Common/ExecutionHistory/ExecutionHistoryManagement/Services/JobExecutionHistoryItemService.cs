using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    /// <summary>
    /// Job execution history management service.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement.IJobExecutionHistoryItemService" />
    public class JobExecutionHistoryItemService : IJobExecutionHistoryItemService
    {
        IJobExecutionHistoryItemRepository jobExecutionHistoryRepository;

        public JobExecutionHistoryItemService(IJobExecutionHistoryItemRepository jobExecutionHistoryRepository)
        {
            this.jobExecutionHistoryRepository = jobExecutionHistoryRepository;
        }

        public JobRequestExecution CreateJobExecutionHistoryItem(string commandName, string[] argumentsString)
        {

            var jobExecutionHistoryitem = new JobRequestExecution
            {
                Status = JobExecutionStatus.Processing,
                JobRequest = new JobRequest
                {
                    AttemptNumber = 1,
                    CommandExecutionArguments = "test stuff",
                    CommandName = commandName,
                    Status = JobRequestStatus.Processing
                },
                StartTime = DateTime.UtcNow
            };
            jobExecutionHistoryRepository.AddItem(jobExecutionHistoryitem);
            return jobExecutionHistoryitem;
        }
    }
}
