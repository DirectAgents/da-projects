using EomTool.Domain.DTOs;
using System.Linq;

namespace EomToolWeb.Models
{
    public class ChooseAmountsModel
    {
        public string AdvertiserName { get; set; }
        public IQueryable<CampaignAmount> CampaignAmounts { get; set; }
    }
}