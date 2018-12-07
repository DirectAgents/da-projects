using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{ 
    // AdSet(Ad group)
    public class AmazonApiAdSetExtracter : BaseAmazonExtractor<AdSetSummary>
    {
        public AmazonApiAdSetExtracter(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, campaignFilter: campaignFilter, campaignFilterOut: campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                this.clientId, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var sums = ExtractSummaries(date);
                var items = TransformSummaries(sums, date);
                Add(items);
            }
            End();
        }

        public IEnumerable<AmazonAdGroupSummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.campaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AdSetSummary> TransformSummaries(IEnumerable<AmazonAdGroupSummary> adGroupStats, DateTime date)
        {
            adGroupStats = adGroupStats.Where(x => !x.AllZeros()).ToList();
            foreach (var adGroupStat in adGroupStats)
            {
                var sum = new AdSetSummary
                {
                    AdSetEid = adGroupStat.adGroupId,
                    AdSetName = adGroupStat.adGroupName,
                    StrategyEid = adGroupStat.campaignId,
                    StrategyName = adGroupStat.campaignName
                };
                SetCPProgStats(sum, adGroupStat, date);
                yield return sum;
            }
        }
    }
}
