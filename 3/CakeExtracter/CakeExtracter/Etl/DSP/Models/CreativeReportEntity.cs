using System;

namespace CakeExtracter.Etl.DSP.Models
{
    internal class CreativeReportEntity
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

        public decimal Purchases { get; set; }

        public decimal PurchasesViews { get; set; }

        public decimal PurchasesClicks { get; set; }        
    }
}
