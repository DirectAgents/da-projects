using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.DBM.Loaders.EntitiesLoaders;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.DBM.Loaders.SummariesLoaders
{
    /// <summary>
    /// Loader for DBM summaries of creative
    /// </summary>
    public class DbmCreativeSummaryLoader
    {
        private readonly DbmCreativeLoader creativeLoader;
        private readonly DateRange dateRange;

        private static readonly object lockObject = new object();
        
        public DbmCreativeSummaryLoader(DateRange dateRange) 
        {
            var advertiserLoader = new DbmAdvertiserLoader();
            creativeLoader = new DbmCreativeLoader(advertiserLoader);
            this.dateRange = dateRange;
        }

        public void Load(IEnumerable<DbmCreativeSummary> summaries)
        {
            Logger.Info("Started loading creative summaries...");

            var summaryGroups = GetSummariesGroupedByAccount(summaries);
            foreach (var summaryGroup in summaryGroups)
            {
                LoadSummariesForAccount(summaryGroup);
            }
            Logger.Info("Finished loading creative summaries.");
        }

        private static IEnumerable<IGrouping<int?, DbmCreativeSummary>> GetSummariesGroupedByAccount(
            IEnumerable<DbmCreativeSummary> summaries)
        {
            return summaries.GroupBy(summary => summary.Creative.AccountId);
        }

        private void LoadSummariesForAccount(IGrouping<int?, DbmCreativeSummary> summaryForAccount)
        {
            var accountId = Convert.ToInt32(summaryForAccount.Key);
            EnsureCreativeEntitiesData(summaryForAccount);
            DeleteOldSummariesFromDb(accountId);
            LoadSummariesToDb(summaryForAccount);
        }

        private void EnsureCreativeEntitiesData(IEnumerable<DbmCreativeSummary> summaries)
        {
            var creatives = summaries.Select(summary => summary.Creative).Where(summary => summary != null).ToList();
            creativeLoader.AddUpdateDependentEntities(creatives);
            summaries.ForEach(summary => { summary.EntityId = summary.Creative.Id; });
        }

        private void DeleteOldSummariesFromDb(int accountId)
        {
            Logger.Info(accountId, $"Started cleaning of creative summaries has begun - {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.DbmCreativeSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                && x.Creative.AccountId == accountId).DeleteFromQuery();
            }, lockObject, "BulkDeleteByQuery");
            Logger.Info(accountId, $"The cleaning of creative summaries is over - {dateRange.ToString()})");
        }

        private void LoadSummariesToDb(IGrouping<int?, DbmCreativeSummary> summaryForAccount)
        {
            var accountId = Convert.ToInt32(summaryForAccount.Key);
            Logger.Info(accountId, $"Started loading of creative summaries has begun - {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaryForAccount);
            }, lockObject, "BulkInsert");
            Logger.Info(accountId, $"The loading of creative summaries is over - {dateRange.ToString()})");
        }
    }
}
