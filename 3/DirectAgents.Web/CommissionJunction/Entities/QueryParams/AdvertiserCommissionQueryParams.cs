namespace CommissionJunction.Entities.QueryParams
{
    internal class AdvertiserCommissionQueryParams
    {
        public string AdvertiserId { get; set; }

        public string SinceDateTime { get; set; }

        public string BeforeDateTime { get; set; }

        public string SinceCommissionId { get; set; }
    }
}
