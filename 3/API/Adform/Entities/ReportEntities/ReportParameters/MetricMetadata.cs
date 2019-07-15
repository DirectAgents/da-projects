namespace Adform.Entities.ReportEntities.ReportParameters
{
    /// <summary>
    /// The class represents an entity for setting required parameters of an Adform metric.
    /// </summary>
    public class MetricMetadata
    {
        /// <summary>
        /// Gets or sets metric name.
        /// </summary>
        public string Metric { get; set; }

        /// <summary>
        /// Gets or sets additional filtering possibilities for individual metric.
        /// </summary>
        public dynamic Specs { get; set; }
    }
}
