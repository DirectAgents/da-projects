using ClientPortal.Data.Contexts;
using System;

namespace ClientPortal.Data.DTOs
{
    public class OfferInfo
    {
        public int OfferId { get; set; }
        public string AdvertiserId { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }

        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }
        public decimal Price
        {
            get { return (Conversions == 0) ? 0 : Math.Round(Revenue / Conversions, 2); }
        }
        public string Currency
        {
            set { Culture = CurrencyToCulture(value); }
        }
        public string Culture { get; set; }

        public static string CurrencyToCulture(string currency)
        {
            switch (currency)
            {
                case "EUR":
                    return "de-DE";
                case "GBP":
                    return "en-GB";
                case "AUD":
                    return "en-AU";
                default:
                    return "en-US";
            }
        }
    }
}
