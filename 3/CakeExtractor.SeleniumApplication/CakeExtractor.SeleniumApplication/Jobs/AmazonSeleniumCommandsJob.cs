using CakeExtracter;
using CakeExtracter.Common;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace CakeExtractor.SeleniumApplication.Jobs
{
    public class AmazonSeleniumCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Logger.Info("Started execution of selenium jobs.");
            var args = context.Scheduler.Context.Get(JobConstants.CommandLineArgs) as string[];
            var commands = context.Scheduler.Context.Get(JobConstants.AllCommands) as IEnumerable<ConsoleCommand>;
            RunCommand(args, commands);
            Logger.Info("Selenium Jobs finished.");
            Logger.Info("Waiting for the scheduled selenium jobs: next run time - {0} (UTC)...", context.NextFireTimeUtc);
        }

        private void RunCommand(string[] args, IEnumerable<ConsoleCommand> Commands)
        {
            try
            {
                Logger.Info("Started execution of {0} command", args[0]);
                ManyConsole.ConsoleCommandDispatcher.DispatchCommand(Commands, args, Console.Out);
                Logger.Info("Finished execution of {0} command", args[0]);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Logger.Warn("Error occured while execution {0} command. Command Skipped", args[0]);
            }
        }
    }
}
