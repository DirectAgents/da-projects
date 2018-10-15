using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchCampSums : ConsoleCommand
    {
        public string OfferIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysAgoToStart { get; set; }

        public override void ResetProperties()
        {
            OfferIds = null;
            StartDate = null;
            EndDate = null;
            DaysAgoToStart = null;
        }

        public DASynchCampSums()
        {
            IsCommand("daSynchCampSums", "synch daily CampaignSummaries");
            HasOption("o|offerIds=", "Offer Ids (default: all offers)", c => OfferIds = c);
            HasOption("s|startDate=", "Start Date (default: 'daysAgo')", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default: today)", c => EndDate = DateTime.Parse(c));
            HasOption<int>("d|daysAgo=", "Days Ago to start, if startDate not specified (default: 41; use -1 for first-of-month)", c => DaysAgoToStart = c);
        }

        private DateRange GetDateRange() //TODO: unhardcode defaultDaysAgo
        {
            return GetDateRange(StartDate, EndDate, DaysAgoToStart, defaultDaysAgo: 41, useYesterday: false);
        }
        // "useYesterday": when computing first-of-month and default end-date
        public static DateRange GetDateRange(DateTime? startDate, DateTime? endDate, int? daysAgoToStart, int defaultDaysAgo = 0, bool useYesterday = false)
        {
            var today = DateTime.Today;
            var upTo = useYesterday ? today.AddDays(-1) : today;
            if (!startDate.HasValue && daysAgoToStart == -1) // use "first-of-month"
                startDate = new DateTime(upTo.Year, upTo.Month, 1);
            daysAgoToStart = daysAgoToStart ?? defaultDaysAgo;
            return new DateRange(startDate ?? today.AddDays(-daysAgoToStart.Value), endDate ?? upTo);
        }
        //TODO: use the above in other ETL commands

        //TODO: option to delete-all-for-daterange first
        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            Logger.Info("Cake CampSums ETL. DateRange {0}.", dateRange);

            var offerIds = new List<int>();
            if (this.OfferIds != null)
            {
                var offerIds_string = this.OfferIds.Split(new char[] { ',' });
                int tempInt;
                foreach (var offerId_string in offerIds_string)
                {
                    if (int.TryParse(offerId_string, out tempInt))
                        offerIds.Add(tempInt);
                }
            }
            if (offerIds.Count == 0)
                offerIds.Add(0); // all offers

            foreach (var offerId in offerIds)
            {
                var extracter = new CampaignSummaryExtracter(dateRange, offerId: offerId, groupByOffAff: false, getDailyStats: true);
                var loader = new DACampSumLoader(keepAllNonZero: true);
                var extracterThread = extracter.Start();
                var loaderThread = loader.Start(extracter);
                extracterThread.Join();
                loaderThread.Join();
                //LoadMissingOffers(loader.OfferIdsSaved);
                //LoadMissingCampaigns(loader.CampIdsSaved);
            }
            return 0;
        }

        //private void LoadMissingOffers(IEnumerable<int> offerIdsToCheck)
        //{
        //    int[] existingOfferIds;
        //    using (var db = new DAContext())
        //    {
        //        existingOfferIds = db.Offers.Select(x => x.OfferId).ToArray();
        //    }
        //    var offerIdsToLoad = offerIdsToCheck.Where(id => !existingOfferIds.Contains(id));

        //    var extracter = new OffersExtracter(offerIds: offerIdsToLoad);
        //    var loader = new DAOffersLoader(loadInactive: true);
        //    var extracterThread = extracter.Start();
        //    var loaderThread = loader.Start(extracter);
        //    extracterThread.Join();
        //    loaderThread.Join();
        //}

        //private void LoadMissingCampaigns(IEnumerable<int> campIdsToCheck)
        //{
        //    //TODO: check if all affiliates are in db; save any that aren't
        //    //TODO: make camp.AffId a foreign key

        //    int[] existingCampIds;
        //    using (var db = new DAContext())
        //    {
        //        existingCampIds = db.Camps.Select(x => x.CampaignId).ToArray();
        //    }
        //    var campIdsToLoad = campIdsToCheck.Where(id => !existingCampIds.Contains(id));

        //    var extracter = new CampaignsExtracter(campaignIds: campIdsToLoad);
        //    var loader = new DACampLoader();
        //    var extracterThread = extracter.Start();
        //    var loaderThread = loader.Start(extracter);
        //    extracterThread.Join();
        //    loaderThread.Join();
        //}
    }
}
