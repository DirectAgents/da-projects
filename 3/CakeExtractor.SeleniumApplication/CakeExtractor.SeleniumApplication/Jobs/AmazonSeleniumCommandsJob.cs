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
            var commands = context.Scheduler.Context.Get(JobConstants.CommandsJobContextValue) as List<BaseAmazonSeleniumCommand>;
            commands.ForEach(RunCommand);
        }

        private void RunCommand(BaseAmazonSeleniumCommand command)
        {
            command.Run();
        }
    }
}
