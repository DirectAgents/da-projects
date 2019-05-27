using System;

namespace Apple.Entities
{
    /// <summary>
    /// The class for reporting response metrics - the result of request to the API.
    /// More info - https://searchads.apple.com/advanced/help/campaign-management/#campaign-management-api.
    /// </summary>
    public class AppleReportingResponse
    {
        /// <summary>
        /// Gets or sets the reporting field for the date of the report.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the reporting field for the number of impressions.
        /// </summary>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets the reporting field for the number of taps.
        /// </summary>
        public int Taps { get; set; }

        /// <summary>
        /// Gets or sets the reporting field for conversions (renamed since API version 2.0).
        /// </summary>
        public int Installs { get; set; }

        /// <summary>
        /// Gets or sets the reporting field for the calculated sum of cost
        /// associated with each impression served.
        /// </summary>
        public CurrAmount LocalSpend { get; set; }
    }
}
