using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonKeywordSummaryLoader : Loader<KeywordSummary>
    {
        private readonly KeywordSummaryLoader summaryLoader;

        private readonly AmazonSummaryMetricLoader<KeywordSummaryMetric> summaryMetricsLoader;

        public AmazonKeywordSummaryLoader(int accountId)
        {
            summaryLoader = new KeywordSummaryLoader(accountId);
            summaryMetricsLoader = new AmazonSummaryMetricLoader<KeywordSummaryMetric>();
        }

        protected override int Load(List<KeywordSummary> summaryItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon Keyword Daily Summaries..", summaryItems.Count);
            EnsureRelatedItems(summaryItems);
            UpsertKeywordSummaryItems(summaryItems);
            var summaryMetricItems = GetSummaryMetricsToInsert(summaryItems);
            summaryMetricsLoader.UpsertSummaryMetrics(summaryMetricItems);
            return summaryItems.Count;
        }

        private void UpsertKeywordSummaryItems(List<KeywordSummary> summaryItems)
        {
            using (var db = new ClientPortalProgContext())
            {
                db.BulkInsert(summaryItems);
            }
        }

        private void EnsureRelatedItems(List<KeywordSummary> keywordItems)
        {
            summaryLoader.PrepareData(keywordItems);
            summaryLoader.AddUpdateDependentStrategies(keywordItems);
            summaryLoader.AddUpdateDependentAdSets(keywordItems);
            summaryLoader.AddUpdateDependentKeywords(keywordItems);
            summaryLoader.AssignKeywordIdToItems(keywordItems);
        }

        private List<SummaryMetric> GetSummaryMetricsToInsert(List<KeywordSummary> summaries)
        {
            var summaryMetricsToInsert = new List<SummaryMetric>();
            summaries.ForEach(summary =>
            {
                var metrics = summary.InitialMetrics == null
                ? summary.Metrics
                : summary.Metrics == null
                    ? summary.InitialMetrics
                    : summary.InitialMetrics.Concat(summary.Metrics);
                metrics.ForEach(metric =>
                {
                    metric.EntityId = summary.KeywordId;
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
