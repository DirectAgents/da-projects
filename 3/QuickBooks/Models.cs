using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace QuickBooks
{
    public class QuickBooksContext : DbContext
    {
        public DbSet<CompanyFile> CompanyFiles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<InvoiceLine> InvoiceLines { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
    }

    public class CompanyFile
    {
        [Key]
        public int CompanyId { get; set; }

        [MaxLength(255), Required]
        public string CompanyName { get; set; }
    }

    public partial class Customer
    {
        [Key, Column(Order = 0)]
        public int CompanyFileId { get; set; }

        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        [Required, MaxLength(16)]
        public string EditSequence { get; set; }

        [Required, MaxLength(41)]
        public string Name { get; set; }

        [MaxLength(209)]
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(41)]
        public string CompanyName { get; set; }

        [MaxLength(15)]
        public string Salutation { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(5)]
        public string MiddleName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        [MaxLength(41)]
        public string BillAddressAddr1 { get; set; }

        [MaxLength(41)]
        public string BillAddressAddr2 { get; set; }

        [MaxLength(41)]
        public string BillAddressAddr3 { get; set; }

        [MaxLength(41)]
        public string BillAddressAddr4 { get; set; }

        [MaxLength(41)]
        public string BillAddressAddr5 { get; set; }

        [MaxLength(31)]
        public string BillAddressCity { get; set; }

        [MaxLength(21)]
        public string BillAddressState { get; set; }

        [MaxLength(21)]
        public string BillAddressProvince { get; set; }

        [MaxLength(21)]
        public string BillAddressCounty { get; set; }

        [MaxLength(13)]
        public string BillAddressPostalCode { get; set; }

        [MaxLength(31)]
        public string BillAddressCountry { get; set; }

        [MaxLength(41)]
        public string BillAddressNote { get; set; }

        [MaxLength(41)]
        public string BillAddressBlockAddr1 { get; set; }

        [MaxLength(41)]
        public string BillAddressBlockAddr2 { get; set; }

        [MaxLength(41)]
        public string BillAddressBlockAddr3 { get; set; }

        [MaxLength(41)]
        public string BillAddressBlockAddr4 { get; set; }

        [MaxLength(41)]
        public string BillAddressBlockAddr5 { get; set; }

        [MaxLength(41)]
        public string ShipAddressAddr1 { get; set; }

        [MaxLength(41)]
        public string ShipAddressAddr2 { get; set; }

        [MaxLength(41)]
        public string ShipAddressAddr3 { get; set; }

        [MaxLength(41)]
        public string ShipAddressAddr4 { get; set; }

        [MaxLength(41)]
        public string ShipAddressAddr5 { get; set; }

        [MaxLength(31)]
        public string ShipAddressCity { get; set; }

        [MaxLength(21)]
        public string ShipAddressState { get; set; }

        [MaxLength(21)]
        public string ShipAddressProvince { get; set; }

        [MaxLength(21)]
        public string ShipAddressCounty { get; set; }

        [MaxLength(13)]
        public string ShipAddressPostalCode { get; set; }

        [MaxLength(31)]
        public string ShipAddressCountry { get; set; }

        [MaxLength(41)]
        public string ShipAddressNote { get; set; }

        [MaxLength(41)]
        public string ShipAddressBlockAddr1 { get; set; }

        [MaxLength(41)]
        public string ShipAddressBlockAddr2 { get; set; }

        [MaxLength(41)]
        public string ShipAddressBlockAddr3 { get; set; }

        [MaxLength(41)]
        public string ShipAddressBlockAddr4 { get; set; }

        [MaxLength(41)]
        public string ShipAddressBlockAddr5 { get; set; }

        [MaxLength(21)]
        public string Phone { get; set; }

        [MaxLength(21)]
        public string AltPhone { get; set; }

        [MaxLength(21)]
        public string Fax { get; set; }

        [MaxLength(1023)]
        public string Email { get; set; }

        [MaxLength(41)]
        public string Contact { get; set; }

        [MaxLength(41)]
        public string AltContact { get; set; }

        [MaxLength(36)]
        public string CustomerTypeRefListID { get; set; }

        [MaxLength(159)]
        public string CustomerTypeRefFullName { get; set; }

        [MaxLength(36)]
        public string TermsRefListID { get; set; }

        [MaxLength(31)]
        public string TermsRefFullName { get; set; }

        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } 

        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; }

        public decimal Balance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal OpenBalance { get; set; }

        public DateTime OpenBalanceDate { get; set; }

        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } 

        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; }

        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } 

        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; }

        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; }

        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; }

        [MaxLength(31)]
        public string SalesTaxCountry { get; set; }

        [MaxLength(15)]
        public string ResaleNumber { get; set; }

        [MaxLength(99)]
        public string AccountNumber { get; set; }

        [MaxLength(99)]
        public string BusinessNumber { get; set; }

        public decimal CreditLimit { get; set; }

        [MaxLength(36)]
        public string PreferredPaymentMethodRefListID { get; set; } 

        [MaxLength(31)]
        public string PreferredPaymentMethodRefFullName { get; set; } 

        [MaxLength(25)]
        public string CreditCardInfoCreditCardNumber { get; set; }

        public int CreditCardInfoExpirationMonth { get; set; }

        public int CreditCardInfoExpirationYear { get; set; }

        [MaxLength(41)]
        public string CreditCardInfoNameOnCard { get; set; }

        [MaxLength(41)]
        public string CreditCardInfoCreditCardAddress { get; set; }

        [MaxLength(41)]
        public string CreditCardInfoCreditCardPostalCode { get; set; }

        [MaxLength(10)]
        public string JobStatus { get; set; }

        public DateTime JobStartDate { get; set; }

        public DateTime JobProjectedEndDate { get; set; }

        public DateTime JobEndDate { get; set; }

        [MaxLength(99)]
        public string JobDesc { get; set; }

        [MaxLength(36)]
        public string JobTypeRefListID { get; set; } 

        [MaxLength(159)]
        public string JobTypeRefFullName { get; set; } 

        [MaxLength(4095)]
        public string Notes { get; set; }

        public bool IsUsingCustomerTaxCode { get; set; }

        [MaxLength(36)]
        public string PriceLevelRefListID { get; set; } 

        [MaxLength(31)]
        public string PriceLevelRefFullName { get; set; }

        [MaxLength(40)]
        public string ExternalGUID { get; set; }

        [MaxLength(30)]
        public string TaxRegistrationNumber { get; set; }

        [MaxLength(36)]
        public string CurrencyRefListID { get; set; }

        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }
    }

    public partial class InvoiceLine
    {
        [Key, Column(Order = 0)]
        public int CompanyFileId { get; set; }

        [MaxLength(36)]
        public string TxnID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        [Required, MaxLength(16)]
        public string EditSequence { get; set; }

        public int TxnNumber { get; set; }

        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; }

        [ForeignKey("CompanyFileId,CustomerRefListID")]
        public virtual Customer Customer { get; set; }

        [MaxLength(36)]
        public string ClassRefListID { get; set; }

        [MaxLength(159)]
        public string ClassRefFullName { get; set; }

        [MaxLength(36)]
        public string ARAccountRefListID { get; set; }

        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; }

        [MaxLength(36)]
        public string TemplateRefListID { get; set; }

        [MaxLength(159)]
        public string TemplateRefFullName { get; set; }

        public DateTime TxnDate { get; set; }

        [MaxLength(11)]
        public string RefNumber { get; set; }

        public bool IsPending { get; set; }

        public bool IsFinanceCharge { get; set; }

        [MaxLength(25)]
        public string PONumber { get; set; }

        [MaxLength(36)]
        public string TermsRefListID { get; set; }

        [MaxLength(31)]
        public string TermsRefFullName { get; set; }

        public DateTime DueDate { get; set; }

        [MaxLength(36)]
        public string SalesRepRefListID { get; set; }

        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; }

        [MaxLength(13)]
        public string FOB { get; set; }

        public DateTime ShipDate { get; set; }

        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } 

        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; }

        public decimal Subtotal { get; set; }

        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } 

        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; }

        public decimal SalesTaxPercentage { get; set; }

        public decimal SalesTaxTotal { get; set; }

        public decimal AppliedAmount { get; set; }

        public decimal BalanceRemaining { get; set; }

        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } 

        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal BalanceRemainingInHomeCurrency { get; set; }

        [MaxLength(4095)]
        public string Memo { get; set; }

        public bool IsPaid { get; set; }

        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; }

        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; }

        public bool IsToBePrinted { get; set; }

        public bool IsToBeEmailed { get; set; }

        public bool IsTaxIncluded { get; set; }

        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; }

        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; }

        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; }

        public decimal SuggestedDiscountAmount { get; set; }

        public DateTime SuggestedDiscountDate { get; set; }

        [MaxLength(29)]
        public string CustomFieldOther { get; set; }

        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }

        [MaxLength(11)]
        public string InvoiceLineType { get; set; }

        public int InvoiceLineSeqNo { get; set; }

        [MaxLength(36)]
        public string InvoiceLineGroupTxnLineID { get; set; }

        [MaxLength(36)]
        public string InvoiceLineGroupItemGroupRefListID { get; set; }

        [MaxLength(31)]
        public string InvoiceLineGroupItemGroupRefFullName { get; set; }

        [MaxLength(4095)]
        public string InvoiceLineGroupDesc { get; set; }

        public decimal InvoiceLineGroupQuantity { get; set; }

        [MaxLength(31)]
        public string InvoiceLineGroupUnitOfMeasure { get; set; }

        [MaxLength(36)]
        public string InvoiceLineGroupOverrideUOMSetRefListID { get; set; } 

        [MaxLength(31)]
        public string InvoiceLineGroupOverrideUOMSetRefFullName { get; set; }

        public bool InvoiceLineGroupIsPrintItemsInGroup { get; set; }

        public decimal InvoiceLineGroupTotalAmount { get; set; }

        public DateTime InvoiceLineGroupServiceDate { get; set; }

        public int InvoiceLineGroupSeqNo { get; set; }

        [MaxLength(36)]
        public string InvoiceLineTxnLineID { get; set; }

        [MaxLength(36)]
        public string InvoiceLineItemRefListID { get; set; } 

        [MaxLength(159)]
        public string InvoiceLineItemRefFullName { get; set; } 

        [MaxLength(4095)]
        public string InvoiceLineDesc { get; set; }

        public decimal InvoiceLineQuantity { get; set; }

        [MaxLength(31)]
        public string InvoiceLineUnitOfMeasure { get; set; } 

        [MaxLength(36)]
        public string InvoiceLineOverrideUOMSetRefListID { get; set; }

        [MaxLength(31)]
        public string InvoiceLineOverrideUOMSetRefFullName { get; set; } 

        public decimal InvoiceLineRate { get; set; }

        public decimal InvoiceLineRatePercent { get; set; }

        [MaxLength(36)]
        public string InvoiceLinePriceLevelRefListID { get; set; }

        [MaxLength(159)]
        public string InvoiceLinePriceLevelRefFullName { get; set; } 

        [MaxLength(36)]
        public string InvoiceLineClassRefListID { get; set; }

        [MaxLength(159)]
        public string InvoiceLineClassRefFullName { get; set; }

        public decimal InvoiceLineAmount { get; set; }

        public decimal InvoiceLineTaxAmount { get; set; }

        public DateTime InvoiceLineServiceDate { get; set; }

        [MaxLength(36)]
        public string InvoiceLineSalesTaxCodeRefListID { get; set; } 

        [MaxLength(3)]
        public string InvoiceLineSalesTaxCodeRefFullName { get; set; }

        [MaxLength(36)]
        public string InvoiceLineTaxCodeRefListID { get; set; } 

        [MaxLength(3)]
        public string InvoiceLineTaxCodeRefFullName { get; set; } 

        [MaxLength(36)]
        public string InvoiceLineOverrideItemAccountRefListID { get; set; }

        [MaxLength(159)]
        public string InvoiceLineOverrideItemAccountRefFullName { get; set; }

        [MaxLength(29)]
        public string CustomFieldInvoiceLineOther1 { get; set; }

        [MaxLength(29)]
        public string CustomFieldInvoiceLineOther2 { get; set; }

        [MaxLength(36)]
        public string InvoiceLineLinkToTxnTxnID { get; set; }

        [MaxLength(36)]
        public string InvoiceLineLinkToTxnTxnLineID { get; set; }

        public decimal Tax1Total { get; set; }

        public decimal Tax2Total { get; set; }

        public bool AmountIncludesVAT { get; set; }

        public bool FQSaveToCache { get; set; }

        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }

        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }

    public partial class Invoice
    {
        [Key, Column(Order = 0)]
        public int CompanyFileId { get; set; }

        [Required, MaxLength(36)]
        public string TxnID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        [Required, MaxLength(16)]
        public string EditSequence { get; set; }

        public int TxnNumber { get; set; }

        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; }

        [ForeignKey("CompanyId,CustomerRefListID")]
        public virtual Customer CustomerRefCustomer { get; set; }

        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; }

        [MaxLength(36)]
        public string ClassRefListID { get; set; }

        [MaxLength(159)]
        public string ClassRefFullName { get; set; }

        [MaxLength(36)]
        public string ARAccountRefListID { get; set; }

        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } 

        [MaxLength(36)]
        public string TemplateRefListID { get; set; }

        [MaxLength(159)]
        public string TemplateRefFullName { get; set; }

        public DateTime TxnDate { get; set; }

        [MaxLength(23)]
        public string TxnDateMacro { get; set; }

        [MaxLength(11)]
        public string RefNumber { get; set; }

        public bool IsPending { get; set; }

        public bool IsFinanceCharge { get; set; }

        [MaxLength(25)]
        public string PONumber { get; set; }

        [MaxLength(36)]
        public string TermsRefListID { get; set; }

        [MaxLength(31)]
        public string TermsRefFullName { get; set; }

        public DateTime DueDate { get; set; }

        [MaxLength(36)]
        public string SalesRepRefListID { get; set; }

        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; }

        [MaxLength(13)]
        public string FOB { get; set; }

        public DateTime ShipDate { get; set; }

        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } 

        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } 

        public decimal Subtotal { get; set; }

        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } 

        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; }

        public decimal SalesTaxPercentage { get; set; }

        public decimal SalesTaxTotal { get; set; }

        public decimal AppliedAmount { get; set; }

        public decimal BalanceRemaining { get; set; }

        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } 

        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } 

        public decimal ExchangeRate { get; set; }

        public decimal BalanceRemainingInHomeCurrency { get; set; }

        [MaxLength(4095)]
        public string Memo { get; set; }

        public bool IsPaid { get; set; }

        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } 

        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } 

        public bool IsToBePrinted { get; set; }

        public bool IsToBeEmailed { get; set; }

        public bool IsTaxIncluded { get; set; }

        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } 

        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } 

        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; }

        public decimal SuggestedDiscountAmount { get; set; }

        public DateTime SuggestedDiscountDate { get; set; }

        [MaxLength(29)]
        public string CustomFieldOther { get; set; }

        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }

        public decimal Tax1Total { get; set; }

        public decimal Tax2Total { get; set; }

        public bool AmountIncludesVAT { get; set; }
    }
}
