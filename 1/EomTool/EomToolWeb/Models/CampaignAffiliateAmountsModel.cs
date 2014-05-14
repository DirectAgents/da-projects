using EomTool.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace EomToolWeb.Models
{
    public class CampaignAffiliateAmountsModel
    {
        public int? AdvertiserId { get; set; }
        public string AdvertiserName { get; set; }
        public IEnumerable<CampaignAmount> CampaignAmounts { get; set; }
        public int? CampaignStatusId { get; set; }
        public bool ShowCheckboxes { get; set; }

        // totals for a campaign
        public CampaignAmount SummaryAmount(int pid)
        {
            var pidAmounts = CampaignAmounts.Where(ca => ca.Pid == pid);

            string currency = "(mixed)";
            var currencies = pidAmounts.Select(a => a.RevenueCurrency).Distinct();
            if (currencies.Count() == 1)
                currency = currencies.First();

            var summaryAmount = new CampaignAmount()
            {
                Pid = pid,
                NumUnits = pidAmounts.Sum(a => a.NumUnits),
                RevenueCurrency = currency,
                Revenue = pidAmounts.Sum(a => a.Revenue),
                InvoicedAmount = pidAmounts.Sum(a => a.InvoicedAmount)
            };
            return summaryAmount;
        }
    }
}