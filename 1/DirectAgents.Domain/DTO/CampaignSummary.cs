using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectAgents.Domain.DTO
{
    public class CampaignSummary
    {
        public int Pid { get; set; }

        public decimal Revenue { get; set; }

        public string CampaignName { get; set; }
    }
}
