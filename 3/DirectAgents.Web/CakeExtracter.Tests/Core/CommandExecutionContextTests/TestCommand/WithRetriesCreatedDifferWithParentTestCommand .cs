using System;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand
{
    internal class WithRetriesCreatedDifferWithParentTestCommand : ConsoleCommand
    {
        private readonly bool logError;

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public WithRetriesCreatedDifferWithParentTestCommand(bool logError)
        {
            IsCommand("WithRetriesCreatedDifferWithParentTestCommand", "WithRetriesCreatedDifferWithParentTestCommand");
            HasOption("s|startDate=", "Start Date (default is 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
            this.logError = logError;
        }

        public override int Execute(string[] remainingArguments)
        {
            if (logError)
            {
                var exception = new Exception("Test error");
                Logger.Error(exception);
            }

            ScheduleNewCommandLaunch((WithRetriesCreatedDifferWithParentTestCommand command) =>
            {
                command.StartDate = command.StartDate?.AddDays(1);
                command.EndDate = command.EndDate?.AddDays(-1);
            });
            return 0;
        }
    }
}
