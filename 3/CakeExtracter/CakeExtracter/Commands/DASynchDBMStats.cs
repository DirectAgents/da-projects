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
    public class DASynchDBMStats : ConsoleCommand
    {
        //Note: if make a RunStatic, be sure to add 'DBM_AllSiteBucket', etc to the web.config

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Historical { get; set; }
        public string StatsType { get; set; }

        public override void ResetProperties()
        {
            EndDate = null;
            Historical = false;
            StatsType = null;
        }

        public DASynchDBMStats()
        {
            IsCommand("daSynchDBMStats", "synch DBM Daily Stats - by lineitem/creative/site...");
            HasOption<DateTime>("s|startDate=", "Start Date (default is yesterday)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption("h|Historical=", "Get historical stats (ignore endDate)", c => Historical = bool.Parse(c));
            HasOption<string>("t|statsType=", "Stats Type (default: all)", c => StatsType = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            
            DoConvTest(2334723);
            return 0;
            /*
            if (Historical)
                DoHistorical();
            else
                DoRegular();
            return 0;*/
        }

        // testing...
        public void DoConvTest(int? insertOrderId) //null for all
        {
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year,today.Month,7);
            var dateRange = new DateRange(StartDate ?? today, EndDate ?? today);
            //var DAbucket = "gdbm-479"; //generic DA bucket
            //var bucket = "gdbm-479-320231"; //bucket for Crackle TODO--add lookup values to database
            var bucket = "gdbm-479-914580"; //bucket for Dashlane

            int timezoneOffset = -5; // w/o daylight savings
            var convConverter = new CakeExtracter.Etl.TradingDesk.Loaders.DbmConvConverter(timezoneOffset);

            var extracter = new DbmConversionExtracter(dateRange, bucket, insertOrderId, true);
            var loader = new DbmConvLoader(convConverter);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        public void DoRegular()
        {
            // Note: The reportDate will be one day after the endDate of the desired stats
            DateTime endDate = EndDate ?? DateTime.Today.AddDays(-1);
            var reportDate = endDate.AddDays(1);

            var statsType = new StatsTypeAgg(this.StatsType);

            if (statsType.Daily)
                DoETL_Daily(reportDate: reportDate);
            if (statsType.Strategy)
                DoETL_Strategy(reportDate: reportDate);
            if (statsType.Creative)
                DoETL_Creative(reportDate: reportDate);
            if (statsType.Site)
                DoETL_Site(reportDate: reportDate);
            //if (statsType.Conv)
                //DoConvTest();
        }

        public void DoHistorical()
        {
            var statsType = new StatsTypeAgg(this.StatsType);
            if (statsType.Daily)
                DoETL_Daily(buckets: BucketNamesFromConfig("DBM_AllIOBucket_Historical"));
            if (statsType.Strategy)
                DoETL_Strategy(buckets: BucketNamesFromConfig("DBM_AllLineItemBucket_Historical"));
            if (statsType.Creative)
                DoETL_Creative(buckets: BucketNamesFromConfig("DBM_AllCreativeBucket_Historical"));
            if (statsType.Site)
                DoETL_Site(buckets: BucketNamesFromConfig("DBM_AllSiteBucket_Historical"));
            //if (statsType.Conv)
            // TODO: implement
        }
        public static IEnumerable<string> BucketNamesFromConfig(string configKey)
        {
            var configVal = ConfigurationManager.AppSettings[configKey];
            if (configVal == null)
                configVal = String.Empty;
            return configVal.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void DoETL_Daily(DateTime? reportDate = null, IEnumerable<string> buckets = null)
        {
            if (buckets == null)
                buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllIOBucket"] };

            var extracter = new DbmCloudStorageExtracter(reportDate, buckets);
            var loader = new DbmDailySummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Strategy(DateTime? reportDate = null, IEnumerable<string> buckets = null)
        {
            if (buckets == null)
                buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllLineItemBucket"] };

            var extracter = new DbmCloudStorageExtracter(reportDate, buckets, byLineItem: true);
            var loader = new DbmLineItemSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Creative(DateTime? reportDate = null, IEnumerable<string> buckets = null)
        {
            if (buckets == null)
                buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllCreativeBucket"] };

            var extracter = new DbmCloudStorageExtracter(reportDate, buckets, byCreative: true);
            var loader = new DbmCreativeSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }
        public void DoETL_Site(DateTime? reportDate = null, IEnumerable<string> buckets = null)
        {
            if (buckets == null)
                buckets = new List<string> { ConfigurationManager.AppSettings["DBM_AllSiteBucket"] };

            var extracter = new DbmCloudStorageExtracter(reportDate, buckets, bySite: true);
            var impThresholdString = ConfigurationManager.AppSettings["TD_SiteStats_ImpressionThreshold"];
            int impThreshold;
            if (int.TryParse(impThresholdString, out impThreshold))
                extracter.ImpressionThreshold = impThreshold;

            var loader = new DbmSiteSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

    }
}
