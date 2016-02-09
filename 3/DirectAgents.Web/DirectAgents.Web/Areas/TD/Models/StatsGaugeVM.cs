using System;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class StatsGaugeVM
    {
        public Platform Platform { get; set; }
        public ExtAccount ExtAccount { get; set; }

        public TDStatsGauge Gauge { get; set; }
    }
}