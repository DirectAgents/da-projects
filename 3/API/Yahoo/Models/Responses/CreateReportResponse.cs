namespace Yahoo.Models.Responses
{
    /// <summary>
    /// The class used to respond after a report creation request.
    /// </summary>
    internal class CreateReportResponse
    {
        /// <summary>
        /// Unique report ID.
        /// </summary>
        public string CustomerReportId { get; set; }

        /// <summary>
        /// Current status of the report generation.
        /// </summary>
        public string Status { get; set; }
    }
}