﻿using System;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Utils;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Job execution history management service.
    /// </summary>
    /// <seealso cref="IJobExecutionItemService" />
    public class JobExecutionItemService : IJobExecutionItemService
    {
        private readonly IJobExecutionItemRepository jobExecutionHistoryRepository;

        private readonly object executionItemHistoryLockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionItemService"/> class.
        /// </summary>
        /// <param name="jobExecutionHistoryRepository">The job execution history repository.</param>
        public JobExecutionItemService(IJobExecutionItemRepository jobExecutionHistoryRepository)
        {
            this.jobExecutionHistoryRepository = jobExecutionHistoryRepository;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the job execution item.
        /// </summary>
        /// <param name="jobRequest">The job request to create.</param>
        /// <returns></returns>
        public JobRequestExecution CreateJobExecutionItem(JobRequest jobRequest)
        {
            var jobRequestExecutionItem = new JobRequestExecution
            {
                Status = JobExecutionStatus.Processing,
                JobRequestId = jobRequest.Id,
                StartTime = DateTime.UtcNow
            };
            lock (executionItemHistoryLockObject)
            {
                jobExecutionHistoryRepository.AddItem(jobRequestExecutionItem);
            }
            return jobRequestExecutionItem;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the state of the job execution item failed.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        public void SetJobExecutionItemFailedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Failed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            lock (executionItemHistoryLockObject)
            {
                jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the state of the job execution item finished.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        public void SetJobExecutionItemFinishedState(JobRequestExecution executionHistoryItem)
        {
            executionHistoryItem.Status = JobExecutionStatus.Completed;
            executionHistoryItem.EndTime = DateTime.UtcNow;
            lock (executionItemHistoryLockObject)
            {
                jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the error to job execution item.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void AddErrorToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            lock (executionItemHistoryLockObject)
            {
                executionHistoryItem.Errors = accountId.HasValue
                    ? ExecutionLoggingUtils.AddAccountMessageToLogData(executionHistoryItem.Errors, message, accountId.Value)
                    : ExecutionLoggingUtils.AddCommonMessageToLogData(executionHistoryItem.Errors, message);
                jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the warning to job execution item.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void AddWarningToJobExecutionItem(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            lock (executionItemHistoryLockObject)
            {
                executionHistoryItem.Warnings = accountId.HasValue
                    ? ExecutionLoggingUtils.AddAccountMessageToLogData(executionHistoryItem.Warnings, message, accountId.Value)
                    : ExecutionLoggingUtils.AddCommonMessageToLogData(executionHistoryItem.Warnings, message);
                jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the state message.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void SetStateMessage(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            lock (executionItemHistoryLockObject)
            {
                executionHistoryItem.CurrentState = accountId.HasValue
                    ? ExecutionLoggingUtils.SetSingleAccountMessageInLogData(executionHistoryItem.CurrentState, message, accountId.Value)
                    : ExecutionLoggingUtils.SetSingleCommonMessageInLogData(executionHistoryItem.CurrentState, message);
                jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the state message to job execution item.
        /// </summary>
        /// <param name="executionHistoryItem">The execution history item.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        public void AddStateMessage(JobRequestExecution executionHistoryItem, string message, int? accountId = null)
        {
            var messageWithTimeStamp = $"{DateTime.UtcNow.ToLongTimeString()}: {message}";
            lock (executionItemHistoryLockObject)
            {
                executionHistoryItem.CurrentState = accountId.HasValue
                    ? ExecutionLoggingUtils.AddAccountMessageToLogData(executionHistoryItem.CurrentState, messageWithTimeStamp, accountId.Value)
                    : ExecutionLoggingUtils.AddCommonMessageToLogData(executionHistoryItem.CurrentState, messageWithTimeStamp);
                jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
            }
        }

        /// <summary>
        /// Sets the state of the job execution items to aborted.
        /// </summary>
        /// <param name="itemIds">The item ids.</param>
        public void SetJobExecutionItemsAbortedState(int[] itemIds)
        {
            if (itemIds != null && itemIds.Length >= 1)
            {
                var itemsToUpdate = jobExecutionHistoryRepository.GetAll(item => itemIds.Contains(item.Id)).ToList();
                itemsToUpdate.ForEach(executionHistoryItem =>
                {
                    executionHistoryItem.Status = JobExecutionStatus.Aborted;
                    lock (executionItemHistoryLockObject)
                    {
                        jobExecutionHistoryRepository.UpdateItem(executionHistoryItem);
                    }
                });
            }
        }
    }
}
