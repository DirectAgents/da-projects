using EomTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomTool.Domain.DTOs
{
    public class CampaignAmount
    {
        public int AdvId { get; set; }
        public string AdvertiserName { get; set; }

        public int Pid { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDisplayName { get; set; }

        public int? AffId { get; set; }
        public string AffiliateName { get; set; }

        public Currency RevenueCurrency { get; set; }
        public decimal Revenue { get; set; }

        public decimal InvoicedAmount { get; set; }

        public Currency CostCurrency { get; set; }
        public decimal Cost { get; set; }

        public decimal? Margin
        {
            get
            {
                if (RevenueCurrency.id == CostCurrency.id)
                    return Revenue - Cost;
                else
                    return null;
            }
        }
        public decimal? MarginPct
        {
            get
            {
                if (Revenue == 0)
                    return null;
                else
                {
                    var revUSD = Revenue * RevenueCurrency.to_usd_multiplier;
                    var costUSD = Cost * CostCurrency.to_usd_multiplier;
                    return (1 - costUSD / revUSD);
                }
            }
        }

        public int NumUnits { get; set; }
        public int NumAffs { get; set; }
        public UnitType UnitType { get; set; }
        public IEnumerable<int> ItemIds { get; set; }

        public string ItemIdsString
        {
            get { return String.Join(",", ItemIds); }
        }
    }
}
