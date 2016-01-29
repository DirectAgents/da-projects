﻿using System;

namespace DirectAgents.Web
{
    public class Common
    {
        public static DateTime FirstOfMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1);
        }

        public static DateTime FirstOfYear()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, 1, 1);
        }
    }

    public static class Extensions
    {
        public static DateTime FirstDayOfMonth(this DateTime dateTime, int addMonths = 0)
        {
            var firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstOfMonth.AddMonths(addMonths);
        }

        public static string ToRouteString(this DateTime dateTime)
        {
            return ToRouteString((DateTime?)dateTime);
        }
        public static string ToRouteString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToString("M-d-yy");
            else
                return null;
        }
    }
}