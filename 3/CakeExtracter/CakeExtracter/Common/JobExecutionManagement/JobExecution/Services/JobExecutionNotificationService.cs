using System;
using System.Collections.Generic;
using System.Configuration;
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

        private readonly IEmailNotificationsService emailNotificationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionNotificationService"/> class.
        /// </summary>
        /// <param name="jobRequestExecutionRepository">Job Requests Execution Repository.</param>
        /// <param name="emailNotificationsService">Email Notification Service.</param>
        public JobExecutionNotificationService(IBaseRepository<JobRequestExecution> jobRequestExecutionRepository, IEmailNotificationsService emailNotificationsService)
        {
            this.jobRequestExecutionRepository = jobRequestExecutionRepository;
            this.emailNotificationsService = emailNotificationsService;
        }

        /// <summary>
        /// Notifies about failed jobs.
        /// </summary>
        public void NotifyAboutFailedJobs()
        {
            try
            {
                var jobsToNotify = GetExecutionItemsForErrorNotifying();
                if (jobsToNotify?.Count > 0)
                {
                    NotifyAboutFailedJobs(jobsToNotify);
                    MarkFailedJobsAsProcessed(jobsToNotify);
                    CommandExecutionContext.Current.SetJobExecutionStateInHistory($"Sent {jobsToNotify.Count} failed jobs notifications.");
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

        private void NotifyAboutFailedJobs(List<JobRequestExecution> jobsToNotify)
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

       

        private void MarkFailedJobsAsProcessed(List<JobRequestExecution> jobs)
        {
            jobs.ForEach(job =>
            {
                job.ErrorEmailSent = true;
            });
            jobRequestExecutionRepository.UpdateItems(jobs);
        }
    }
}
