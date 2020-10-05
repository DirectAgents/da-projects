using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl.AdWords.Exceptions;
using CakeExtracter.Etl.AdWords.Extractors;
using CakeExtracter.Etl.AdWords.Loaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAdWordsCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;

        public static int RunStatic(int? searchProfileId = null, string clientId = null, DateTime? start = null, DateTime? end = null, int? daysAgoToStart = null, bool getClickAssistConvStats = false, bool getConversionTypeStats = false, bool getAllStats = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new SynchSearchDailySummariesAdWordsCommand
            {
                SearchProfileId = searchProfileId,
                ClientId = clientId,
                StartDate = start,
                EndDate = end,
                DaysAgoToStart = daysAgoToStart,
                GetAllStats = getAllStats ? "true" : null,
                GetClickAssistConvStats = getClickAssistConvStats,
                GetConversionTypeStats = getConversionTypeStats
            };
            return cmd.Run();
        }

        public int? SearchProfileId { get; set; }

        public string ClientId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DaysAgoToStart { get; set; }

        public bool IncludeClickType { get; set; }

        public int? MinSearchAccountId { get; set; }

        public string GetAllStats { get; set; }

        public bool GetClickAssistConvStats { get; set; }

        public bool GetConversionTypeStats { get; set; }

        // If all are false, will get standard stats
        public bool GetStandardStats
        {
            get { return !GetClickAssistConvStats && !GetConversionTypeStats; }
        }

        public override void ResetProperties()
        {
            SearchProfileId = null;
            ClientId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            IncludeClickType = false;
            MinSearchAccountId = null;

            GetAllStats = null;
            GetClickAssistConvStats = false;
            GetConversionTypeStats = false;
        }

        public SynchSearchDailySummariesAdWordsCommand()
        {
            IsCommand("synchSearchDailySummariesAdWords", "synch SearchDailySummaries for AdWords");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<string>("v|clientId=", "Client Id", c => ClientId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (optional)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<bool>("b|includeClickType=", "Include ClickType (default is false)", c => IncludeClickType = c);
            HasOption<int>("m|minSearchAccountId=", "Include this and all higher searchAccountIds (optional)", c => MinSearchAccountId = c);

            HasOption<string>("g|getAllStats=", "Get all types of stats ('true', 'yes' or 'both', default is false)", c => GetAllStats = c);
            HasOption<bool>("k|getClickAssistConvStats=", "Get click-assisted-conversion stats (default is false)", c => GetClickAssistConvStats = c);
            HasOption<bool>("n|getConversionTypeStats=", "Get conversion-type stats (default is false)", c => GetConversionTypeStats = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("AdWords ETL. DateRange {0}.", dateRange);

            var searchAccounts = GetSearchAccounts();
            foreach (var searchAccount in searchAccounts)
            {
                dateRange = DoEtl(dateRange, searchAccount);
            }
            return 0;
        }

        private DateRange DoEtl(DateRange dateRange, SearchAccount searchAccount)
        {
            DateTime startDate = dateRange.FromDate;
            if (searchAccount.MinSynchDate.HasValue && (startDate < searchAccount.MinSynchDate.Value))
            {
                startDate = searchAccount.MinSynchDate.Value;
            }

            var revisedDateRange = new DateRange(startDate, dateRange.ToDate);

            bool getAll = GetAllStats == "true" || GetAllStats == "yes" || GetAllStats == "both";

            if (GetStandardStats || getAll)
            {
                var extractor = new AdWordsStandartExtractor(searchAccount.AccountCode, revisedDateRange);
                var loader = new AdWordsStandartLoader(searchAccount.SearchAccountId);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
            }
            if (GetClickAssistConvStats || getAll)
            {
                var extractor = new AdWordsAssistConvExtractor(searchAccount.AccountCode, revisedDateRange);
                var loader = new AdWordsAssistConvLoader(searchAccount.SearchAccountId);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
            }
            if (GetConversionTypeStats || getAll)
            {
                var extractor = new AdWordsConversionTypeExtractor(searchAccount.AccountCode, revisedDateRange);
                var loader = new AdWordsConversionTypeLoader(searchAccount.SearchAccountId);
                InitEtlEvents(extractor, loader);
                CommandHelper.DoEtl(extractor, loader);
            }

            return dateRange;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccount = commands.GroupBy(x => (x.Command as SynchSearchDailySummariesAdWordsCommand)?.ClientId);
            foreach (var commandsGroup in commandsGroupedByAccount)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        public IEnumerable<SearchAccount> GetSearchAccounts()
        {
            return GetSearchAccounts("Google", this.SearchProfileId, this.ClientId, minAccountId: this.MinSearchAccountId);
        }

        public static IEnumerable<SearchAccount> GetSearchAccounts(string channelName, int? searchProfileId, string accountCode, int? minAccountId = null)
        {
            var searchAccounts = new List<SearchAccount>();

            using (var db = new ClientPortalSearchContext())
            {
                if (accountCode == null) // AccountCode not specified
                {
                    // Start with all channel SearchAccounts with an account code
                    var searchAccountsQ = db.SearchAccounts.Where(sa => sa.Channel == channelName && !String.IsNullOrEmpty(sa.AccountCode));
                    if (searchProfileId.HasValue)
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchProfileId == searchProfileId.Value); // ...for the specified SearchProfile
                    else
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchProfileId.HasValue); // ...that are children of a SearchProfile

                    if (minAccountId.HasValue)
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchAccountId >= minAccountId.Value);

                    searchAccounts = searchAccountsQ.OrderBy(sa => sa.SearchAccountId).ToList();
                    //searchAccounts = searchAccountsQ.OrderBy(sa => sa.SearchProfileId).ThenBy(sa => sa.SearchAccountId).ToList();
                }
                else // AccountCode specified
                {
                    var searchAccount = db.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == accountCode && sa.Channel == channelName);
                    if (searchAccount != null)
                    {
                        if (searchProfileId.HasValue && searchAccount.SearchProfileId != searchProfileId.Value)
                            Logger.Warn("SearchProfileId does not match that of SearchAccount specified by AccountCode");

                        searchAccounts.Add(searchAccount);
                    }
                    else // didn't find a matching SearchAccount; see about creating a new one
                    {
                        if (searchProfileId.HasValue)
                        {
                            var searchProfile = db.SearchProfiles.Find(searchProfileId.Value);
                            if (searchProfile != null)
                            {
                                searchAccount = new SearchAccount()
                                {
                                    SearchProfile = searchProfile,
                                    Channel = channelName,
                                    AccountCode = accountCode
                                    // to fill in later: Name, ExternalId
                                };
                                db.SearchAccounts.Add(searchAccount);
                                db.SaveChanges();
                                searchAccounts.Add(searchAccount);
                            }
                            else
                            {
                                Logger.Info("SearchAccount with AccountCode {0} not found and SearchProfileId {1} not found", accountCode, searchProfileId);
                            }
                        }
                        else
                        {
                            Logger.Info("SearchAccount with AccountCode {0} not found and no SearchProfileId specified", accountCode);
                        }
                    }
                }
            }
            return searchAccounts;
        }

        private void InitEtlEvents(AdWordsBaseApiExtractor extractor, AdWordsBaseApiLoader loader)
        {
            GeneralInitEtlEvents(extractor, loader);
            extractor.ProcessFailedExtraction += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAdWordsCommand>(command =>
                    UpdateCommandParameters(command, exception));
            loader.ProcessFailedLoading += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAdWordsCommand>(command =>
                    UpdateCommandParameters(command, exception));
        }

        private void GeneralInitEtlEvents(AdWordsBaseApiExtractor extractor, AdWordsBaseApiLoader loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAdWordsCommand>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesAdWordsCommand>(command => { });
        }

        private IEnumerable<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands =
                new List<Tuple<SynchSearchDailySummariesAdWordsCommand, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (SynchSearchDailySummariesAdWordsCommand)commandWithSchedule.Command;
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
                    new Tuple<SynchSearchDailySummariesAdWordsCommand, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<SynchSearchDailySummariesAdWordsCommand, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private void UpdateCommandParameters(SynchSearchDailySummariesAdWordsCommand command, AdwordsFailedEtlException exception)
        {
            command.StartDate = exception.StartDate;
            command.EndDate = exception.EndDate;
            command.ClientId = exception.ClientId;
            command.GetAllStats = null;
            command.IncludeClickType = false;

            switch (exception.StatsType)
            {
                case "ClickAssistConvStats":
                    command.GetClickAssistConvStats = true;
                    break;
                case "ConversionTypeStats":
                    command.GetConversionTypeStats = true;
                    break;
            }
        }
    }
}
