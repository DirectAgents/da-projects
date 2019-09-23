namespace Yahoo.Models.Responses
{
    /// <summary>
    /// The class used to respond after a request to read a current report status.
    /// </summary>
    internal class ReadReportStatusResponse
    {
        /// <summary>
        /// Specifies the current status of the report request. 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// If the report is generated, displays a temporary URL that can be used to download the report.
        /// </summary>
        public string Url { get; set; }
    }
}