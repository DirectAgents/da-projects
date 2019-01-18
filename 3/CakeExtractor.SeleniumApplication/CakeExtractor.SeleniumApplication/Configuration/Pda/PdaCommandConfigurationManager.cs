using System;
using CakeExtractor.SeleniumApplication.Properties;

namespace CakeExtractor.SeleniumApplication.Configuration.Pda
{
    internal class PdaCommandConfigurationManager
    {
        private const int DefaultDaysAgoValue = 41;

        public string GetCampaignsPageUrl()
        {
            return PdaSettings.Default.CampaignsPageUrl;
        }

        public string GetDownloadsDirectoryName()
        {
            return PdaSettings.Default.DownloadsDirectoryName;
        }

        public string GetFilesNameTemplate()
        {
            return PdaSettings.Default.FilesNameTemplate;
        }

        public int GetDaysAgo()
        {
            return PdaSettings.Default.DaysAgo != 0 ? PdaSettings.Default.DaysAgo : DefaultDaysAgoValue;
        }

        public DateTime GetStartDate(int daysAgo)
        {
            return PdaSettings.Default.StartDate == default(DateTime)
                ? DateTime.Today.AddDays(-daysAgo)
                : PdaSettings.Default.StartDate;
        }

        public DateTime GetEndDate()
        {
            return PdaSettings.Default.EndDate == default(DateTime)
                ? DateTime.Today.AddDays(-1)
                : PdaSettings.Default.EndDate;
        }

        public string GetCookiesDirectory()
        {
            return PdaSettings.Default.CookiesDirectory;
        }

        public bool GetFromDatabase()
        {
            return PdaSettings.Default.FromDatabase;
        }
    }
}
