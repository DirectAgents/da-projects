using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.Cake;

namespace DirectAgents.Domain.DTO
{
    public class CakeGauge
    {
        public CakeGauge()
        {
            this.CampSums = new Range();
            this.EventConvs = new Range();
        }

        public Advertiser Advertiser { get; set; }

        public Range CampSums { get; set; }
        public Range EventConvs { get; set; }

        public class Range
        {
            public DateTime? Earliest { get; set; }
            public DateTime? Latest { get; set; }

            public decimal NumConvs { get; set; }
            public decimal NumConvsPaid { get; set; }
        }
    }

    public class EntityWithStats
    {
        public Advertiser Advertiser { get; set; }

        public IQueryable<CampSum> CampSums { get; set; }
        public IQueryable<EventConversion> EventConvs { get; set; }
    }
}
