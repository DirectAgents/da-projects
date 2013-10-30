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
            HasOption<int>("a|advertiserId=", "Advertiser Id (default is 0, meaning all search advertisers, unless Client Customer Id specified)", c => AdvertiserId = c);
            HasOption<string>("v|clientCustomerId=", "Client Customer Id", c => ClientCustomerId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is one month ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var oneMonthAgo = DateTime.Today.AddMonths(-1);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? oneMonthAgo, EndDate ?? yesterday);

            foreach (var clientId in GetClientCustomerIds(this.AdvertiserId, this.ClientCustomerId))
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

        internal static IEnumerable<string> GetClientCustomerIds(int advertiserId, string clientCustomerId)
        {
            List<string> clientIds = new List<string>();
            if (advertiserId != 0)
            {
                clientIds.Add(GetClientIdFromAdvertiserId(advertiserId));
            }
            else if (string.IsNullOrWhiteSpace(clientCustomerId))
            {   // no advertiserid or adwords accountid specified
                // get all adwords ids that exist in the advertisers table
                using (var db = new ClientPortalContext())
                {
                    var ids = db.Advertisers.AsQueryable().Where(a => a.AdWordsAccountId != null)
                                .Select(a => a.AdWordsAccountId).ToList();
                    clientIds.AddRange(ids);
                }
            }

            if (!string.IsNullOrWhiteSpace(clientCustomerId))
                clientIds.Add(clientCustomerId);

            return clientIds;
        }

        internal static string GetClientIdFromAdvertiserId(int advertiserId)
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
