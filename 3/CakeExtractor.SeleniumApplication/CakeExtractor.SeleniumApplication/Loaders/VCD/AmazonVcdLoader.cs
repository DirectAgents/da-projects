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

        public AmazonVcdLoader()
        {
            metricTypes= new Dictionary<string, int>();
        }

        public void PrepareLoader()
        {
            EnsureMetricTypes();
        }

        public void LoadDailyData(VcdReportData reportData, DateTime date, ExtAccount extAccount)
        {
            try
            {
                var dbBrands = LoadBrandsData(reportData.Brands, date, extAccount);
                var dbCategories = LoadCategoriesData(reportData.Categories, date, extAccount, dbBrands);
                var dbSubcategories = LoadSubcategoriesData(reportData.Subcategories, date, extAccount, dbCategories, dbBrands);
                var dbParentProducts = LoadParentProductsData(reportData.ParentProducts, date, extAccount, dbBrands, dbCategories, dbSubcategories);
                var dbProducts = LoadProductsData(reportData.Products, date, extAccount,dbBrands, dbCategories, dbSubcategories, dbParentProducts);
            }
            catch (Exception ex)
            {
                Logger.Warn("Error occured while loading data for {0} account on {1} date", extAccount.Id, date);
                Logger.Error(ex);
                throw ex;
            }
        }

        private List<VendorBrand> LoadBrandsData(List<Brand> brands, DateTime date, ExtAccount extAccount)
        {
            var brandsSummaryLoader = new BrandsSummaryLoader(metricTypes);
            var dbBrands = brandsSummaryLoader.EnsureVendorEntitiesInDataBase(brands, extAccount);
            brandsSummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            brandsSummaryLoader.LoadNewAccountSummaryMetricsDataForDate(brands, dbBrands, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading brands data. Loaded metrics of {0} brands", dbBrands.Count);
            return dbBrands;
        }

        private List<VendorCategory> LoadCategoriesData(List<Category> categories, DateTime date, ExtAccount extAccount,
            List<VendorBrand> brands)
        {
            var categorySummaryLoader = new CategoriesSummaryLoader(metricTypes, brands);
            var dbCategories = categorySummaryLoader.EnsureVendorEntitiesInDataBase(categories, extAccount);
            categorySummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            categorySummaryLoader.LoadNewAccountSummaryMetricsDataForDate(categories, dbCategories, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading categories data. Loaded metrics of {0} categories", dbCategories.Count);
            return dbCategories;
        }

        private List<VendorSubcategory> LoadSubcategoriesData(List<Subcategory> subcategories, DateTime date, 
            ExtAccount extAccount, List<VendorCategory> dbCategories, List<VendorBrand> brands)
        {
            var subcategorySummaryLoader = new SubcategoriesSummaryLoader(metricTypes, dbCategories, brands);
            var dbSubcategories = subcategorySummaryLoader.EnsureVendorEntitiesInDataBase(subcategories, extAccount);
            subcategorySummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            subcategorySummaryLoader.LoadNewAccountSummaryMetricsDataForDate(subcategories, dbSubcategories, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading subcategories data. Loaded metrics of {0} subCategories", dbSubcategories.Count);
            return dbSubcategories;
        }

        private List<VendorParentProduct> LoadParentProductsData(List<ParentProduct> parentProducts, DateTime date,
            ExtAccount extAccount, List<VendorBrand> brands, List<VendorCategory> dbCategories, 
            List<VendorSubcategory> dbSubcategories)
        {
            var parentProductSummaryLoader = new ParentProductSummaryLoader(dbCategories, dbSubcategories, brands, metricTypes);
            var dbParentProducts = parentProductSummaryLoader.EnsureVendorEntitiesInDataBase(parentProducts, extAccount);
            parentProductSummaryLoader.CleanExistingAccountSummaryMetricsDataForDate(date, extAccount);
            parentProductSummaryLoader.LoadNewAccountSummaryMetricsDataForDate(parentProducts, dbParentProducts, date, extAccount);
            Logger.Info("Amazon VCD, Finished loading parent products data. Loaded metrics of {0} parent products", dbParentProducts.Count);
            return dbParentProducts;
        }

        private List<VendorProduct> LoadProductsData(List<Product> products, DateTime date,
            ExtAccount extAccount,List<VendorBrand> dbBrands, List<VendorCategory> dbCategories, List<VendorSubcategory> dbSubcategories, 
            List<VendorParentProduct> dbParentProducts)
        {
            var productSummaryLoader = new ProductsSummaryLoader(dbCategories, dbSubcategories,dbBrands, dbParentProducts, metricTypes);
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
