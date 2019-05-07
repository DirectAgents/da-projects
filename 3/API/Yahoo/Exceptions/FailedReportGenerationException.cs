using System;
using Yahoo.Models;

namespace Yahoo.Exceptions
{
    public class FailedReportGenerationException : Exception
    {
        private const string ExceptionMessage = "Could not generate a report: ";

        /// <summary>
        /// Account ID in Yahoo portal for which statistics was extracted.
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Start date from which statistics was extracted.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date to which statistics was extracted.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Flag for Campaign dimension (may be null)
        /// </summary>
        public bool ByCampaign { get; set; }

        /// <summary>
        /// Flag for Line dimension (may be null)
        /// </summary>
        public bool ByLine { get; set; }

        /// <summary>
        /// Flag for Ad dimension (may be null)
        /// </summary>
        public bool ByAd { get; set; }

        /// <summary>
        /// Flag for Creative dimension (may be null)
        /// </summary>
        public bool ByCreative { get; set; }

        /// <summary>
        /// Flag for Pixel dimension (may be null)
        /// </summary>
        public bool ByPixel { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedReportGenerationException"/> class.
        /// </summary>
        /// <param name="exception">Source exception</param>
        public FailedReportGenerationException(ReportSettings reportSettings, Exception exception) : base(GetMessage(reportSettings, exception))
        {
            StartDate = reportSettings.FromDate;
            EndDate = reportSettings.ToDate;
            AccountId = reportSettings.AccountId;
            ByCampaign = reportSettings.ByCampaign;
            ByLine = reportSettings.ByLine;
            ByAd = reportSettings.ByAd;
            ByCreative = reportSettings.ByCreative;
            ByPixel = reportSettings.ByPixel;
        }

        private static string GetMessage(ReportSettings reportSettings, Exception exception)
        {
            return ExceptionMessage +
                   $"accountExternalId = {reportSettings.AccountId}, " +
                   $"fromDate = {reportSettings.FromDate.ToShortDateString()}, " +
                   $"toDate = {reportSettings.ToDate.ToShortDateString()}, " +
                   $"byCampaign = {reportSettings.ByCampaign}, " +
                   $"byLine = {reportSettings.ByLine}, " +
                   $"byAd = {reportSettings.ByAd}, " +
                   $"byCreative = {reportSettings.ByCreative}, " +
                   $"byPixel = {reportSettings.ByPixel}, " +
                   $"exception - {exception.Message}";
        }
    }
}
