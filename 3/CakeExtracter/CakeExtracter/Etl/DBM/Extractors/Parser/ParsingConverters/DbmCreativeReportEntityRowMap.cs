using CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters;
using CsvHelper.Configuration;
using DBM.Parser.Models;

namespace CakeExtracter.Etl.DBM.Extractors.Parser.ParsingConverters
{
    /// <summary>DBM report csv convert rules.</summary>
    internal sealed class DbmCreativeReportEntityRowMap : CsvClassMap<DbmCreativeReportRow>
    {
        public DbmCreativeReportEntityRowMap()
        {
            Map(m => m.Date);
            Map(m => m.AdvertiserId).Name("Advertiser ID");
            Map(m => m.AdvertiserName).Name("Advertiser");
            Map(m => m.AdvertiserCurrency).Name("Advertiser Currency");
            Map(m => m.CampaignId).Name("Campaign ID");
            Map(m => m.CampaignName).Name("Campaign");
            Map(m => m.InsertionOrderId).Name("Insertion Order ID");
            Map(m => m.InsertionOrderName).Name("Insertion Order");
            Map(m => m.LineItemId).Name("Line Item ID");
            Map(m => m.LineItemName).Name("Line Item");
            Map(m => m.CreativeId).Name("Creative ID");
            Map(m => m.CreativeName).Name("Creative");
            Map(m => m.CreativeHeight).Name("Creative Height");
            Map(m => m.CreativeSize).Name("Creative Size");
            Map(m => m.CreativeType).Name("Creative Type");
            Map(m => m.CreativeWidth).Name("Creative Width");
            Map(m => m.Revenue).Name("Revenue (USD)").TypeConverter<DecimalReportConverter>();
            Map(m => m.Impressions).TypeConverter<DecimalReportConverter>();
            Map(m => m.Clicks).TypeConverter<DecimalReportConverter>();
            Map(m => m.PostClickConv).Name("Post-Click Conversions").TypeConverter<DecimalReportConverter>();
            Map(m => m.PostViewConv).Name("Post-View Conversions").TypeConverter<DecimalReportConverter>();
            Map(m => m.CMPostClickRevenue).Name("CM Post-Click Revenue").TypeConverter<DecimalReportConverter>();
            Map(m => m.CMPostViewRevenue).Name("CM Post-Click Revenue").TypeConverter<DecimalReportConverter>();
        }
    }
}
