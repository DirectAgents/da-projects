using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Bootstrappers;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchCampSums : ConsoleCommand
    {
        public int? OfferId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override void ResetProperties()
        {
            OfferId = null;
            StartDate = null;
            EndDate = null;
        }

        public DASynchCampSums()
        {
            IsCommand("daSynchCampSums", "synch monthly CampaignSummaries");
            HasOption<int>("o|offerId=", "Offer Id (default = 0 / all offers)", c => OfferId = c);
            HasOption("s|startDate=", "Start Date (default is 1st of month (via yesterday))", c => StartDate = DateTime.Parse(c));
            HasOption("e|endDate=", "End Date (default is last day of the same month or yesterday if current month)", c => EndDate = DateTime.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            DateTime from = StartDate ?? new DateTime(yesterday.Year, yesterday.Month, 1);
            DateTime to;
            if (EndDate.HasValue)
            {
                to = EndDate.Value;
                if (!StartDate.HasValue)
                    while (from > to)
                    {
                        from = from.AddMonths(-1);
                    }
            }
            else
            { // EndDate not specified...
                if (from.Year == yesterday.Year && from.Month == yesterday.Month)
                    to = yesterday;
                else
                    to = new DateTime(from.Year, from.Month, 1).AddMonths(1).AddDays(-1);
                    // last day of the month of "from"
            }
            var dateRange = new DateRange(from, to);

            //TODO: option to delete all first

            var extracter = new CampaignSummaryExtracter(dateRange, offerId: OfferId ?? 0, groupByOffAff: false);
            var loader = new DACampSumLoader(dateRange.FromDate);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            //LoadMissingOffers(loader.OfferIdsSaved);
            //LoadMissingCampaigns(loader.CampIdsSaved);

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
