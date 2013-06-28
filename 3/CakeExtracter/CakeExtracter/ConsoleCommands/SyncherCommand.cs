using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;

namespace CakeExtracter.ConsoleCommands
{
    [Export(typeof(ConsoleCommand))]
    public class SyncherCommand : ConsoleCommand
    {
        public override int Execute(string[] remainingArguments)
        {
            var syncher = new Syncher();
            syncher.Run(this);
            return 0;
        }

        public SyncherCommand()
        {
            IsCommand("synch", "Synch with Cake Marketing");
            HasOption("c|clicks", "Clicks", c => SynchClicks = true);
            HasOption("n|conversions", "Conversions", c => SynchConversions = true);
            HasOption("m|metrics", "Metrics", c => SynchMetrics = true);
            HasRequiredOption("a|advertiserId=", "Advertiser Id", c => AdvertiserId = c);
            HasRequiredOption("s|startDate=", "Start Date", c => StartDate = DateTime.Parse(c));
            HasRequiredOption("e|endDate=", "End Date", c => EndDate = DateTime.Parse(c));
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AdvertiserId { get; set; }
        public bool SynchClicks;
        public bool SynchConversions;
        public bool SynchMetrics;
    }
}
