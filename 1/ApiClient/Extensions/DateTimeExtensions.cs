using System;

namespace Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return FirstDayOfMonth(dt).AddDays(DateTime.DaysInMonth(dt.Year, dt.Month));
        }
    }
}
