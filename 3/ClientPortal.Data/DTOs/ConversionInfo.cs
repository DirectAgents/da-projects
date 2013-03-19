using System;

namespace ClientPortal.Data.DTOs
{
    public class ConversionInfo
    {
        public int ConversionId { get; set; }
        public DateTime Date { get; set; }
        public int AffId { get; set; }
        public int OfferId { get; set; }
        public string Offer { get; set; }

        public decimal PriceReceived { get; set; }
        public string Currency
        {
            set { Culture = OfferInfo.CurrencyToCulture(value); }
        }
        public string Culture { get; set; }

        public string TransactionId { get; set; }
        public bool? Positive { get; set; }
    }
}
