using System.Collections.Generic;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Models
{
    public class DashboardVM
    {
        public ABClient ABClient { get; set; }
        public IEnumerable<ABStat> ABStats { get; set; }
    }
}