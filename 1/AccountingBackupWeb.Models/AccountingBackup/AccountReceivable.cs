using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class AccountReceivable
    {
        /// <summary>
        /// By convention, the currency name is the last three letters of the Name
        /// </summary>
        public string CurrencyName
        {
            get { return Name.Substring(Name.Length - 3, 3); }
        }

        public static AccountReceivable ByName(string name, AccountingBackupEntities model, bool create)
        {
            return model.AccountsReceivable.FirstOrDefault(c => c.Name == name) ?? (create ? Create(model, name) : null);
        }

        private static AccountReceivable Create(AccountingBackupEntities model, string name)
        {
            var currencies = model.Currencies.ToList();

            string lastThreeCharsOfARAccountName = name.Substring(name.Length - 3, 3);

            var currency = currencies.FirstOrDefault(c => c.Name == lastThreeCharsOfARAccountName);

            if (currency == null)
                currency = currencies.First(c => c.Name == "USD");

            return new AccountReceivable {
                Name = name,
                Currency = currency
            };
        }
    }
}
