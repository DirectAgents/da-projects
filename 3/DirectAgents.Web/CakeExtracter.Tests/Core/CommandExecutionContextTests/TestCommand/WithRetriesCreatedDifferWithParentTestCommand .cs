using CakeExtracter.Common;
using System;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    class WithRetriesCreatedDifferWithParentTestCommand : ConsoleCommand
    {
        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public WithRetriesCreatedDifferWithParentTestCommand()
        {
            IsCommand("WithRetriesCreatedDifferWithParentTestCommand", "WithRetriesCreatedDifferWithParentTestCommand");
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            ScheduleNewCommandLaunch((WithRetriesCreatedDifferWithParentTestCommand command) => {
                command.StartDate = command.StartDate?.AddDays(1);
                command.EndDate = command.EndDate?.AddDays(-1);
            });
            return 0;
        }
    }
}
