using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDataComposer
{
    internal class VcdReportComposer
    {
        public VcdReportData ComposeReportData(List<Product> shippedRevenueProducts, List<Product> shippedCogsProducts)
        {
            shippedRevenueProducts = ProcessDuplicatedProducts(shippedRevenueProducts);
            shippedCogsProducts = ProcessDuplicatedProducts(shippedCogsProducts);
            return new VcdReportData
            {
                Products = shippedRevenueProducts,
                Categories = GetCategoriesFromProducts(shippedRevenueProducts),
                Subcategories = GetSubcategoriesFromProducts(shippedRevenueProducts)
            };
        }

        // Replace all duplicated products with one product for each duplication group. 
        // Summ metrics values of all items in duplication group
        private List<Product> ProcessDuplicatedProducts(List<Product> products)
        {
            var duplicatedAsins = products.GroupBy(p => p.Asin).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            duplicatedAsins.ForEach(asin =>
            {
                var productsWithDuplicatedAsin = products.Where(p => p.Asin == asin);
                products = products.Except(productsWithDuplicatedAsin).ToList();
                var summarizedProduct = new Product
                {
                    Asin = asin,
                    Name = productsWithDuplicatedAsin.First().Name,
                    Category = productsWithDuplicatedAsin.First().Category,
                    Subcategory = productsWithDuplicatedAsin.First().Subcategory,
                    OrderedUnits = productsWithDuplicatedAsin.Sum(p => p.OrderedUnits),
                    ShippedUnits = productsWithDuplicatedAsin.Sum(p => p.ShippedUnits),
                    ShippedRevenue = productsWithDuplicatedAsin.Sum(p => p.ShippedRevenue),
                    FreeReplacements = productsWithDuplicatedAsin.Sum(p => p.FreeReplacements),
                    ShippedCogs = productsWithDuplicatedAsin.Sum(p => p.ShippedCogs),
                    CustomerReturns = productsWithDuplicatedAsin.Sum(p => p.CustomerReturns),
                };
                products.Add(summarizedProduct);
            });
            return products;
        }

        private List<Category> GetCategoriesFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Category, (key, gr) =>
                new Category
                {
                    Name = key,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue)
                }
            ).ToList();
        }

        private List<Subcategory> GetSubcategoriesFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Subcategory, (key, gr) =>
            {
                var firstProduct = gr.FirstOrDefault();
                var categoryName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Category)) ?
                    firstProduct.Category : null;
                return new Subcategory
                {
                    Name = key,
                    CategoryName = categoryName,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue)
                };
            }).ToList();
        }
    }
}
