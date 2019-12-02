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
    /// Loader for DBM summaries of creative.
    /// </summary>
    public class DbmCreativeSummaryLoader
    {
        private static readonly object LockObject = new object();
        private readonly DbmCreativeLoader creativeLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmCreativeSummaryLoader"/> class.
        /// </summary>
        public DbmCreativeSummaryLoader()
        {
            var advertiserLoader = new DbmAdvertiserLoader();
            creativeLoader = new DbmCreativeLoader(advertiserLoader);
        }

        /// <summary>
        /// Loads DBM creative summaries to Db.
        /// </summary>
        /// <param name="summaries">List of DBM Creative summaries for loading.</param>
        public void Load(List<DbmCreativeSummary> summaries)
        {
            Logger.Info("Started loading creative summaries...");
            EnsureCreativeEntitiesData(summaries);
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
            var dateRange = GetDateRangeForLoadingSummaries(summaryForAccount);
            Logger.Info(accountId, "DBM ETL Creative. Account ID = {0}", accountId);
            DeleteOldSummariesFromDb(accountId, dateRange);
            LoadSummariesToDb(accountId, summaryForAccount, dateRange);
            Logger.Info(accountId, "Finished DBM ETL Creative for account (ID = {0})", accountId);
        }

        private void EnsureCreativeEntitiesData(IEnumerable<DbmCreativeSummary> summaries)
        {
            var creatives = summaries.Select(summary => summary.Creative).Where(summary => summary != null).ToList();
            creativeLoader.AddUpdateDependentEntities(creatives);
            summaries.ForEach(summary => { summary.EntityId = summary.Creative.Id; });
        }

        private void DeleteOldSummariesFromDb(int accountId, DateRange summaryDateRange)
        {
            Logger.Info(accountId, $"Started cleaning of creative summaries has begun - {summaryDateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock(
                (ClientPortalProgContext db) =>
                {
                    db.DbmCreativeSummaries
                        .Where(x => (x.Date >= summaryDateRange.FromDate && x.Date <= summaryDateRange.ToDate) && x.Creative.AccountId == accountId)
                        .DeleteFromQuery();
                }, LockObject,
                "BulkDeleteByQuery");
            Logger.Info(accountId, $"The cleaning of creative summaries is over - {summaryDateRange.ToString()})");
        }

        private void LoadSummariesToDb(int accountId, IGrouping<int?, DbmCreativeSummary> summaryForAccount, DateRange summaryDateRange)
        {
            Logger.Info(accountId, $"Started loading of creative summaries has begun - {summaryDateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock(
                (ClientPortalProgContext db) => { db.BulkInsert(summaryForAccount); },
                LockObject,
                "BulkInsert");
            Logger.Info(accountId, $"The loading of creative summaries is over - {summaryDateRange.ToString()})");
        }

        private DateRange GetDateRangeForLoadingSummaries(IGrouping<int?, DbmCreativeSummary> summaryForAccount)
        {
            var startDate = summaryForAccount.Select(s => s.Date).Min();
            var endDate = summaryForAccount.Select(s => s.Date).Max();
            return new DateRange(startDate, endDate);
        }
    }
}