using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Adform.Entities.ReportEntities.ReportParameters
{
    /// <summary>
    /// The class represents an entity for storing query parameters for generating a report.
    /// More info - https://api.adform.com/help/references/buyer-solutions/reporting/stats.
    /// </summary>
    public class ReportParams
    {
        /// <summary>
        /// Gets or sets dimensions break down metrics across some common criteria, such as campaign or browser. 
        /// </summary>
        public string[] Dimensions { get; set; }

        /// <summary>
        /// Gets or sets metrics are the individual measurements of user activity on your property, such as impressions and pageviews.
        /// </summary>
        public MetricMetadata[] Metrics { get; set; }

        /// <summary>
        /// Gets or sets filter object provides filters in the request.
        /// </summary>
        public dynamic Filter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the report includes the total number of rows as a result of the query.
        /// </summary>
        public bool IncludeRowCount { get; set; }

        /// <summary>
        /// The method clones and returns a current object.
        /// </summary>
        /// <returns>Clone of the current object.</returns>
        public ReportParams Clone()
        {
            return new ReportParams
            {
                Filter = Filter,
                IncludeRowCount = IncludeRowCount,
                Dimensions = Dimensions,
                Metrics = Metrics,
            };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var dimensionsStr = string.Join(", ", Dimensions);
            var metricsWithSpecsStr = Metrics.Select(x => $"{x.Metric} - {x.Specs}");
            var metricsStr = string.Join(", ", metricsWithSpecsStr);
            var datesStr = $"{Filter.Date.From}/{Filter.Date.To}";
            var clientsStr = string.Join(", ", Filter.Client);
            var trackingStr = GetTrackingStringIfPropertyExists();
            var filtersStr = $"dates - {datesStr}, clients - {clientsStr}{trackingStr}";
            return $"dimensions: {dimensionsStr}; metrics: {metricsStr}; filters: {filtersStr}";
        }

        private string GetTrackingStringIfPropertyExists()
        {
            const string trackingPropertyKey = "Tracking";
            return IsFilterPropertyExist(trackingPropertyKey)
                ? $", tracking - {string.Join(", ", Filter.Tracking)}"
                : string.Empty;
        }

        private bool IsFilterPropertyExist(string name)
        {
            if (Filter is ExpandoObject)
            {
                return ((IDictionary<string, object>)Filter).ContainsKey(name);
            }
            return Filter.GetType().GetProperty(name) != null;
        }
    }
}