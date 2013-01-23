using System.Collections.Generic;
using System.Linq;
using AccountingBackupWeb.Models.AccountingBackup;
using AccountingBackupWeb.Models.Staging;
using DAgents.Common;

namespace QuickBooksService
{
    public class StartingBalanceLoader : ProgramAction
    {
        class AdvertiserCustomerPair
        {
            public AdvertiserCustomerPair(Advertiser advertiser, Customer customer)
            {
                Advertiser = advertiser;
                Customer = customer;
            }

            public Advertiser Advertiser { get; set; }

            public Customer Customer { get; set; }

            public override bool Equals(object obj)
            {
                var a = obj as AdvertiserCustomerPair;
                return (Advertiser.Id == a.Advertiser.Id) && (Customer.Id == a.Customer.Id);
            }

            public override int GetHashCode()
            {
                var a = Advertiser.Id.GetHashCode();
                var b = Customer.Id.GetHashCode();
                return a ^ b;
            }
        }

        public override void Execute()
        {
            var seen = new List<AdvertiserCustomerPair>();

            var startingBalances =
                from c in CustomerStartingBalance.GetAll()
                where c.StartingBalanceAmount != 0
                select c;

            using (var model = AccountingBackupEntities.Instance)
            {
                //var entryTypes = model.EntryTypes.ToDictionary(c => Enum.Parse(typeof(eEntryTypes), c.Name), c => c.Description);

                foreach (var startingBalance in startingBalances)
                {
                    string startingBalanceCustomerName = startingBalance.CustomerName;

                    // get customers with matching name

                    var customers =
                        from c in model.Customers
                        where c.FullName == startingBalanceCustomerName
                        select c;

                    // more than one customer? report and skip

                    if (customers.Count() == 0)
                    {
                        Logger.LogError(string.Format("customer {0} not found, skipping..", startingBalanceCustomerName));

                        continue;
                    }
                    else if (customers.Count() != 1)
                    {
                        Logger.LogError(string.Format("multiple customers found for {0}, skipping..", startingBalanceCustomerName));

                        continue;
                    }

                    // get advertiser from customer
                    
                    var customer = customers.First();
                    string customerName = customer.FullName;

                    var advertiser = customer.Advertiser;
                    string advertiserName = advertiser.Name;

                    // have we seen this advertiser yet? if so, report and skip

                    var pair = new AdvertiserCustomerPair(advertiser, customer);

                    var existing = seen.FirstOrDefault(c => c.Equals(pair));

                    if (existing != null)
                    {
                        Logger.LogError(string.Format("customer {0} has same mapping as customer {1} which was already used to set the starting balance for advertiser {2}, skipping..", 
                            startingBalance.CustomerName, customerName, advertiserName));

                        continue;
                    }
                    else
                    {
                        seen.Add(pair);
                    }

                    // is the advertiser currency name the same as the starting balance currency name? if not, report and skip

                    if (advertiser.Currency.Name != startingBalance.CurrencyName)
                    {
                        Logger.LogError(string.Format("for customer {0} mapped to advertiser {1} with currency {2} does not match starting balance currency {3}, skipping..",
                            startingBalance.CustomerName, advertiser.Name, advertiser.Currency.Name, startingBalance.CurrencyName));

                        continue;
                    }

                    // set the advertiser starting balance

                    advertiser.StartingBalance = startingBalance.StartingBalanceAmount;

                    // save changes

                    model.SaveChanges();

                    Logger.Log(string.Format("{0} starting balance is now {1} {2}",
                        customer.FullName, startingBalance.CurrencyName, startingBalance.StartingBalanceAmount));
                }
            }
        }
    }
}
