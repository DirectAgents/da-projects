using System;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels
{
    public class YamRow
    {
        public DateTime Date { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int ClickThroughConversion { get; set; }

        public int ViewThroughConversion { get; set; }

        public decimal ConversionValue { get; set; }

        public decimal AdvertiserSpending { get; set; }
        
        public string PixelParameter { get; set; }

        public string CampaignName { get; set; }

        public int CampaignId { get; set; }

        public string LineName { get; set; }

        public int LineId { get; set; }

        public string CreativeName { get; set; }

        public int CreativeId { get; set; }

        public string AdName { get; set; }

        public int AdId { get; set; }

        public string PixelName { get; set; }

        public string PixelId { get; set; }
    }
}