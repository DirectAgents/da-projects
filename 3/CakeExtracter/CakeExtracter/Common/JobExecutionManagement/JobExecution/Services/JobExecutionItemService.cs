using System;
using System.Linq;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    /// <summary>
    /// Job execution history management service.
    /// </summary>
    /// <seealso cref="IJobExecutionHistoryItemService" />
    public class JobExecutionItemService : IJobExecutionItemService
    {
        IJobExecutionItemRepository jobExecutionHistoryRepository;

        public JobExecutionItemService(IJobExecutionItemRepository jobExecutionHistoryRepository)
        {
            this.jobExecutionHistoryRepository = jobExecutionHistoryRepository;
        }

        public JobRequestExecution CreateJobExecutionItem(JobRequest jobRequest)
        {
            var jobRequestExecutionItem = new JobRequestExecution
            {
                Status = JobExecutionStatus.Processing,
                JobRequestId = 2,//TODO rid of job request  hardcode value
                StartTime = DateTime.UtcNow
            };
            jobExecutionHistoryRepository.AddItem(jobRequestExecutionItem);
            return jobRequestExecutionItem;
        }

        public void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Failed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Completed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        private string GetCommandArgumentsString(string[] arguments)
        {
            arguments = arguments.Skip(1).ToArray(); // first item is .exe file path
            return string.Join(" ", arguments);
        }

        public void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem)
        {
            throw new NotImplementedException();
        }

        public void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem)
        {
            throw new NotImplementedException();
        }
    }
}
