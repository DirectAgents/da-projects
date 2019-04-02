using System;
using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.Configuration.Models
{
    /// <summary>
    /// Vendor Central Data execution profile configuration entity.
    /// </summary>
    public class VcdExecutionProfileConfiguration
    {
        public string SignInUrl { get; set; }

        public string LoginEmail { get; set; }

        public string LoginPassword { get; set; }

        public List<int> AccountIds { get; set; }

        public string CookiesDirectory { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<int> DayIntervalsToProcess { get; set; }

        public int ReportDownloadingStartedDelayInSeconds { get; set; }

        public int MinDelayBetweenReportDownloadingInSeconds { get; set; }

        public int ReportDownloadingAttemptCount { get; set; }

        public int MaxDelayBetweenReportDownloadingInSeconds { get; set; }

        public int ExtractDailyDataAttemptCount { get; set; }
    }
}
