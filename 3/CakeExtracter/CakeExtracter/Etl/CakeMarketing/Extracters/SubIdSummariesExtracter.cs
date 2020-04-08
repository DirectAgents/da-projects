using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Exceptions;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class SubIdSummariesExtracter : Extracter<SubIdSummary>
    {
        private readonly DateRange dateRange;
        private readonly int? affiliateId;
        private readonly int? advertiserId;
        private readonly int? offerId;
        private readonly bool getDailyStats;

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<CakeAffSubSumsFailedEtlException> ProcessFailedExtraction;

        public SubIdSummariesExtracter(DateRange dateRange, int? affiliateId = null, int? advertiserId = null, int? offerId = null, bool getDailyStats = false)
        {
            this.dateRange = dateRange;
            this.affiliateId = affiliateId;
            this.advertiserId = advertiserId;
            this.offerId = offerId;
            this.getDailyStats = getDailyStats;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting SubIdSummaries from {0:d} to {1:d}, AffId:{2} AdvId:{3} OffId:{4}",
                dateRange.FromDate, dateRange.ToDate, affiliateId, advertiserId, offerId);

            if (getDailyStats)
            {
                foreach (var date in dateRange.Dates)
                {
                    Logger.Info("Extracting SubIdSummaries for {0:d}...", date);
                    var oneDay = new DateRange(date, date);
                    LoopThroughAffiliates(oneDay);
                }
            }
            else
            {
                LoopThroughAffiliates(dateRange);
            }
            End();
        }

        private void LoopThroughAffiliates(DateRange dateRange)
        {
            var lastDatePlusOne = dateRange.ToDate.AddDays(1);
            var dateRangeForCake = new DateRange(dateRange.FromDate, lastDatePlusOne);

            // ?Is it better to do this outside the loop under getDailyStats?
            // As is, will be the least number of API calls...
            var affOfferIds = GetAffiliateOfferIds(dateRange, this.affiliateId, this.advertiserId, this.offerId);

            foreach (var affId in affOfferIds.Keys)
            {
                var offerIds = affOfferIds[affId];
                foreach (var offId in offerIds)
                {
                    ExtractSubIdSums(dateRange, dateRangeForCake, affId, offId);
                }
            }
        }

        private void ExtractSubIdSums(DateRange dateRange, DateRange dateRangeForCake, int affId, int offId)
        {
            try
            {
                var sums = CakeMarketingUtility.SubIdSummaries(dateRangeForCake, affId, offerId: offId);
                foreach (var sum in sums)
                {
                    sum.Date = dateRange.FromDate; // for MTD stats, should be the 1st of month
                    sum.affiliateId = affId;
                    sum.offerId = offId;
                    Add(sum);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                var exception = new CakeAffSubSumsFailedEtlException(dateRange.FromDate, dateRange.ToDate, affId, advertiserId, offId, e);
                ProcessFailedExtraction?.Invoke(exception);
            }
        }

        private static Dictionary<int, int[]> GetAffiliateOfferIds(DateRange dateRange, int? affiliateId, int? advertiserId, int? offerId)
        {
            var affToOffersDict = new Dictionary<int, int[]>();
            using (var db = new DAContext())
            {
                var campSums = db.CampSums.AsQueryable();
                campSums = campSums.Where(cs => cs.Date >= dateRange.FromDate && cs.Date <= dateRange.ToDate);
                if (affiliateId.HasValue)
                    campSums = campSums.Where(cs => cs.Camp.AffiliateId == affiliateId.Value);
                    //campSums = campSums.Where(cs => cs.AffId == affiliateId.Value); //not indexed?
                if (advertiserId.HasValue)
                    campSums = campSums.Where(cs => cs.Camp.Offer.AdvertiserId == advertiserId.Value);
                if (offerId.HasValue)
                    campSums = campSums.Where(cs => cs.Camp.OfferId == offerId.Value);
                    //campSums = campSums.Where(cs => cs.OfferId == offerId.Value); //not indexed?

                var csGroups = campSums.GroupBy(cs => cs.Camp.AffiliateId);
                foreach (var grp in csGroups)
                {
                    var offerIds = grp.Select(cs => cs.OfferId).Distinct().OrderBy(x => x).ToArray();
                    affToOffersDict[grp.Key] = offerIds;
                }
            }
            return affToOffersDict;
        }
    }
}
