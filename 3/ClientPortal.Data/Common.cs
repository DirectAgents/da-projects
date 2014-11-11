using System;

namespace ClientPortal.Data
{
    public static class Common
    {
        public static DateTime GetNext_WeekStartDate(DayOfWeek startDayOfWeek, bool includeToday = false)
        {
            var date = DateTime.Today;
            if (!includeToday) date = date.AddDays(1);
            while (date.DayOfWeek != startDayOfWeek)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        public static DateTime GetLast_WeekEndDate(DayOfWeek startDayOfWeek, bool includeToday = false)
        {
            var date = DateTime.Today;
            if (!includeToday) date = date.AddDays(-1);
            while (date.AddDays(1).DayOfWeek != startDayOfWeek)
            {
                date = date.AddDays(-1);
            }
            return date;
        }
    }
}
