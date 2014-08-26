using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ClientPortal.Web.Models
{
    public class DashboardModel
    {
        public List<DateRangeSummary> AdvertiserSummaries { get; set; }
        public List<OfferGoalSummary> OfferGoalSummaries { get; set; }

        public DateRangeSummary SummaryWTD { get { return (AdvertiserSummaries.Count >= 1) ? AdvertiserSummaries[0] : null; } }
        public DateRangeSummary SummaryMTD { get { return (AdvertiserSummaries.Count >= 2) ? AdvertiserSummaries[1] : null; } }
        public DateRangeSummary SummaryLMTD { get { return (AdvertiserSummaries.Count >= 3) ? AdvertiserSummaries[2] : null; } }
        public DateRangeSummary SummaryLM { get { return (AdvertiserSummaries.Count >= 4) ? AdvertiserSummaries[3] : null; } }

        //public string Culture { get { return (AdvertiserSummaries.Count > 0) ? AdvertiserSummaries[0].Culture : null; } }
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }
        // TODO: make CultureInfo a singleton (on demand)

        public string DateRangeType { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public bool ShowConVal { get; set; }
        public string ConValName { get; set; }
        public bool ConValIsNum { get; set; }

        public DateTime? LatestDaySums { get; set; }
        public DateTime? LatestClicks { get; set; }

        public DatesModel Dates { get; set; }

        public DashboardModel(UserInfo userInfo)
        {
            Culture = userInfo.Culture;

            var dates = new Dates(userInfo.UseYesterdayAsLatest, userInfo.StartDayOfWeek);
            Dates = new DatesModel(dates, this.CultureInfo);
        }
    }

    public class OfferGoalSummary
    {
        public string Id
        {
            get {
                StringBuilder id = new StringBuilder(Offer.OfferId.ToString());
                foreach (var goal in Goals)
                    id.Append("_" + goal.Id);
                return id.ToString();
            }
        }

        public Offer Offer { get; set; }
        public List<GoalVM> Goals { get; set; }
        public List<DateRangeSummary> DateRangeSummaries { get; set; }

        // for Monthly goal
        public DateRangeSummary SummaryMTD { get { return (DateRangeSummaries.Count >= 1) ? DateRangeSummaries[0] : null; } }
        public DateRangeSummary SummaryLMTD { get { return (DateRangeSummaries.Count >= 2) ? DateRangeSummaries[1] : null; } }
        public DateRangeSummary SummaryLM { get { return (DateRangeSummaries.Count >= 3) ? DateRangeSummaries[2] : null; } }

        // for Custom goal
        public DateRangeSummary SummaryCustom { get { return (DateRangeSummaries.Count >= 1) ? DateRangeSummaries[0] : null; } }
        public DateRangeSummary SummaryGoal { get { return (DateRangeSummaries.Count >= 2) ? DateRangeSummaries[1] : null; } }

        public string Culture { get { return (DateRangeSummaries.Count > 0) ? DateRangeSummaries[0].Culture : null; } }
    }
}