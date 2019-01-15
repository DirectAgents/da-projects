using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal abstract class BaseVendorItemLoader<TReportEntity, TDbEntity, TSummaryMetricEntity> 
        where TReportEntity : ShippingItem
        where TDbEntity : BaseVendorEntity
        where TSummaryMetricEntity : SummaryMetric
    {
        public List<TDbEntity> EnsureVendorEntitiesInDataBase(List<TReportEntity> reportEntities, ExtAccount extAccount)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var itemsToBeAdded = new List<TDbEntity>();
                var allItems = new List<TDbEntity>();
                var dbSet = GetVendorDbSet(dbContext);
                reportEntities.ForEach(reportEntity =>
                {
                    var correspondingDbEntity = dbSet
                       .FirstOrDefault(c => c.Name == reportEntity.Name && c.AccountId == extAccount.Id);
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

        //!!!! Probably need add account id to summary metrics data
        public void UpdateSummaryMetricData(List<TReportEntity> reportShippingEntities,
            List<TDbEntity> dbVendorEntities, DateTime date, Dictionary<string, MetricType> metricTypesDictionary)
        {
            CleanExistingSummaryMetricsDataForDate(date);
            using (var dbContext = new ClientPortalProgContext())
            {
                var dbSet = GetSummaryMetricDbSet(dbContext);
                var summaryMetricsToAdd = reportShippingEntities.SelectMany(reportEntity =>
                {
                    var dbEntity = dbVendorEntities.Find(dbe => dbe.Name == reportEntity.Name);
                    return GetSummaryMetricEntities(reportEntity, dbEntity, date, metricTypesDictionary);
                }).ToList();
                dbSet.AddRange(summaryMetricsToAdd);
                dbContext.SaveChanges();
            }
        }

        //!!!!! Probably need remove Summarymetrics only for processing Account !!!
        private void CleanExistingSummaryMetricsDataForDate(DateTime date)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var dbSet = GetSummaryMetricDbSet(dbContext);
                var itemsToBeRemoved = dbSet.Where(csm => csm.Date == date).ToList();
                dbSet.RemoveRange(itemsToBeRemoved);
                dbContext.SaveChanges();
            }
        }

        protected abstract List<TSummaryMetricEntity> GetSummaryMetricEntities(TReportEntity reportEntity, TDbEntity dbEntity, DateTime date, Dictionary<string, MetricType> metricTypesDictionary);

        protected abstract TDbEntity MapReportEntityToDbEntity(TReportEntity reportEntity, ExtAccount extAccount);

        protected abstract DbSet<TDbEntity> GetVendorDbSet(ClientPortalProgContext dbContext);

        protected abstract DbSet<TSummaryMetricEntity> GetSummaryMetricDbSet(ClientPortalProgContext dbContext);
    }
}
