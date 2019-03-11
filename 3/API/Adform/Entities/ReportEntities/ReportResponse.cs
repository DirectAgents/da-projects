namespace Adform.Entities.ReportEntities
{
    /// <summary>
    /// The class summarizes all the response fields returned by the API. More info - http://api.adform.com/help/references/buyer-solutions/reporting/stats/response-reference-guide
    /// </summary>
    public class ReportResponse
    {
        /// <summary>
        /// The returned report data.
        /// </summary>
        public ReportData reportData { get; set; }

        /// <summary>
        /// The total number of rows in the query result, regardless of the number of rows returned in the response. 
        /// </summary>
        public int totalRowCount { get; set; }

        /// <summary>
        /// A unique code for reference of the response.
        /// </summary>
        public string correlationCode { get; set; }
    }
}