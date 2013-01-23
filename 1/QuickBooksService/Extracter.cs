using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;
using QuickBooksLibrary;

namespace QuickBooksService
{
    public class Extracter : ProgramAction
    {
        Company _company;
        QuickBooksCompanyFileQuery _quickBooksQuery;
        DateTime _since;
        eTargets _targets;
        string _outputFileName;

        public Extracter(
            Company company,
            QuickBooksCompanyFileQuery quickBooksQuery,
            DateTime dateTime,
            eTargets targets
        )
        {
            _company = company;
            _quickBooksQuery = quickBooksQuery;
            _since = dateTime;
            _targets = targets;
            _outputFileName = @"\\ad1\accounting$\Xml\qb_" + this._company.Name + ".xml";
        }

        IEnumerable<QuickBooksTable> QuickBooksTables
        {
            get
            {
                using (var model = new AccountingBackupEntities())
                {
                    return model.QuickBooksTables.ToList();
                }
            }
        }

        public override void Execute()
        {
            if (_quickBooksQuery.CanRun())
            {
                WriteExtract();
            }
            else
            {
                Logger.LogError(_quickBooksQuery.Error);
            }
        }

        private void WriteExtract()
        {
            var xmlWriterSettings = new XmlWriterSettings { Indent = true };
            using (var writer = XmlWriter.Create(_outputFileName, xmlWriterSettings))
            {
                writer.WriteStartElement("QB");
                if ((_targets & eTargets.All) > 0)
                {
                    ExtractAll(writer);
                }
                else
                {
                    ExtractTargets(writer);
                }
                writer.WriteEndElement();
            }
            Logger.Log("wrote " + _outputFileName);
        }

        void ExtractTargets(XmlWriter writer)
        {
            if ((_targets & eTargets.Customer) > 0)
            {
                ExtractCustomers(writer);
            }

            if ((_targets & eTargets.Invoice) > 0)
            {
                ExtractInvoices(writer);
            }

            if ((_targets & eTargets.Payment) > 0)
            {
                ExtractPayments(writer);
            }

            if ((_targets & eTargets.CreditMemo) > 0)
            {
                ExtractCreditMemos(writer);
            }
        }

        void ExtractCreditMemos(XmlWriter writer)
        {
            Extract(writer, "CreditMemo");
            Extract(writer, "CreditMemoLine");
            Extract(writer, "CreditMemoLinkedTxn");
        }

        void ExtractPayments(XmlWriter writer)
        {
            Extract(writer, "ReceivePaymentLine");
        }

        void ExtractInvoices(XmlWriter writer)
        {
            Extract(writer, "Invoice");
            Extract(writer, "InvoiceLine");
        }

        private void ExtractCustomers(XmlWriter writer)
        {
            Extract(writer, "Customer");
        }

        void ExtractAll(XmlWriter writer)
        {
            foreach (var quickBooksTable in QuickBooksTables)
            {
                Extract(writer, quickBooksTable.TABLENAME);
            }
        }

        void Extract(XmlWriter writer, string quickBooksTableName)
        {
            //ExtractSchema(writer, quickBooksTableName);
            ExtractData(writer, quickBooksTableName);
        }

        void ExtractSchema(XmlWriter writer, string quickBooksTableName)
        {
            Logger.Log("extracting schema from QB table " + quickBooksTableName);

            var dataTable = _quickBooksQuery.GetColumns(quickBooksTableName);
            dataTable.TableName += "_Schema";
            dataTable.WriteXml(writer);
        }

        void ExtractData(XmlWriter writer, string quickBooksTableName)
        {
            Logger.Log("extracting data from QB table " + quickBooksTableName);

            var dataTable = _quickBooksQuery.GetTable(quickBooksTableName);
            dataTable.WriteXml(writer);
        }
    }
}
