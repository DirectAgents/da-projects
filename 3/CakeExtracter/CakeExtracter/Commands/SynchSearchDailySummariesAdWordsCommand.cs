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
    public class SynchSearchDailySummariesAdWordsCommand : ConsoleCommand
    {
        public int AdvertiserId { get; set; }
        public string ClientCustomerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = 0;
            ClientCustomerId = null;
            StartDate = null;
            EndDate = null;
        }

        public SynchSearchDailySummariesAdWordsCommand()
        {
            IsCommand("synchSearchDailySummariesAdWords", "synch SearchDailySummaries for AdWords");
            HasOption<int>("aid|advertiserId=", "Advertiser Id (default is 0, meaning all search advertisers)", c => AdvertiserId = c);
            HasOption<string>("v|clientCustomerId=", "Client Customer Id", c => ClientCustomerId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? yesterday);

            foreach (var clientId in GetClientCustomerIds())
            {
                var extracter = new AdWordsApiExtracter(clientId, dateRange);
                var loader = new AdWordsApiLoader();
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }
        private IEnumerable<string> GetClientCustomerIds()
        {
            List<string> clientIds = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.ClientCustomerId))
                clientIds.Add(ClientCustomerId);

            if (AdvertiserId == 0)
            {
                using (var db = new ClientPortalContext())
                {
                    var ids = db.Advertisers.AsQueryable().Where(a => a.AdWordsAccountId != null)
                                .Select(a => a.AdWordsAccountId).ToList();
                    clientIds.AddRange(ids);
                }
            }
            else
            {
                clientIds.Add(GetClientIdFromAdvertiserId(AdvertiserId));
            }
            return clientIds;
        }

        private string GetClientIdFromAdvertiserId(int advertiserId)
        {
            Logger.Info("Getting AdWords id for advertiser id {0}", advertiserId);

            using (var db = new ClientPortalContext())
            {
                var idString = db.Advertisers.Find(advertiserId).AdWordsAccountId;

                if (string.IsNullOrWhiteSpace(idString))
                    throw new Exception(string.Format("No AdWords id is set for advertiser id {0}", advertiserId));

                return idString;
            }
        }

    }
}
