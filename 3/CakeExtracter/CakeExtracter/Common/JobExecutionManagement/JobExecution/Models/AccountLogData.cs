using System.Collections.Generic;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Models
{
    public class AccountLogData
    {
        public int AccountId
        {
            get;
            set;
        }

        public List<string> Messages
        {
            get;
            set;
        }
    }
}
