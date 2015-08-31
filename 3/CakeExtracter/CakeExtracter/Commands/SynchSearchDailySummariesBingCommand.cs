using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCommand : ConsoleCommand
    {
        public int? SearchProfileId { get; set; }
        public int AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }

        public override void ResetProperties()
        {
            SearchProfileId = null;
            AccountId = 0;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        public SynchSearchDailySummariesBingCommand()
        {
            IsCommand("synchSearchDailySummariesBing", "synch SearchDailySummaries for Bing API Report");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<int>("v|accountId=", "Account Id", c => AccountId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (optional)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default = 62)", c => DaysAgoToStart = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            if (!DaysAgoToStart.HasValue)
                DaysAgoToStart = 62; // used if StartDate==null
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? today.AddDays(-DaysAgoToStart.Value), EndDate ?? yesterday);

            foreach (var searchAccount in GetSearchAccounts())
            {
                DateTime startDate = dateRange.FromDate;
                DateTime endDate = dateRange.ToDate;
                if (searchAccount.MinSynchDate.HasValue && (startDate < searchAccount.MinSynchDate.Value))
                    startDate = searchAccount.MinSynchDate.Value;

                int accountId = Int32.Parse(searchAccount.AccountCode);
                var extracter = new BingDailySummaryExtracter(accountId, startDate, endDate);
                var loader = new BingLoader(searchAccount.SearchAccountId);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
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

    }
}
