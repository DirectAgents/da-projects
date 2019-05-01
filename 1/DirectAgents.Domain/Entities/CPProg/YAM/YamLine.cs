using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamLine : BaseYamEntity
    {
        public int CampaignId { get; set; }

        public virtual YamCampaign Campaign { get; set; }

        public virtual Collection<YamAd> Ads { get; set; }
    }
}
