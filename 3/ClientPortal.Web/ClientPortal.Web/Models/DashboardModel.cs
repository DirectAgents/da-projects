using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System.Collections.Generic;

namespace ClientPortal.Web.Models
{
    public class DashboardModel
    {
        public List<DateRangeSummary> AdvertiserSummaries { get; set; }
        public List<OfferGoalSummary> OfferGoalSummaries { get; set; }
    }

    public class OfferGoalSummary
    {
        public CakeOffer Offer { get; set; }
        public List<GoalVM> Goals { get; set; }
        public List<DateRangeSummary> DateRangeSummaries { get; set; }
    }
}