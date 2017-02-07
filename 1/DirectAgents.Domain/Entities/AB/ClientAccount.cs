using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Domain.Entities.AB
{
    // NOT CURRENTLY USED

    public class ClientAccount
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual ABClient Client { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProtoCampaign> ProtoCampaigns { get; set; }
        public virtual ICollection<ProtoPaymentBit> ProtoPaymentBits { get; set; }

        public virtual ICollection<AccountBudget> AccountBudgets { get; set; }

        //public AccountBudget AccountBudgetFor(DateTime date)
        public decimal BudgetFor(DateTime date)
        {
            if (AccountBudgets == null)
                return 0;
            var accountBudget = AccountBudgets.Where(x => x.Date == date).FirstOrDefault();
            if (accountBudget == null)
                return 0;
            return accountBudget.Value;
        }
    }

    public class AccountBudget
    {
        public int ClientAccountId { get; set; }
        public virtual ClientAccount ClientAccount { get; set; }

        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

}
