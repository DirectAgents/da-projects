using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using EomApp1.Formss.AB2.Model;
using System.Collections.Generic;
using AccountingBackupWeb.Models.AccountingBackup;
using System.Text.RegularExpressions;

namespace QuickBooksService
{
    public class Program2
    {
        private EomApp1.QuickBooks.QuickBooksQuery _service;
        private string[] _args;

        public Program2()
        {
        }

        public void Push(string[] args)
        {
            string inputFile = args[1];

            Log("pushing data from " + inputFile + " to QB");

            var x = XDocument.Load(inputFile);

            // top level node must be QB
            if (x.Root.Name != "QB")
                throw new Exception("invalid input file, root node must be named QB");

            // QB children are taken to be collections of objects to push
            foreach (var item in x.Root.Elements())
            {
                var elements = item.Elements();
                int count = elements.Count();
                string type = item.Name.LocalName;
                Console.WriteLine(string.Format("found {0} {1}", count, type));
                foreach (var element in elements)
                {
                    Push(element);
                }
            }
        }

        private void Push(XElement element)
        {
            string type = element.Name.LocalName;

            using (var model = new AccountingBackupEntities())
            {
                dynamic x = new DAgents.Common.Utilities.XSettings(element);

                Company qbc = GetCompany(model, x.QuickBooksCompanyFileName);

                Log(string.Format("pushing {0} {1}", qbc.Name, type));

                if (type == "Customer")
                {
                    AccountingBackupWeb.Models.AccountingBackup.Customer o;

                    string fullName = x.FullName;

                    o = model.Customers.FirstOrDefault(c =>
                            c.FullName == fullName &&
                            c.QuickBooksCompany.Id == qbc.Id);

                    if (o == null)
                    {
                        Log("creating customer " + fullName);

                        o = new AccountingBackupWeb.Models.AccountingBackup.Customer();
                    }
                    else
                        Log("updating customer " + fullName);

                    o.QuickBooksCompany = qbc;
                    o.Advertiser = model.Advertisers.FirstOrDefault(c => c.Name == fullName);
                    o.FullName = fullName;
                    o.ListID = x.ListID;

                    Log("saving");

                    model.SaveChanges();
                }

                if (type == "Invoice")
                {
                    Invoice o;

                    string txnID = x.TxnID;

                    o = model.Invoices.FirstOrDefault(c =>
                            c.TxnID == txnID &&
                            c.Customer.QuickBooksCompany.Id == qbc.Id);

                    if (o == null)
                    {
                        Log("creating invoice " + txnID);

                        o = new Invoice();

                        o.TxnID = txnID;
                    }
                    else
                        Log("updating invoice " + txnID);

                    string customerListID = x.CustomerRefListID;

                    o.Customer = model.Customers.First(c =>
                        c.ListID == customerListID &&
                        c.QuickBooksCompany.Id == qbc.Id);
                    o.TxnDate = x.TxnDate;
                    o.RefNumber = x.RefNumber;
                    o.Term = x.TermsRefFullName != null ? GetTerms(model, x.TermsRefFullName) : null;
                    o.AppliedAmount = x.AppliedAmount;
                    o.BalanceRemaining = x.BalanceRemaining;

                    Log("saving");

                    model.SaveChanges();
                }

                if (type == "ReceivedPayment")
                {
                    ReceivedPayment o;

                    string txnID = x.TxnID;

                    o = model.ReceivedPayments.FirstOrDefault(c =>
                            c.TxnID == txnID &&
                            c.Customer.QuickBooksCompany.Id == qbc.Id);

                    if (o == null)
                    {
                        Log("creating received payment" + txnID);

                        o = new ReceivedPayment();

                        o.TxnID = txnID;
                    }
                    else
                        Log("updating received payment " + txnID);

                    string customerListID = x.CustomerRefListID;

                    o.Customer = model.Customers.First(c =>
                                    c.ListID == customerListID &&
                                    c.QuickBooksCompany.Id == qbc.Id);
                    o.ARAccount = GetAccountReceivable(model, x.ARAccountRefFullName);
                    o.TxnDate = x.TxnDate;
                    o.TotalAmount = x.TotalAmount;
                    o.Memo = x.Memo;

                    // TODO: invoice columns

                    Log("saving");

                    model.SaveChanges();
                }
            }
        }

        private AccountReceivable GetAccountReceivable(AccountingBackupEntities model, string name)
        {
            var o = model.AccountsReceivable.FirstOrDefault(c => c.Name == name)
                ?? new AccountReceivable {
                    Name = name
                };

            o.Currency = GetCurrency(model, o.CurrencyName);

            return o;
        }

        private Currency GetCurrency(AccountingBackupEntities model, string name)
        {
            var o = model.Currencies.FirstOrDefault(c => c.Name == name)
                ?? new Currency {
                    Name = name
                };

            return o;
        }

        // idea; attribute to "publish side effects.." to be visible, optionally, at call site in srouce
        // ex; [Effect, Returns Existing Or Creates New]
        private Company GetCompany(AccountingBackupEntities model, string company)
        {
            var q = from c in model.Companies
                    where c.Name == company
                    select c;

            Company qbc;

            if (q.Count() == 0)
            {
                Log("creating qb company");

                qbc = new Company();
                qbc.Name = company;
                model.Companies.AddObject(qbc);
            }
            else
            {
                qbc = q.First();
            }

            return qbc;
        }

        private Terms GetTerms(AccountingBackupEntities model, string name)
        {
            var q = from c in model.Terms
                    where c.Name == name
                    select c;

            Terms t;

            if (q.Count() == 0)
            {
                Log("creating terms");

                t = new Terms();
                t.Name = name;
                t.Days = 30; // TODO: unhardcode
                model.Terms.AddObject(t);
            }
            else
            {
                t = q.First();
            }

            return t;
        }

        public Program2(string[] args)
        {
            this._args = args;
        }

        static void Main(string[] args)
        {
            if (args[0] == "dump")
            {
                new QuickBooksTableWriter().Run();
            }

            if (args[0] == "push")
            {
                new Program2().Push(args);
            }

            if (args[0] == "run")
            {
                var service = new EomApp1.QuickBooks.QuickBooksQuery();
                service.Init();
                if (service.ErrorMessage != String.Empty)
                    Log(service.ErrorMessage);
                else
                    Log(service.CompanyFile);
                string queryName = "GetCustomers";
                string queryFileName = "QodbcQueries2.xml";
                string companyName = "UK";
                string nameAttr = "name";
                string qbCompanyElmName = "QuickBooksCompanyFileName";
                F(service, queryName, queryFileName, companyName, nameAttr, qbCompanyElmName);
                F(service, "GetInvoices", queryFileName, companyName, nameAttr, qbCompanyElmName);
                F(service, "GetReceivedPayments", queryFileName, companyName, nameAttr, qbCompanyElmName);
            }
        }

        private static void F(EomApp1.QuickBooks.QuickBooksQuery service, string queryName, string queryFileName, string companyName, string nameAttr, string qbCompanyElmName)
        {
            var dataTable = new DataTable(queryName);
            var query = XDocument
                .Load(queryFileName)
                .Root
                .Elements()
                .FirstOrDefault(c => c.Attribute(nameAttr).Value == queryName);
            if (query != null)
            {
                Log(query.Value);
                if (service.FillData(dataTable, query.Value))
                {
                    // Add a column to identify the rows by company name
                    var addCol = dataTable.Columns.Add(qbCompanyElmName);
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item.SetField<string>(addCol, companyName);
                    }
                    dataTable.AcceptChanges();
                }
                else throw new Exception(service.ErrorMessage);
            }
            else throw new Exception("query name not found");
            dataTable.WriteXml(Console.Out);
        }

        static void Log(string format, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Console.WriteLine(format, formatArgs);
            }
            else
            {
                Console.WriteLine(format);
            }
        }

        /// <summary>
        /// Main Logic
        /// </summary>
        public void Go()
        {
            if (_args[0] == "-query")
            {
                ConnectToQuickBooks();
                DataTable queryResultTable = QueryQuickbooks(_args[1], _args[2]);
                WriteResultFiles(_args[1], _args[2], queryResultTable);
            }
            else if (_args[0] == "-push")
            {
                Save((RecordType)Enum.Parse(typeof(RecordType), _args[1]), _args[2]);
            }
        }

        private void ConnectToQuickBooks()
        {
            _service = new EomApp1.QuickBooks.QuickBooksQuery();
            _service.Init();
            if (_service.ErrorMessage != String.Empty)
                Log(_service.ErrorMessage);
            else
                Log(_service.CompanyFile);
        }

        enum RecordType
        {
            Customers,
            ReceivedPayments
        }

        private void Save(RecordType recordType, string xmlDataFile)
        {
            XDocument data = XDocument.Load(xmlDataFile);
            switch (recordType)
            {
                case RecordType.Customers:
                    SaveCustomers(data);
                    break;
                case RecordType.ReceivedPayments:
                    SaveReceivedPayments(data);
                    break;
                default:
                    throw new Exception("invalid record type");
            }
        }

        private void SaveReceivedPayments(XDocument data)
        {
            var receivedPayments = data.Root.Elements("GetReceivedPayments");

            using (var model = new DirectAgentsEntities())
            {
                foreach (var receivedPayment in receivedPayments)
                {
                    // These are always present in the XML element for a customer
                    string companyFileName = receivedPayment.Element("QuickBooksCompanyFileName").Value;
                    string txnID = receivedPayment.Element("TxnID").Value;

                    // Check customer ref exists in the XML element
                    string customerRefListID;
                    if (receivedPayment.Element("CustomerRefListID") == null)
                    {
                        Log("{0} is missing customer ref, skipping", txnID);
                        continue;
                    }
                    else
                    {
                        customerRefListID = receivedPayment.Element("CustomerRefListID").Value;
                    }

                    // Check customer record exists in database
                    var qbCustomer = model.QuickBooksCustomers.FirstOrDefault(c => c.ListId == customerRefListID && c.QuickBooksCompanyFile.Name == companyFileName);
                    if (qbCustomer == null)
                    {
                        Log("{0} is missing customer record, skipping", txnID);
                        continue;
                    }

                    Log("TxnID: {0}", txnID);

                    // The company file record already exists
                    var companyFile = model.QuickBooksCompanyFiles.First(c => c.Name == companyFileName);

                    // The received payment may or may not exist
                    var qbReceivedPayment = model.QuickBooksReceivedPayments.FirstOrDefault(c => c.TxnId == txnID && c.QuickBooksCompanyFile.Name == companyFileName);

                    if (qbReceivedPayment == null)
                    {
                        qbReceivedPayment = new QuickBooksReceivedPayment();
                        model.QuickBooksReceivedPayments.AddObject(qbReceivedPayment);
                    }

                    // Populate fields
                    qbReceivedPayment.TxnId = txnID;
                    qbReceivedPayment.TxnNumber = receivedPayment.Element("TxnNumber").Value;
                    qbReceivedPayment.ARAccountRefFullName = receivedPayment.Element("ARAccountRefFullName").Value;
                    qbReceivedPayment.TxnDate = DateTime.Parse(receivedPayment.Element("TxnDate").Value);
                    qbReceivedPayment.Memo = receivedPayment.Element("Memo") != null ? receivedPayment.Element("Memo").Value : null;
                    qbReceivedPayment.PaymentMethodRefFullName = receivedPayment.Element("PaymentMethodRefFullName") != null ? receivedPayment.Element("PaymentMethodRefFullName").Value : "unknown";
                    qbReceivedPayment.QuickBooksCustomer = qbCustomer;
                    qbReceivedPayment.QuickBooksCompanyFile = companyFile;
                }
                model.SaveChanges();
            }
        }

        private void SaveCustomers(XDocument data)
        {
            var customers = data.Root.Elements("GetCustomers");

            using (var model = new DirectAgentsEntities())
            {
                foreach (var customer in customers)
                {
                    Log("ListID: {0}", customer.Element("ListID"));

                    // These are always present in the XML element for a customer
                    string companyFileName = customer.Element("QuickBooksCompanyFileName").Value;
                    string listID = customer.Element("ListID").Value;

                    // The company file record already exists
                    var companyFile = model.QuickBooksCompanyFiles.First(c => c.Name == companyFileName);

                    // The customer may or may not exist
                    var qbCustmer = model.QuickBooksCustomers.FirstOrDefault(c => c.ListId == listID && c.QuickBooksCompanyFile.Name == companyFileName);

                    if (qbCustmer == null)
                    {
                        qbCustmer = new QuickBooksCustomer();
                        model.QuickBooksCustomers.AddObject(qbCustmer);
                    }

                    // Populate fields
                    qbCustmer.ListId = listID;
                    qbCustmer.FullName = customer.Element("FullName").Value;
                    qbCustmer.CompanyName = customer.Element("CompanyName") != null ? customer.Element("CompanyName").Value : "unknown";
                    qbCustmer.Phone = customer.Element("Phone") != null ? customer.Element("Phone").Value : "unknown";
                    qbCustmer.Email = customer.Element("Email") != null ? customer.Element("Email").Value : "unknown";
                    qbCustmer.TermsRefFullName = customer.Element("TermsRefFullName") != null ? customer.Element("TermsRefFullName").Value : "unknown";
                    qbCustmer.QuickBooksCompanyFile = companyFile;
                }
                model.SaveChanges();
            }
        }

        DataTable QueryQuickbooks(string queryName, string companyName)
        {
            DataTable dataTable = new DataTable(queryName);
            XElement query = XDocument.Load("QodbcQueries.xml")
                                    .Root
                                    .Elements()
                                    .FirstOrDefault(c => c.Attribute("name").Value == queryName);
            if (query != null)
            {
                Log(query.Value);
                if (_service.FillData(dataTable, query.Value))
                {
                    // Add a column to identify the rows by company name
                    var addCol = dataTable.Columns.Add("QuickBooksCompanyFileName");
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item.SetField<string>(addCol, companyName);
                    }
                    dataTable.AcceptChanges();
                }
                else throw new Exception(_service.ErrorMessage);
            }
            else throw new Exception("query name not found");

            return dataTable;
        }

        private void WriteResultFiles(string queryName, string companyName, DataTable queryResultTable)
        {
            queryResultTable.WriteXmlSchema(companyName + "_" + queryName + ".xsd");
            queryResultTable.WriteXml(companyName + "_" + queryName + ".xml");
        }
    }
}
