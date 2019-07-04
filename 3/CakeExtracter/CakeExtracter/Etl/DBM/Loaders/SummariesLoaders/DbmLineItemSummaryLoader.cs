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
    /// Loader for DBM summaries of line item.
    /// </summary>
    public class DbmLineItemSummaryLoader
    {
        private readonly DbmLineItemLoader lineItemLoader;

        private static readonly object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="DbmLineItemSummaryLoader"/> class.
        /// </summary>
        public DbmLineItemSummaryLoader()
        {
            var advertiserLoader = new DbmAdvertiserLoader();
            var campaignLoader = new DbmCampaignLoader(advertiserLoader);
            var insertionOrderLoader = new DbmInsertionOrderLoader(campaignLoader);
            lineItemLoader = new DbmLineItemLoader(insertionOrderLoader);
        }

        /// <summary>
        /// Loads DBM LineItem summaries to Db.
        /// </summary>
        /// <param name="summaries">List of DBM LineItem summaries for loading.</param>
        public void Load(List<DbmLineItemSummary> summaries)
        {
            Logger.Info("Started loading line item summaries...");

            EnsureLineItemEntitiesData(summaries);
            var summaryGroups = GetSummariesGroupedByAccount(summaries);
            foreach (var summaryGroup in summaryGroups)
            {
                LoadSummariesForAccount(summaryGroup);
            }
            Logger.Info("Finished loading line item summaries.");
        }

        private static IEnumerable<IGrouping<int?, DbmLineItemSummary>> GetSummariesGroupedByAccount(
            IEnumerable<DbmLineItemSummary> summaries)
        {
            return summaries.GroupBy(summary => summary.LineItem.AccountId);
        }

        private void LoadSummariesForAccount(IGrouping<int?, DbmLineItemSummary> summaryForAccount)
        {
            var accountId = Convert.ToInt32(summaryForAccount.Key);
            var dateRange = GetDateRangeForLoadingSummaries(summaryForAccount);

            Logger.Info(accountId, "DBM ETL Line Item. Account (ID = {0})", accountId);

            DeleteOldSummariesFromDb(accountId, dateRange);
            LoadSummariesToDb(accountId, summaryForAccount, dateRange);

            Logger.Info(accountId, "Finished DBM ETL Line Item for account (ID = {0})", accountId);
        }

        private void EnsureLineItemEntitiesData(IEnumerable<DbmLineItemSummary> summaries)
        {
            var lineItems = summaries.Select(summary => summary.LineItem).Where(summary => summary != null).ToList();
            lineItemLoader.AddUpdateDependentEntities(lineItems);
            summaries.ForEach(summary => { summary.EntityId = summary.LineItem.Id; });
        }

        private void DeleteOldSummariesFromDb(int accountId, DateRange summaryDateRange)
        {
            Logger.Info(accountId, $"Started cleaning of line item summaries has begun - {summaryDateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock(
                (ClientPortalProgContext db) =>
                {
                    db.DbmLineItemSummaries
                        .Where(x => (x.Date >= summaryDateRange.FromDate && x.Date <= summaryDateRange.ToDate) && x.LineItem.AccountId == accountId)
                        .DeleteFromQuery();
                }, lockObject,
                "BulkDeleteByQuery");
            Logger.Info(accountId, $"The cleaning of line item summaries is over - {summaryDateRange.ToString()})");
        }

        private void LoadSummariesToDb(int accountId, IGrouping<int?, DbmLineItemSummary> summaryForAccount, DateRange summaryDateRange)
        {
            Logger.Info(accountId, $"Started loading of line item summaries has begun - {summaryDateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock(
                (ClientPortalProgContext db) => { db.BulkInsert(summaryForAccount); },
                lockObject,
                "BulkInsert");
            Logger.Info(accountId, $"The loading of line item summaries is over - {summaryDateRange.ToString()})");
        }

        private DateRange GetDateRangeForLoadingSummaries(IGrouping<int?, DbmLineItemSummary> summaryForAccount)
        {
            var startDate = summaryForAccount.Select(s => s.Date).Min();
            var endDate = summaryForAccount.Select(s => s.Date).Max();
            return new DateRange(startDate, endDate);
        }
    }
}
