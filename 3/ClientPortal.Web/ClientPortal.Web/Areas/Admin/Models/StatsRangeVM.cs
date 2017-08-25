using System;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class StatsRangeVM
    {
        public string Name { get; set; }
        public DateTime? Earliest { get; set; }
        public DateTime? Latest { get; set; }

        public string EarliestDisp
        {
            get { return Earliest.HasValue ? Earliest.Value.ToShortDateString() : null; }
        }
        public string LatestDisp
        {
            get { return Latest.HasValue ? Latest.Value.ToShortDateString() : null; }
        }
    }
}