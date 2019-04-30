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
    /// Loader for DBM summaries of line item
    /// </summary>
    public class DbmLineItemSummaryLoader
    {
        private readonly DbmLineItemLoader lineItemLoader;
        private readonly DateRange dateRange;

        private static readonly object lockObject = new object();
        
        public DbmLineItemSummaryLoader(DateRange dateRange) 
        {
            var advertiserLoader = new DbmAdvertiserLoader();
            var campaignLoader = new DbmCampaignLoader(advertiserLoader);
            var insertionOrderLoader = new DbmInsertionOrderLoader(campaignLoader);
            lineItemLoader = new DbmLineItemLoader(insertionOrderLoader);
            this.dateRange = dateRange;
        }

        public void Load(IEnumerable<DbmLineItemSummary> summaries)
        {
            Logger.Info("Started loading line item summaries...");

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
            EnsureLineItemEntitiesData(summaryForAccount);
            DeleteOldSummariesFromDb(accountId);
            LoadSummariesToDb(summaryForAccount);
        }

        private void EnsureLineItemEntitiesData(IEnumerable<DbmLineItemSummary> summaries)
        {
            var lineItems = summaries.Select(summary => summary.LineItem).Where(summary => summary != null).ToList();
            lineItemLoader.AddUpdateDependentEntities(lineItems);
            summaries.ForEach(summary => { summary.EntityId = summary.LineItem.Id; });
        }

        private void DeleteOldSummariesFromDb(int accountId)
        {
            Logger.Info(accountId, $"Started cleaning of line item summaries has begun - {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.DbmLineItemSummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                && x.LineItem.AccountId == accountId).DeleteFromQuery();
            }, lockObject, "BulkDeleteByQuery");
            Logger.Info(accountId, $"The cleaning of line item summaries is over - {dateRange.ToString()})");
        }

        private void LoadSummariesToDb(IGrouping<int?, DbmLineItemSummary> summaryForAccount)
        {
            var accountId = Convert.ToInt32(summaryForAccount.Key);
            Logger.Info(accountId, $"Started loading of line item summaries has begun - {dateRange.ToString()}");
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaryForAccount);
            }, lockObject, "BulkInsert");
            Logger.Info(accountId, $"The loading of line item summaries is over - {dateRange.ToString()})");
        }
    }
}
