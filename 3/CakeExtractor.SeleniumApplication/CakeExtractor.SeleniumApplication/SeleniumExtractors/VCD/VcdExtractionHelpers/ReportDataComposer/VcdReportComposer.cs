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
            var mergedProducts = MergeShippedRevenueAndShippedCogsProductsdata(shippedRevenueProducts, shippedCogsProducts);
            return new VcdReportData
            {
                Products = mergedProducts,
                Categories = GetCategoriesFromProducts(mergedProducts),
                Subcategories = GetSubcategoriesFromProducts(mergedProducts),
                Brands = GetBrandsFromProducts(mergedProducts),
                ParentProducts = GetParentProductsFromProducts(mergedProducts)
            };
        }

        private List<Product> MergeShippedRevenueAndShippedCogsProductsdata(List<Product> shippedRevenueProducts,
            List<Product> shippedCogsProducts)
        {
            var resultList = new List<Product>();
            shippedRevenueProducts.ForEach(shippedRevProduct =>
            {
                var correspondingShippedCogProduct = shippedCogsProducts.FirstOrDefault(p => p.Asin == shippedRevProduct.Asin);
                if (correspondingShippedCogProduct != null)
                {
                    shippedRevProduct.ShippedCogs = correspondingShippedCogProduct.ShippedCogs;
                    shippedRevProduct.CustomerReturns = correspondingShippedCogProduct.CustomerReturns;
                    shippedRevProduct.FreeReplacements = correspondingShippedCogProduct.FreeReplacements;
                }
                resultList.Add(shippedRevProduct);
            });
            return resultList;
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
                    //properties
                    Asin = asin,
                    Name = productsWithDuplicatedAsin.First().Name,
                    ParentAsin = productsWithDuplicatedAsin.First().ParentAsin,
                    Category = productsWithDuplicatedAsin.First().Category,
                    Subcategory = productsWithDuplicatedAsin.First().Subcategory,
                    Ean = productsWithDuplicatedAsin.First().Ean,
                    Upc = productsWithDuplicatedAsin.First().Upc,
                    Brand = productsWithDuplicatedAsin.First().Brand,
                    ApparelSize = productsWithDuplicatedAsin.First().ApparelSize,
                    ApparelSizeWidth = productsWithDuplicatedAsin.First().ApparelSizeWidth,
                    Binding = productsWithDuplicatedAsin.First().Binding,
                    Color = productsWithDuplicatedAsin.First().Color,
                    ModelStyleNumber = productsWithDuplicatedAsin.First().ModelStyleNumber,
                    ReleaseDate = productsWithDuplicatedAsin.First().ReleaseDate,
                    //metrics
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

        private List<Brand> GetBrandsFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Brand, (key, gr) =>
                new Brand
                {
                    Name = key,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue),
                    CustomerReturns = gr.Sum(p => p.CustomerReturns),
                    FreeReplacements = gr.Sum(p => p.FreeReplacements),
                    ShippedCogs = gr.Sum(p => p.ShippedCogs),
                }
            ).ToList();
        }

        private List<ParentProduct> GetParentProductsFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.ParentAsin, (key, gr) =>
            {

                var firstProduct = gr.FirstOrDefault();
                var categoryName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Category)) ?
                    firstProduct.Category : null;
                var subcategoryName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Subcategory)) ?
                    firstProduct.Subcategory : null;
                var brandName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Brand)) ?
                   firstProduct.Brand : null;
                return new ParentProduct
                {
                    Asin = key,
                    Category =categoryName,
                    Subcategory = subcategoryName,
                    Brand = brandName,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue),
                    CustomerReturns = gr.Sum(p => p.CustomerReturns),
                    FreeReplacements = gr.Sum(p => p.FreeReplacements),
                    ShippedCogs = gr.Sum(p => p.ShippedCogs),
                };
            }).ToList();
        }

        private List<Category> GetCategoriesFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Category, (key, gr) =>
            {
                var firstProduct = gr.FirstOrDefault();
                var brandName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Brand)) ?
                    firstProduct.Brand : null;
                return new Category
                {
                    Name = key,
                    Brand = brandName,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue),
                    CustomerReturns = gr.Sum(p => p.CustomerReturns),
                    FreeReplacements = gr.Sum(p => p.FreeReplacements),
                    ShippedCogs = gr.Sum(p => p.ShippedCogs),
                };
            }).ToList();
        }

        private List<Subcategory> GetSubcategoriesFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Subcategory, (key, gr) =>
            {
                var firstProduct = gr.FirstOrDefault();
                var categoryName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Category)) ?
                    firstProduct.Category : null;
                var brandName = (firstProduct != null && !string.IsNullOrEmpty(firstProduct.Brand)) ?
                    firstProduct.Brand : null;
                return new Subcategory
                {
                    Name = key,
                    Category= categoryName,
                    Brand = brandName,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue),
                    CustomerReturns = gr.Sum(p => p.CustomerReturns),
                    FreeReplacements = gr.Sum(p => p.FreeReplacements),
                    ShippedCogs = gr.Sum(p => p.ShippedCogs),
                };
            }).ToList();
        }
    }
}
