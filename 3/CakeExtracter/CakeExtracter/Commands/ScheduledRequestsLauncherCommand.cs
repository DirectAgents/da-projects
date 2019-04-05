using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    internal class ScheduledRequestsLauncherCommand : ConsoleCommand
    {
        public ScheduledRequestsLauncherCommand()
        {
            IsCommand("ScheduledRequestsLauncherCommand", "Runs Job Requests that have been scheduled");
        }

        public override void ResetProperties()
        {
        }

        public override int Execute(string[] remainingArguments)
        {
            var maxNumberOfJobRequests = int.Parse(ConfigurationManager.AppSettings["MaxNumberOfRequestsToRunWithUniqueArguments"]);
            CommandExecutionContext.Current.JobRequestManager.ExecuteScheduledInPastJobRequests(maxNumberOfJobRequests);
            return 0;
        }
    }
}
