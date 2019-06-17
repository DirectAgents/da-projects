namespace Adform.Entities.ReportEntities.ReportParameters
{
    /// <summary>
    /// Class for Adform report filter.
    /// </summary>
    public class ReportFilter
    {
        /// <summary>
        /// Gets or sets IDs of Adform clients.
        /// </summary>
        public int[] Client { get; set; }

        /// <summary>
        /// Gets or sets tracking ID.
        /// </summary>
        public string Tracking { get; set; }

        /// <summary>
        /// Gets or sets report dates.
        /// </summary>
        public Dates Date { get; set; }

        /// <summary>
        /// Gets or sets media.
        /// </summary>
        public Media Media { get; set; }
    }
}
