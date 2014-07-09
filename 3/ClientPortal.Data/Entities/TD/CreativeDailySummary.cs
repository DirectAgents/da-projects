﻿using System;

namespace ClientPortal.Data.Entities.TD
{
    public class CreativeDailySummary
    {
        public DateTime Date { get; set; }
        public int CreativeID { get; set; }
        public virtual Creative Creative { get; set; }

        public string AdvertiserCurrency { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }
    }
}
