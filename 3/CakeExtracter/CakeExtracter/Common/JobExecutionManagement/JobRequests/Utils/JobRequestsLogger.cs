using System;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    static class JobRequestsLogger
    {
        public static string LogInfo(string message, JobRequest request)
        {
            var info = GetMessageAboutJobRequest(message, request);
            Logger.Info(info);
            return info;
        }

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
