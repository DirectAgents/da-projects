﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Analytic.Common;
using CakeExtracter.Commands.Core;
using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Constants;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Models;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Utils;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <summary>
    /// Job Execution Notification Service.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.JobExecutionManagement.JobExecution.Services.IJobExecutionNotificationService" />
    public class JobExecutionNotificationService : IJobExecutionNotificationService
    {
        private readonly IBaseRepository<JobRequestExecution> jobRequestExecutionRepository;

        private readonly IJobRequestsRepository jobRequestsRepository;

        private readonly IEmailNotificationsService emailNotificationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionNotificationService"/> class.
        /// </summary>
        /// <param name="jobRequestExecutionRepository">Job requests execution repository.</param>
        /// <param name="emailNotificationsService">Email notification service.</param>
        /// <param name="jobRequestsRepository">Job requests repository.</param>
        public JobExecutionNotificationService(
                IBaseRepository<JobRequestExecution> jobRequestExecutionRepository,
                IJobRequestsRepository jobRequestsRepository,
                IEmailNotificationsService emailNotificationsService)
        {
            this.jobRequestExecutionRepository = jobRequestExecutionRepository;
            this.emailNotificationsService = emailNotificationsService;
            this.jobRequestsRepository = jobRequestsRepository;
        }

        /// <inheritdoc />
        public void NotifyAboutFailedJobs()
        {
            try
            {
                var failedJobs = GetFailedParentJobsForNotifying();
                if (failedJobs?.Count > 0)
                {
                    NotifyAboutFailedJobs(failedJobs);
                    MarkJobRequestItemsAsEmailSent(failedJobs);
                    CommandExecutionContext.Current.AppendJobExecutionStateInHistory($"Sent {failedJobs.Count} failed jobs notifications.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <inheritdoc />
        public void NotifyAboutErrorsInJobExecution()
        {
            try
            {
                var jobExecutionItemsToNotify = GetExecutionItemsForErrorNotifying();
                if (jobExecutionItemsToNotify?.Count > 0)
                {
                    NotifyAboutErrorsInJobExecutionItems(jobExecutionItemsToNotify);
                    MarkJobExecutionItemsAsEmailSent(jobExecutionItemsToNotify);
                    CommandExecutionContext.Current.AppendJobExecutionStateInHistory($"Sent {jobExecutionItemsToNotify.Count} execution items with errors notifications.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <inheritdoc />
        public void NotifyAboutProcessingJobs(Dictionary<string, string> filter)
        {
            try
            {
                var processingJobs = GetProcessingJobsForNotifying(filter);
                if (processingJobs?.Count > 0)
                {
                    NotifyAboutProcessingJobs(processingJobs);
                    CommandExecutionContext.Current.AppendJobExecutionStateInHistory($"Sent {processingJobs.Count} processing jobs notifications.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <inheritdoc />
        public void NotifyAboutSynchAnalyticIssues()
        {
            var analyticHealthCheckService = new AnalyticHealthCheckService();
            var notUpdatedTables = analyticHealthCheckService.GetNotUpdatedTables().ToList();
            NotifyAboutAnalyticIssues(notUpdatedTables);
        }

        private List<JobRequestExecution> GetExecutionItemsForErrorNotifying()
        {
            return jobRequestExecutionRepository.GetItemsWithIncludes(
                    item => item.ErrorEmailSent == false &&
                    item.Errors != null &&
                    item.JobRequest.CommandName != FailedJobsNotifierCommand.CommandName, nameof(JobRequestExecution.JobRequest));
        }

        private List<JobRequest> GetFailedParentJobsForNotifying()
        {
            return jobRequestsRepository.GetItems(
                    item => item.FailureEmailSent == false &&
                    item.Status == JobRequestStatus.Failed &&
                    item.ParentJobRequestId == null && // Emails should be not sent for child(retries) requests.
                    item.CommandName != FailedJobsNotifierCommand.CommandName);
        }

        private List<JobRequestExecution> GetProcessingJobsForNotifying(Dictionary<string, string> filter)
        {
            var jobNamesList = filter.Keys.ToList();
            return jobRequestExecutionRepository.GetItemsWithIncludes(
                item => item.JobRequest.Status == JobRequestStatus.Processing &&
                item.StartTime >= DateTime.Today &&
                jobNamesList.Contains(item.JobRequest.CommandName) &&
                item.JobRequest.CommandExecutionArguments.Contains(filter[item.JobRequest.CommandName]),
                nameof(JobRequestExecution.JobRequest));
        }

        private void NotifyAboutErrorsInJobExecutionItems(List<JobRequestExecution> jobExecutionsToNotify)
        {
            var toEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobErrorOccurredToConfigKey).ToArray();
            var ccEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobErrorOccurredCcConfigKey).ToArray();
            jobExecutionsToNotify.ForEach(jobExecution =>
            {
                var model = PrepareErrorInJobNotificationModel(jobExecution);
                emailNotificationsService.SendEmail(toEmails, ccEmails, model, EmailConfigConstants.JobErrorOccurredBodyTemplateName, EmailConfigConstants.JobErrorOccurredSubjectTemplateName);
            });
        }

        private void NotifyAboutProcessingJobs(List<JobRequestExecution> jobExecutionsToNotify)
        {
            var toEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobProcessingToConfigKey).ToArray();
            var ccEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobProcessingCcConfigKey).ToArray();

            jobExecutionsToNotify.ForEach(jobExecution =>
            {
                var model = PrepareErrorInJobNotificationModel(jobExecution);
                emailNotificationsService.SendEmail(toEmails, ccEmails, model, EmailConfigConstants.JobProcessingBodyTemplateName, EmailConfigConstants.JobProcessingSubjectTemplateName);
            });
        }

        private void NotifyAboutFailedJobs(List<JobRequest> jobsToNotify)
        {
            var toEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobFailedToConfigKey).ToArray();
            var ccEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobFailedCcConfigKey).ToArray();
            jobsToNotify.ForEach(job =>
            {
                var model = PrepareJobFailedNotificationModel(job);
                emailNotificationsService.SendEmail(toEmails, ccEmails, model, EmailConfigConstants.JobFailedBodyTemplateName, EmailConfigConstants.JobFailedSubjectTemplateName);
            });
        }

        private void NotifyAboutAnalyticIssues(List<string> tablesToNotify)
        {
            var toEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobErrorOccurredToConfigKey).ToArray();
            var ccEmails = ConfigurationHelper.ExtractEnumerableFromConfig(EmailConfigConstants.JobErrorOccurredCcConfigKey).ToArray();

            tablesToNotify.ForEach(table =>
            {
                emailNotificationsService.SendEmail(
                    toEmails,
                    ccEmails,
                    table,
                    EmailConfigConstants.AnalyticIssueBodyTemplateName,
                    EmailConfigConstants.AnalyticIssueSubjectTemplateName);
            });
        }

        private FailedJobNotificationModel PrepareJobFailedNotificationModel(JobRequest jobRequest)
        {
            return new FailedJobNotificationModel
            {
                JobRequest = jobRequest,
            };
        }

        private ErrorInJobNotificationModel PrepareErrorInJobNotificationModel(JobRequestExecution jobToNotify)
        {
            var localExecutionStartTime = jobToNotify.StartTime?.ToLocalTime();
            return new ErrorInJobNotificationModel
            {
                JobRequest = jobToNotify.JobRequest,
                JobRequestExecution = jobToNotify,
                ExecutionStartTime = localExecutionStartTime?.ToString(),
                ExecutionStartDate = localExecutionStartTime?.ToShortDateString(),
                Errors = ExecutionLoggingUtils.GetJobExecutionLogDataFromMessageText(jobToNotify.Errors),
            };
        }

        private ProcessingJobNotificationModel PrepareJobProcessingNotificationModel(JobRequest jobRequest)
        {
            return new ProcessingJobNotificationModel
            {
                JobRequest = jobRequest,
            };
        }

        private void MarkJobExecutionItemsAsEmailSent(List<JobRequestExecution> jobs)
        {
            jobs.ForEach(job =>
            {
                job.ErrorEmailSent = true;
            });
            jobRequestExecutionRepository.UpdateItems(jobs);
        }

        private void MarkJobRequestItemsAsEmailSent(List<JobRequest> jobRequests)
        {
            jobRequests.ForEach(jobRequest =>
            {
                jobRequest.FailureEmailSent = true;
            });
            jobRequestsRepository.UpdateItems(jobRequests);
        }
    }
}
