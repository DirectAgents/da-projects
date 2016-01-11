using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats2 : ConsoleCommand
    {
        //Note: if make a RunStatic, be sure to add 'DBM_AllSiteBucket' to the web.config

        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            EndDate = null;
        }

        public DASynchDBMStats2()
        {
            IsCommand("daSynchDBMStats2", "synch DBM Daily Stats - by site/account");
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            // Note: The reportDate will be one day after the endDate of the desired stats
            DateTime endDate = EndDate ?? DateTime.Today.AddDays(-1);
            var reportDate = endDate.AddDays(1);
            var dateRange = new DateRange(reportDate, reportDate);

            var buckets = GetBuckets();

            var extracter = new DbmCloudStorageExtracter(dateRange, buckets, bySite: true);
            var loader = new DbmSiteSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            return 0;
        }

        public IEnumerable<string> GetBuckets()
        {
            var buckets = new List<string>();
            buckets.Add(ConfigurationManager.AppSettings["DBM_AllSiteBucket"]);
            return buckets;
        }
    }
}
