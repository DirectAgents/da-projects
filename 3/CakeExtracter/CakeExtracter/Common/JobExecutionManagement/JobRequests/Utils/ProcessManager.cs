using System.Diagnostics;
using System.Reflection;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    internal static class ProcessManager
    {
        public static void RestartApplicationInNewProcess(string arguments)
        {
            var location = GetApplicationEntryFileLocation();
            StartNewProcess(location, arguments);
        }
        
        private static string GetApplicationEntryFileLocation()
        {
            var location = Assembly.GetEntryAssembly().Location;
            return location;
        }

        private static void StartNewProcess(string exeLocation, string exeArguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exeLocation,
                Arguments = exeArguments,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(startInfo);
        }
    }
}
