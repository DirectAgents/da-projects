using System;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class AccountMaintenanceVM
    {
        public ExtAccount ExtAccount { get; set; }
        public bool Syncable { get; set; }

        public TDStatsGauge StatsGauge { get; set; }
    }
}