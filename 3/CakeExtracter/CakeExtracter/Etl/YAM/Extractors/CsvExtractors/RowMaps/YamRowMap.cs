using System.Configuration;
using System.Globalization;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowMaps
{
    public sealed class YamRowMap : CsvClassMap<YamRow>
    {
        private const NumberStyles DecimalNumberStyles = NumberStyles.Float;

        public YamRowMap()
        {
            Map(x => x.Date)
                .Name(GetMappingPropertyName("Day", "YAM_Map_Day"));
            Map(x => x.Impressions)
                .Name(GetMappingPropertyName("Impressions", "YAM_Map_Impressions"));
            Map(x => x.Clicks)
                .Name(GetMappingPropertyName("Clicks", "YAM_Map_Clicks"));
            Map(x => x.ClickThroughConversion)
                .Name(GetMappingPropertyName("Click Through Conversion", "YAM_Map_ClickThroughConversion"));
            Map(x => x.ViewThroughConversion)
                .Name(GetMappingPropertyName("View Through Conversion", "YAM_Map_ViewThroughConversion"));
            Map(x => x.ConversionValue)
                .Name(GetMappingPropertyName("ROAS Action Value", "YAM_Map_ConversionValue"))
                .TypeConverterOption(DecimalNumberStyles);
            Map(x => x.AdvertiserSpending)
                .Name(GetMappingPropertyName("Advertiser Spending", "YAM_Map_AdvertiserSpending"))
                .TypeConverterOption(DecimalNumberStyles);

            Map(x => x.CampaignName)
                .Name(GetMappingPropertyName("Order", "YAM_Map_CampaignName"));
            Map(x => x.CampaignId)
                .Name(GetMappingPropertyName("Order ID", "YAM_Map_CampaignId"));
            Map(x => x.LineName)
                .Name(GetMappingPropertyName("Line", "YAM_Map_LineName"));
            Map(x => x.LineId)
                .Name(GetMappingPropertyName("Line ID", "YAM_Map_LineId"));
            Map(x => x.CreativeName)
                .Name(GetMappingPropertyName("Creative Name", "YAM_Map_CreativeName"));
            Map(x => x.CreativeId)
                .Name(GetMappingPropertyName("Creative ID", "YAM_Map_CreativeId"));
            Map(x => x.AdName)
                .Name(GetMappingPropertyName("Ad", "YAM_Map_AdName"));
            Map(x => x.AdId)
                .Name(GetMappingPropertyName("Ad ID", "YAM_Map_AdId"));
            Map(x => x.PixelName)
                .Name(GetMappingPropertyName("Pixel", "YAM_Map_PixelName"));
            Map(x => x.PixelId)
                .Name(GetMappingPropertyName("Pixel ID", "YAM_Map_PixelId"));
            Map(x => x.PixelParameter)
                .Name(GetMappingPropertyName("Pixel Query String", "YAM_Map_PixelParameter"));
        }

        private static string GetMappingPropertyName(string sourcePropertyName, string configPropertyName)
        {
            var mapVal = ConfigurationManager.AppSettings[configPropertyName];
            return string.IsNullOrEmpty(mapVal) ? sourcePropertyName : mapVal;
        }
    }
}
