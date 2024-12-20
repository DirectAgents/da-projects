﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.Constants;
using CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.MetricTypesLoader;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using CakeExtracter.Logging.TimeWatchers.Amazon;

using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Loaders
{
    /// <summary>
    /// Loader for Amazon Vendor Central statistics.
    /// </summary>
    internal class AmazonVcdLoader : Loader<VcdReportData>
    {
        private const int LoaderBatchSize = 1;

        private static readonly Dictionary<string, int> MetricTypes;
        private readonly ExtAccount extAccount;

        static AmazonVcdLoader()
        {
            MetricTypes = new Dictionary<string, int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonVcdLoader"/> class.
        /// </summary>
        /// <param name="extAccount">Current account.</param>
        public AmazonVcdLoader(ExtAccount extAccount)
        {
            this.extAccount = extAccount;
            BatchSize = LoaderBatchSize;
        }

        /// <summary>
        /// Preparing loader.
        /// </summary>
        public static void PrepareLoader()
        {
            EnsureMetricTypes();
        }

        /// <summary>
        /// Loads Amazon Vendor Central statistics.
        /// </summary>
        /// <param name="items">List of Amazon Vendor Central stats.</param>
        /// <returns>Count of loading items.</returns>
        protected override int Load(List<VcdReportData> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                    {
                        foreach (var item in items)
                        {
                            try
                            {
                                LoadDailyData(item);
                            }
                            catch (Exception e)
                            {
                                Logger.Error(
                                    extAccount.Id,
                                    new Exception($"Could not load data for date {item.Date}. Details: {e}", e));
                            }
                        }
                    },
                extAccount.Id,
                "VCD Data Loader",
                AmazonJobOperations.LoadSummaryItemsData);
            return items.Count;
        }

        private static void EnsureMetricTypes()
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
                    MetricTypes[metricTypeName] = metricType.Id;
                });
            }
        }

        private void LoadDailyData(VcdReportData reportData)
        {
            var date = reportData.Date;
            var dbBrands = LoadBrandsData(reportData.Brands, date);
            var dbCategories = LoadCategoriesData(reportData.Categories, date, dbBrands);
            var dbSubcategories = LoadSubcategoriesData(reportData.Subcategories, date, dbCategories, dbBrands);
            var dbParentProducts = LoadParentProductsData(reportData.ParentProducts, date, dbBrands, dbCategories, dbSubcategories);
            var dbProducts = LoadProductsData(reportData.Products, date, dbBrands, dbCategories, dbSubcategories, dbParentProducts);
        }

        private List<VendorBrand> LoadBrandsData(List<Brand> brands, DateTime date)
        {
            var brandsSummaryLoader = new BrandsSummaryLoader(MetricTypes);
            var dbBrands = brandsSummaryLoader.EnsureVendorEntitiesInDataBase(brands, extAccount);
            brandsSummaryLoader.UpdateAccountSummaryMetricsDataForDate(brands, dbBrands, date, extAccount);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading brands data. Loaded metrics of {0} brands", dbBrands.Count);
            return dbBrands;
        }

        private List<VendorCategory> LoadCategoriesData(List<Category> categories, DateTime date, List<VendorBrand> brands)
        {
            var categorySummaryLoader = new CategoriesSummaryLoader(MetricTypes, brands);
            var dbCategories = categorySummaryLoader.EnsureVendorEntitiesInDataBase(categories, extAccount);
            categorySummaryLoader.UpdateAccountSummaryMetricsDataForDate(categories, dbCategories, date, extAccount);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading categories data. Loaded metrics of {0} categories", dbCategories.Count);
            return dbCategories;
        }

        private List<VendorSubcategory> LoadSubcategoriesData(
            List<Subcategory> subcategories,
            DateTime date,
            List<VendorCategory> dbCategories,
            List<VendorBrand> brands)
        {
            var subcategorySummaryLoader = new SubcategoriesSummaryLoader(MetricTypes, dbCategories, brands);
            var dbSubcategories = subcategorySummaryLoader.EnsureVendorEntitiesInDataBase(subcategories, extAccount);
            subcategorySummaryLoader.UpdateAccountSummaryMetricsDataForDate(subcategories, dbSubcategories, date, extAccount);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading subcategories data. Loaded metrics of {0} subCategories", dbSubcategories.Count);
            return dbSubcategories;
        }

        private List<VendorParentProduct> LoadParentProductsData(
            List<ParentProduct> parentProducts,
            DateTime date,
            List<VendorBrand> brands,
            List<VendorCategory> dbCategories,
            List<VendorSubcategory> dbSubcategories)
        {
            var parentProductSummaryLoader = new ParentProductSummaryLoader(dbCategories, dbSubcategories, brands, MetricTypes);
            var dbParentProducts = parentProductSummaryLoader.EnsureVendorEntitiesInDataBase(parentProducts, extAccount);
            parentProductSummaryLoader.UpdateAccountSummaryMetricsDataForDate(parentProducts, dbParentProducts, date, extAccount);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading parent products data. Loaded metrics of {0} parent products", dbParentProducts.Count);
            return dbParentProducts;
        }

        private List<VendorProduct> LoadProductsData(
            List<Product> products,
            DateTime date,
            List<VendorBrand> dbBrands,
            List<VendorCategory> dbCategories,
            List<VendorSubcategory> dbSubcategories,
            List<VendorParentProduct> dbParentProducts)
        {
            var productSummaryLoader = new ProductsSummaryLoader(dbCategories, dbSubcategories, dbBrands, dbParentProducts, MetricTypes);
            var dbProducts = productSummaryLoader.EnsureVendorEntitiesInDataBase(products, extAccount);
            productSummaryLoader.UpdateAccountSummaryMetricsDataForDate(products, dbProducts, date, extAccount);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading products data. Loaded metrics of {0} products", dbProducts.Count);
            return dbProducts;
        }
    }
}