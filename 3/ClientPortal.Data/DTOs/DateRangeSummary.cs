using System;
using System.Globalization;

namespace ClientPortal.Data.DTOs
{
    public class DateRangeSummary
    {
        public string Name { get; set; }

        public int? Clicks { get; set; }
        public int? Conversions { get; set; }
        public decimal? Revenue { get; set; }

        public string RevenueFormatted
        {
            get { return String.Format(new CultureInfo(Culture), "{0:c}", Revenue); }
        }

        public string Currency
        {
            set { Culture = OfferInfo.CurrencyToCulture(value); }
        }
        public string Culture { get; set; }

        public string Link { get; set; }
    }
}
