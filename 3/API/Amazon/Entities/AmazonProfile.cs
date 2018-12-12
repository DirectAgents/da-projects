namespace Amazon.Entities
{
    public class AmazonProfile
    {
        public string ProfileId { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string Timezone { get; set; }
        public AmazonAccountInfo AccountInfo { get; set; }
    }
}