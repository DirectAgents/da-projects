namespace Adform.Entities.ReportEntities.ReportParameters
{
    /// <inheritdoc />
    /// <summary>
    /// Class for Adform report filter including a tracking field.
    /// </summary>
    internal class ReportFilterWithTracking : ReportFilter
    {

        /// <summary>
        /// Gets or sets tracking IDs.
        /// </summary>
        public string[] Tracking { get; set; }
    }
}
