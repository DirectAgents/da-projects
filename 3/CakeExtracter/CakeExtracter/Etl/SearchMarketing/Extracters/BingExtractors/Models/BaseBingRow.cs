namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models
{
    /// <summary>
    /// Base model for Bing row that extracted from CSV report.
    /// </summary>
    public class BaseBingRow
    {
        /// <summary>
        /// Gets or sets the TimePeriod column.
        /// </summary>
        public string TimePeriod { get; set; }

        /// <summary>
        /// Gets or sets the AccountId column.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the AccountName column.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets AccountNumber column.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets CampaignId column.
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Gets or sets CampaignName column.
        /// </summary>
        public string CampaignName { get; set; }
    }
}
