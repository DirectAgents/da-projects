using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class TDStatsVM
    {
        public string Name { get; set; }
        public IEnumerable<TDStat> Stats { get; set; }
        public TDStat StatsTotal
        {
            get
            {
                return new TDStat
                {
                    Name = "TOTAL",
                    Impressions = Stats.Sum(s => s.Impressions),
                    Clicks = Stats.Sum(s => s.Clicks),
                    PostClickConv = Stats.Sum(s => s.PostClickConv),
                    PostViewConv = Stats.Sum(s => s.PostViewConv),
                    Cost = Stats.Sum(s => s.Cost)
                };
            }
        }
    }
}