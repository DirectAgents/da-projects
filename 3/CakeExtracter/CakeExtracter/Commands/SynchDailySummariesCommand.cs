using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchDailySummariesCommand : ConsoleCommand
    {
        public string Advertiser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public SynchDailySummariesCommand()
        {
            StartDate = new DateTime(2013, 1, 1); // default value
            EndDate = DateTime.Today.AddDays(1);

            RunBefore(new SynchAdvertisersCommand());
            RunBefore(new SynchOffersCommand());

            IsCommand("synchDailySummaries", "synch DailySummaries for an advertisers offers in a date range");
            HasOption("a|advertiserId=", "Advertiser Id (* = all advertisers)", c => Advertiser = c);
            HasOption("s|startDate=", "Start Date (default is 1/1/2013)", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is today)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var dateRange = new DateRange(StartDate, EndDate ?? tomorrow);

            foreach (var advertiserId in GetAdvertiserIds())
            {
                var extracter = new DailySummariesExtracter(dateRange, advertiserId);
                var loader = new DailySummariesLoader();
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
            }
            return 0;
        }

        private IEnumerable<int> GetAdvertiserIds()
        {
            if (string.IsNullOrWhiteSpace(Advertiser) || Advertiser == "*")
            {
                List<int> advertiserIds;
                using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
                {
                    advertiserIds = db.Advertisers
                                      .OrderBy(c => c.AdvertiserId)
                                      .Select(c => c.AdvertiserId)
                                      .ToList();
                }
                foreach (var advertiserId in advertiserIds)
                {
                    yield return advertiserId;
                }
            }
            else
            {
                yield return int.Parse(Advertiser);
            }
        }

    }
}