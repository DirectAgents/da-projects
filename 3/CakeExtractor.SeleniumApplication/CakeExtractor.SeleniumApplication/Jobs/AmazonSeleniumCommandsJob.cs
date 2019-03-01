using CakeExtracter;
using CakeExtractor.SeleniumApplication.Commands;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CakeExtractor.SeleniumApplication.Jobs
{
    public class AmazonSeleniumCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Logger.Info("Started execution of selenium jobs.");
            var commands = context.Scheduler.Context.Get(JobConstants.CommandsJobContextValue) as List<BaseAmazonSeleniumCommand>;
            commands.ForEach(RunCommand);
            Logger.Info("Selenium Jobs finished.");
            Logger.Info("Waiting for the scheduled selenium jobs: next run time - {0} (UTC)...", context.NextFireTimeUtc);
        }

        private void RunCommand(BaseAmazonSeleniumCommand command)
        {
            try
            {
                Logger.Info("Started execution of {0} command", command.CommandName);
                command.Run();
                Logger.Info("Finished execution of {0} command", command.CommandName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Warn("Error occured while execution {0} command. Command Skipped", command.CommandName);
            }
        }
    }
}
