using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAdWordsCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public string ClientCustomerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public SynchSearchDailySummariesAdWordsCommand()
        {
            IsCommand("synchSearchDailySummariesAdWords", "synch SearchDailySummaries for AdWords");
            HasRequiredOption<int>("aid|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
            HasOption<string>("v|clientCustomerId=", "Client Customer Id", c => ClientCustomerId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var extracter = new AdWordsApiExtracter(GetClientCustomerId(), StartDate ?? oneMonthAgo, EndDate ?? yesterday);
            var loader = new AdWordsApiLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        private string GetClientCustomerId()
        {
            if (!string.IsNullOrWhiteSpace(this.ClientCustomerId))
                return ClientCustomerId;

            Logger.Info("Getting AdWords id for advertiser id {0}", this.AdvertiserId);

            using (var db = new ClientPortalContext())
            {
                var idString = db.Advertisers.Find(this.AdvertiserId).AdWordsAccountId;

                if (string.IsNullOrWhiteSpace(idString))
                    throw new Exception(string.Format("No AdWords id is set for advertiser id {0}", this.AdvertiserId));

                return idString;
            }
        }

    }
}
