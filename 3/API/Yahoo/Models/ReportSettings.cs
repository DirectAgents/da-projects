using System;

namespace Yahoo.Models
{
    /// <summary>
    /// The class with fields for customizing the requested report.
    /// </summary>
    public class ReportSettings
    {
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Account Id (may be null)
        /// </summary>
        public int? AccountId { get; set; }

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

        /// <summary>
        /// Flag for Pixel Parameter dimension (may be null)
        /// </summary>
        public bool ByPixelParameter { get; set; }

        public bool IsOutdated { get; set; }
    }
}
