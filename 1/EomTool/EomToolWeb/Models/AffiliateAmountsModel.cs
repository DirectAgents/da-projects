using System.Collections.Generic;
using EomTool.Domain.DTOs;

namespace EomToolWeb.Models
{
    public class AffiliateAmountsModel
    {
        public string CurrentEomDateString { get; set; }
        public IEnumerable<CampaignAmount> CampaignAmounts { get; set; }
        public IEnumerable<CampAffItem> CampAffItems { get; set; }
        public string Sort { get; set; }
    }
}