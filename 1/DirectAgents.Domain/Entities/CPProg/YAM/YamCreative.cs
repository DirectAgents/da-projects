using System.Collections.ObjectModel;

namespace DirectAgents.Domain.Entities.CPProg.YAM
{
    public class YamCreative
    {
        public int Id { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public int AccountId { get; set; }

        public virtual ExtAccount Account { get; set; }

        public virtual Collection<YamAd> Ads { get; set; }
    }
}
