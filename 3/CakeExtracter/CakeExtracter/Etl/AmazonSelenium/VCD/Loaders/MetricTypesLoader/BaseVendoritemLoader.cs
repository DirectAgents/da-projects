using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.Constants;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.MetricTypesLoader
{
    internal abstract class BaseVendorItemLoader<TReportEntity, TDbEntity, TSummaryMetricEntity>
        where TReportEntity : ShippingItem
        where TDbEntity : BaseVendorEntity
        where TSummaryMetricEntity : SummaryMetric, new()
    {
        private const int BatchSize = 10000;

        protected Dictionary<string, int> metricTypes;

        protected BaseVendorItemLoader(Dictionary<string, int> metricTypes)
        {
            this.metricTypes = metricTypes;
        }

        public List<TDbEntity> EnsureVendorEntitiesInDataBase(List<TReportEntity> reportEntities, ExtAccount account)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var accountRelatedVendorEntities = dbContext.Set<TDbEntity>().Where(e => e.AccountId == account.Id).ToList();
                var reportDbEntities = new List<TDbEntity>();
                var itemsToBeAdded = new List<TDbEntity>();

                reportEntities.ForEach(reportEntity =>
                {
                    var correspondingDbEntity = accountRelatedVendorEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, account));
                    if (correspondingDbEntity == null)
                    {
                        correspondingDbEntity = MapReportEntityToDbEntity(reportEntity, account);
                        itemsToBeAdded.Add(correspondingDbEntity);
                    }
                    reportDbEntities.Add(correspondingDbEntity);
                });

                dbContext.BulkInsert(itemsToBeAdded, options => options.BatchSize = BatchSize);
                return reportDbEntities;
            }
        }

        public void UpdateAccountSummaryMetricsDataForDate(
            List<TReportEntity> reportShippingEntities,
            List<TDbEntity> accountRelatedVendorEntities,
            DateTime date,
            ExtAccount account)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var accountRelatedVendorEntityIds = accountRelatedVendorEntities.Select(e => e.Id).ToArray();
                var existingAccountDailySummaries = dbContext.Set<TSummaryMetricEntity>()
                    .Where(sums => sums.Date == date && accountRelatedVendorEntityIds.Contains(sums.EntityId))
                    .ToList();
                var actualAccountDailySummaries =
                    GetActualDailySummariesFromReportEntities(reportShippingEntities, accountRelatedVendorEntities, account, date);

                dbContext.BulkDelete(existingAccountDailySummaries, options => options.BatchSize = BatchSize);
                dbContext.BulkInsert(actualAccountDailySummaries, options => options.BatchSize = BatchSize);
                Logger.Info(account.Id, "Amazon VCD, Inserted {0}, Deleted {1}", actualAccountDailySummaries.Count, existingAccountDailySummaries.Count);
            }
        }

        protected virtual Func<TDbEntity, bool> GetEntityMappingPredicate(TReportEntity reportEntity, ExtAccount extAccount)
        {
            return dbEntity => dbEntity.Name == reportEntity.Name && dbEntity.AccountId == extAccount.Id;
        }

        protected abstract TDbEntity MapReportEntityToDbEntity(TReportEntity reportEntity, ExtAccount extAccount);

        private List<TSummaryMetricEntity> GetSummaryMetricEntities(TReportEntity reportEntity, TDbEntity dbEntity, DateTime date)
        {
            var metricEntities = new List<TSummaryMetricEntity>
            {
                InitMetricValue(dbEntity.Id, date, reportEntity.ShippedUnits, VendorCentralDataLoadingConstants.ShippedUnitsMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.OrderedUnits, VendorCentralDataLoadingConstants.OrderedUnitsMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.ShippedRevenue, VendorCentralDataLoadingConstants.ShippedRevenueMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.CustomerReturns, VendorCentralDataLoadingConstants.CustomerReturnsMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.FreeReplacements, VendorCentralDataLoadingConstants.FreeReplacementMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.ShippedCogs, VendorCentralDataLoadingConstants.ShippedCogsMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.OrderedRevenue, VendorCentralDataLoadingConstants.OrderedRevenueMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.LostBuyBox, VendorCentralDataLoadingConstants.LostBuyBoxMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.RepOos, VendorCentralDataLoadingConstants.RepOosMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.RepOosPercentOfTotal, VendorCentralDataLoadingConstants.RepOosPercentOfTotalMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.RepOosPriorPeriodPercentChange, VendorCentralDataLoadingConstants.RepOosPriorPeriodPercentChangeMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.GlanceViews, VendorCentralDataLoadingConstants.GlanceViewsMetricName),
                InitMetricValue(dbEntity.Id, date, reportEntity.SalesRank, VendorCentralDataLoadingConstants.SalesRank),
                InitMetricValue(dbEntity.Id, date, reportEntity.AverageSalesPrice, VendorCentralDataLoadingConstants.AverageSalesPrice),
                InitMetricValue(dbEntity.Id, date, reportEntity.SellableOnHandUnits, VendorCentralDataLoadingConstants.SellableOnHandUnits),
                InitMetricValue(dbEntity.Id, date, reportEntity.NumberOfCustomerReviews, VendorCentralDataLoadingConstants.NumberOfCustomerReviews),
                InitMetricValue(dbEntity.Id, date, reportEntity.NumberOfCustomerReviewsLifeToDate, VendorCentralDataLoadingConstants.NumberOfCustomerReviewsLifeToDate),
                InitMetricValue(dbEntity.Id, date, reportEntity.AverageCustomerRating, VendorCentralDataLoadingConstants.AverageCustomerRating),
                InitMetricValue(dbEntity.Id, date, reportEntity.FiveStars, VendorCentralDataLoadingConstants.FiveStars),
                InitMetricValue(dbEntity.Id, date, reportEntity.FourStars, VendorCentralDataLoadingConstants.FourStars),
                InitMetricValue(dbEntity.Id, date, reportEntity.ThreeStars, VendorCentralDataLoadingConstants.ThreeStars),
                InitMetricValue(dbEntity.Id, date, reportEntity.TwoStars, VendorCentralDataLoadingConstants.TwoStars),
                InitMetricValue(dbEntity.Id, date, reportEntity.OneStar, VendorCentralDataLoadingConstants.OneStar),
            };
            metricEntities.RemoveAll(item => item == null);
            return metricEntities;
        }

        private List<TSummaryMetricEntity> GetActualDailySummariesFromReportEntities(
            List<TReportEntity> reportShippingEntities,
            List<TDbEntity> dbVendorEntities,
            ExtAccount account,
            DateTime date)
        {
            var actualAccountDailySummaries = reportShippingEntities
                .SelectMany(reportEntity =>
                {
                    var dbEntity = dbVendorEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, account));
                    return GetSummaryMetricEntities(reportEntity, dbEntity, date);
                }).ToList();
            return actualAccountDailySummaries;
        }

        private TSummaryMetricEntity InitMetricValue(int entityId, DateTime date, decimal value, string metricType)
        {
            return VendorCentralDataLoadingConstants.AllowedZeroValueMetricTypeNames.Contains(metricType)
                ? InitMetricValue(entityId, date, value, metricTypes[metricType], false)
                : InitMetricValue(entityId, date, value, metricTypes[metricType]);
        }

        private TSummaryMetricEntity InitMetricValue(int entityId, DateTime date, decimal value, int metricTypeId, bool setZeroValueToNull = true)
        {
            const decimal emptyValueForComparison = -0.000001M;
            if ((setZeroValueToNull && value == 0) || (!setZeroValueToNull && value == emptyValueForComparison))
            {
                return null;
            }
            return new TSummaryMetricEntity
            {
                EntityId = entityId,
                MetricTypeId = metricTypeId,
                Date = date,
                Value = value,
            };
        }
    }
}