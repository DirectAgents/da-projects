using System;
using System.ComponentModel.Composition;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAffSubSums : ConsoleCommand
    {
        public static int RunStatic(int? affiliateId = null, int? advertiserId = null, int? offerId = null, DateTime? startDate = null, DateTime? endDate = null, int? daysAgoToStart = null)
            //, bool clearFirst = false)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAffSubSums
            {
                AffiliateId = affiliateId,
                AdvertiserId = advertiserId,
                OfferId = offerId,
                StartDate = startDate,
                EndDate = endDate,
                DaysAgoToStart = daysAgoToStart
            };
            return cmd.Run();
        }

        public int? AffiliateId { get; set; }
        public int? AdvertiserId { get; set; }
        public int? OfferId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }
        //public bool ClearFirst { get; set; }

        public override void ResetProperties()
        {
            AffiliateId = null;
            AdvertiserId = null;
            OfferId = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
            //ClearFirst = false;
        }

        //Note: Uses CampSums to determine which Affs & Offers to loop thru (i.e. those with campsum stats)
        public DASynchAffSubSums()
        {
            IsCommand("daSynchAffSubSums", "synch stats by AffSub and Offer");
            HasOption<int>("f|affiliateId=", "Affiliate Id (default: all affiliates)", c => AffiliateId = c);
            HasOption<int>("a|advertiserId=", "Advertiser Id (default: all advertisers)", c => AdvertiserId = c);
            HasOption<int>("o|offerId=", "Offer Id (default: all offers)", c => OfferId = c);
            HasOption("s|startDate=", "Start Date (default: 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default: today)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default: -1 == first-of-month)", c => DaysAgoToStart = c);
            //HasOption<bool>("w|wipeFirst=", "Clear existing convs in the db first (default: false)", c => ClearFirst = c);
        }

        private DateRange GetDateRange()
        {
            return DASynchCampSums.GetDateRange(StartDate, EndDate, DaysAgoToStart, defaultDaysAgo: -1, useYesterday: false);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            Logger.Info("Cake AffSubSummaries ETL. DateRange {0}.", dateRange);

            var extracter = new SubIdSummariesExtracter(dateRange, affiliateId: AffiliateId, advertiserId: AdvertiserId, offerId: OfferId, getDailyStats: true);
            var loader = new DAAffSubSummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
