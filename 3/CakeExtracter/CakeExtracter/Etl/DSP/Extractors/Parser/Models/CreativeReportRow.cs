using System;

namespace CakeExtracter.Etl.DSP.Extractors.Parser.Models
{
    internal class CreativeReportRow
    {
        public DateTime Date { get; set; }

        public string AdvertiserId { get; set; }

        public string AdvertiserName { get; set; }

        public string OrderId { get; set; }

        public string OrderName { get; set; }

        public string LineItemId { get; set; }

        public string LineItemName { get; set; }

        public string CreativeId { get; set; }

        public string CreativeName { get; set; }

        public decimal TotalCost { get; set; }

        public decimal Impressions { get; set; }

        public decimal ClickThroughs { get; set; }

        public decimal TotalPixelEvents { get; set; }

        public decimal TotalPixelEventsViews { get; set; }

        public decimal TotalPixelEventsClicks { get; set; }

        public decimal DPV { get; set; }

        public decimal ATC { get; set; }

        public decimal Purchase { get; set; }

        public decimal PurchaseViews { get; set; }

        public decimal PurchaseClicks { get; set; }
    }
}
