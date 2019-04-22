namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamLine
    {
        public int Id { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public int CampaignId { get; set; }

        public virtual YamCampaign Campaign { get; set; }
    }
}
