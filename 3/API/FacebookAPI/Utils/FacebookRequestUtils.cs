using System;

namespace FacebookAPI.Utils
{
    /// <summary>
    /// Facebook Api request utils.
    /// </summary>
    public static class FacebookRequestUtils
    {
        /// <summary>
        /// Gets the date string for Facebook api request params.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static string GetDateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-M-d");
        }
    }
}
