using System.Collections.Generic;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
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
