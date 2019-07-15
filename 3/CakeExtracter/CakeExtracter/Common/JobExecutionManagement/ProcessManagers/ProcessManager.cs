using System;
using System.Diagnostics;
using System.Reflection;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces;

namespace CakeExtracter.Common.JobExecutionManagement.ProcessManagers
{
    /// <inheritdoc />
    public class ProcessManager : IProcessManager
    {
        /// <inheritdoc />
        public void RestartApplicationInNewProcess(string arguments)
        {
            var location = GetApplicationEntryFileLocation();
            StartNewProcess(location, arguments);
        }

        /// <inheritdoc />
        public void EndCurrentProcess()
        {
            Environment.Exit(0);
        }

        private static string GetApplicationEntryFileLocation()
        {
            var assembly = Assembly.GetEntryAssembly();
            return assembly?.Location;
        }

        private static void StartNewProcess(string exeLocation, string exeArguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exeLocation,
                Arguments = exeArguments,
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            Process.Start(startInfo);
        }
    }
}
