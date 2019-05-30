﻿using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces
{
    /// <summary>
    /// Service for work with job request items.
    /// </summary>
    public interface IJobExecutionRequestScheduler
    {
        /// <summary>
        /// Returns a saved job request object for a command.
        /// </summary>
        /// <param name="command">The command for which job request is extracting.</param>
        /// <returns>The saved job request object.</returns>
        JobRequest GetJobRequest(ConsoleCommand command);

        /// <summary>
        /// Sets a status of the job request to Processing.
        /// </summary>
        /// <param name="request">The job request.</param>
        void SetJobRequestAsProcessing(JobRequest request);

        /// <summary>
        /// Schedules a new command that should become scheduled job requests.
        /// </summary>
        /// <param name="command">The command to launch.</param>
        void ScheduleCommandLaunch(ConsoleCommand command);

        /// <summary>
        /// Schedules a new job request.
        /// </summary>
        /// <param name="jobRequest">The job request to launch.</param>
        void ScheduleJobRequest(JobRequest jobRequest);

        /// <summary>
        /// Reschedules the job request.
        /// </summary>
        /// <param name="request">The job request.</param>
        /// <param name="sourceCommand">The source command of the request.</param>
        void RescheduleRequest(JobRequest request, ConsoleCommand sourceCommand);

        /// <summary>
        /// Creates job requests for scheduled commands based on the current command and job request.
        /// </summary>
        /// <param name="sourceCommand">The source command.</param>
        /// <param name="sourceRequest">The source job request.</param>
        void CreateRequestsForScheduledCommands(ConsoleCommand sourceCommand, JobRequest sourceRequest);

        /// <summary>
        /// Ends the job request.
        /// </summary>
        /// <param name="request">The job request.</param>
        void EndRequest(JobRequest request);
    }
}
