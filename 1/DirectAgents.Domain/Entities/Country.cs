using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }

        [NotMapped]
        public IEnumerable<Campaign> ActiveCampaigns
        {
            get
            {
                return Campaigns.Where(c => c.StatusId != Status.Inactive);
            }
        }
    }
}
