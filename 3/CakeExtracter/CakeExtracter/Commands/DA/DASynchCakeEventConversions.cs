using System;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Domain.Helpers;

namespace CakeExtracter.Commands
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
                ClearFirst = clearFirst
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

        private DateRange GetDateRange()
        {
            return DASynchCampSums.GetDateRange(StartDate, EndDate, DaysAgoToStart, defaultDaysAgo: 0, useYesterday: false);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            Logger.Info("Cake EventConversions ETL. DateRange {0}.", dateRange);

            if (ClearFirst)
            {
                ClearEventConversions(dateRange);
                Logger.Info("Cleared EventConversions");
            }
            var extracter = new EventConversionExtracter(dateRange, AdvertiserId ?? 0, OfferId ?? 0);
            var loader = new DAEventConversionLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            return 0;
        }

        public void ClearEventConversions(DateRange dateRange)
        {
            using (var db = new DAContext())
            {
                var eventConvs = db.EventConversions.AsQueryable();
                eventConvs = FilterEventConversions(eventConvs, advertiserId: AdvertiserId, offerId: OfferId, startDate: dateRange.FromDate, endDate: dateRange.ToDate);
                MainHelper.DeleteEventConversions(db, eventConvs);
            }
        }

        //TODO: move this to DirectAgents.Domain.Helpers.MainHelper ?
        public static IQueryable<EventConversion> FilterEventConversions(IQueryable<EventConversion> ec, int? advertiserId = null, int? offerId = null, int? affiliateId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (advertiserId.HasValue)
                ec = ec.Where(x => x.Offer.AdvertiserId == advertiserId.Value);
            if (offerId.HasValue)
                ec = ec.Where(x => x.OfferId == offerId.Value);
            if (affiliateId.HasValue)
                ec = ec.Where(x => x.AffiliateId == affiliateId.Value);
            if (startDate.HasValue)
                ec = ec.Where(x => x.ConvDate >= startDate.Value);
            if (endDate.HasValue)
            {
                var endDatePlusOne = endDate.Value.AddDays(1);
                ec = ec.Where(x => x.ConvDate < endDatePlusOne);
            }
            return ec;
        }
    }
}
