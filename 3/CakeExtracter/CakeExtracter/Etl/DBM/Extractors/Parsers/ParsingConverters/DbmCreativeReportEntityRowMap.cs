using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters
{
    /// <summary>DBM creative report csv convert rules.</summary>
    internal sealed class DbmCreativeReportEntityRowMap : CsvClassMap<DbmCreativeReportRow>
    {
        public DbmCreativeReportEntityRowMap()
        {
            Map(m => m.Date);
            Map(m => m.AdvertiserId).Name("Advertiser ID");
            Map(m => m.AdvertiserName).Name("Advertiser");
            Map(m => m.AdvertiserCurrencyCode).Name("Advertiser Currency");
            Map(m => m.CreativeId).Name("Creative ID");
            Map(m => m.CreativeName).Name("Creative");
            Map(m => m.CreativeHeight).Name("Creative Height");
            Map(m => m.CreativeSize).Name("Creative Size");
            Map(m => m.CreativeType).Name("Creative Type");
            Map(m => m.CreativeWidth).Name("Creative Width");
            Map(m => m.Revenue).Name("Revenue (USD)").TypeConverter<DecimalReportConverter>();
            Map(m => m.Impressions).ConvertUsing(row => (int)row.GetField<decimal>("Impressions"));
            Map(m => m.Clicks).ConvertUsing(row => (int)row.GetField<decimal>("Clicks"));
            Map(m => m.PostClickConversions).ConvertUsing(row => (int)row.GetField<float>("Post-Click Conversions"));
            Map(m => m.PostViewConversions).ConvertUsing(row => (int)row.GetField<float>("Post-View Conversions"));
            Map(m => m.CMPostClickRevenue).Name("CM Post-Click Revenue").TypeConverter<DecimalReportConverter>();
            Map(m => m.CMPostViewRevenue).Name("CM Post-Click Revenue").TypeConverter<DecimalReportConverter>();
        }
    }
}
