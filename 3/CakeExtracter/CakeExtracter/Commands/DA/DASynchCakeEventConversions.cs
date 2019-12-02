using System;
using System.ComponentModel.Composition;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Cleaners;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Helpers;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchCakeEventConversions : ConsoleCommand
    {
        public static int RunStatic(int? advertiserId = null, int? offerId = null, DateTime? startDate = null, DateTime? endDate = null, int? daysAgoToStart = null, bool clearFirst = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchCakeEventConversions
            {
                AdvertiserId = advertiserId,
                OfferId = offerId,
                StartDate = startDate,
                EndDate = endDate,
                DaysAgoToStart = daysAgoToStart,
                ClearFirst = clearFirst,
            };
            return cmd.Run();
        }

        public int? AdvertiserId { get; set; }

        public int? OfferId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? DaysAgoToStart { get; set; }

        public bool ClearFirst { get; set; }

        public override void ResetProperties()
        {
            AdvertiserId = null;
            OfferId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            ClearFirst = false;
        }

        // e.g. daSynchCakeEventConversions -a=806 -s=9/17/18 -e=10/1/18

        public DASynchCakeEventConversions()
        {
            IsCommand("daSynchCakeEventConversions", "synch EventConversions");
            HasOption<int>("a|advertiserId=", "Advertiser Id (default: 0 / all advertisers)", c => AdvertiserId = c);
            HasOption<int>("o|offerId=", "Offer Id (default: 0 / all offers)", c => OfferId = c);
            HasOption("s|startDate=", "Start Date (default: 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default: today)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default: 0(today); use -1 for first-of-month)", c => DaysAgoToStart = c);
            HasOption<bool>("w|wipeFirst=", "Clear existing convs in the db first (default: false)", c => ClearFirst = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            Logger.Info("Cake EventConversions ETL. DateRange {0}.", dateRange);
            if (ClearFirst)
            {
                CleanEventConversions(dateRange);
            }
            var extractor = new EventConversionExtracter(dateRange, AdvertiserId ?? 0, OfferId ?? 0);
            var loader = new DAEventConversionLoader();
            CommandHelper.DoEtl(extractor, loader);
            return 0;
        }

        private DateRange GetDateRange()
        {
            return DASynchCampSums.GetDateRange(StartDate, EndDate, DaysAgoToStart, defaultDaysAgo: 0, useYesterday: false);
        }

        private void CleanEventConversions(DateRange dateRange)
        {
            var cleaner = new DaEventConversionCleaner(AdvertiserId, OfferId, dateRange);
            cleaner.ClearEventConversions();
        }
    }
}