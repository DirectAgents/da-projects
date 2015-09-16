using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class Advertiser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? SalesRepId { get; set; }
        [ForeignKey("SalesRepId")]
        public Employee SalesRep { get; set; }

        public int? AMId { get; set; }
        [ForeignKey("AMId")]
        public Employee AM { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
