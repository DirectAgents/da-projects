using Yahoo.Constants;
using Yahoo.Constants.Enums;

namespace Yahoo.Models.Requests
{
    /// <summary>
    /// The object identifies the dimensions of data and metrics returned in the report,
    /// dimension-value filters for selecting impression events based on dimension values,
    /// and metric-value filters for selecting groups of events based on summary statistics.
    /// </summary>
    internal class ReportOption
    {
        /// <summary>
        /// Specifies a string that defines time zone of report data.
        /// If no timezone value is specified, the America/New_York attribute is used.
        /// <see cref="Timezone"/>
        /// </summary>
        public string timezone { get; set; }

        /// <summary>
        /// Specifies the denomination used in reports.
        /// By default, all report data is in US dollars (USD).
        /// <see cref="Currency"/>
        /// </summary>
        public int currency { get; set; }

        /// <summary>
        /// Specifies an array of advertiser account IDs.
        /// If you have access to multiple advertiser accounts, you can filter the report data returned by account ID.
        /// </summary>
        public int[] accountIds { get; set; }

        /// <summary>
        /// Specifies an array of dimension type IDs that identify the dimensions of data returned in the report.
        /// If omitted, the report returns the time dimension only.
        /// Specifies the group by.
        /// <see cref="Dimension"/>
        /// </summary>
        public int[] dimensionTypeIds { get; set; }

        /// <summary>
        /// Specifies an array of the metric type IDs that specify the metrics measured in the report.
        /// <see cref="Metric"/>
        /// </summary>
        public int[] metricTypeIds { get; set; }
    }
}