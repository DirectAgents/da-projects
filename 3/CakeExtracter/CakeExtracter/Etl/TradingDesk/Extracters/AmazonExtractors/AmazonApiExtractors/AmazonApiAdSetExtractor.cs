using Amazon;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using CakeExtracter.Logging.TimeWatchers;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors
{
    // AdSet(Ad group)
    public class AmazonApiAdSetExtractor : BaseAmazonExtractor<AdSetSummary>
    {
        public AmazonApiAdSetExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account, bool clearBeforeLoad, string campaignFilter = null, string campaignFilterOut = null)
            : base(amazonUtility, dateRange, account, clearBeforeLoad, campaignFilter, campaignFilterOut)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSetSummaries from Amazon API for ({0}) from {1:d} to {2:d}",
                clientId, dateRange.FromDate, dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                try
                {
                    Extract(date);
                }
                catch (Exception e)
                {
                    Logger.Error(accountId, e);
                }
            }

            End();
        }

        private void Extract(DateTime date)
        {
            IEnumerable<AdSetSummary> items = null;
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                var sums = ExtractSummaries(date);
                items = TransformSummaries(sums, date);
            }, accountId, AmazonJobLevels.adSet, AmazonJobOperations.reportExtracting);
            if (ClearBeforeLoad)
            {
                RemoveOldData(date);
            }
            Add(items);
        }

        public IEnumerable<AmazonAdGroupSummary> ExtractSummaries(DateTime date)
        {
            var spSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredProducts, date, clientId, true);
            var sbSums = _amazonUtility.ReportAdGroups(CampaignType.SponsoredBrands, date, clientId, true);
            var sums = spSums.Concat(sbSums);
            var filteredSums = FilterByCampaigns(sums, x => x.CampaignName);
            return filteredSums.ToList();
        }

        private IEnumerable<AdSetSummary> TransformSummaries(IEnumerable<AmazonAdGroupSummary> adGroupStats, DateTime date)
        {
            var notEmptyStats = adGroupStats.Where(x => !x.AllZeros()).ToList();
            var summaries = notEmptyStats.Select(stat => CreateSummary(stat, date));
            return summaries.ToList();
        }

        private AdSetSummary CreateSummary(AmazonAdGroupSummary adGroupStat, DateTime date)
        {
            var sum = new AdSetSummary
            {
                AdSetEid = adGroupStat.AdGroupId,
                AdSetName = adGroupStat.AdGroupName,
                StrategyEid = adGroupStat.CampaignId,
                StrategyName = adGroupStat.CampaignName,
                StrategyType = adGroupStat.CampaignType
            };
            SetCPProgStats(sum, adGroupStat, date);
            return sum;
        }

        private void RemoveOldData(DateTime date)
        {
            Logger.Info(accountId, "The cleaning of AdSetSummaries for account ({0}) has begun - {1}.", accountId, date);
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(() =>
            {
                SafeContextWrapper.TryMakeTransaction((ClientPortalProgContext db) =>
                {
                    db.AdSetSummaryMetrics.Where(x => x.Date == date && x.AdSet.AccountId == accountId).DeleteFromQuery();
                    db.AdSetSummaries.Where(x => x.Date == date && x.AdSet.AccountId == accountId).DeleteFromQuery();
                }, "DeleteFromQuery");
            }, accountId, AmazonJobLevels.adSet, AmazonJobOperations.cleanExistingData);
            Logger.Info(accountId, "The cleaning of AdSetSummaries for account ({0}) is over - {1}.", accountId, date);
        }
    }
}
