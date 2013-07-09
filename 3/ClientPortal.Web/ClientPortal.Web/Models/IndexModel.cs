using System;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class IndexModel
    {
        public CultureInfo CultureInfo { get; set; }
        public bool HasLogo { get; set; }
        public bool ShowCPMRep { get; set; }

        public IndexModel(UserInfo userInfo)
        {
            CultureInfo = userInfo.CultureInfo;
            HasLogo = (userInfo.Logo != null);
            ShowCPMRep = userInfo.ShowCPMRep;

            SetupDates();
        }

        public void SetupDates()
        {
            Now = DateTime.Now;
            SundayDT = Now;
            while (SundayDT.DayOfWeek != DayOfWeek.Sunday)
            {
                SundayDT = SundayDT.AddDays(-1);
            }
            FirstOfMonthDT = new DateTime(Now.Year, Now.Month, 1);
        }

        private DateTime Now { get; set; }
        private DateTime SundayDT { get; set; }
        private DateTime FirstOfMonthDT { get; set; }

        public string Today { get { return Now.ToString("d", CultureInfo); } }
        public string Yesterday { get { return Now.AddDays(-1).ToString("d", CultureInfo); } }
        public string FirstOfMonth { get { return FirstOfMonthDT.ToString("d", CultureInfo); } }
        public string FirstOfYear { get { return new DateTime(Now.Year, 1, 1).ToString("d", CultureInfo); } }
        public string FirstOfLastMonth { get { return FirstOfMonthDT.AddMonths(-1).ToString("d", CultureInfo); } }
        public string LastOfLastMonth { get { return FirstOfMonthDT.AddDays(-1).ToString("d", CultureInfo); } }
        public string FirstOfWeek { get { return SundayDT.ToString("d", CultureInfo); } }
        public string FirstOfLastWeek { get { return SundayDT.AddDays(-7).ToString("d", CultureInfo); } }
        public string LastOfLastWeek { get { return SundayDT.AddDays(-1).ToString("d", CultureInfo); } }

        public string TodayMY { get { return Now.ToString("MM/yyyy"); } }
        public string LastMonthMY { get { return FirstOfMonthDT.AddMonths(-1).ToString("MM/yyyy"); } }
        public string ThreeMonthsAgoMY { get { return FirstOfMonthDT.AddMonths(-3).ToString("MM/yyyy"); } }
        public string FirstOfYearMY { get { return new DateTime(Now.Year, 1, 1).ToString("MM/yyyy"); } }
    }
}