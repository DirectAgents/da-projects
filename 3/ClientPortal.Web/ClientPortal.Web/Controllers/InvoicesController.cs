using ClientPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Controllers
{
    public class InvoicesController : Controller
    {
        //
        // GET: /Invoices/

        public ActionResult Index()
        {
            return PartialView(GetInvoices());
        }

        private List<InvoiceModel> GetInvoices()
        {
            var result = new List<InvoiceModel>();
            string conStr = @"data source=biz2\da;initial catalog=QB_Import_Intl;User Id=sa;Password=sp0ngbOb;";
            string customerRefListID = "80000103-1285963347";
            string query = "select * from InvoiceLines where [CustomerRefListID]=@1";
            using (var con = new SqlConnection(conStr))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@1", customerRefListID);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var invoiceModel = new InvoiceModel
                    {
                        TxnDate = DateTime.Parse((string)reader["TxnDate"]),
                        Amount = decimal.Parse((string)reader["InvoiceLineAmount"]),
                        Currency = new string(((string)reader["ARAccountRefFullName"]).ToCharArray().Reverse().Take(3).Reverse().ToArray()),
                        ClassName = (string)reader["ClassRefFullName"],
                        RefNumber = (string)reader["RefNumber"],
                        Memo = (string)reader["Memo"],

                    };
                    result.Add(invoiceModel);
                }
            }
            return result.OrderByDescending(c => c.TxnDate).ToList();
        }
    }
}
//[TxnID]
//,[TimeCreated]
//,[TimeModified]
//,[EditSequence]
//,[TxnNumber]
//,[CustomerRefListID]
//,[CustomerRefFullName]
//,[ClassRefListID]
//,[ClassRefFullName]
//,[ARAccountRefListID]
//,[ARAccountRefFullName]
//,[TemplateRefListID]
//,[TemplateRefFullName]
//,[TxnDate]
//,[TxnDateMacro]
//,[RefNumber]
//,[BillAddressAddr1]
//,[BillAddressAddr2]
//,[BillAddressAddr3]
//,[BillAddressAddr4]
//,[BillAddressAddr5]
//,[BillAddressCity]
//,[BillAddressState]
//,[BillAddressProvince]
//,[BillAddressCounty]
//,[BillAddressPostalCode]
//,[BillAddressCountry]
//,[BillAddressNote]
//,[BillAddressBlockAddr1]
//,[BillAddressBlockAddr2]
//,[BillAddressBlockAddr3]
//,[BillAddressBlockAddr4]
//,[BillAddressBlockAddr5]
//,[ShipAddressAddr1]
//,[ShipAddressAddr2]
//,[ShipAddressAddr3]
//,[ShipAddressAddr4]
//,[ShipAddressAddr5]
//,[ShipAddressCity]
//,[ShipAddressState]
//,[ShipAddressProvince]
//,[ShipAddressCounty]
//,[ShipAddressPostalCode]
//,[ShipAddressCountry]
//,[ShipAddressNote]
//,[ShipAddressBlockAddr1]
//,[ShipAddressBlockAddr2]
//,[ShipAddressBlockAddr3]
//,[ShipAddressBlockAddr4]
//,[ShipAddressBlockAddr5]
//,[IsPending]
//,[IsFinanceCharge]
//,[PONumber]
//,[TermsRefListID]
//,[TermsRefFullName]
//,[DueDate]
//,[SalesRepRefListID]
//,[SalesRepRefFullName]
//,[FOB]
//,[ShipDate]
//,[ShipMethodRefListID]
//,[ShipMethodRefFullName]
//,[Subtotal]
//,[ItemSalesTaxRefListID]
//,[ItemSalesTaxRefFullName]
//,[SalesTaxPercentage]
//,[SalesTaxTotal]
//,[AppliedAmount]
//,[BalanceRemaining]
//,[Memo]
//,[IsPaid]
//,[CustomerMsgRefListID]
//,[CustomerMsgRefFullName]
//,[IsToBePrinted]
//,[IsToBeEmailed]
//,[IsTaxIncluded]
//,[CustomerSalesTaxCodeRefListID]
//,[CustomerSalesTaxCodeRefFullName]
//,[CustomerTaxCodeRefListID]
//,[SuggestedDiscountAmount]
//,[SuggestedDiscountDate]
//,[CustomFieldOther]
//,[LinkToTxnID1]
//,[InvoiceLineType]
//,[InvoiceLineSeqNo]
//,[InvoiceLineGroupTxnLineID]
//,[InvoiceLineGroupItemGroupRefListID]
//,[InvoiceLineGroupItemGroupRefFullName]
//,[InvoiceLineGroupDesc]
//,[InvoiceLineGroupQuantity]
//,[InvoiceLineGroupUnitOfMeasure]
//,[InvoiceLineGroupOverrideUOMSetRefListID]
//,[InvoiceLineGroupOverrideUOMSetRefFullName]
//,[InvoiceLineGroupIsPrintItemsInGroup]
//,[InvoiceLineGroupTotalAmount]
//,[InvoiceLineGroupServiceDate]
//,[InvoiceLineGroupSeqNo]
//,[InvoiceLineTxnLineID]
//,[InvoiceLineItemRefListID]
//,[InvoiceLineItemRefFullName]
//,[InvoiceLineDesc]
//,[InvoiceLineQuantity]
//,[InvoiceLineUnitOfMeasure]
//,[InvoiceLineOverrideUOMSetRefListID]
//,[InvoiceLineOverrideUOMSetRefFullName]
//,[InvoiceLineRate]
//,[InvoiceLineRatePercent]
//,[InvoiceLinePriceLevelRefListID]
//,[InvoiceLinePriceLevelRefFullName]
//,[InvoiceLineClassRefListID]
//,[InvoiceLineClassRefFullName]
//,[InvoiceLineAmount]
//,[InvoiceLineTaxAmount]
//,[InvoiceLineServiceDate]
//,[InvoiceLineSalesTaxCodeRefListID]
//,[InvoiceLineSalesTaxCodeRefFullName]
//,[InvoiceLineTaxCodeRefListID]
//,[InvoiceLineTaxCodeRefFullName]
//,[InvoiceLineOverrideItemAccountRefListID]
//,[InvoiceLineOverrideItemAccountRefFullName]
//,[CustomFieldInvoiceLineOther1]
//,[CustomFieldInvoiceLineOther2]
//,[InvoiceLineLinkToTxnTxnID]
//,[InvoiceLineLinkToTxnTxnLineID]
//,[Tax1Total]
//,[Tax2Total]
//,[ExchangeRate]
//,[AmountIncludesVAT]
//,[FQSaveToCache]
//,[FQPrimaryKey]
//,[FQTxnLinkKey]
//,[CustomFieldInvoiceLineGroupOther1]
//,[CustomFieldInvoiceLineGroupOther2]