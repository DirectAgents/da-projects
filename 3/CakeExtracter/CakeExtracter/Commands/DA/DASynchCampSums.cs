﻿using System;
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

        public override void ResetProperties()
        {
            OfferIds = null;
            StartDate = null;
            EndDate = null;
        }

        public DASynchCampSums()
        {
            IsCommand("daSynchCampSums", "synch monthly CampaignSummaries");
            HasOption("o|offerIds=", "Offer Ids (default = all offers)", c => OfferIds = c);
            HasOption("s|startDate=", "Start Date (default is 1st of month (via yesterday))", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is yesterday)", c => EndDate = DateTime.Parse(c));
        }

        //TODO: an option to go back X days

        private DateRange GetDateRange()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            DateTime from = StartDate ?? new DateTime(yesterday.Year, yesterday.Month, 1);
            DateTime to = EndDate ?? yesterday;
            return new DateRange(from, to);
        }

        public override int Execute(string[] remainingArguments)
        {
            var dateRange = GetDateRange();
            //TODO: option to delete all first

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
