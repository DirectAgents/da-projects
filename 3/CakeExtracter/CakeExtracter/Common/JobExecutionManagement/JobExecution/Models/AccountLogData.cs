using System.Collections.Generic;

namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement.Models
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
