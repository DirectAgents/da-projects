using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;

namespace QuickBooksService
{
    public class LoaderInputReader : ProgramObject, IEnumerable<XElement>
    {
        public LoaderInputReader(string inputFile, Company company, eTargets targets)
        {
            // TODO: make it so this is not needed by writing correctly to a stream in the writer...
            InputDoc = XDocument.Load(new StreamReader(inputFile, Encoding.ASCII));
            QBCompany = company;
            Targets = targets;
        }

        public XDocument InputDoc { get; set; }
        public Company QBCompany { get; set; }
        public eTargets Targets { get; set; }

        public IEnumerator<XElement> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerable<XElement> Elements
        {
            get
            {
                //int take = 1;

                XElement root = InputDoc.Root;

                if ((Targets & eTargets.Customer) > 0)
                {
                    foreach (var customer in root.XPathSelectElements("//Customer")
                        //.Take(take)
                        )
                    {
                        yield return customer;
                    }
                }

                if ((Targets & eTargets.Invoice) > 0)
                {
                    foreach (var invoiceLine in root.XPathSelectElements("//InvoiceLine")
                        //.Take(take)
                        )
                    {
                        yield return invoiceLine;
                    }
                }

                if ((Targets & eTargets.Payment) > 0)
                {
                    foreach (var paymentLine in root.XPathSelectElements("//ReceivePaymentLine")
                        //.Take(take)
                        )
                    {
                        yield return paymentLine;
                    }
                }
            }
        }
    }
}
