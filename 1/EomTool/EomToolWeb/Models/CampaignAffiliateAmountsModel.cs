using EomTool.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace EomToolWeb.Models
{
    public class CampaignAffiliateAmountsModel
    {
        public string CurrentEomDateString { get; set; }
        public int? AdvertiserId { get; set; }
        public string AdvertiserName { get; set; }
        public IEnumerable<CampaignAmount> CampaignAmounts { get; set; }
        public int? CampaignStatusId { get; set; }
        public bool ShowCheckboxes { get; set; }

        // totals for a campaign
        public CampaignAmount SummaryAmount(int pid)
        {
            var pidAmounts = CampaignAmounts.Where(ca => ca.Pid == pid);

            string revCurr = "(mixed)";
            var revCurrs = pidAmounts.Select(a => a.RevenueCurrency).Distinct();
            if (revCurrs.Count() == 1)
                revCurr = revCurrs.First();

            string costCurr = "(mixed)";
            var costCurrs = pidAmounts.Select(a => a.CostCurrency).Distinct();
            if (costCurrs.Count() == 1)
                costCurr = costCurrs.First();

            var summaryAmount = new CampaignAmount()
            {
                Pid = pid,
                NumUnits = pidAmounts.Sum(a => a.NumUnits),
                RevenueCurrency = revCurr,
                Revenue = pidAmounts.Sum(a => a.Revenue),
                InvoicedAmount = pidAmounts.Sum(a => a.InvoicedAmount),
                CostCurrency = costCurr,
                Cost = pidAmounts.Sum(a => a.Cost)
            };
            return summaryAmount;
        }
    }
}