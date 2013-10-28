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
            HasOption<int>("aid|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
            HasOption<int>("v|accountId=", "Account Id", c => AccountId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var dates = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? yesterday);

            foreach (var advertiser in GetAdvertisers())
            {
                int accountId = Int32.Parse(advertiser.BingAdsAccountId);
                var extracter = new BingDailySummaryExtracter(accountId, dates.FromDate, dates.ToDate);
                var loader = new BingLoader(advertiser.AdvertiserId);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        private IEnumerable<Advertiser> GetAdvertisers()
        {
            List<Advertiser> advertisers = new List<Advertiser>();
            using (var db = new ClientPortalContext())
            {
                if (this.AdvertiserId != 0) // get specified advertiser
                {
                    var adv = db.Advertisers.Where(a => a.AdvertiserId == AdvertiserId).FirstOrDefault();
                    if (adv == null)
                        Logger.Warn("Could not find advertiser with advertiserId {0}", AdvertiserId);
                    else if (String.IsNullOrWhiteSpace(adv.BingAdsAccountId))
                        Logger.Warn("No Bing id is set for advertiserId {0}", AdvertiserId);
                    else
                        advertisers.Add(adv);
                }
                else if (this.AccountId == 0) // get all advertisers with a bing id
                {
                    var advs = db.Advertisers.Where(a => a.BingAdsAccountId != null);
                    advertisers.AddRange(advs);
                }

                if (this.AccountId != 0) // get advertiser with specified bing id
                {
                    var accountIdString = AccountId.ToString();
                    var advertiser = db.Advertisers.Where(a => a.BingAdsAccountId == accountIdString).FirstOrDefault();
                    if (advertiser == null)
                        Logger.Warn("Could not find advertiser with BingAdsAccountId {0}", AccountId);
                    else
                        advertisers.Add(advertiser);
                }
            }
            return advertisers;
        }

    }
}
