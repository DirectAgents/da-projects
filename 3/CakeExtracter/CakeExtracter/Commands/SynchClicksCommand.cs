using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchClicksCommand : ConsoleCommand
    {
        public SynchClicksCommand()
        {
            IsCommand("synchClicks", "synch Clicks for an advertisers offers in a date range");
            HasRequiredOption("a|advertiserId=", "Advertiser Id", c => AdvertiserId = int.Parse(c));
            HasRequiredOption("s|startDate=", "Start Date", c => StartDate = DateTime.Parse(c));
            HasRequiredOption("e|endDate=", "End Date", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = new DateRange(StartDate, EndDate);
            var extracter = new ClicksExtracter(dateRange, AdvertiserId);
            var loader = new ClicksLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AdvertiserId { get; set; }
    }
}
