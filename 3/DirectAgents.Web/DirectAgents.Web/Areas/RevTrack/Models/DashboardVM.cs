using System.Collections.Generic;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Web.Areas.RevTrack.Models
{
    public class DashboardVM
    {
        public IEnumerable<ABStat> ABStats { get; set; }
    }
}