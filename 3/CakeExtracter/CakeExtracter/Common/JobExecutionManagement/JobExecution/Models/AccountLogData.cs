using System.Collections.Generic;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Models
{
    /// <summary>
    /// Account Log Data.
    /// </summary>
    public class AccountLogData
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public List<string> Messages { get; set; }
    }
}
