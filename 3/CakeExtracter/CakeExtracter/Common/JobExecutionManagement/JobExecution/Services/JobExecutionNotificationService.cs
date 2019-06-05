using System;
using System.Collections.Generic;
using CakeExtracter.Commands.Core;
using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Models;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Utils;
using CakeExtracter.Helpers;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Services
{
    /// <summary>
    /// Job Execution Notification Service.
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.JobExecutionManagement.JobExecution.Services.IJobExecutionNotificationService" />
    public class JobExecutionNotificationService : IJobExecutionNotificationService
    {
        private readonly IBaseRepository<JobRequestExecution> jobRequestExecutionRepository;

        private readonly IBaseRepository<JobRequest> jobRequestsRepository;

        private readonly IEmailNotificationsService emailNotificationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionNotificationService"/> class.
        /// </summary>
        /// <param name="jobRequestExecutionRepository">Job requests execution repository.</param>
        /// <param name="emailNotificationsService">Email notification service.</param>
        /// <param name="jobRequestsRepository">Job requests repository.</param>
        public JobExecutionNotificationService(
                IBaseRepository<JobRequestExecution> jobRequestExecutionRepository,
                IBaseRepository<JobRequest> jobRequestsRepository,
                IEmailNotificationsService emailNotificationsService)
        {
            this.jobRequestExecutionRepository = jobRequestExecutionRepository;
            this.emailNotificationsService = emailNotificationsService;
            this.jobRequestsRepository = jobRequestsRepository;
        }

        /// <inheritdoc />
        public void NotifyAboutFailedJobs()
        {
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
                    CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Sent {jobExecutionItemsToNotify.Count} execution items with errors notifications.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private List<JobRequestExecution> GetExecutionItemsForErrorNotifying()
        {
            return jobRequestExecutionRepository.GetItemsWithIncludes(
                    item => item.ErrorEmailSent == false &&
                    item.Errors != null &&
                    item.JobRequest.CommandName != FailedJobsNotifierCommand.CommandName, "JobRequest");
        }

        private void NotifyAboutErrorsInJobExecutionItems(List<JobRequestExecution> jobsToNotify)
        {
            const string toEmailsConfigurationKey = "JEM_Failure_ToEmails";
            const string copyEmailsConfigurationKey = "JEM_Failure_CcEmails";
            var toEmails = ConfigurationHelper.ExtractEnumerableFromConfig(toEmailsConfigurationKey).ToArray();
            var copyEmails = ConfigurationHelper.ExtractEnumerableFromConfig(copyEmailsConfigurationKey).ToArray();
            jobsToNotify.ForEach(job =>
            {
                SendFailedJobNotification(job, toEmails, copyEmails);
            });
        }

        private void SendFailedJobNotification(JobRequestExecution jobToNotify, string[] toEmails, string[] copyEmails)
        {
            var model = PrepareFailedJobNotificationModel(jobToNotify);
            const string bodyTemplateName = "JobFailedBody";
            const string subjectTemplateName = "JobFailedSubject";
            emailNotificationsService.SendEmail(toEmails, copyEmails, model, bodyTemplateName, subjectTemplateName);
        }

        private FailedJobNotificationModel PrepareFailedJobNotificationModel(JobRequestExecution jobToNotify)
        {
            return new FailedJobNotificationModel
            {
                JobRequest = jobToNotify.JobRequest,
                JobRequestExecution = jobToNotify,
                ExecutionStartTime = jobToNotify.StartTime?.ToLocalTime().ToString(),
                Errors = ExecutionLoggingUtils.GetJobExecutionLogDataFromMessageText(jobToNotify.Errors),
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
    }
}
