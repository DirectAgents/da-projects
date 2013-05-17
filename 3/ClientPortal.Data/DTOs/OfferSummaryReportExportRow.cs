namespace ClientPortal.Data.DTOs
{
    public class OfferSummaryReportExportRow
    {
        public int OfferId { get; set; }
        public string AdvertiserId { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }
        public decimal Price { get; set; }
        public string Culture { get; set; }
    }
}
