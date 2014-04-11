using System.Collections.Generic;

namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class OfferSummaryReportResponse
    {
        public bool Success { get; set; }
        public int RowCount { get; set; }
        public List<OfferSummary> Offers { get; set; }
    }

    public class OfferSummary
    {
        public Offer1 Offer { get; set; }
    }

    public class Offer1
    {
        public int OfferId { get; set; }
    }
}
