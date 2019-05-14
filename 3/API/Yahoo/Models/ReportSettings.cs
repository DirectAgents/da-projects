using System;

namespace Yahoo.Models
{
    /// <summary>
    /// The class with fields for customizing the requested report.
    /// </summary>
    public class ReportSettings
    {
        /// <summary>
        /// The start date.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// The account Id (may be null).
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// The flag for Campaign dimension (may be null).
        /// </summary>
        public bool ByCampaign { get; set; }

        /// <summary>
        /// The flag for Line dimension (may be null).
        /// </summary>
        public bool ByLine { get; set; }

        /// <summary>
        /// The flag for Ad dimension (may be null).
        /// </summary>
        public bool ByAd { get; set; }

        /// <summary>
        /// The flag for Creative dimension (may be null).
        /// </summary>
        public bool ByCreative { get; set; }

        /// <summary>
        /// The flag for Pixel dimension (may be null).
        /// </summary>
        public bool ByPixel { get; set; }

        /// <summary>
        /// The flag for Pixel Parameter dimension (may be null).
        /// </summary>
        public bool ByPixelParameter { get; set; }

        /// <summary>
        /// The flag for internal use that indicates for which command a report should be generated.
        /// </summary>
        public bool IsOutdated { get; set; }
    }
}
