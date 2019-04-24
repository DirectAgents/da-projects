using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Loaders.SummariesLoaders
{
    public class DbmLineItemSummaryLoader : Loader<DbmLineItemSummary>
    {
        private readonly DbmLineItemLoader lineItemLoader;
        private readonly DateRange dateRange;

        private static readonly object lockObject = new object();
        private const int batchSize = 1000;

        private readonly List<DbmLineItemSummary> latestSummaries = new List<DbmLineItemSummary>();

        public DbmLineItemSummaryLoader(int accountId, DateRange dateRange) 
            : base(accountId, batchSize)
        {
            var advertiserLoader = new DbmAdvertiserLoader();
            var campaignLoader = new DbmCampaignLoader(advertiserLoader);
            var insertionOrderLoader = new DbmInsertionOrderLoader(advertiserLoader, campaignLoader);
            lineItemLoader = new DbmLineItemLoader(advertiserLoader, campaignLoader, insertionOrderLoader);
            this.dateRange = dateRange;
        }

        protected override int Load(List<DbmLineItemSummary> summaries)
        {
            EnsureLineItemEntitiesData(summaries);
            var uniqueSummaries = summaries.GroupBy(s => new {s.EntityId, s.Date}).Select(gr => gr.First()).ToList();
            var notProcessedSummaries = uniqueSummaries
                .Where(s => !latestSummaries.Any(ls => s.EntityId == ls.EntityId && s.Date == ls.Date)).ToList();
            latestSummaries.AddRange(notProcessedSummaries);
            return summaries.Count;
        }

        protected override void AfterLoad()
        {
            DeleteOldSummariesFromDb();
            LoadLatestSummariesToDb(latestSummaries);
        }

        private void EnsureLineItemEntitiesData(List<DbmLineItemSummary> summaries)
        {
            var lineItems = summaries.Select(summary => summary.LineItem).Where(summary => summary != null).ToList();
            lineItemLoader.AddUpdateDependentEntities(lineItems);
            summaries.ForEach(summary => { summary.EntityId = summary.LineItem.Id; });
        }

        private void DeleteOldSummariesFromDb()
        {
            Logger.Info(accountId, $"Started cleaning old line item summaries for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.DbmLineItemSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                && x.LineItem.InsertionOrder.Campaign.Advertiser.AccountId == accountId).DeleteFromQuery();
            }, lockObject, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(IReadOnlyCollection<DbmLineItemSummary> summaries)
        {
            Logger.Info(accountId, $"Started loading line item summaries for {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObject, "BulkInsert");
        }
    }
}
