using System;
using System.Globalization;

namespace ClientPortal.Web.Controllers
{
    internal class ControllerHelpers
    {
        internal static DateTime? ParseDate(string dateString, CultureInfo cultureInfo)
        {
            DateTime? date;
            ParseDate(dateString, cultureInfo, out date);
            return date;
        }

        // returns true iff parsed; a null or whitespace dateString qualifies as parsed, resulting in the out parameter being null
        internal static bool ParseDate(string dateString, CultureInfo cultureInfo, out DateTime? date)
        {
            date = null;
            if (String.IsNullOrWhiteSpace(dateString)) return true;

            DateTime parseDate;
            bool parsed = DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parseDate);
            if (parsed)
                date = parseDate;

            return parsed;
        }

        // returns false if either of the dates couldn't be parsed
        internal static bool ParseDates(string startdate, string enddate, CultureInfo cultureInfo, out DateTime? start, out DateTime? end)
        {
            bool startParsed = ParseDate(startdate, cultureInfo, out start);
            bool endParsed = ParseDate(enddate, cultureInfo, out end);
            return (startParsed && endParsed);
        }
    }
}