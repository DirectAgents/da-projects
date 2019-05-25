using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// The logger for info about job request items.
    /// </summary>
    internal static class JobRequestsLogger
    {
        /// <summary>
        /// Writes a new log like an info.
        /// </summary>
        /// <param name="message">Message for logging.</param>
        /// <param name="request">Logged job request.</param>
        /// <returns>Logged message.</returns>
        public static string LogInfo(string message, JobRequest request)
        {
            var info = GetMessageAboutJobRequest(message, request);
            Logger.Info(info);
            return info;
        }

        /// <summary>
        /// Writes a new log like an error.
        /// </summary>
        /// <param name="message">Message for logging.</param>
        /// <param name="request">Logged job request.</param>
        /// <returns>Logged message.</returns>
        public static string LogError(string message, JobRequest request)
        {
            var info = GetMessageAboutJobRequest(message, request);
            Logger.Error(new Exception(info));
            return info;
        }

        private static string GetMessageAboutJobRequest(string message, JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            var info = $"{message} ({request.Id}, {request.ScheduledTime}): \"{request.CommandName} {arguments}\"";
            return info;
        }
    }
}
