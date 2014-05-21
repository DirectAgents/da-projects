using System.Collections.Generic;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.Wiki;

namespace EomToolWeb.Models
{
    public class TopViewModel
    {
        public IEnumerable<CampaignSummary> CampaignSummaries { get; set; }
        public TopCampaignsBy By { get; set; }
        public TrafficType TrafficType { get; set; }
    }
}