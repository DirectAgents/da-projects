using System;
using System.Linq;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class CampaignSummaryExtracter : Extracter<CampaignSummary>
    {
        private readonly DateRange dateRange;
        private readonly int offerId;

        public CampaignSummaryExtracter(DateRange dateRange, int offerId = 0)
        {
            this.dateRange = new DateRange(dateRange.FromDate, dateRange.ToDate.AddDays(1));
            //Cake needs dateRange.ToDate to be the day after the last day requested
            this.offerId = offerId;
        }
        public CampaignSummaryExtracter(DateTime date, int offerId = 0)
        {
            this.dateRange = new DateRange(date, date.AddDays(1));
            this.offerId = offerId;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting CampaignSummaries from {0:d} to {1:d}, OffId {2}",
                        dateRange.FromDate, dateRange.ToDate.AddDays(-1), offerId);

            var campaignSummaries = CakeMarketingUtility.CampaignSummaries(dateRange, offerId: offerId);
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
                        Add(cSums);
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
                        Add(first);
                        // (Assume they all have the same AccountManager,Advertiser,AdvertiserManager, though only the stats are loaded.)
                    }
                }
            }

            End();
        }
    }
}
