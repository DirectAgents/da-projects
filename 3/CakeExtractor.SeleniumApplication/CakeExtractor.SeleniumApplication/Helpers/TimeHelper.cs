using System;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    internal class TimeHelper
    {
        private static readonly DateTime OriginUnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            var diff = date - OriginUnixDateTime;
            return Math.Floor(diff.TotalMilliseconds);
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            return OriginUnixDateTime.AddMilliseconds(timestamp);
        }
    }
}
