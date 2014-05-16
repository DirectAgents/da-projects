﻿using System;
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

        public string RevenueCurrency { get; set; }
        public decimal Revenue { get; set; }

        public decimal InvoicedAmount { get; set; }

        public string CostCurrency { get; set; }
        public decimal Cost { get; set; }

        public decimal MarginPct
        {
            get
            {
                if (Revenue != 0 && RevenueCurrency == CostCurrency)
                    return (1 - Cost / Revenue);
                else
                    return 0;
            }
        }

        public int NumUnits { get; set; }
        public int NumAffs { get; set; }
    }
}
