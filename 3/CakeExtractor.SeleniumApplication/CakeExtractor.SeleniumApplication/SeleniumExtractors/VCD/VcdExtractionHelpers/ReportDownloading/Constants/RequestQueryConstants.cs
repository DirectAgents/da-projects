using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants
{
    internal class RequestQueryConstants
    {
        public static Dictionary<string, string> GetRequiestQueryparameters(string tokenValue)
        {
            return new Dictionary<string, string>()
            {
                {"token", tokenValue }
            };
        }
    }
}
