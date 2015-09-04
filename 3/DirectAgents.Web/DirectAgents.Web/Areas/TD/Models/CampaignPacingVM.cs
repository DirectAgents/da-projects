using System.Collections.Generic;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class CampaignPacingVM
    {
        public IEnumerable<BudgetWithStats> CampaignStats { get; set; }
    }
}