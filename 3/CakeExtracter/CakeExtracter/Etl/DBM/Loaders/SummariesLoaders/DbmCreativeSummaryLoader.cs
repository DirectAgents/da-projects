using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Loaders.SummariesLoaders
{
    /// <summary>
    /// Loader for DBM summaries of creative
    /// </summary>
    public class DbmCreativeSummaryLoader : Loader<DbmCreativeSummary>
    {
        private readonly DbmCreativeLoader creativeLoader;
        private readonly DateRange dateRange;

        private static readonly object lockObject = new object();
        private const int batchSize = 1000;

        private readonly List<DbmCreativeSummary> latestSummaries = new List<DbmCreativeSummary>();

        public DbmCreativeSummaryLoader(int accountId, DateRange dateRange) 
            : base(accountId, batchSize)
        {
            var advertiserLoader = new DbmAdvertiserLoader();
            creativeLoader = new DbmCreativeLoader(advertiserLoader);
            this.dateRange = dateRange;
        }

        protected override int Load(List<DbmCreativeSummary> summaries)
        {
            EnsureCreativeEntitiesData(summaries);
            var uniqueSummaries = summaries.GroupBy(s => new { s.EntityId, s.Date }).Select(gr => gr.First()).ToList();
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

        private void EnsureCreativeEntitiesData(List<DbmCreativeSummary> summaries)
        {
            var creatives = summaries.Select(summary => summary.Creative).Where(summary => summary != null).ToList();
            creativeLoader.AddUpdateDependentEntities(creatives);
            summaries.ForEach(summary => { summary.EntityId = summary.Creative.Id; });
        }

        private void DeleteOldSummariesFromDb()
        {
            Logger.Info(accountId, $"Started cleaning of creative summaries has begun - {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.DbmCreativeSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                && x.Creative.Advertiser.AccountId == accountId).DeleteFromQuery();
            }, lockObject, "BulkDeleteByQuery");
            Logger.Info(accountId, $"The cleaning of creative summaries is over - {dateRange.ToString()})");
        }

        private void LoadLatestSummariesToDb(IReadOnlyCollection<DbmCreativeSummary> summaries)
        {
            Logger.Info(accountId, $"Started loading of creative summaries has begun - {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObject, "BulkInsert");
            Logger.Info(accountId, $"The loading of creative summaries is over - {dateRange.ToString()})");
        }
    }
}
