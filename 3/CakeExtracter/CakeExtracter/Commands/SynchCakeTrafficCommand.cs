using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchCakeTrafficCommand : ConsoleCommand
    {
        public SynchCakeTrafficCommand()
        {
            IsCommand("synchCakeTraffic", "synch cake traffic");
            HasRequiredOption("s|startDate=", "Start Date", c => StartDate = DateTime.Parse(c));
            HasRequiredOption("e|endDate=", "End Date", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = new DateRange(StartDate, EndDate);
            var extracter = new TrafficExtracter(dateRange);
            var loader = new TrafficLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
