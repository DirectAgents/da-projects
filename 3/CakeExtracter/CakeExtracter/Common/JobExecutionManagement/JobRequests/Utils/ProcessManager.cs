using System.Diagnostics;
using System.Reflection;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils
{
    internal static class ProcessManager
    {
        public static void StartConsoleApplicationInNewProcess(string arguments)
        {
            var location = GetConsoleApplicationLocation();
            StartNewProcess(location, arguments);
        }

        //TODO: what to do if the entry assembly is a web application and is not a console? 
        private static string GetConsoleApplicationLocation()
        {
            var location = Assembly.GetEntryAssembly().Location;
            return location;
        }

        private static void StartNewProcess(string exeLocation, string exeArguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exeLocation,
                Arguments = exeArguments
            };
            Process.Start(startInfo);
        }
    }
}
