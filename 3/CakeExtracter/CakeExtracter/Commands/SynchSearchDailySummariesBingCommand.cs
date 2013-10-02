using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesBingCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public int AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public SynchSearchDailySummariesBingCommand()
        {
            IsCommand("synchSearchDailySummariesBing", "synch SearchDailySummaries for Bing API Report");
            HasRequiredOption<int>("v|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
            HasOption<int>("a|accountId=", "Account Id", c => AccountId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var extracter = new BingDailySummaryExtracter(GetAccountId(), StartDate ?? oneMonthAgo, EndDate ?? yesterday);
            var loader = new BingLoader(AdvertiserId);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        private int GetAccountId()
        {
            if (this.AccountId != 0)
                return AccountId;

            Logger.Info("Getting Bing id for advertiser id {0}", this.AdvertiserId);

            using (var db = new ClientPortalContext())
            {
                var idString = db.Advertisers.Find(this.AdvertiserId).BingAdsAccountId;

                if (string.IsNullOrWhiteSpace(idString))
                    throw new Exception(string.Format("No Bing id is set for advertiser id {0}", this.AdvertiserId));

                return int.Parse(idString);
            }
        }

    }
}
