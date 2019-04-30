using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowMaps
{
    public sealed class YamRowMap : CsvClassMap<YamRow>
    {
        public YamRowMap()
        {
            Map(x => x.Date).Name("Day");
            Map(x => x.Impressions).Name("Impressions");
            Map(x => x.Clicks).Name("Clicks");
            Map(x => x.ClickThroughConversion).Name("Click Through Conversion");
            Map(x => x.ViewThroughConversion).Name("View Through Conversion");
            Map(x => x.ConversionValue).Name("ROAS Action Value");
            Map(x => x.AdvertiserSpending).Name("Cost");

            Map(x => x.CampaignName).Name("Order");
            Map(x => x.CampaignId).Name("Order ID");
            Map(x => x.LineName).Name("Line");
            Map(x => x.LineId).Name("Line ID");
            Map(x => x.CreativeName).Name("Creative Name");
            Map(x => x.CreativeId).Name("Creative ID");
            Map(x => x.AdName).Name("Ad");
            Map(x => x.AdId).Name("Ad ID");
            Map(x => x.PixelName).Name("Pixel");
            Map(x => x.PixelId).Name("Pixel ID");
            Map(x => x.PixelParameter).Name("Pixel Query String");
        }
    }
}
