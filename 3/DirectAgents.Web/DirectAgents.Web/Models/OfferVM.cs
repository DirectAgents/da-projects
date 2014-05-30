using DirectAgents.Domain.Entities.Cake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectAgents.Web.Models
{
    public class OfferVM
    {
        public OfferVM(Offer offer, IQueryable<OfferDailySummary> offerDailySummaries)
        {
            this.Offer = offer;
            this.OfferDailySummaries = offerDailySummaries.OrderBy(o => o.Date).AsEnumerable();
        }

        public Offer Offer { get; set; }
        public IEnumerable<OfferDailySummary> OfferDailySummaries { get; set; }

        public int Views
        {
            get { return OfferDailySummaries.Sum(o => o.Views); }
        }
        public int Clicks
        {
            get { return OfferDailySummaries.Sum(o => o.Clicks); }
        }
        public int Conversions
        {
            get { return OfferDailySummaries.Sum(o => o.Conversions); }
        }
        public int Paid
        {
            get { return OfferDailySummaries.Sum(o => o.Paid); }
        }
        public int Sellable
        {
            get { return OfferDailySummaries.Sum(o => o.Sellable); }
        }
        public decimal Revenue
        {
            get { return OfferDailySummaries.Sum(o => o.Revenue); }
        }
        public decimal Cost
        {
            get { return OfferDailySummaries.Sum(o => o.Cost); }
        }

        public DateTime? EarliestStatDate
        {
            get
            {
                var ods = OfferDailySummaries.FirstOrDefault();
                if (ods == null)
                    return null;
                else
                    return ods.Date;
            }
        }

        public DateTime? LatestStatDate
        {
            get
            {
                var ods = OfferDailySummaries.LastOrDefault();
                if (ods == null)
                    return null;
                else
                    return ods.Date;
            }
        }
    }
}