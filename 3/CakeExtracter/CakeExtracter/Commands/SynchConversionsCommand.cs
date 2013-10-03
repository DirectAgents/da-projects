using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchConversionsCommand : ConsoleCommand
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AdvertiserId { get; set; }

        public SynchConversionsCommand()
        {
            IsCommand("synchConversions", "synch Conversions for an advertisers offers in a date range");
            HasRequiredOption("a|advertiserId=", "Advertiser Id", c => AdvertiserId = int.Parse(c));
            HasRequiredOption("s|startDate=", "Start Date", c => StartDate = DateTime.Parse(c));
            HasRequiredOption("e|endDate=", "End Date", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = new DateRange(StartDate, EndDate);
            foreach (var date in dateRange.Dates)
            {
                try
                {
                    ExtractAndLoadConversionsForDate(date);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.Message);
                    Logger.Warn(ex.StackTrace);
                    Logger.Error(ex);
                }
            }
            return 0;
        }

        private void ExtractAndLoadConversionsForDate(DateTime date)
        {
            Logger.Info("Extracting conversions for {0}..", date.ToShortDateString());
            var dateRange = new DateRange(date, date.AddDays(1));
            var extracter = new ConversionsExtracter(dateRange, AdvertiserId);
            var loader = new ConversionsLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            Logger.Info("Finished extracting conversions for {0}.", date.ToShortDateString());
        }

    }
}
