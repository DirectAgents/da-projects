using System.Collections.Generic;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Models
{
    /// <summary>
    /// Job Execution Log Data Model.
    /// </summary>
    public class JobExecutionLogData
    {
        /// <summary>
        /// Gets or sets the common messages.
        /// </summary>
        public List<string> CommonMessages { get; set; }

        /// <summary>
        /// Gets or sets the accounts data.
        /// </summary>
        public List<AccountLogData> AccountsData { get; set; }
    }
}
