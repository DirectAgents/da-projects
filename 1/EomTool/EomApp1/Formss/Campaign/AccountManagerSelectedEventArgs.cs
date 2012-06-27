using System;

namespace EomApp1.Formss.Campaign
{
    public class AccountManagerSelectedEventArgs : EventArgs
    {
        public AccountManagerSelectedEventArgs(int accountManagerId, string accountManagerName)
        {
            AccountManagerId = accountManagerId;
            AccountManagerName = accountManagerName;
        }
        public int AccountManagerId { get; set; }
        public string AccountManagerName { get; set; }
    }
}
