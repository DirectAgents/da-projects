using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.Cake;

namespace DirectAgents.Web.Models
{
    public class ScreensVM
    {
        public IEnumerable<Advertiser> Advertisers { get; set; }
        public StatsSummary Total
        {
            get
            {
                var stats = new StatsSummary
                {
                    Views = Advertisers.Sum(a => a.Stats.Views),
                    Clicks = Advertisers.Sum(a => a.Stats.Clicks),
                    Conversions = Advertisers.Sum(a => a.Stats.Conversions),
                    Paid = Advertisers.Sum(a => a.Stats.Paid),
                    Sellable = Advertisers.Sum(a => a.Stats.Sellable),
                    Revenue = Advertisers.Sum(a => a.Stats.Revenue),
                    Cost = Advertisers.Sum(a => a.Stats.Cost)
                };
                return stats;
            }
        }
    }
}