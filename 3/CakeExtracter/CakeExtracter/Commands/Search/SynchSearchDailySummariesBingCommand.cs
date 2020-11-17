using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BingAds.Utilities;
using CakeExtracter.Analytic.Bing;
using CakeExtracter.Analytic.Common;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Models;
using CakeExtracter.Etl;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Commands.Search
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;
        private const int DefaultIntervalBetweenRequestsInMinutes = 40;

        private bool? includeShopping;
        private bool? includeNonShopping;

        public int? SearchProfileId { get; set; }
        public int AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        public bool GetConversionTypeStats { get; set; }

        public bool IncludeShopping => !includeShopping.HasValue || includeShopping.Value;    // default: true
        public bool IncludeNonShopping => (!includeNonShopping.HasValue || includeNonShopping.Value);    // default: true

        public SynchSearchDailySummariesBingCommand()
        {
            IsCommand("synchSearchDailySummariesBing", "synch SearchDailySummaries for Bing API Report");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<int>("v|accountId=", "Account Id", c => AccountId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (optional)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<int>("d|daysAgo=", $"Days Ago to start, if startDate not specified (default = {DefaultDaysAgo})", c => DaysAgoToStart = c);
            HasOption<bool>("r|includeRegular=", "Include Regular(NonShopping) campaigns (default is true)", c => includeNonShopping = c);
            HasOption<bool>("h|includeShopping=", "Include Shopping campaigns (default is true)", c => includeShopping = c);
            HasOption<bool>("n|getConversionTypeStats=", "Get conversion-type stats (default is false)", c => GetConversionTypeStats = c);
            //TODO? change to default:true ?
        }

        public static int RunStatic(int? searchProfileId = null, int? accountId = null, DateTime? start = null,
            DateTime? end = null, int? daysAgoToStart = null, bool getConversionTypeStats = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new SynchSearchDailySummariesBingCommand
            {
                SearchProfileId = searchProfileId,
                AccountId = accountId ?? 0,
                StartDate = start,
                EndDate = end,
                DaysAgoToStart = daysAgoToStart,
                GetConversionTypeStats = getConversionTypeStats
            };
            return cmd.Run();
        }

        public override void ResetProperties()
        {
            SearchProfileId = null;
            AccountId = 0;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            includeShopping = null;
            includeNonShopping = null;
            GetConversionTypeStats = false;
        }

        public override int Execute(string[] remainingArguments)
        {
            IntervalBetweenUnsuccessfulAndNewRequestInMinutes = ConfigurationHelper.GetIntConfigurationValue(
                "BingIntervalBetweenRequestsInMinutes", DefaultIntervalBetweenRequestsInMinutes);
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Bing ETL. DateRange {0}.", dateRange);

            BingUtility.RefreshTokens = GetTokens();
            foreach (var searchAccount in GetSearchAccounts())
            {
                var startDate = dateRange.FromDate;
                var endDate = dateRange.ToDate;
                if (searchAccount.MinSynchDate.HasValue && (startDate < searchAccount.MinSynchDate.Value))
                {
                    startDate = searchAccount.MinSynchDate.Value;
                }

                if (int.TryParse(searchAccount.AccountCode, out var accountId))
                {
                    DoEtls(searchAccount, accountId, startDate, endDate);
                }
                else
                {
                    Logger.Info("AccountCode should be an int. Skipping: {0}", searchAccount.AccountCode);
                }
            }
            SyncDailyAnalyticData(dateRange);
            SyncConversionsAnalyticData(dateRange);
            SyncCallAnalyticData(dateRange);
            SaveTokens(BingUtility.RefreshTokens);
            return 0;
        }

        public IEnumerable<SearchAccount> GetSearchAccounts()
        {
            var searchAccounts = new List<SearchAccount>();

            using (var db = new ClientPortalSearchContext())
            {
                if (this.AccountId == 0) // AccountId not specified
                {
                    // Start with all bing SearchAccounts with an account code
                    var searchAccountsQ = db.SearchAccounts.Where(sa => sa.Channel == "Bing" && !String.IsNullOrEmpty(sa.AccountCode));
                    if (this.SearchProfileId.HasValue)
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchProfileId == this.SearchProfileId.Value); // ...for the specified SearchProfile
                    else
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchProfileId.HasValue); // ...that are children of a SearchProfile

                    searchAccounts = searchAccountsQ.ToList();
                }
                else // AccountId specified
                {
                    var accountIdString = AccountId.ToString();
                    var searchAccount = db.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == accountIdString && sa.Channel == "Bing");
                    if (searchAccount != null)
                    {
                        if (SearchProfileId.HasValue && searchAccount.SearchProfileId != SearchProfileId.Value)
                            Logger.Warn("SearchProfileId does not match that of SearchAccount specified by AccountId");

                        searchAccounts.Add(searchAccount);
                    }
                    else // didn't find a matching SearchAccount; see about creating a new one
                    {
                        if (SearchProfileId.HasValue)
                        {
                            searchAccount = new SearchAccount()
                            {
                                SearchProfileId = this.SearchProfileId.Value,
                                Channel = "Bing",
                                AccountCode = accountIdString
                                // to fill in later: Name, ExternalId
                            };
                            db.SearchAccounts.Add(searchAccount);
                            db.SaveChanges();
                            searchAccounts.Add(searchAccount);
                        }
                        else
                        {
                            Logger.Info("SearchAccount with AccountCode {0} not found and no SearchProfileId specified", AccountId);
                        }
                    }
                }
            }
            return searchAccounts;
        }

        /// <inheritdoc />
        public override IEnumerable<CommandWithSchedule> GetUniqueBroadCommands(
            IEnumerable<CommandWithSchedule> commands)
        {
            var broadCommands = new List<CommandWithSchedule>();
            var commandsGroupedByAccount = commands.GroupBy(x => (x.Command as SynchSearchDailySummariesBingCommand)?.AccountId);
            foreach (var commandsGroup in commandsGroupedByAccount)
            {
                var accountBroadCommands = GetUniqueBroadAccountCommands(commandsGroup);
                broadCommands.AddRange(accountBroadCommands);
            }

            return broadCommands;
        }

        private void DoEtlDailyShopping(BingUtility bingUtility, SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            var extractor = new BingDailyShoppingSummaryExtractor(bingUtility, accountId, startDate, endDate);
            var loader = new BingLoader(searchAccount.SearchAccountId);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoEtlDailyNonShopping(BingUtility bingUtility, SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            var extractor = new BingDailyNonShoppingSummaryExtractor(bingUtility, accountId, startDate, endDate);
            var loader = new BingLoader(searchAccount.SearchAccountId);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoEtlConv(BingUtility bingUtility, SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            //TODO: handle dates with no stats... keep track of all dates within the range and for those missing when done, delete the SCS's
            //      (could do in extracter or loader or have loader return dates loaded, or missing dates, or have a method to call to delete SCS's
            //       that didn't have any items)
            var extractor = new BingConvSummaryExtractor(bingUtility, accountId, startDate, endDate);
            var loader = new BingConvSummaryLoader(searchAccount.SearchAccountId);
            InitEtlEvents(extractor, loader);
            CommandHelper.DoEtl(extractor, loader);
        }

        private void DoEtls(SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            var bingUtility = new BingUtility(m => Logger.Info(m), m => Logger.Warn(m));
            if (IncludeShopping)
            {
                DoEtlDailyShopping(bingUtility, searchAccount, accountId, startDate, endDate);
            }
            if (IncludeNonShopping)
            {
                DoEtlDailyNonShopping(bingUtility, searchAccount, accountId, startDate, endDate);
            }
            if (GetConversionTypeStats)
            {
                DoEtlConv(bingUtility, searchAccount, accountId, startDate, endDate);
            }
        }

        private string[] GetTokens()
        {
            return Platform.GetPlatformTokens(Platform.Code_Bing);
        }

        private void SaveTokens(string[] tokens)
        {
            Platform.SavePlatformTokens(Platform.Code_Bing, tokens);
        }

        private void InitEtlEvents<T>(Extracter<T> extractor, Loader<T> loader)
        {
            extractor.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesBingCommand>(command => { });
            loader.ProcessEtlFailedWithoutInformation += exception =>
                ScheduleNewCommandLaunch<SynchSearchDailySummariesBingCommand>(command => { });
        }

        private List<CommandWithSchedule> GetUniqueBroadAccountCommands(IEnumerable<CommandWithSchedule> commandsWithSchedule)
        {
            var accountCommands = new List<Tuple<SynchSearchDailySummariesBingCommand, DateRange, CommandWithSchedule>>();
            foreach (var commandWithSchedule in commandsWithSchedule)
            {
                var command = (SynchSearchDailySummariesBingCommand)commandWithSchedule.Command;
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

                accountCommands.Add(new Tuple<SynchSearchDailySummariesBingCommand, DateRange, CommandWithSchedule>(command, commandDateRange, commandWithSchedule));
            }

            var broadCommands = accountCommands.Select(GetCommandWithCorrectDateRange).ToList();
            return broadCommands;
        }

        private CommandWithSchedule GetCommandWithCorrectDateRange(Tuple<SynchSearchDailySummariesBingCommand, DateRange, CommandWithSchedule> setting)
        {
            setting.Item1.StartDate = setting.Item2.FromDate;
            setting.Item1.EndDate = setting.Item2.ToDate;
            setting.Item1.DaysAgoToStart = null;
            setting.Item3.Command = setting.Item1;
            return setting.Item3;
        }

        private void SyncDailyAnalyticData(DateRange dateRange)
        {
            SyncDataToAnalyticTable(dateRange, new BingDailySummarySynchronizer(dateRange.FromDate, dateRange.ToDate));
        }

        private void SyncConversionsAnalyticData(DateRange dateRange)
        {
            SyncDataToAnalyticTable(dateRange, new BingSearchConversionsSynchronizer(dateRange.FromDate, dateRange.ToDate));
        }

        private void SyncCallAnalyticData(DateRange dateRange)
        {
            SyncDataToAnalyticTable(dateRange, new BingCallDailySummarySynchronizer(dateRange.FromDate, dateRange.ToDate));
        }

        private void SyncDataToAnalyticTable(DateRange dateRange, BaseAnalyticSynchronizer synchronizer)
        {
            try
            {
                CommandExecutionContext.Current.SetJobExecutionStateInHistory("Sync analytic table data.");
                Logger.Info("Sync analytic table data.");
                synchronizer.RunSynchronizer();
            }
            catch (Exception ex)
            {
                var exception = new Exception("Error occured whyle sync Bing data to analytic table", ex);
                Logger.Error(exception);
            }
        }
    }
}
