using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

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
                    Conversions = Stats.Sum(s => s.Conversions),
                    Cost = Stats.Sum(s => s.Cost)
                };
            }
        }
    }
}