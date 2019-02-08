using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonDailySummaryLoader : Loader<DailySummary>
    {
        private readonly TDDailySummaryLoader summaryItemsLoader;

        private readonly AmazonSummaryMetricLoader<DailySummaryMetric> summaryMetricsItemsLoader;

        public AmazonDailySummaryLoader(int accountId) : base(accountId)
        {
            summaryItemsLoader = new TDDailySummaryLoader(accountId);
            summaryMetricsItemsLoader = new AmazonSummaryMetricLoader<DailySummaryMetric>();
        }

        protected override int Load(List<DailySummary> summaryItems)
        {
            Logger.Info(accountId, "Loading {0} AmazonDailySummaries..", summaryItems.Count);
            EnsureRelatedItems(summaryItems);
            UpsertSummaryItems(summaryItems);
            var summaryMetricItems = GetSummaryMetricsToInsert(summaryItems);
            summaryMetricsItemsLoader.UpsertSummaryMetrics(summaryMetricItems);
            return summaryItems.Count;
        }

        private void UpsertSummaryItems(List<DailySummary> summaryItems)
        {
            using (var db = new ClientPortalProgContext())
            {
                db.BulkInsert(summaryItems);
            }
        }

        private void EnsureRelatedItems(List<DailySummary> summaryItems)
        {
            summaryItemsLoader.AssignIdsToItems(summaryItems);
        }

        private List<SummaryMetric> GetSummaryMetricsToInsert(List<DailySummary> summaryItems)
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
                    metric.EntityId = adSummary.AccountId;
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
