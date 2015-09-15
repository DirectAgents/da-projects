using System.Collections.Generic;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class CampaignPacingVM
    {
        public IEnumerable<TDStatWithBudget> CampaignBudgetStats { get; set; }
        public bool ShowPerfStats { get; set; }
    }
}