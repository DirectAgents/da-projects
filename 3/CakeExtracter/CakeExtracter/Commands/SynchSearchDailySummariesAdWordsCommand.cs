using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAdWordsCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public string ClientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
            ClientId = null;
            StartDate = null;
            EndDate = null;
        }

        public SynchSearchDailySummariesAdWordsCommand()
        {
            IsCommand("synchSearchDailySummariesAdWords", "synch SearchDailySummaries for AdWords");
            HasOption<int>("a|advertiserId=", "Advertiser Id (default is 0, meaning all search advertisers, unless Client Id specified)", c => AdvertiserId = c);
            HasOption<string>("v|clientId=", "Client Id", c => ClientId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? yesterday);

            foreach (var searchAccount in GetSearchAccounts())
            {
                var extracter = new AdWordsApiExtracter(searchAccount.AccountCode, dateRange);
                var loader = new AdWordsApiLoader(searchAccount.SearchAccountId);
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
                if (this.ClientId == null)
                {
                    if (this.AdvertiserId > 0)
                    {   // No ClientId specified; get all google SearchAccounts for this Advertiser
                        searchAccounts = db.SearchAccounts.Where(sa => sa.AdvertiserId == AdvertiserId && sa.Channel == "google").ToList();
                    }
                }
                else // ClientId specified
                {
                    var searchAccount = db.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == ClientId && sa.Channel == "google");
                    if (searchAccount != null)
                    {
                        if (AdvertiserId > 0 && searchAccount.AdvertiserId != AdvertiserId)
                            Logger.Info("AdvertiserId does not match that of SearchAccount specified by ClientId");

                        searchAccounts.Add(searchAccount);
                    }
                    else // didn't find a matching SearchAccount; see about creating a new one
                    {
                        if (AdvertiserId > 0)
                        {
                            searchAccount = new SearchAccount()
                            {
                                AdvertiserId = this.AdvertiserId,
                                Channel = "google",
                                AccountCode = ClientId
                                // to fill in later: Name, ExternalId
                            };
                            db.SearchAccounts.Add(searchAccount);
                            db.SaveChanges();
                            searchAccounts.Add(searchAccount);
                        }
                        else
                        {
                            Logger.Info("SearchAccount with AccountCode {0} not found and no AdvertiserId specified", ClientId);
                        }
                    }
                }
            }
            return searchAccounts;
        }

    }
}
