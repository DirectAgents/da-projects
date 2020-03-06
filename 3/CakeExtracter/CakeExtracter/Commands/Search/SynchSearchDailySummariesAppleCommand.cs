using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Apple;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.Apple.Exceptions;
using CakeExtracter.Etl.Apple.Extractors;
using CakeExtracter.Etl.Apple.Loaders;
using CakeExtracter.Helpers;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands.Search
{
    /// <inheritdoc />
    /// The class represents a command that is used to retrieve
    /// statistics from the Apple Search Ads Campaign Management API
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAppleCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        /// <summary>
        /// Gets or sets a command argument:
        /// Database identifier of search profile for which the command will be executed (default = all).
        /// </summary>
        public int? SearchProfileId { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// Database account code of search account (like external Id)
        /// for which the command will be executed (default = all).
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// Start date from which statistics will be extracted (default is 'daysAgo').
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// End date to which statistics will be extracted (default is yesterday).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets a command argument:
        /// The number of days ago to calculate the start date from which statistics will be retrieved,
        /// used if StartDate not specified (default = 31).
        /// </summary>
        public int? DaysAgoToStart { get; set; }

        /// <summary>
        /// The static method used for sync from Admin Portal.
        /// </summary>
        /// <param name="searchProfileId">Id of search profile.</param>
        /// <param name="clientId">Account code of search account.</param>
        /// <param name="start">Start date.</param>
        /// <param name="end">End date.</param>
        /// <param name="daysAgoToStart">Days ago.</param>
        /// <returns>Execution code.</returns>
        public static int RunStatic(int? searchProfileId = null, string clientId = null, DateTime? start = null, DateTime? end = null, int? daysAgoToStart = null)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new SynchSearchDailySummariesAppleCommand
            {
                SearchProfileId = searchProfileId,
                ClientId = clientId,
                StartDate = start,
                EndDate = end,
                DaysAgoToStart = daysAgoToStart,
            };
            return cmd.Run();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchSearchDailySummariesAppleCommand"/> class.
        /// </summary>
        public SynchSearchDailySummariesAppleCommand()
        {
            IsCommand("synchSearchDailySummariesApple", "synch SearchDailySummaries for Apple");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<string>("v|clientId=", "Client Id", c => ClientId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (optional)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
        }

        /// <inheritdoc />
        /// <summary>
        /// The method resets command arguments to defaults.
        /// </summary>
        public override void ResetProperties()
        {
            SearchProfileId = null;
            ClientId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        /// <inheritdoc />
        /// <summary>
        /// The method runs the current command and extract and save statistics from
        /// the Apple Search Ads Campaign Management API based on the command arguments.
        /// </summary>
        /// <param name="remainingArguments"></param>
        /// <returns>Execution code.</returns>
        public override int Execute(string[] remainingArguments)
        {
            try
            {
                var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
                Logger.Info("Apple ETL. DateRange {0}.", dateRange);

                var appleAdsUtility = new AppleAdsUtility(m => Logger.Warn(m), m => Logger.Error(m));
                var searchAccounts = GetSearchAccounts();

                foreach (var searchAccount in searchAccounts)
                {
                    var revisedDateRange = ReviseDateRange(dateRange, searchAccount);
                    var extractor = new AppleApiExtracter(appleAdsUtility, revisedDateRange, searchAccount.AccountCode, searchAccount.ExternalId);
                    var loader = new AppleApiLoader(searchAccount);
                    InitEtlEvents(extractor, loader);
                    CommandHelper.DoEtl(extractor, loader);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return 0;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccount = commands.GroupBy(x => (x.Command as SynchSearchDailySummariesAppleCommand)?.ClientId);
            foreach (var commandsGroup in commandsGroupedByAccount)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        private void InitEtlEvents(AppleApiExtracter extractor, AppleApiLoader loader)
        {
            GeneralInitEtlEvents(extractor, loader);
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAppleCommand>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAppleCommand>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void GeneralInitEtlEvents(AppleApiExtracter extractor, AppleApiLoader loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAppleCommand>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAppleCommand>(command => { });
        }

        private DateRange ReviseDateRange(DateRange dateRange, SearchAccount searchAccount)
        {
            var startDate = dateRange.FromDate;
            if (searchAccount.MinSynchDate.HasValue && startDate < searchAccount.MinSynchDate.Value)
            {
                startDate = searchAccount.MinSynchDate.Value;
            }

            var revisedDateRange = new DateRange(startDate, dateRange.ToDate);
            return revisedDateRange;
        }

        private IEnumerable<SearchAccount> GetSearchAccounts()
        {
            return SynchSearchDailySummariesAdWordsCommand.GetSearchAccounts("Apple", this.SearchProfileId, this.ClientId);
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands =
                new List<Tuple<SynchSearchDailySummariesAppleCommand, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (SynchSearchDailySummariesAppleCommand) commandWithSchedule.Command;
                var commandDateRange = CommandHelper.GetDateRange(command.StartDate, command.EndDate, command.DaysAgoToStart, DefaultDaysAgo);
                var crossCommands = accountCommands.Where(x => commandDateRange.IsCrossDateRange(x.Item2)).ToList();
                foreach (var crossCommand in crossCommands)
                {
                    commandDateRange = commandDateRange.MergeDateRange(crossCommand.Item2);
                    commandWithSchedule.ScheduledTime =
                        crossCommand.Item3.ScheduledTime > commandWithSchedule.ScheduledTime
                            ? crossCommand.Item3.ScheduledTime
                            : commandWithSchedule.ScheduledTime;
                    accountCommands.Remove(crossCommand);
                }

                accountCommands.Add(
                    new Tuple<SynchSearchDailySummariesAppleCommand, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<SynchSearchDailySummariesAppleCommand, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private void UpdateCommandParameters(SynchSearchDailySummariesAppleCommand command, AppleFailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.ClientId = exception.AccountCode;
        }
    }
}