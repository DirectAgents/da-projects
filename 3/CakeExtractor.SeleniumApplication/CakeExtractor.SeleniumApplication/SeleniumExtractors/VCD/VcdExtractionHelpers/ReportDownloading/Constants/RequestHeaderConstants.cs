using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants
{
    internal class RequestHeaderConstants
    {
        public static Dictionary<string, string> GetHeadersDictionary()
        {
            return new Dictionary<string, string>()
            {
                { "X-Amzn-RequestId","39c40206-dc79-45cc-b0e5-c02847342e4e" }, // ToDo: Parametrise request id
                { "X-Amzn-Product","ara"},
                { "X-Amzn-Vendor-Group","797780"}, // ToDO : parametrise amzn-vendor-group
                { "X-Amzn-McId","0"},
                { "Content-Type","application/json"},
                { "Host","ara.amazon.com"}
            };
        }
    }
}
