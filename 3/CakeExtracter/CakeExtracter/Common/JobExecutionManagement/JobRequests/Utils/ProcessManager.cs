using System.Diagnostics;
using System.Reflection;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    /// <summary>
    /// A utility that helps manage the creation of processes.
    /// </summary>
    internal static class ProcessManager
    {
        /// <summary>
        /// Starts a new process of the current application with certain parameters.
        /// </summary>
        /// <param name="arguments">Command-line arguments to use when starting the application.</param>
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
