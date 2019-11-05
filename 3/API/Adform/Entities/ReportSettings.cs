using System;
using System.Collections.Generic;
using Adform.Enums;

namespace Adform.Entities
{
    /// <summary>
    /// Adform report settings.
    /// </summary>
    public class ReportSettings
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ClientId { get; set; }

        public string[] TrackingIds { get; set; }

        public bool BasicMetrics { get; set; }

        public bool ConvMetrics { get; set; }

        public List<Dimension> Dimensions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include "Real-Time Bidding" media only
        /// (for real-time bidding campaigns only).
        /// </summary>
        public bool RtbMediaOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include
        /// the "Unique impressions" metric for all media types.
        /// </summary>
        public bool UniqueImpressionsMetric { get; set; }
    }
}