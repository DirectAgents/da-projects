using System;

namespace ClientPortal.Data.DTOs
{
    public class ConversionSummary
    {
        public string AdvertiserId { get; set; }
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public string Format { get; set; }

        public int Count { get; set; }
        public int PositiveCount { get; set; }
        public decimal Revenue { get; set; }

        public string Currency
        {
            set { Culture = OfferInfo.CurrencyToCulture(value); }
        }
        public string Culture { get; set; }
    }
}
