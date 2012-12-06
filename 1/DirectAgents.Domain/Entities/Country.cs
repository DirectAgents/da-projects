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
        public IEnumerable<Campaign> FilteredCampaigns(string[] excludeInName, bool excludeHidden, bool excludeInactive)
        {
            var campaigns = this.Campaigns.AsEnumerable();
            if (excludeInName != null)
            {
                foreach (string excludeString in excludeInName)
                {
                    campaigns = campaigns.Where(c => !c.Name.ToUpper().Contains(excludeString.ToUpper()));
                }
            }
            if (excludeHidden)
            {
                campaigns = campaigns.Where(c => !c.Hidden);
            }
            if (excludeInactive)
            {
                campaigns = campaigns.Where(c => c.StatusId != Status.Inactive);
            }
            return campaigns;
        }
    }
}
