using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtractor.SeleniumApplication.Configuration.Models
{
    /// <summary>
    /// Entity composing vcd portal and database information about accounts.
    /// </summary>
    internal class AccountInfo
    {
        /// <summary>
        /// Gets or sets the account's db entity.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public ExtAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vendor group identifier prom amazon ara portal.
        /// </summary>
        /// <value>
        /// The vendor group identifier.
        /// </value>
        public int VendorGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mc identifier from amazon ara portal.
        /// </summary>
        /// <value>
        /// The mc identifier.
        /// </value>
        public long McId
        {
            get;
            set;
        }
    }
}
