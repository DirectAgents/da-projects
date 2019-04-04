using System;
using System.Collections.Generic;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Utils;
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

        public void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            if (accountId.HasValue)
            {
                executionHistoryItem.Errors = 
                    ExecutionLoggingUtils.AddAccountMessageToLogData(executionHistoryItem.Errors, message, accountId.Value);
            }
            else
            {
                executionHistoryItem.Errors =
                    ExecutionLoggingUtils.AddCommonMessageToLogData(executionHistoryItem.Errors, message);
            }
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            if (accountId.HasValue)
            {
                executionHistoryItem.Warnings =
                    ExecutionLoggingUtils.AddAccountMessageToLogData(executionHistoryItem.Warnings, message, accountId.Value);
            }
            else
            {
                executionHistoryItem.Warnings =
                    ExecutionLoggingUtils.AddCommonMessageToLogData(executionHistoryItem.Warnings, message);
            }
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public void AddStateMessage(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            if (accountId.HasValue)
            {
                executionHistoryItem.CurrentState =
                    ExecutionLoggingUtils.SetSingleAccountMessageInLogData(executionHistoryItem.Errors, message, accountId.Value);
            }
            else
            {
                executionHistoryItem.CurrentState =
                    ExecutionLoggingUtils.SetSingleCommonMessageInLogData(executionHistoryItem.Errors, message);
            }
            jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
        }

        public List<JobRequestExecution> GetJobExecutionHistoryItems()
        {
            return jobExecutionHistoryRepository.GetAll(x=>true);
        }
    }
}
