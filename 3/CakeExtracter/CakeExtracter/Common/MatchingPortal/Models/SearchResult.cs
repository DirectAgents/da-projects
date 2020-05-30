namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// A model to provide options for matching.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets search result identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets image link for search result.
        /// </summary>
        public string ImageLink { get; set; }

        /// <summary>
        /// Gets or sets value of the search result title.
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// Gets or sets search result link.
        /// </summary>
        public string ResultLink { get; set; }
    }
}