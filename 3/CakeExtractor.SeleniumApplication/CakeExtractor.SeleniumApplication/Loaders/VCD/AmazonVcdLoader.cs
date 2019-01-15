using CakeExtracter.Helpers;
using CakeExtractor.SeleniumApplication.Loaders.VCD.Constants;
using CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD
{
    internal class AmazonVcdLoader
    {
        private Dictionary<string, MetricType> metricTypesDictionary;

        private CategoriesSummaryLoader categorySummaryLoader;

        public AmazonVcdLoader()
        {
            metricTypesDictionary = new Dictionary<string, MetricType>();
            EnsureMetricTypes();
        }

        public void LoadDailyVendorCentralData(VcdReportData reportData, DateTime date, ExtAccount extAccount)
        {
            //upload categories data
            categorySummaryLoader = new CategoriesSummaryLoader();
            var dbReportCategories = categorySummaryLoader.EnsureVendorEntitiesInDataBase(reportData.Categories, extAccount);
            categorySummaryLoader.UpdateSummaryMetricData(reportData.Categories, dbReportCategories, date, metricTypesDictionary);

            //upload subcategories data
            var subcategorySummaryLoader = new SubcategoriesSummaryLoader(dbReportCategories);
            var dbSubcategories = subcategorySummaryLoader.EnsureVendorEntitiesInDataBase(reportData.Subcategories, extAccount);
            subcategorySummaryLoader.UpdateSummaryMetricData(reportData.Subcategories, dbSubcategories, date, metricTypesDictionary);

            //upload producs data
            var productSummaryLoader = new ProductsSummaryLoader(dbReportCategories, dbSubcategories);
            var dbProducts = productSummaryLoader.EnsureVendorEntitiesInDataBase(reportData.Products, extAccount);
            productSummaryLoader.UpdateSummaryMetricData(reportData.Products, dbProducts, date, metricTypesDictionary);
        }

        private void EnsureMetricTypes()
        {
            SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
            SafeContextWrapper.MetricTypeLocker, db =>
            {
                VendorCentralDataLoadingConstants.VendorMetricTypeNames.ForEach(metricTypeName =>
                {
                    var metricType = db.MetricTypes.FirstOrDefault(mt => mt.Name == metricTypeName);
                    if (metricType == null)
                    {
                        metricType = new MetricType(metricTypeName, VendorCentralDataLoadingConstants.VendorMetricsDaysInterval);
                        db.MetricTypes.Add(metricType);
                    }
                    metricTypesDictionary[metricTypeName] = metricType;
                });
            });
        }
    }
}
