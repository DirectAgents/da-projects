using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DirectAgents.Domain.Entities.RevTrack;

namespace DirectAgents.Domain.Entities.AB
{
    public class ABClient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }

        private ClientAccount _defaultAccount;
        public ClientAccount DefaultAccount()
        {
            if (_defaultAccount != null)
                return _defaultAccount;

            if (ClientAccounts == null || !ClientAccounts.Any())
                _defaultAccount = new ClientAccount(); // ? guarantees returning non-null
            else
                _defaultAccount = ClientAccounts.First();

            return _defaultAccount;
        }
        public decimal DefaultAccountBudgetFor(DateTime date)
        {
            var defaultAccount = DefaultAccount();
            if (defaultAccount == null)
                return 0;
            return defaultAccount.BudgetFor(date);
        }

        //? DefaultExtCredit, DefaultIntCredit ?
    }

    public class ClientAccount
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual ABClient Client { get; set; }
        public decimal ExtCredit { get; set; }
        public decimal IntCredit { get; set; }

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
        public DateTime Date { get; set; }
        public virtual ClientAccount ClientAccount { get; set; }
        public decimal Value { get; set; }
    }

    //TODO
    // do proof-of-concept with ChildFund
    //
    // credit limit
    // plan other stuff... e.g. budgets - by service/dept - each month
}
