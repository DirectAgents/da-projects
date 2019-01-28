using CakeExtracter.Etl.DSP.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters
{
    internal sealed class CreativeReportentityRowMap : CsvClassMap<CreativeReportEntity>
    {
        public CreativeReportentityRowMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.AdvertiserId).Name("Advertiser ID");
            Map(m => m.AdvertiserName).Name("Advertiser");
            Map(m => m.OrderId).Name("Order ID");
            Map(m => m.OrderName).Name("Order Name");
            Map(m => m.LineItemId).Name("Line Item ID");
            Map(m => m.LineItemName).Name("Line Item");
            Map(m => m.CreativeId).Name("Creative ID");
            Map(m => m.CreativeName).Name("Creative");
            Map(m => m.TotalCost).Name("Total Cost");
            Map(m => m.Impressions).Name("Impressions");
            Map(m => m.ClickThroughs).Name("Clickthroughs");
            Map(m => m.TotalPixelEvents).Name("Total Pixel Events");
            Map(m => m.TotalPixelEventsViews).Name("Total Pixel Events - Views");
            Map(m => m.TotalPixelEventsClicks).Name("Total Pixel Events - Clicks");
            Map(m => m.DPV).Name("DPV");
            Map(m => m.ATC).Name("ATC");
            Map(m => m.PurchasesViews).Name("Purchase - Views");
            Map(m => m.PurchasesClicks).Name("Purchase - Clicks");
        }
    }
}
