namespace SeleniumDataBrowser.VCD.Models
{
    /// <summary>
    /// Entity composing vcd portal and database information about accounts.
    /// </summary>
    public class VcdAccountInfo
    {
        /// <summary>
        /// Gets or sets the name of account.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the vendor group identifier prom amazon ara portal.
        /// </summary>
        /// <value>
        /// The vendor group identifier.
        /// </value>
        public int VendorGroupId { get; set; }

        /// <summary>
        /// Gets or sets the mc identifier from amazon ara portal.
        /// </summary>
        /// <value>
        /// The mc identifier.
        /// </value>
        public long McId { get; set; }
    }
}