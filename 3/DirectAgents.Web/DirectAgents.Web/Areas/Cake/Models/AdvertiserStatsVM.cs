﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities.Cake;

namespace DirectAgents.Web.Areas.Cake.Models
{
    public class AdvertiserStatsVM
    {
        public Advertiser Advertiser { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int NumOffers { get; set; }
        public decimal CampSum_Convs { get; set; }
        public decimal CampSum_Paid { get; set; }

        public int EventConvs_Count { get; set; }
        public DateTime? EventConvs_Earliest { get; set; }
        public DateTime? EventConvs_Latest { get; set; }
    }
}