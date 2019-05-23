using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    static class JobRequestsLogger
    {
        public static void LogInfo(string message, JobRequest request)
        {
            var info = GetMessageAboutJobRequest(message, request);
            Logger.Info(info);
        }

        public static void LogError(string message, JobRequest request)
        {
            var info = GetMessageAboutJobRequest(message, request);
            Logger.Error(new Exception(info));
        }

        private static string GetMessageAboutJobRequest(string message, JobRequest request)
        {
            var arguments = CommandArgumentsConverter.GetJobArgumentsAsArgumentsForConsole(request);
            var info = $"{message} ({request.Id}, {request.ScheduledTime}): \"{request.CommandName} {arguments}\"";
            return info;
        }
    }
}
