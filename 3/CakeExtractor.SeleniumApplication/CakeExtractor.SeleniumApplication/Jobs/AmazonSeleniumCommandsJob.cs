using CakeExtractor.SeleniumApplication.Commands;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CakeExtractor.SeleniumApplication.Jobs
{
    public class AmazonSeleniumCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var commands = context.Scheduler.Context.Get(JobConstants.CommandsJobContextValue) as List<BaseAmazoneSeleniumCommand>;
            var args = context.Scheduler.Context.Get("args") as string[];
            commands.ForEach(command =>
            {
                command.Run(args);
            });
        }
    }
}
