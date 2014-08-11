using EomTool.Domain.DTOs;
using System.Collections.Generic;

namespace EomToolWeb.Models
{
    public class AffiliateAmountsModel
    {
        public string CurrentEomDateString { get; set; }
        public IEnumerable<CampaignAmount> CampaignAmounts { get; set; }
        public string Sort { get; set; }
    }
}