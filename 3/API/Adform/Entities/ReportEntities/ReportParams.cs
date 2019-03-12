namespace Adform.Entities.ReportEntities
{
    /// <summary>
    /// The class represents an entity for storing query parameters for generating a report. More info - https://api.adform.com/help/references/buyer-solutions/reporting/stats
    /// </summary>
    public class ReportParams
    {
        /// <summary>
        /// Dimensions break down metrics across some common criteria, such as campaign or browser. 
        /// </summary>
        public string[] dimensions { get; set; }

        /// <summary>
        /// Metrics are the individual measurements of user activity on your property, such as impressions and pageviews.
        /// </summary>
        public MetricMetadata[] metrics { get; set; }

        /// <summary>
        /// The filter object provides filters in the request.
        /// </summary>
        public object filter { get; set; }

        /// <summary>
        /// Paging defines the maximum number of rows to include in the response. 
        /// </summary>
        public Paging paging { get; set; }

        /// <summary>
        /// The field turns on a return of the total number of rows in the query result.
        /// </summary>
        public bool includeRowCount { get; set; }

        /// <summary>
        /// The method clones and returns a current object.
        /// </summary>
        public ReportParams Clone()
        {
            return new ReportParams
            {
                filter = filter,
                includeRowCount = includeRowCount,
                paging = paging,
                dimensions = dimensions,
                metrics = metrics
            };
        }
    }
}