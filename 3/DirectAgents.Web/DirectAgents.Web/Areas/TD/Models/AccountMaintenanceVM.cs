using System;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class AccountMaintenanceVM
    {
        public ExtAccount ExtAccount { get; set; }
        public DateTime? LatestStatDate { get; set; }
        public bool Synchable { get; set; }
    }
}