using System.Collections.Generic;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExecutionNotificationService"/> class.
        /// </summary>
        /// <param name="jobRequestExecutionRepository">The job request execution repository.</param>
        public JobExecutionNotificationService(IBaseRepository<JobRequestExecution> jobRequestExecutionRepository)
        {
            this.jobRequestExecutionRepository = jobRequestExecutionRepository;
        }

        /// <summary>
        /// Gets the execution items for error notifying.
        /// </summary>
        /// <returns>
        /// Collection of jobs with error that was not notified.
        /// </returns>
        public List<JobRequestExecution> GetExecutionItemsForErrorNotifying()
        {
            return jobRequestExecutionRepository.GetItems(item => item.ErrorEmailSent == false && item.Errors != null);
        }

        /// <summary>
        /// Notifies the about failed jobs.
        /// </summary>
        /// <param name="jobsToNotify">The jobs to notify.</param>
        public void NotifyAboutFailedJobs(List<JobRequestExecution> jobsToNotify)
        {
            jobsToNotify.ForEach(job =>
            {

            });
        }
    }
}
