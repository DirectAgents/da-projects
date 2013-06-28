using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.ConsoleCommands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchDailySummariesCommand : ConsoleCommand
    {
        public SynchDailySummariesCommand()
        {
            IsCommand("synchDailySummaries", "synch DailySummaries for an advertisers offers in a date range");
            HasRequiredOption("a|advertiserId=", "Advertiser Id", c => AdvertiserId = int.Parse(c));
            HasRequiredOption("s|startDate=", "Start Date", c => StartDate = DateTime.Parse(c));
            HasRequiredOption("e|endDate=", "End Date", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = new DateRange(StartDate, EndDate);
            var extracter = new DailySummariesExtracter(dateRange, AdvertiserId);
            var loader = new DailySummariesLoader();
            var extracterThread = extracter.BeginExtracting();
            var loaderThread = loader.BeginLoading(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AdvertiserId { get; set; }
    }
}