using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportDataComposer
{
    /// <summary>
    /// Composer for product data of reports (Shipped Revenue, Shipped COGS, Ordered Revenue).
    /// </summary>
    internal class VcdReportComposer
    {
        /// <summary>
        /// Composes separated data of reports to common data.
        /// </summary>
        /// <param name="shippedRevenueProducts">Data of report Shipped Revenue.</param>
        /// <param name="shippedCogsProducts">Data of report Shipped COGS.</param>
        /// <param name="orderedRevenueProducts">Data of report Ordered Revenue.</param>
        /// <returns>Common VCD report data.</returns>
        public VcdReportData ComposeReportData(
            List<Product> shippedRevenueProducts,
            List<Product> shippedCogsProducts,
            List<Product> orderedRevenueProducts)
        {
            shippedRevenueProducts = ProcessDuplicatedProducts(shippedRevenueProducts);
            shippedCogsProducts = ProcessDuplicatedProducts(shippedCogsProducts);
            orderedRevenueProducts = ProcessDuplicatedProducts(orderedRevenueProducts);
            var mergedProducts = MergeShippedRevenueAndShippedCogsProductsData(
                shippedRevenueProducts, shippedCogsProducts, orderedRevenueProducts);
            return new VcdReportData
            {
                Products = mergedProducts,
                Categories = GetCategoriesFromProducts(mergedProducts),
                Subcategories = GetSubcategoriesFromProducts(mergedProducts),
                Brands = GetBrandsFromProducts(mergedProducts),
                ParentProducts = GetParentProductsFromProducts(mergedProducts),
            };
        }

        // Replace all duplicated products with one product for each duplication group.
        // Summ metrics values of all items in duplication group
        private List<Product> ProcessDuplicatedProducts(List<Product> products)
        {
            var duplicatedAsins = GetDuplicatedAsins(products);
            duplicatedAsins.ForEach(asin =>
            {
                var productsWithDuplicatedAsin = products.Where(p => p.Asin == asin);
                products = products.Except(productsWithDuplicatedAsin).ToList();
                var summarizedProduct = GetProduct(asin, productsWithDuplicatedAsin);
                products.Add(summarizedProduct);
            });
            return products;
        }

        private List<string> GetDuplicatedAsins(IEnumerable<Product> products)
        {
            var productGroupsByAsin = products.GroupBy(p => p.Asin);
            var productsWithDuplicatedAsins = productGroupsByAsin.Where(g => g.Count() > 1);
            var duplicatedAsins = productsWithDuplicatedAsins.Select(y => y.Key).ToList();
            return duplicatedAsins;
        }

        private List<Product> MergeShippedRevenueAndShippedCogsProductsData(
            List<Product> shippedRevenueProducts, List<Product> shippedCogsProducts, List<Product> orderedRevenueProducts)
        {
            shippedRevenueProducts.ForEach(shippedRevProduct =>
            {
                AddShippedCogMetrics(shippedRevProduct, shippedCogsProducts);
                AddOrderedRevenueMetrics(shippedRevProduct, orderedRevenueProducts);
            });
            return shippedRevenueProducts.ToList();
        }

        private void AddShippedCogMetrics(Product product, IEnumerable<Product> shippedCogsProducts)
        {
            var correspondingShippedCogProduct = shippedCogsProducts.FirstOrDefault(p => p.Asin == product.Asin);
            if (correspondingShippedCogProduct == null)
            {
                return;
            }
            product.ShippedCogs = correspondingShippedCogProduct.ShippedCogs;
            product.CustomerReturns = correspondingShippedCogProduct.CustomerReturns;
            product.FreeReplacements = correspondingShippedCogProduct.FreeReplacements;
        }

        private void AddOrderedRevenueMetrics(Product product, IEnumerable<Product> orderedRevenueProducts)
        {
            var correspondingOrderedRevenueProduct = orderedRevenueProducts.FirstOrDefault(x => x.Asin == product.Asin);
            if (correspondingOrderedRevenueProduct != null)
            {
                product.OrderedRevenue = correspondingOrderedRevenueProduct.OrderedRevenue;
                product.GlanceViews = correspondingOrderedRevenueProduct.GlanceViews;
            }
        }

        private List<Brand> GetBrandsFromProducts(IEnumerable<Product> products)
        {
            return products.GroupBy(p => p.Brand, GetBrand).ToList();
        }

        private List<ParentProduct> GetParentProductsFromProducts(IEnumerable<Product> products)
        {
            return products.GroupBy(p => p.ParentAsin, GetParentProduct).ToList();
        }

        private List<Category> GetCategoriesFromProducts(IEnumerable<Product> products)
        {
            return products.GroupBy(p => p.Category, GetCategory).ToList();
        }

        private List<Subcategory> GetSubcategoriesFromProducts(IEnumerable<Product> products)
        {
            return products.GroupBy(p => p.Subcategory, GetSubcategory).ToList();
        }

        private Brand GetBrand(string name, IEnumerable<Product> products)
        {
            var item = new Brand
            {
                Name = name,
            };
            item.SetMetrics(products);
            return item;
        }

        private Category GetCategory(string name, IEnumerable<Product> products)
        {
            var firstProduct = products.FirstOrDefault();
            var item = new Category
            {
                Name = name,
                Brand = GetBrandName(firstProduct),
            };
            item.SetMetrics(products);
            return item;
        }

        private Subcategory GetSubcategory(string name, IEnumerable<Product> products)
        {
            var firstProduct = products.FirstOrDefault();
            var item = new Subcategory
            {
                Name = name,
                Category = GetCategoryName(firstProduct),
                Brand = GetBrandName(firstProduct),
            };
            item.SetMetrics(products);
            return item;
        }

        private ParentProduct GetParentProduct(string asin, IEnumerable<Product> products)
        {
            var firstProduct = products.FirstOrDefault();
            var item = new ParentProduct
            {
                Asin = asin,
                Category = GetCategoryName(firstProduct),
                Subcategory = GetSubcategoryName(firstProduct),
                Brand = GetBrandName(firstProduct),
            };
            item.SetMetrics(products);
            return item;
        }

        private Product GetProduct(string asin, IEnumerable<Product> products)
        {
            var firstProduct = products.FirstOrDefault();
            var item = new Product
            {
                Asin = asin,
                Name = firstProduct.Name,
                ParentAsin = firstProduct.ParentAsin,
                Category = firstProduct.Category,
                Subcategory = firstProduct.Subcategory,
                Ean = firstProduct.Ean,
                Brand = firstProduct.Brand,
                ApparelSize = firstProduct.ApparelSize,
                ApparelSizeWidth = firstProduct.ApparelSizeWidth,
                Binding = firstProduct.Binding,
                Color = firstProduct.Color,
                ModelStyleNumber = firstProduct.ModelStyleNumber,
                ReleaseDate = firstProduct.ReleaseDate,
                GlanceViews = firstProduct.GlanceViews,
            };
            item.SetMetrics(products);
            return item;
        }

        private string GetBrandName(Product product)
        {
            return product == null
                ? null
                : GetName(product.Brand);
        }

        private string GetCategoryName(Product product)
        {
            return product == null
                ? null
                : GetName(product.Category);
        }

        private string GetSubcategoryName(Product product)
        {
            return product == null
                ? null
                : GetName(product.Subcategory);
        }

        private string GetName(string sourceName)
        {
            return !string.IsNullOrEmpty(sourceName)
                ? sourceName
                : null;
        }
    }
}