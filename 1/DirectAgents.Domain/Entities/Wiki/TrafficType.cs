using System.Collections.Generic;

namespace DirectAgents.Domain.Entities.Wiki
{
    public class TrafficType
    {
        public int TrafficTypeId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
