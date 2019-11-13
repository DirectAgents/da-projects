using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    public class AdfLineItem : AdfBaseEntity
    {
        public int CampaignId { get; set; }

        public virtual AdfCampaign Campaign { get; set; }

        public virtual Collection<AdfBanner> Banners { get; set; }
    }
}
