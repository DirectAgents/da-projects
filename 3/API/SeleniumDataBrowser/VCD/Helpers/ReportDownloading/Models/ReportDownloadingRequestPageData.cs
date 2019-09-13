using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Models
{
    public class ReportDownloadingRequestPageData // TODo think about more appropriate class name
    {
        public string Token
        {
            get;
            set;
        }

        public IEnumerable<Cookie> Cookies
        {
            get;
            set;
        }
    }
}
