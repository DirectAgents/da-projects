using System;
using CakeExtracter.Common;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    internal class WithRetriesCreatedEqualToParentTestCommand : ConsoleCommand
    {
        private readonly bool logError;

        public WithRetriesCreatedEqualToParentTestCommand(bool logError)
        {
            this.logError = logError;
        }

        public override int Execute(string[] remainingArguments)
        {
            if (logError)
            {
                var exception = new Exception("Test error");
                Logger.Error(exception);
            }
            ScheduleNewCommandLaunch((WithRetriesCreatedEqualToParentTestCommand command) => { });
            ScheduleNewCommandLaunch((WithRetriesCreatedEqualToParentTestCommand command) => { });
            return 0;
        }
    }
}
