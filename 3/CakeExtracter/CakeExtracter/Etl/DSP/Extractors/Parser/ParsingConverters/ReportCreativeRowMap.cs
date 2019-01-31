using CakeExtracter.Etl.DSP.Extractors.Parser.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters
{
    internal sealed class CreativeReportEntityRowMap : CsvClassMap<CreativeReportRow>
    {
        public CreativeReportEntityRowMap()
        {
            Map(m => m.Date);
            Map(m => m.AdvertiserId).Name("Advertiser ID");
            Map(m => m.AdvertiserName).Name("Advertiser");
            Map(m => m.OrderId).Name("Order ID");
            Map(m => m.OrderName).Name("Order Name");
            Map(m => m.LineItemId).Name("Line Item ID");
            Map(m => m.LineItemName).Name("Line Item");
            Map(m => m.CreativeId).Name("Creative ID");
            Map(m => m.CreativeName).Name("Creative");
            Map(m => m.TotalCost).Name("Total Cost").TypeConverter<DecimalReportConverter>();
            Map(m => m.Impressions).TypeConverter<DecimalReportConverter>();
            Map(m => m.ClickThroughs).Name("Clickthroughs").TypeConverter<DecimalReportConverter>();
            Map(m => m.DPV).Name("DPV").TypeConverter<DecimalReportConverter>();
            Map(m => m.ATC).Name("ATC").TypeConverter<DecimalReportConverter>();
            Map(m => m.Purchase).Name("Purchase").TypeConverter<DecimalReportConverter>();
            Map(m => m.PurchaseViews).Name("Purchase - Views").TypeConverter<DecimalReportConverter>();
            Map(m => m.PurchaseClicks).Name("Purchase - Clicks").TypeConverter<DecimalReportConverter>();
            Map(m => m.TotalPixelEvents).Name("Total Pixel Events").TypeConverter<DecimalReportConverter>();
            Map(m => m.TotalPixelEventsViews).Name("Total Pixel Events - Views").TypeConverter<DecimalReportConverter>();
            Map(m => m.TotalPixelEventsClicks).Name("Total Pixel Events - Clicks").TypeConverter<DecimalReportConverter>();
        }
    }
}
