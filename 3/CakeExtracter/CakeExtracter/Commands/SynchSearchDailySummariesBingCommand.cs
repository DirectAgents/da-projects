using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public int AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
            AccountId = 0;
            StartDate = null;
            EndDate = null;
        }

        public SynchSearchDailySummariesBingCommand()
        {
            IsCommand("synchSearchDailySummariesBing", "synch SearchDailySummaries for Bing API Report");
            HasOption<int>("a|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
            HasOption<int>("v|accountId=", "Account Id", c => AccountId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var dates = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? yesterday);

            foreach (var searchAccount in GetSearchAccounts())
            {
                int accountId = Int32.Parse(searchAccount.AccountCode);
                var extracter = new BingDailySummaryExtracter(accountId, dates.FromDate, dates.ToDate);
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
                if (this.AccountId == 0)
                {
                    if (this.AdvertiserId > 0)
                    {   // No AccountId specified; get all bing SearchAccounts for this Advertiser
                        searchAccounts = db.SearchAccounts.Where(sa => sa.AdvertiserId == AdvertiserId && sa.Channel == "bing").ToList();
                    }
                }
                else // AccountId specified
                {
                    var accountIdString = AccountId.ToString();
                    var searchAccount = db.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == accountIdString && sa.Channel == "bing");
                    if (searchAccount != null)
                    {
                        if (AdvertiserId > 0 && searchAccount.AdvertiserId != AdvertiserId)
                            Logger.Info("AdvertiserId does not match that of SearchAccount specified by AccountId");

                        searchAccounts.Add(searchAccount);
                    }
                    else // didn't find a matching SearchAccount; see about creating a new one
                    {
                        if (AdvertiserId > 0)
                        {
                            searchAccount = new SearchAccount()
                            {
                                AdvertiserId = this.AdvertiserId,
                                Channel = "bing",
                                AccountCode = accountIdString
                                // to fill in later: Name, ExternalId
                            };
                            db.SearchAccounts.Add(searchAccount);
                            db.SaveChanges();
                            searchAccounts.Add(searchAccount);
                        }
                        else
                        {
                            Logger.Info("SearchAccount with AccountCode {0} not found and no AdvertiserId specified", AccountId);
                        }
                    }
                }
            }
            return searchAccounts;
        }

    }
}
