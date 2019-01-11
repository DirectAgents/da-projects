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
        public static VcdReportData GetReportData(string reportCsvText)
        {
            var products = ParseProductsFromReport(reportCsvText);
            return new VcdReportData
            {
                Products = products,
                Categories = GetCategoriesFromProducts(products),
                Subcategories = GetSubcategoriesFromProducts(products)
            };
        }

        private static List<Category> GetCategoriesFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Category, (key, gr) =>
                new Category
                {
                    Title = key,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue)
                }
            ).ToList();
        }

        private static List<Subcategory> GetSubcategoriesFromProducts(List<Product> products)
        {
            return products.GroupBy(p => p.Subcategory, (key, gr) =>
                new Subcategory
                {
                    Title = key,
                    OrderedUnits = gr.Sum(p => p.OrderedUnits),
                    ShippedUnits = gr.Sum(p => p.ShippedUnits),
                    ShippedRevenue = gr.Sum(p => p.ShippedRevenue)
                }
            ).ToList();
}

        private static List<Product> ParseProductsFromReport(string reportCsvText)
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

        private sealed class ProductRowMap : CsvClassMap<Product>
        {
            public ProductRowMap()
            {
                Map(m => m.Asin).Name("ASIN");
                Map(m => m.Title).Name("Product Title");
                Map(m => m.Category).Name("Category");
                Map(m => m.Subcategory).Name("Subcategory");
                Map(m => m.ShippedRevenue).Name("Shipped Revenue").TypeConverter<ShippedRevenueConverter>();
                Map(m => m.ShippedUnits).Name("Shipped Units");
                Map(m => m.OrderedUnits).Name("Ordered Units");
            }
        }

        private sealed class ShippedRevenueConverter : StringConverter
        {
            public ShippedRevenueConverter()
            {
            }

            public override object ConvertFromString(TypeConverterOptions options, string text)
            {
                if (!string.IsNullOrEmpty(text) && text[0] == '$')
                    text = text.Remove(0, 1);
                decimal d = decimal.Parse(text, CultureInfo.InvariantCulture);
                return d;
            }
        }

        private static string RemoveFirstLines(string text)
        {
            var lines = Regex.Split(text, "\r\n|\r|\n").Skip(1);
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
