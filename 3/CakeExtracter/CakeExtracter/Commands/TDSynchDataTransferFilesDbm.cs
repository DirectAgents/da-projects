using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TDSynchDataTransferFilesDbm : ConsoleCommand
    {            // for views, clicks, conversions

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            StartDate = null;
            EndDate = null;
        }

        public TDSynchDataTransferFilesDbm()
        {
            IsCommand("tdSynchDataTransferFilesDbm", "synch Data Transfer Files from DBM");
            HasOption<DateTime>("s|startDate=", "Start Date (default is 2 days ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            string bucketName = "gdbm-479-320231"; // location of our data transfer files
            int timezoneOffest = -5; // -4;

            var twoDaysAgo = DateTime.Today.AddDays(-2);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? twoDaysAgo, EndDate ?? yesterday);

            var extracter = new DbmConversionExtracter(dateRange, bucketName);
            var loader = new DbmConversionLoader(timezoneOffest);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            return 0;
        }

    }
}
