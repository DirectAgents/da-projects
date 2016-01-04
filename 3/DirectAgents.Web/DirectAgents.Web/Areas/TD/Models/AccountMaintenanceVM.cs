using System;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class AccountMaintenanceVM
    {
        public ExtAccount ExtAccount { get; set; }
        public DateTime? LatestDailyStat { get; set; }
        public DateTime? LatestStrategyStat { get; set; }
        public bool Syncable { get; set; }
    }
}