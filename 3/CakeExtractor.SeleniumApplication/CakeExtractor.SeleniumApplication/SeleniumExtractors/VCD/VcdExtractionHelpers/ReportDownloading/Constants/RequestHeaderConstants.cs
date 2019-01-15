using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants
{
    internal class RequestHeaderConstants
    {
        public const string RequestIdHeaderName = "X-Amzn-RequestId";

        public const string VendorGroupHeaderName = "X-Amzn-Vendor-Group";

        public static Dictionary<string, string> GetHeadersDictionary()
        {
            return new Dictionary<string, string>()
            {
                { RequestIdHeaderName, "" },  // Should be filled dynamically with guid value
                { "X-Amzn-Product","ara"},
                { VendorGroupHeaderName, ""}, // Should be filled dynamically with value of current account
                { "X-Amzn-McId","0"},
                { "Content-Type","application/json"},
                { "Host","ara.amazon.com"}
            };
        }
    }
}
