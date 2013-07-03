using System.ComponentModel.Composition;
using CakeExtracter.Common;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SchedulerCommand : ConsoleCommand
    {
        public SchedulerCommand()
        {
            IsCommand("scheduler", "Run the scheduler");
        }

        public override int Execute(string[] remainingArguments)
        {
            var scheduler = new Scheduler();
            scheduler.Run();
            return 0;
        }
    }
}
