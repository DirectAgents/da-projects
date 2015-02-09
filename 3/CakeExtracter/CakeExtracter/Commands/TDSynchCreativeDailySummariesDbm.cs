using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TDSynchCreativeDailySummariesDbm : ConsoleCommand
    {
        //public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            //StartDate = null;
            EndDate = null;
        }

        public TDSynchCreativeDailySummariesDbm()
        {
            IsCommand("tdSynchCreativeDailySummariesDbm", "synch CreativeDailySummaries for DBM Report");
            //HasOption<DateTime>("s|startDate=", "Start Date (default is 2 days ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            string bucketName = "151075984680687222131410283081521_report"; // Betterment_creative

            //var twoDaysAgo = DateTime.Today.AddDays(-2);
            //var yesterday = DateTime.Today.AddDays(-1);
            //var dateRange = new DateRange(StartDate ?? twoDaysAgo, EndDate ?? yesterday);

            DateTime endDate = EndDate ?? DateTime.Today.AddDays(-1);
            var reportDate = endDate.AddDays(1);
            var dateRange = new DateRange(reportDate, reportDate);

            var extracter = new DbmCloudStorageExtracter(dateRange, bucketName);
            var loader = new DbmCreativeDailySummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            return 0;
        }

    }
}
