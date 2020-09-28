using Newtonsoft.Json;

namespace Amazon.Entities.HelperEntities
{
    /// <summary>
    /// Class represents a parameters for the amazon attribution report request.
    /// <see cref="https://advertising.amazon.com/API/docs/en-us/amazon-attribution-prod-3p/#/Reports/getAttributionTagsByCampaign"/>.
    /// </summary>
    public class AmazonAttributionApiReportParams
    {
        /// <summary>
        /// Gets or sets one or more advertiser Ids to filter reporting by. If requesting reporting for multiple advertiser Ids, input via a comma-delimited list.
        /// </summary>
        [JsonProperty("advertiserIds")]
        public string AdvertiserIds { get; set; }

        /// <summary>
        /// Gets or sets the start date for the report, in "YYYYMMDD" format.
        /// </summary>
        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date for the report, form as YYYYMMDD.
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of entries to include in the report.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets a comma-delimited list of metrics to include in the report.
        /// </summary>
        [JsonProperty("metrics")]
        public string Metrics { get; set; }

        /// <summary>
        /// Gets or sets a cursor Id from previous response.
        /// <remarks>The value of cursorId must be set to null without "", or set to "" for the first request.</remarks>
        /// </summary>
        [JsonProperty("cursorId")]
        public string CursorId { get; set; }
    }
}