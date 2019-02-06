using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BingAds;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using CakeExtracter.Helpers;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands.Search
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCommand : ConsoleCommand
    {
        private const int DefaultDaysAgo = 41;
        
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
            //GlobalProxySelection.Select = new WebProxy("127.0.0.1", 8888);
            var dateRange = CommandHelper.GetDateRange(StartDate, EndDate, DaysAgoToStart, DefaultDaysAgo);
            Logger.Info("Bing ETL. DateRange {0}.", dateRange);

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
            return 0;
        }

        public IEnumerable<SearchAccount> GetSearchAccounts()
        {
            var searchAccounts = new List<SearchAccount>();

            using (var db = new ClientPortalContext())
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

        private static void DoEtlDailyShopping(BingUtility bingUtility, SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            var extractor = new BingDailyShoppingSummaryExtractor(bingUtility, accountId, startDate, endDate);
            var loader = new BingLoader(searchAccount.SearchAccountId);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoEtlDailyNonShopping(BingUtility bingUtility, SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            var extractor = new BingDailyNonShoppingSummaryExtractor(bingUtility, accountId, startDate, endDate);
            var loader = new BingLoader(searchAccount.SearchAccountId);
            CommandHelper.DoEtl(extractor, loader);
        }

        private static void DoEtlConv(BingUtility bingUtility, SearchAccount searchAccount, int accountId, DateTime startDate, DateTime endDate)
        {
            //TODO: handle dates with no stats... keep track of all dates within the range and for those missing when done, delete the SCS's
            //      (could do in extracter or loader or have loader return dates loaded, or missing dates, or have a method to call to delete SCS's
            //       that didn't have any items)
            var extractor = new BingConvSummaryExtractor(bingUtility, accountId, startDate, endDate);
            var loader = new BingConvSummaryLoader(searchAccount.SearchAccountId);
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
    }
}
