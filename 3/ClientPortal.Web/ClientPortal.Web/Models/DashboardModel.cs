using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System.Collections.Generic;

namespace ClientPortal.Web.Models
{
    public class DashboardModel
    {
        public List<DateRangeSummary> AdvertiserSummaries { get; set; }
        public List<OfferGoalSummary> OfferGoalSummaries { get; set; }

        public string Culture { get { return (AdvertiserSummaries.Count > 0) ? AdvertiserSummaries[0].Culture : null; } }
    }

    public class OfferGoalSummary
    {
        public CakeOffer Offer { get; set; }
        public List<GoalVM> Goals { get; set; }
        public List<DateRangeSummary> DateRangeSummaries { get; set; }

        public DateRangeSummary SummaryMTD { get { return (DateRangeSummaries.Count >= 1) ? DateRangeSummaries[0] : null; } }
        public DateRangeSummary SummaryLMTD { get { return (DateRangeSummaries.Count >= 2) ? DateRangeSummaries[1] : null; } }
        public DateRangeSummary SummaryLM { get { return (DateRangeSummaries.Count >= 3) ? DateRangeSummaries[2] : null; } }

        public string Culture { get { return (DateRangeSummaries.Count > 0) ? DateRangeSummaries[0].Culture : null; } }
    }
}