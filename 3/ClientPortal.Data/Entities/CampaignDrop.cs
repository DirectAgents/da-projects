using ClientPortal.Data.Contexts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public partial class CampaignDrop
    {
        [NotMapped]
        public string DisplayName
        {
            get { return string.Format("{0} Aff: {1} Vol: {2}", Date.ToShortDateString(), Campaign.AffiliateId, Volume.HasValue ? Volume.Value.ToString("N0") : ""); }
        }

        [NotMapped]
        public decimal? OpenRate
        {
            get
            {
                if (Opens == null || Volume == null || Volume.Value == 0)
                    return null;
                else
                    return (decimal)Opens.Value / Volume.Value;
            }
        }

        private CreativeStat creativeStatTotals;
        [NotMapped]
        public CreativeStat CreativeStatTotals
        {
            get
            {
                if (creativeStatTotals == null)
                {
                    creativeStatTotals = new CreativeStat
                    {
                        Clicks = CreativeStats.Sum(cs => cs.Clicks ?? 0),
                        Leads = CreativeStats.Sum(cs => cs.Leads ?? 0)
                    };
                }
                return creativeStatTotals;
            }
        }

        [NotMapped]
        public decimal? ClickThroughRate
        {
            get
            {
                if (Opens == null || Opens.Value == 0)
                    return null;
                else
                    return (decimal)CreativeStatTotals.Clicks.Value / Opens.Value;
            }
        }

        [NotMapped]
        public decimal? CostPerLead
        {
            get
            {
                if (Cost == null || CreativeStatTotals.Leads == null || creativeStatTotals.Leads.Value == 0)
                    return null;
                else
                    return Cost.Value / CreativeStatTotals.Leads.Value;
            }
        }

    }
}
