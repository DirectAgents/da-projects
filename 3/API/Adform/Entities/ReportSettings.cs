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
    }
}