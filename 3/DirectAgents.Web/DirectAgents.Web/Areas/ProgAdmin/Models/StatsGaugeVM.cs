using System.Collections.Generic;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class StatsGaugeVM
    {
        public string PlatformCode { get; set; }
        public int? CampaignId { get; set; }

        public IEnumerable<TDStatsGauge> StatsGauges { get; set; }
    }
}