namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Model for Bing daily row that extracted from CSV report.
    /// </summary>
    public class BingDailyRow : BaseBingRow
    {
        /// <summary>
        /// Gets or sets the Conversions column.
        /// </summary>
        public int Conversions { get; set; }

        /// <summary>
        /// Gets or sets the Revenue column.
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// Gets or sets the Impressions column.
        /// </summary>
        public int Impressions { get; set; }

        /// <summary>
        /// Gets or sets the Clicks column.
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// Gets or sets the Spend column.
        /// </summary>
        public decimal Spend { get; set; }
    }
}
