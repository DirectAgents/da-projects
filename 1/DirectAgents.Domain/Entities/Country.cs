using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DirectAgents.Domain.Entities
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
