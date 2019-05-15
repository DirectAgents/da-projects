namespace Taboola.Entities.ResponseEntities
{
    internal class TaboolaAccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public string[] PartnerTypes { get; set; }
        public string Type { get; set; }
        public string[] CampaignTypes { get; set; }
    }
}
