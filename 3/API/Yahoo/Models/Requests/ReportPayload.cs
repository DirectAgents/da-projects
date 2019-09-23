using Yahoo.Constants.Enums;

namespace Yahoo.Models.Requests
{
    /// <summary>
    /// The object consists of attributes that enable you to define the advertiser account associated with the data,
    /// report dimensions and metrics, reporting periods, and the granularity of report data.
    /// </summary>
    internal class ReportPayload
    {
        /// <summary>
        /// Specifies report options. <see cref="ReportOption"/>
        /// </summary>
        public ReportOption reportOption { get; set; }

        /// <summary>
        /// Specifies the granularity of report intervals.
        /// <see cref="IntervalTypeId"/>
        /// </summary>
        public int intervalTypeId { get; set; }

        /// <summary>
        /// Specifies the reporting period, the span of time covered by the data set.
        /// <see cref="DateTypeId"/>
        /// </summary>
        public int dateTypeId { get; set; }

        /// <summary>
        /// Specifies the start time of date range of the report in the format yyyy-MM-dd'T'HH:mm:ss.
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// Specifies the end time of date range of the report in the format yyyy-MM-dd'T'HH:mm:ss.
        /// </summary>
        public string endDate { get; set; }
    }
}
