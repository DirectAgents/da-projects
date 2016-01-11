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
        public int? InsertionOrderId { get; set; }
        public bool IncludeViewThrus { get; set; }

        public override void ResetProperties()
        {
            StartDate = null;
            EndDate = null;
            InsertionOrderId = null;
            IncludeViewThrus = false;
        }

        public TDSynchDataTransferFilesDbm()
        {
            IsCommand("tdSynchDataTransferFilesDbm", "synch Data Transfer Files from DBM");
            HasOption<DateTime>("s|startDate=", "Start Date (default is yesterday)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is today)", c => EndDate = c);
            HasOption<int>("i|insertionOrderId=", "InsertionOrder Id (default is all)", c => InsertionOrderId = c);
            HasOption<bool>("v|include viewthrus=", "Include ViewThru conversions (default is false)", c => IncludeViewThrus = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            string bucketName = "gdbm-479-320231"; // location of our data transfer files
            int timezoneOffset = -5;
            //int timezoneOffset = -4; // daylight savings
            DateRange daylightSavingsRange = new DateRange(new DateTime(2016, 3, 13), new DateTime(2016, 11, 6));

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? yesterday, EndDate ?? today);

            var extracter = new DbmConversionExtracter(dateRange, bucketName, InsertionOrderId, IncludeViewThrus);
            var loader = new DbmConversionLoader(timezoneOffset, daylightSavingsRange);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            return 0;
        }

    }
}
