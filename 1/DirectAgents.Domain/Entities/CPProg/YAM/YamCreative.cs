using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamCreative : BaseYamEntity
    {
        public int AccountId { get; set; }

        public virtual ExtAccount Account { get; set; }

        public virtual Collection<YamAd> Ads { get; set; }
    }
}
