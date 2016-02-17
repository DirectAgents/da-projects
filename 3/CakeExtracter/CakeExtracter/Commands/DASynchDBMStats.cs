﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats : ConsoleCommand
    {
        //Note: if make a RunStatic, be sure to add 'DBM_AllSiteBucket', etc to the web.config

        public DateTime? EndDate { get; set; }
        public string StatsType { get; set; }

        public override void ResetProperties()
        {
            EndDate = null;
            StatsType = null;
        }

        public DASynchDBMStats()
        {
            IsCommand("daSynchDBMStats", "synch DBM Daily Stats - by lineitem/creative/site...");
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<string>("t|statsType=", "Stats Type (default: daily)", c => StatsType = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            // Note: The reportDate will be one day after the endDate of the desired stats
            DateTime endDate = EndDate ?? DateTime.Today.AddDays(-1);
            var reportDate = endDate.AddDays(1);
            var dateRange = new DateRange(reportDate, reportDate);

            StatsType = (StatsType == null) ? "" : StatsType.ToLower(); // make it lowered and not null

            if (StatsType.StartsWith("strat"))
                DoETL_Strategy(dateRange);
            else if (StatsType.StartsWith("creat"))
                DoETL_Creative(dateRange);
            else if (StatsType.StartsWith("site"))
                DoETL_Site(dateRange);
            //else
            //    DoETL_Daily(dateRange);


            return 0;
        }

        public void DoETL_Strategy(DateRange dateRange)
        {
            var buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllLineItemBucket"] };

            var extracter = new DbmCloudStorageExtracter(dateRange, buckets, byLineItem: true);
            var loader = new DbmLineItemSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Creative(DateRange dateRange)
        {
            var buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllCreativeBucket"] };

            var extracter = new DbmCloudStorageExtracter(dateRange, buckets, byCreative: true);
            var loader = new DbmCreativeSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Site(DateRange dateRange)
        {
            var buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllSiteBucket"] };

            var extracter = new DbmCloudStorageExtracter(dateRange, buckets, bySite: true);
            var loader = new DbmSiteSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

    }
}
