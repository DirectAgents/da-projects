using System.Linq;
using AccountingBackupWeb.Models.AccountingBackup;
using System.Collections.Generic;
using System;

namespace QuickBooksService
{
    public class Loader_CreditMemo : Loader<CreditMemo>
    {
        public Loader_CreditMemo(Company company)
            : base(company)
        {
        }

        protected override string TargetElementName { get { return "CreditMemo"; } }

        protected override CreditMemo GetOrCreate(dynamic source)
        {
            CreditMemo creditMemo;

            string creditMemoKey = (string)source.Company + source.TxnID;

            creditMemo = Model.CreditMemoes.FirstOrDefault(c =>
                    c.TxnID == creditMemoKey &&
                    c.Customer.QuickBooksCompany.Id == Company.Id);

            string logMessage;

            if (creditMemo == null)
            {
                logMessage = "creating " + creditMemoKey;
                creditMemo = new CreditMemo();
                creditMemo.TxnID = creditMemoKey;
            }
            else
            {
                logMessage = "updating " + creditMemoKey;
            }

            Logger.Log(logMessage);

            return creditMemo;
        }

        protected override void CopyFields(dynamic source, CreditMemo target)
        {
            // todo: IN THE MORNING
        }
    }

    public class Loader_Invoice : Loader<Invoice>
    {
        public Loader_Invoice(Company company)
            : base(company)
        {
        }

        protected override string TargetElementName { get { return "InvoiceLine"; } }

        protected override Invoice GetOrCreate(dynamic source)
        {
            Invoice invoice;

            string key = (string)source.Company + source.TxnID + "," + source.InvoiceLineSeqNo;

            // temp
            string key2 = (string)source.Company + source.TxnID;

            invoice = Model.Invoices.FirstOrDefault(c =>
                    c.TxnID == key &&
                    c.Customer.QuickBooksCompany.Id == Company.Id);

            if (invoice == null)
            {
                Logger.Log("creating " + key);

                invoice = new Invoice();

                invoice.TxnID = key;

                // temp
                invoice.TxnID2 = key2;
            }
            else
                Logger.Log("updating " + key);

            return invoice;
        }

        protected override void CopyFields(dynamic source, Invoice invoice)
        {
            string customerListID = source.CustomerRefListID;
            invoice.Customer = Model.Customers.First(c =>
                c.ListID == customerListID &&
                c.QuickBooksCompany.Id == Company.Id); // TODO: get or create?

            invoice.AccountReceivable = AccountReceivable.ByName(source.ARAccountRefFullName, Model, true); // todo: test

            invoice.TxnDate = source.TxnDate;

            invoice.RefNumber = source.RefNumber ?? string.Empty;

            invoice.IsPending = source.IsPending;

            invoice.Term = source.TermsRefFullName != null
                ? Terms.ByName(source.TermsRefFullName, Model, true)
                : Terms.ByName("Default", Model, true);

            invoice.DueDate = source.DueDate;

            invoice.AppliedAmount = source.AppliedAmount
                ?? 0m;

            invoice.BalanceRemaining = source.BalanceRemaining;

            invoice.Memo = source.Memo ?? string.Empty;

            invoice.IsPaid = source.IsPaid;

            invoice.LineType = source.InvoiceLineType;

            invoice.LineSeq = source.InvoiceLineSeqNo;

            invoice.LineDesc = source.InvoiceLineDesc ?? string.Empty;

            invoice.LineClass = source.InvoiceLineClassRefFullName ?? string.Empty;

            invoice.Description = string.Empty;

            invoice.Notes = string.Empty;
        }
    }
}
