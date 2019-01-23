using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants
{
    internal class RequestHeaderConstants
    {
        public const string RequestIdHeaderName = "X-Amzn-RequestId";

        public static Dictionary<string, string> GetHeadersDictionary()
        {
            return new Dictionary<string, string>()
            {
                { RequestIdHeaderName, "" },  // Should be filled dynamically with guid value
                { "X-Amzn-Product","ara"},
                { "Content-Type","application/json"},
                { "Host","ara.amazon.com"}
            };
        }
    }
}
