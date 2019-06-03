using System.Collections.Generic;
using System.Configuration;
using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Models;
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
            var jobsToNotify = GetExecutionItemsForErrorNotifying();
            NotifyAboutFailedJobs(jobsToNotify);
            MarkFailedJobsAsProcessed(jobsToNotify);
        }

        private List<JobRequestExecution> GetExecutionItemsForErrorNotifying()
        {
            return jobRequestExecutionRepository.GetItems(item => item.ErrorEmailSent == false && item.Errors != null);
        }

        private void NotifyAboutFailedJobs(List<JobRequestExecution> jobsToNotify)
        {
            const string toEmailsConfigurationKey = "JEM_Failure_ToEmails";
            const string copyEmailsConfigurationKey = "JEM_Failure_CopyEmails";

            var toEmails = GetEmailsFromConfig(toEmailsConfigurationKey);
            var copyEmails = GetEmailsFromConfig(copyEmailsConfigurationKey);

            jobsToNotify.ForEach(job =>
            {
                SendFaildJobNotification(job, toEmails, copyEmails);
            });
        }

        private void SendFaildJobNotification(JobRequestExecution jobToNotify,string[] toEmails, string[] copyEmails)
        {
            var notificationModel = PrepareFailedJobNotificationModel(jobToNotify);
            const string notificationTemplateName = "JobFailed";
            emailNotificationsService.SendEmail<FailedJobNotificationModel>(toEmails, copyEmails, notificationModel, notificationTemplateName);
        }

        private FailedJobNotificationModel PrepareFailedJobNotificationModel(JobRequestExecution jobToNotify)
        {
            return new FailedJobNotificationModel
            {
                Errors = jobToNotify.Errors,
            };
        }

        private string[] GetEmailsFromConfig(string configurationKey)
        {
            var emailsConfigValue = ConfigurationManager.AppSettings[configurationKey];
            if (!string.IsNullOrEmpty(emailsConfigValue))
            {
                return emailsConfigValue.Split(';');
            }
            return new string[0];
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
