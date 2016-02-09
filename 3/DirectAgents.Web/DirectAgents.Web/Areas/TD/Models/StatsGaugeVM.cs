using System;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class StatsGaugeVM
    {
        public Platform Platform { get; set; }
        public ExtAccount ExtAccount { get; set; }

        public DateTime? EarliestDaily { get; set; }
        public DateTime? EarliestStrat { get; set; }
        public DateTime? EarliestCreat { get; set; }
        public DateTime? EarliestSite { get; set; }
        public DateTime? EarliestConv { get; set; }

        public DateTime? LatestDaily { get; set; }
        public DateTime? LatestStrat { get; set; }
        public DateTime? LatestCreat { get; set; }
        public DateTime? LatestSite { get; set; }
        public DateTime? LatestConv { get; set; }
    }
}