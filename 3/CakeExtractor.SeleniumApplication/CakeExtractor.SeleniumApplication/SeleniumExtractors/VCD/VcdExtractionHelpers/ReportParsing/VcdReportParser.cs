using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing
{
    internal class VcdReportCSVParser
    {
        public VcdReportData ParseReportData(string reportCsvText)
        {
            var products = ParseProductsFromReport(reportCsvText);
            products = ProcessDuplicatedProducts(products);
            return new VcdReportData
            {
                Products = products,
                Categories = GetCategoriesFromProducts(products),
                Subcategories = GetSubcategoriesFromProducts(products)
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

        private List<Product> ParseProductsFromReport(string reportCsvText)
        {
            reportCsvText = RemoveFirstLines(reportCsvText);
            using (TextReader sr = new StringReader(reportCsvText))
            {
                var csvHelper = new CsvReader(sr);
                csvHelper.Configuration.SkipEmptyRecords = true;
                csvHelper.Configuration.RegisterClassMap<ProductRowMap>();
                var products = csvHelper.GetRecords<Product>().ToList();
                return products;
            }
        }

        private string RemoveFirstLines(string text)
        {
            var lines = Regex.Split(text, "\r\n|\r|\n").Skip(1);
            return string.Join(Environment.NewLine, lines.ToArray());
        }

        private sealed class ProductRowMap : CsvClassMap<Product>
        {
            public ProductRowMap()
            {
                Map(m => m.Asin).Name("ASIN");
                Map(m => m.Name).Name("Product Title");
                Map(m => m.Category).Name("Category");
                Map(m => m.Subcategory).Name("Subcategory");
                Map(m => m.ShippedRevenue).Name("Shipped Revenue").TypeConverter<ShippedRevenueConverter>();
                Map(m => m.ShippedUnits).Name("Shipped Units").TypeConverter<UnitsCountConverter>();
                Map(m => m.OrderedUnits).Name("Ordered Units").TypeConverter<UnitsCountConverter>();
            }
        }

        private sealed class ShippedRevenueConverter : StringConverter
        {
            public ShippedRevenueConverter()
            {
            }

            public override object ConvertFromString(TypeConverterOptions options, string text)
            {
                if (text == "—")
                {
                    return (decimal)0;
                }
                if (!string.IsNullOrEmpty(text) && text[0] == '$')
                    text = text.Remove(0, 1);
                decimal d = decimal.Parse(text, CultureInfo.InvariantCulture);
                return d;
            }
        }

        private sealed class UnitsCountConverter : StringConverter
        {
            public UnitsCountConverter()
            {
            }

            public override object ConvertFromString(TypeConverterOptions options, string text)
            {
                if (text == "—")
                {
                    return 0;
                }
                int count = int.Parse(text, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture); ;
                return count;
            }
        }
    }
}
