namespace Adform.Entities.ReportEntities
{
    /// <summary>
    /// The class represents an entity for setting required parameters of an Adform metric.
    /// </summary>
    public class MetricMetadata
    {
        /// <summary>
        /// Metric name.
        /// </summary>
        public string metric { get; set; }

        /// <summary>
        /// Additional filtering possibilities for individual metric.
        /// </summary>
        public dynamic specs { get; set; }
    }
}
