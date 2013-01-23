using System.Linq;
using System.Xml.Linq;
using AccountingBackupWeb.Models.AccountingBackup;

namespace QuickBooksService
{
    public class Loader_Payment : Loader<ReceivedPayment>
    {
        public Loader_Payment(Company company)
            : base(company) { }

        protected override string TargetElementName { get { return "ReceivePaymentLine"; } }

        protected override ReceivedPayment GetOrCreate(dynamic source)
        {
            string company = source.Company;
            string fqPrimaryKey = source.FQPrimaryKey;
            string key = Unique(fqPrimaryKey);

            ReceivedPayment res;

            var existingPayment = from c in Model.ReceivedPayments
                                  where c.FQPrimaryKey == key && c.Customer.QuickBooksCompany.Id == Company.Id
                                  select c;

            res = existingPayment.FirstOrDefault();

            if (res == null)
            {
                Logger.Log("creating " + fqPrimaryKey);
                res = new ReceivedPayment();
                res.FQPrimaryKey = key;
            }
            else
            {
                Logger.Log("updating " + fqPrimaryKey);
            }

            return res;
        }

        public string Unique(string input)
        {
            string res = input;
            res = Company.Name + input;
            return res;
        }

        protected override void CopyFields(dynamic source, ReceivedPayment payment)
        {
            string customerListID = source.CustomerRefListID;
            payment.TxnID = Unique(source.TxnID);
            payment.Customer = Model.Customers.First(c => c.ListID == customerListID && c.QuickBooksCompany.Id == Company.Id);
            payment.ARAccount = AccountReceivable.ByName(source.ARAccountRefFullName, Model, true);
            payment.TxnDate = source.TxnDate;
            payment.TotalAmount = source.TotalAmount;
            payment.Memo = source.Memo;
            payment.AppliedToTxnTxnID = Unique(source.AppliedToTxnTxnID);
            payment.AppliedToTxnTxnType = source.AppliedToTxnTxnType;
            payment.AppliedToTxnAmount = source.AppliedToTxnAmount;
            payment.RefNumber = source.RefNumber ?? string.Empty;
        }
    }
}
