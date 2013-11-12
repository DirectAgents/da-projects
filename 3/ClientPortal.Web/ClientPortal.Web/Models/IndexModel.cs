﻿using ClientPortal.Web.Controllers;
using System;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class IndexModel
    {
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }
        // TODO: make CultureInfo a singleton (on demand)

        public bool HasLogo { get; set; }
        public bool ShowCPMRep { get; set; }

        public IndexModel(UserInfo userInfo)
        {
            Culture = userInfo.Culture;
            HasLogo = (userInfo.Logo != null);
            ShowCPMRep = userInfo.ShowCPMRep;

            Dt = new Dates(userInfo.UseYesterdayAsLatest, userInfo.WeekStartDay);
        }

        private Dates Dt { get; set; }

        public string Today { get { return Dt.Today.ToString("d", CultureInfo); } }
        public string Yesterday { get { return Dt.Yesterday.ToString("d", CultureInfo); } }
        public string Latest { get { return Dt.Latest.ToString("d", CultureInfo); } }

        public string FirstOfMonth { get { return Dt.FirstOfMonth.ToString("d", CultureInfo); } }
        public string FirstOfYear { get { return Dt.FirstOfYear.ToString("d", CultureInfo); } }
        public string FirstOfLastMonth { get { return Dt.FirstOfLastMonth.ToString("d", CultureInfo); } }
        public string LastOfLastMonth { get { return Dt.LastOfLastMonth.ToString("d", CultureInfo); } }
        public string FirstOfWeek { get { return Dt.FirstOfWeek.ToString("d", CultureInfo); } }
        public string FirstOfLastWeek { get { return Dt.FirstOfLastWeek.ToString("d", CultureInfo); } }
        public string LastOfLastWeek { get { return Dt.LastOfLastWeek.ToString("d", CultureInfo); } }

        public string TodayMY { get { return Dt.Today.ToString("MM/yyyy"); } }
        public string LastMonthMY { get { return Dt.FirstOfLastMonth.ToString("MM/yyyy"); } }
        public string ThreeMonthsAgoMY { get { return Dt.FirstOfMonth.AddMonths(-3).ToString("MM/yyyy"); } }
        public string FirstOfYearMY { get { return Dt.FirstOfYear.ToString("MM/yyyy"); } }
    }
}