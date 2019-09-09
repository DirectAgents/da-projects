using System;

namespace SeleniumDataBrowser.VCD.DataProviders
{
    public interface IVcdDataProvider
    {
        string DownloadShippedRevenueCsvReport(DateTime reportDay);

        string DownloadShippedCogsCsvReport(DateTime reportDay);

        string DownloadOrderedRevenueCsvReport(DateTime reportDay);
    }
}