using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class CampaignSummaryExtracter : Extracter<CampaignSummary>
    {
        private readonly DateRange dateRange;
        private readonly IEnumerable<int> advertiserIds;
        private readonly IEnumerable<int> offerIds;
        private readonly bool groupByOffAff;
        private readonly bool getDailyStats;

        public CampaignSummaryExtracter(DateRange dateRange, IEnumerable<int> advertiserIds = null, IEnumerable<int> offerIds = null, bool groupByOffAff = false, bool getDailyStats = false)
        {
            this.dateRange = dateRange;
            this.advertiserIds = (advertiserIds == null) ? new[] { 0 } : advertiserIds;
            this.offerIds = (offerIds == null) ? new[] { 0 } : offerIds;
            this.groupByOffAff = groupByOffAff;
            this.getDailyStats = getDailyStats;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting CampaignSummaries from {0:d} to {1:d}, AdvIds {2}, OffIds {3}",
                        dateRange.FromDate, dateRange.ToDate, string.Join(",", advertiserIds), string.Join(",", offerIds));

            if (getDailyStats)
            {
                foreach (var date in dateRange.Dates)
                {
                    Logger.Info("Extracting CampaignSummaries for {0:d}...", date);
                    LoopAsNeeded(new DateRange(date, date.AddDays(1)));
                }
            }
            else
            {
                LoopAsNeeded(new DateRange(dateRange.FromDate, dateRange.ToDate.AddDays(1)));
            }
            End();
            //Note: Cake needs dateRange.ToDate to be the day after the last day of stats needed
        }

        //If advId and offId are both 0, will make one call.
        //If advId==0, will loop through offIds.
        //If offId==0, will loop through advIds.
        //If advId and offId are both != 0, will loop through advIds, then loop through offIds.
        private void LoopAsNeeded(DateRange dateRange)
        {
            bool zeroAdvId = !advertiserIds.Any(x => x != 0);
            bool zeroOfferId = !offerIds.Any(x => x != 0);
            bool bothNonZero = !zeroAdvId && !zeroOfferId;

            if (zeroOfferId || bothNonZero)
                LoopThroughAdvertisers(dateRange);
            if (!zeroOfferId)
                LoopThroughOffers(dateRange);
        }

        private void LoopThroughAdvertisers(DateRange dateRangeForStats)
        {
            foreach (var advId in advertiserIds)
            {
                Logger.Info("Extracting CampaignSummaries for advertiser {0}", advId);
                var campaignSummaries = CakeMarketingUtility.CampaignSummaries(dateRangeForStats, advertiserId: advId);
                if (this.groupByOffAff)
                    ExtractWithGrouping(campaignSummaries, dateRangeForStats.FromDate);
                else
                    ExtractWithoutGrouping(campaignSummaries, dateRangeForStats.FromDate);
            }
        }
        private void LoopThroughOffers(DateRange dateRangeForStats)
        {
            foreach (var offerId in offerIds)
            {
                Logger.Info("Extracting CampaignSummaries for offer {0}", offerId);
                var campaignSummaries = CakeMarketingUtility.CampaignSummaries(dateRangeForStats, offerId: offerId);
                if (this.groupByOffAff)
                    ExtractWithGrouping(campaignSummaries, dateRangeForStats.FromDate);
                else
                    ExtractWithoutGrouping(campaignSummaries, dateRangeForStats.FromDate);
            }
            //Note: If not getting daily stats, the CampaignSummaries will have Date set to the first date in the daterange
        }

        private void ExtractWithoutGrouping(IEnumerable<CampaignSummary> campaignSummaries, DateTime date)
        {
            foreach (var campSum in campaignSummaries)
            {
                if (campSum.SiteOffer.SiteOfferId < 0)
                    continue; // skip OfferIds -2 and -1

                //TODO: filter those with zero conversions, rev, cost... etc
                //TODO: allow for CampSums marked for deletion - w/ CampSumWrap?

                campSum.Date = date;
                Add(campSum);
            }
        }
        private void ExtractWithGrouping(IEnumerable<CampaignSummary> campaignSummaries, DateTime date)
        {
            var csOfferGroups = campaignSummaries.GroupBy(cs => cs.SiteOffer.SiteOfferId);

            foreach (var group in csOfferGroups)
            {
                if (group.Key < 0)
                    continue; // skip OfferIds -2 and -1

                var affIds = group.Select(cs => cs.SourceAffiliate.SourceAffiliateId).Distinct();
                foreach (var affId in affIds)
                {
                    var cSums = group.Where(cs => cs.SourceAffiliate.SourceAffiliateId == affId);
                    if (cSums.Count() == 1)
                    {
                        cSums.First().Date = date;
                        Add(cSums);
                    }
                    else
                    { // There's more than one for this off/aff combo.  Sum up the stats.
                        var totals = new CampaignSummary
                        {
                            Views = cSums.Sum(cs => cs.Views),
                            Clicks = cSums.Sum(cs => cs.Clicks),
                            MacroEventConversions = cSums.Sum(cs => cs.MacroEventConversions),
                            Paid = cSums.Sum(cs => cs.Paid),
                            Sellable = cSums.Sum(cs => cs.Sellable),
                            Cost = cSums.Sum(cs => cs.Cost),
                            Revenue = cSums.Sum(cs => cs.Revenue)
                        };
                        var first = cSums.First();
                        first.CopyStatsFrom(totals);
                        first.Date = date;
                        Add(first);
                        // (Assume they all have the same AccountManager,Advertiser,AdvertiserManager,etc though only the stats are loaded.)
                    }
                }
            }
        }
    }
}
