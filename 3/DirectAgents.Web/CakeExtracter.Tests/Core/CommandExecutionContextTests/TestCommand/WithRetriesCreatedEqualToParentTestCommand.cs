using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    class WithRetriesCreatedEqualToParentTestCommand : ConsoleCommand
    {
        public override int Execute(string[] remainingArguments)
        {
            ScheduleNewCommandLaunch((WithRetriesCreatedEqualToParentTestCommand command) => { });
            ScheduleNewCommandLaunch((WithRetriesCreatedEqualToParentTestCommand command) => { });
            return 0;
        }
    }
}
