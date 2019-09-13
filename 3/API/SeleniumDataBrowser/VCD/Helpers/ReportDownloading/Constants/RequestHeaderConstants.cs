using System.Collections.Generic;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Constants
{
    internal class RequestHeaderConstants
    {
        public const string RequestIdHeaderName = "X-Amzn-RequestId";

        public static Dictionary<string, string> GetHeadersDictionary()
        {
            return new Dictionary<string, string>
            {
                { RequestIdHeaderName, "" },  // Should be filled dynamically with guid value
                { "Content-Type", "application/json" },
                { "Host", "vendorcentral.amazon.com" },
            };
        }
    }
}