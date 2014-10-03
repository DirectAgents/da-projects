using System;

namespace ClientPortal.Data.Contexts
{
    public partial class SimpleReport
    {
        public DateTime GetStatsStartDate()
        {
            DateTime dayAfterEnd = this.NextSend ?? DateTime.Today;
            if (this.PeriodMonths > 0)
                return dayAfterEnd.AddMonths(-1 * this.PeriodMonths);
            else
                return dayAfterEnd.AddDays(-1 * this.PeriodDays);
        }

        public DateTime GetStatsEndDate()
        {
            DateTime dayAfterEnd = this.NextSend ?? DateTime.Today;
            return dayAfterEnd.AddDays(-1);
        }

        public DayOfWeek? GetStartDayOfWeek()
        {
            if (this.Advertiser != null)
                return (DayOfWeek)this.Advertiser.StartDayOfWeek;
            else if (this.SearchProfile != null)
                return (DayOfWeek)this.SearchProfile.StartDayOfWeek;
            else
                return null;
        }
    }
}
