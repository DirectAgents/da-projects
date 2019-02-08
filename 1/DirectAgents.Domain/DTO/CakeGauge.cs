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
            this.AffSubSums = new Range();
            this.EventConvs = new Range();
        }

        public Advertiser Advertiser { get; set; }
        public int NumOffers { get; set; }

        public Range CampSums { get; set; }
        public Range AffSubSums { get; set; }
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

        public DateTime? StartDateForStats { get; set; }
        public DateTime? EndDateForStats { get; set; }

        public IQueryable<CampSum> CampSumsAll { get; set; }
        public IQueryable<CampSum> CampSumsForStats {
            get
            {
                if (CampSumsAll == null)
                    return null;
                var campSums = CampSumsAll;
                if (StartDateForStats.HasValue)
                    campSums = campSums.Where(x => x.Date >= StartDateForStats.Value);
                if (EndDateForStats.HasValue)
                    campSums = campSums.Where(x => x.Date <= EndDateForStats.Value);
                return campSums;
            }
        }
        //TODO: use a cache, in case this is called more than once

        public IQueryable<AffSubSummary> AffSubSumsAll { get; set; }
        public IQueryable<AffSubSummary> AffSubSumsForStats { get; }
        //TODO: implement!

        public IQueryable<EventConversion> EventConvsAll { get; set; }
        public IQueryable<EventConversion> EventConvsForStats { get; }
        //TODO: implement!

    }
}
