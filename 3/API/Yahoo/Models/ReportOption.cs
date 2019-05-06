using Yahoo.Constants.Enums;

namespace Yahoo.Models
{
    internal class ReportOption
    {
        public string timezone { get; set; }

        public int currency { get; set; }

        public int[] accountIds { get; set; }

        public int[] dimensionTypeIds { get; set; }

        public int[] metricTypeIds { get; set; }
    }
}