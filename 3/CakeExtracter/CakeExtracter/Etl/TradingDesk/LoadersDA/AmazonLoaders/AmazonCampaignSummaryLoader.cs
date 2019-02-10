using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonCampaignSummaryLoader : BaseAmazonLevelLoader<StrategySummary>
    {
        private readonly TDStrategySummaryLoader summaryItemsLoader;

        private readonly AmazonSummaryMetricLoader<StrategySummaryMetric> summaryMetricsItemsLoader;

        public AmazonCampaignSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDStrategySummaryLoader(accountId);
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<StrategySummaryMetric>();
        }

        protected override int Load(List<StrategySummary> summaryItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon Campaign Daily Summaries..", summaryItems.Count);
            EnsureRelatedItems(summaryItems);
            UpsertSummaryItems(summaryItems);
            var summaryMetricItems = GetSummaryMetricsToInsert(summaryItems);
            summaryMetricsItemsLoader.UpsertSummaryMetrics(summaryMetricItems);
            return summaryItems.Count;
        }

        private void UpsertSummaryItems(List<StrategySummary> summaryItems)
        {
            using (var db = new ClientPortalProgContext())
            {
                db.BulkInsert(summaryItems);
            }
        }

        private void EnsureRelatedItems(List<StrategySummary> summaryItems)
        {
            summaryItemsLoader.PrepareData(summaryItems);
            summaryItemsLoader.AddUpdateDependentStrategies(summaryItems);
            summaryItemsLoader.AssignStrategyIdToItems(summaryItems);
        }

        private List<SummaryMetric> GetSummaryMetricsToInsert(List<StrategySummary> summaryItems)
        {
            var summaryMetricsToInsert = new List<SummaryMetric>();
            summaryItems.ForEach(adSummary =>
            {
                var metrics = adSummary.InitialMetrics == null
                ? adSummary.Metrics
                : adSummary.Metrics == null
                    ? adSummary.InitialMetrics
                    : adSummary.InitialMetrics.Concat(adSummary.Metrics);
                metrics.ForEach(metric =>
                {
                    metric.EntityId = adSummary.StrategyId;
                });
                if (metrics != null)
                {
                    summaryMetricsToInsert.AddRange(metrics);
                }
            });
            return summaryMetricsToInsert;
        }
    }
}
