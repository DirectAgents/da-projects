using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting.Models;
using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Models
{
    public class ReportDownloadingRequestPageData // TODo think about more appropriate class name
    {
        public string Token
        {
            get;
            set;
        }

        public Dictionary<string, string> Cookies
        {
            get;
            set;
        }
    }
}
