using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class Advertiser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public DateTime? StartDate { get; set; }

        public int? SalesRepId { get; set; }
        [ForeignKey("SalesRepId")]
        public virtual Employee SalesRep { get; set; }

        public int? AMId { get; set; }
        [ForeignKey("AMId")]
        public virtual Employee AM { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }

        public string SalesRepName()
        {
            return (SalesRep == null) ? null : SalesRep.FullName;
        }
        public string AMName()
        {
            return (AM == null) ? null : AM.FullName;
        }
    }
}
