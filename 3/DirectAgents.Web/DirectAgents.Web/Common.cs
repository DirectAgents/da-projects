using System;

namespace DirectAgents.Web
{
    public class Common
    {
        public static DateTime FirstOfMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1);
        }
    }
}