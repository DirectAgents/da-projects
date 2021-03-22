using CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Extractors.VcdCustomReportsExtractionHelpers.ReportParsing.RowMaps
{
    /// <inheritdoc />
    /// <summary>
    /// Row map configuration for Geographic Sales Insights reports.
    /// </summary>
    internal sealed class GeographicSalesInsightsProductsRowMap : CsvClassMap<GeographicSalesInsightsProduct>
    {
        /// <inheritdoc cref="BaseProductRowMap"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicSalesInsightsProductsRowMap" /> class.
        /// </summary>
        public GeographicSalesInsightsProductsRowMap()
        {
            Map(m => m.Asin).Name("asin");
            Map(m => m.Region).Name("country");
            Map(m => m.State).Name("state");
            Map(m => m.City).Name("city");
            Map(m => m.Zip).Name("postalcode");
            Map(m => m.Author).Name("author");
            Map(m => m.Ean).Name("ean");
            Map(m => m.Upc).Name("upc");
            Map(m => m.Isbn13).Name("isbn13");
            Map(m => m.ParentAsin).Name("parentasin");
            Map(m => m.Brand).Name("brand");
            Map(m => m.ApparelSize).Name("apparelsize");
            Map(m => m.ApparelSizeWidth).Name("apparelsizewidth");
            Map(m => m.Binding).Name("binding");
            Map(m => m.Color).Name("color");
            Map(m => m.ModelStyleNumber).Name("modelstyle");
            Map(m => m.Name).Name("producttitle");
            Map(m => m.Category).Name("category");
            Map(m => m.Subcategory).Name("subcategory");

            Map(m => m.ShippedRevenue).Name("shippedrevenue").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.ShippedRevenuePriorPeriodPercentChange).Name("shippedrevenuepriorperiodpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.ShippedRevenuePriorYearPercentChange).Name("shippedrevenueprioryearpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.ShippedRevenuePercentOfTotal).Name("shippedrevenuepercentoftotal").TypeConverter<DecimalPercentageReportConverter>();

            Map(m => m.ShippedCogs).Name("shippedcogs").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.ShippedCogsPriorPeriodPercentChange).Name("shippedcogspriorperiodpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.ShippedCogsPriorYearPercentChange).Name("shippedcogsprioryearpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.ShippedCogsPercentOfTotal).Name("shippedcogspercentoftotal").TypeConverter<DecimalPercentageReportConverter>();

            Map(m => m.ShippedUnits).Name("shippedunits").TypeConverter<IntNumberReportConverter>();
            Map(m => m.ShippedUnitsPriorPeriodPercentChange).Name("shippedunitspriorperiodpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.ShippedUnitsPriorYearPercentChange).Name("shippedunitsprioryearpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.ShippedUnitsPercentOfTotal).Name("shippedunitspercentoftotal").TypeConverter<DecimalPercentageReportConverter>();

            Map(m => m.AverageShippedPrice).Name("averageshippedprice").TypeConverter<DecimalAmountReportConverter>();
            Map(m => m.AverageShippedPricePriorPeriodPercentChange).Name("averageshippedpricepriorperiodpercentchange").TypeConverter<DecimalPercentageReportConverter>();
            Map(m => m.AverageShippedPricePriorYearPercentChange).Name("averageshippedpriceprioryearpercentchange").TypeConverter<DecimalPercentageReportConverter>();

        }
    }
}
