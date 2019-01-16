using CakeExtracter;
using CakeExtractor.SeleniumApplication.Loaders.VCD.Constants;
using CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD
{
    internal class AmazonVcdLoader
    {
        private Dictionary<string, int> metricTypes;

        private CategoriesSummaryLoader categorySummaryLoader;

        public AmazonVcdLoader()
        {
            metricTypes= new Dictionary<string, int>();
            EnsureMetricTypes();
        }

        public void LoadDailyData(VcdReportData reportData, DateTime date, ExtAccount extAccount)
        {
            try
            {
                var dbCategories = LoadCategoriesData(reportData.Categories, date, extAccount);
                var dbSubcategories = LoadSubcategoriesData(reportData.Subcategories, date, extAccount, dbCategories);
                var dbProducts = LoadProductsData(reportData.Products, date, extAccount, dbCategories, dbSubcategories);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw ex;
            }
        }

        private List<VendorCategory> LoadCategoriesData(List<Category> categories, DateTime date, ExtAccount extAccount)
        {
            Logger.Info("Amazon VCD, Started categories data loading");
            categorySummaryLoader = new CategoriesSummaryLoader(metricTypes);
            var dbCategories = categorySummaryLoader.EnsureVendorEntitiesInDataBase(categories, extAccount);
            categorySummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            categorySummaryLoader.LoadNewAccountSummaryMetricsDataForDate(categories, dbCategories, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading categories data. Loaded metrics of {0} categories", dbCategories.Count);
            return dbCategories;
        }

        private List<VendorSubcategory> LoadSubcategoriesData(List<Subcategory> subcategories, DateTime date, 
            ExtAccount extAccount, List<VendorCategory> dbCategories)
        {
            Logger.Info("Amazon VCD, Started subcategories data loading");
            var subcategorySummaryLoader = new SubcategoriesSummaryLoader(dbCategories, metricTypes);
            var dbSubcategories = subcategorySummaryLoader.EnsureVendorEntitiesInDataBase(subcategories, extAccount);
            subcategorySummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            subcategorySummaryLoader.LoadNewAccountSummaryMetricsDataForDate(subcategories, dbSubcategories, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading subcategories data. Loaded metrics of {0} subCategoris", dbSubcategories.Count);
            return dbSubcategories;
        }

        private List<VendorProduct> LoadProductsData(List<Product> products, DateTime date,
            ExtAccount extAccount, List<VendorCategory> dbCategories, List<VendorSubcategory> dbSubcategories)
        {
            Logger.Info("Amazon VCD, Started products data loading");
            var productSummaryLoader = new ProductsSummaryLoader(dbCategories, dbSubcategories, metricTypes);
            var dbProducts = productSummaryLoader.EnsureVendorEntitiesInDataBase(products, extAccount);
            productSummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            productSummaryLoader.LoadNewAccountSummaryMetricsDataForDate(products, dbProducts, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading products data. Loaded metrics of {0} products", dbProducts.Count);
            return dbProducts;
        }

        private void EnsureMetricTypes()
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                VendorCentralDataLoadingConstants.VendorMetricTypeNames.ForEach(metricTypeName =>
                {
                    var metricType = dbContext.MetricTypes.FirstOrDefault(mt => mt.Name == metricTypeName);
                    if (metricType == null)
                    {
                        metricType = new MetricType(metricTypeName, VendorCentralDataLoadingConstants.VendorMetricsDaysInterval);
                        dbContext.MetricTypes.Add(metricType);
                        dbContext.SaveChanges();
                    }
                    metricTypes[metricTypeName] = metricType.Id;
                });
            }
        }
    }
}
