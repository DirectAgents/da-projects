﻿using System;
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
        public decimal ExtCredit { get; set; }
        public decimal IntCredit { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<ClientBudget> ClientBudgets { get; set; }
        public virtual ICollection<ClientPayment> ClientPayments { get; set; }

        public decimal BudgetFor(DateTime date)
        {
            if (ClientBudgets == null)
                return 0;
            var clientBudget = ClientBudgets.Where(x => x.Date == date).FirstOrDefault();
            if (clientBudget == null)
                return 0;
            return clientBudget.Value;
        }

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
    }

    public class ClientBudget
    {
        public int ClientId { get; set; }
        public virtual ABClient Client { get; set; }

        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

}
