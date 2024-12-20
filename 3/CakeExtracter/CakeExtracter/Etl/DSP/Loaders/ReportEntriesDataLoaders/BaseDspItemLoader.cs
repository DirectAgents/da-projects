﻿using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    /// <summary>Base class for loading dsp report info into database</summary>
    /// <typeparam name="TReportEntity">The type of the report entity.</typeparam>
    /// <typeparam name="TDbEntity">The type of the database entity.</typeparam>
    /// <typeparam name="TMetricValues">The type of the metric values entity.</typeparam>
    internal abstract class BaseDspItemLoader<TReportEntity, TDbEntity, TMetricValues>
        where TReportEntity : DspReportEntity
        where TDbEntity : DspDbEntity
        where TMetricValues : DspMetricValues, new()
    {
        DspItemMetricManager metricManager;

        /// <summary>Initializes a new instance of the <see cref="BaseDspItemLoader{TReportEntity, TDbEntity, TMetricValues}"/> class.</summary>
        public BaseDspItemLoader()
        {
            metricManager = new DspItemMetricManager();
        }

        /// <summary>Ensures the DSP entities in data base.</summary>
        /// <param name="reportEntities">The report entities to propcess.</param>
        /// <param name="extAccount">The ext account entity.</param>
        /// <returns>List of ensured dsp entities from database</returns>
        public List<TDbEntity> EnsureDspEntitiesInDataBase(List<TReportEntity> reportEntities, ExtAccount extAccount)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<TDbEntity>();
                var allItems = new List<TDbEntity>();
                var dbSet = GetDspEntityDbSet(dbContext);
                var allAccountdbEntities = dbSet.Where(db => db.AccountId == extAccount.Id).ToList();
                var uniqueReportEntries = GetUniqueReportEntities(reportEntities);
                uniqueReportEntries.ForEach(reportEntity =>
                {
                    var correspondingDbEntity = allAccountdbEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, extAccount));
                    if (correspondingDbEntity == null)
                    {
                        correspondingDbEntity = MapReportEntityToDbEntity(reportEntity, extAccount);
                        itemsToBeAdded.Add(correspondingDbEntity);
                    }
                    allItems.Add(correspondingDbEntity);
                });
                dbSet.AddRange(itemsToBeAdded);
                dbContext.SaveChanges();
                return allItems;
            }
        }

        /// <summary>Updates the account summary metrics data for passed date.</summary>
        /// <param name="reportEntities">The report entities.</param>
        /// <param name="dbEntities">The database entities.</param>
        /// <param name="date">The date.</param>
        /// <param name="account">The account.</param>
        public void UpdateAccountSummaryMetricsDataForDate(List<TReportEntity> reportEntities,
            List<TDbEntity> dbEntities, DateTime date, ExtAccount account)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var accountRelatedEntityIds = dbEntities.Select(e => e.Id).ToList();
                var dbSet = GetMetricValuesDbSet(dbContext);
                var existingAccountDailySummaries = dbSet.Where(csm => csm.Date == date && accountRelatedEntityIds.Contains(csm.EntityId)).ToList();
                var actualAccountDailySummaries = GetActualDailySummariesFromReportEntities(reportEntities, dbEntities, account, date);
                MergeDailySummariesAndUpdateInDataBase(dbSet, dbContext, existingAccountDailySummaries, actualAccountDailySummaries);
            }
        }

        /// <summary>Gets the entity mapping predicate.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Predicate function for finding corresponding db entity by report entity</returns>
        protected virtual Func<TDbEntity, bool> GetEntityMappingPredicate(TReportEntity reportEntity, ExtAccount extAccount)
        {
            return dbEntity => dbEntity.ReportId == reportEntity.ReportId && dbEntity.AccountId == extAccount.Id;
        }

        /// <summary>Gets the unique report entities. Removes duplicated report entites.</summary>
        /// <param name="allReportEntities">All report entities.</param>
        /// <returns>Unique report entites</returns>
        protected virtual List<TReportEntity> GetUniqueReportEntities(List<TReportEntity> allReportEntities)
        {
            return allReportEntities.GroupBy(rep => rep.ReportId, (key, gr) => gr.First()).ToList();
        }

        /// <summary>Maps the report entity to database entity.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Initialised db entity based on report entity.</returns>
        protected abstract TDbEntity MapReportEntityToDbEntity(TReportEntity reportEntity, ExtAccount extAccount);

        /// <summary>Gets the DSP entity database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entities dbset</returns>
        protected abstract DbSet<TDbEntity> GetDspEntityDbSet(ClientPortalProgContext dbContext);

        /// <summary>Gets the metric values database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entites metric values dpbset</returns>
        protected abstract DbSet<TMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext);

        private void MergeDailySummariesAndUpdateInDataBase(DbSet<TMetricValues> dbSet, ClientPortalProgContext dbContext,
            List<TMetricValues> existingAccountDailySummaries, List<TMetricValues> actualAccountDailySummaries)
        {
            var dailySummariesToInsert = new List<TMetricValues>();
            var dailySummariesToLeaveUntouched = new List<TMetricValues>();
            actualAccountDailySummaries.ForEach(actualMetric =>
            {
                var alreadyExistingMetricValue = existingAccountDailySummaries.FirstOrDefault(mS => mS.EntityId == actualMetric.EntityId);
                if (alreadyExistingMetricValue != null)
                {
                    if (metricManager.AreMetricValuesEquivalent(alreadyExistingMetricValue, actualMetric))
                    {
                        dailySummariesToLeaveUntouched.Add(alreadyExistingMetricValue);
                    }
                    else
                    {
                        dailySummariesToInsert.Add(actualMetric);
                    }
                }
                else
                {
                    dailySummariesToInsert.Add(actualMetric);
                }
            });
            var dailySummariesToBeRemoved = existingAccountDailySummaries.Except(dailySummariesToLeaveUntouched).ToList();
            ProcessDataBaseMetricValuesUpdate(dbSet, dbContext, dailySummariesToBeRemoved, dailySummariesToInsert);
        }

        private void ProcessDataBaseMetricValuesUpdate(DbSet<TMetricValues> dbSet, ClientPortalProgContext dbContext,
           List<TMetricValues> dailySummariesToBeRemoved, List<TMetricValues> dailySummariesToInsert)
        {
            dbSet.RemoveRange(dailySummariesToBeRemoved);
            dbContext.SaveChanges();
            dbSet.AddRange(dailySummariesToInsert);
            dbContext.SaveChanges();
        }

        private List<TMetricValues> GetActualDailySummariesFromReportEntities(List<TReportEntity> reportShippingEntities, List<TDbEntity> dbVendorEntities, ExtAccount account, DateTime date)
        {
            var actualAccountDailySummaries = reportShippingEntities.Select(reportEntity =>
            {
                var dbEntity = dbVendorEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, account));
                return metricManager.GetMetricValuesEntities<TMetricValues>(reportEntity, dbEntity.Id, date);
            }).Where(mv => mv != null).ToList();
            return actualAccountDailySummaries;
        }
    }
}
