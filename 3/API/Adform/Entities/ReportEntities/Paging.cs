namespace Adform.Entities.ReportEntities
{
    /// <summary>
    /// The class defines the maximum number of rows to include in the response.
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// Row index to start from.
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        /// Rows per page.
        /// </summary>
        public int limit { get; set; }
    }
}