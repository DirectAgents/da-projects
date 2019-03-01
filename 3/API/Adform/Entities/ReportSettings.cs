using System;
using System.Collections.Generic;
using Adform.Enums;

namespace Adform.Entities
{
    public class ReportSettings
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ClientId { get; set; }
        public bool BasicMetrics { get; set; }
        public bool ConvMetrics { get; set; }
        public List<Dimension> Dimensions { get; set; }
        public bool RtbOnly { get; set; } // real-time bidding campaigns only
    }
}
