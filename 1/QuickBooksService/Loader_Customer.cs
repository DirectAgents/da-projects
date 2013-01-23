using System.Linq;
using System.Xml.Linq;
using AccountingBackupWeb.Models.AccountingBackup;

namespace QuickBooksService
{
    public class Loader_Customer : Loader<Customer>
    {
        public Loader_Customer(Company company)
            : base(company) { }

        protected override string TargetElementName { get { return "Customer"; } }

        protected override Customer GetOrCreate(dynamic source)
        {
            Customer customer;
            string fullName = source.FullName;

            customer = Model.Customers.FirstOrDefault(c =>
                    c.FullName == fullName &&
                    c.QuickBooksCompany.Id == Company.Id);

            if (customer == null)
            {
                Logger.Log("creating Customer " + fullName);

                customer = new AccountingBackupWeb.Models.AccountingBackup.Customer();
                Model.Customers.AddObject(customer);
            }
            else
            {
                Logger.Log("updating Customer " + fullName);
            }

            return customer;
        }

        protected override void CopyFields(dynamic source, Customer customer)
        {
            string fullName = source.FullName;

            customer.QuickBooksCompany = Company;

            string termName = source.TermsRefFullName;
            customer.Term = Terms.ByName(termName ?? "Net 30", Model, true);

            customer.Advertiser = Advertiser.ByCustomerName(Model, fullName); // pulls customer/advertiser mappings from the ABWeb_config database
            customer.Advertiser.Term = customer.Term;

            customer.FullName = fullName;

            customer.ListID = source.ListID;
        }
    }
}
