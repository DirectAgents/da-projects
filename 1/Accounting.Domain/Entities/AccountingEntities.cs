using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Accounting.Domain.Entities
{
    public class CompanyFile
    {
        [Key]
        public int CompanyId { get; set; }
        [MaxLength(255), Required]
        public string CompanyName { get; set; }
    }

    public partial class AccountingContext : DbContext
    {
        public DbSet<CompanyFile> CompanyFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesTaxCode>()
                .HasOptional(c => c.ItemPurchaseTaxRefSalesTaxCode)
                .WithMany()
                .HasForeignKey(c => new { c.CompanyId, c.ItemPurchaseTaxRefListID });

            modelBuilder.Entity<SalesTaxCode>()
                .HasOptional(c => c.ItemSalesTaxRefSalesTaxCode)
                .WithMany()
                .HasForeignKey(c => new { c.CompanyId, c.ItemSalesTaxRefListID });
        }
    }

    #region QB Code Gen
    public partial class AccountingContext : DbContext
    {
        //public DbSet<Account> Accounts { get; set; }
        //public DbSet<AccountTaxLineInfo> AccountTaxLineInfoes { get; set; }
        //public DbSet<ARRefundCreditCard> ARRefundCreditCards { get; set; }
        //public DbSet<ARRefundCreditCardRefundAppliedTo> ARRefundCreditCardRefundAppliedToes { get; set; }
        //public DbSet<Bill> Bills { get; set; }
        //public DbSet<BillExpenseLine> BillExpenseLines { get; set; }
        //public DbSet<BillItemLine> BillItemLines { get; set; }
        //public DbSet<BillLinkedTxn> BillLinkedTxns { get; set; }
        //public DbSet<BillingRate> BillingRates { get; set; }
        //public DbSet<BillingRateLine> BillingRateLines { get; set; }
        //public DbSet<BillPaymentCheck> BillPaymentChecks { get; set; }
        //public DbSet<BillPaymentCheckLine> BillPaymentCheckLines { get; set; }
        //public DbSet<BillPaymentCreditCard> BillPaymentCreditCards { get; set; }
        //public DbSet<BillPaymentCreditCardLine> BillPaymentCreditCardLines { get; set; }
        //public DbSet<BillToPay> BillToPays { get; set; }
        //public DbSet<BuildAssembly> BuildAssemblies { get; set; }
        //public DbSet<BuildAssemblyComponentItemLine> BuildAssemblyComponentItemLines { get; set; }
        //public DbSet<Charge> Charges { get; set; }
        //public DbSet<ChargeLinkedTxn> ChargeLinkedTxns { get; set; }
        //public DbSet<Check> Checks { get; set; }
        //public DbSet<CheckApplyCheckToTxn> CheckApplyCheckToTxns { get; set; }
        //public DbSet<CheckExpenseLine> CheckExpenseLines { get; set; }
        //public DbSet<CheckItemLine> CheckItemLines { get; set; }
        //public DbSet<Class> Classes { get; set; }
        //public DbSet<ClearedStatus> ClearedStatus { get; set; }
        //public DbSet<Company> Companies { get; set; }
        //public DbSet<CompanyActivity> CompanyActivities { get; set; }
        //public DbSet<CreditCardCharge> CreditCardCharges { get; set; }
        //public DbSet<CreditCardChargeExpenseLine> CreditCardChargeExpenseLines { get; set; }
        //public DbSet<CreditCardChargeItemLine> CreditCardChargeItemLines { get; set; }
        //public DbSet<CreditCardCredit> CreditCardCredits { get; set; }
        //public DbSet<CreditCardCreditExpenseLine> CreditCardCreditExpenseLines { get; set; }
        //public DbSet<CreditCardCreditItemLine> CreditCardCreditItemLines { get; set; }
        //public DbSet<CreditMemo> CreditMemoes { get; set; }
        //public DbSet<CreditMemoLine> CreditMemoLines { get; set; }
        //public DbSet<CreditMemoLinkedTxn> CreditMemoLinkedTxns { get; set; }
        //public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<CustomerMsg> CustomerMsgs { get; set; }
        //public DbSet<CustomerType> CustomerTypes { get; set; }
        //public DbSet<CustomField> CustomFields { get; set; }
        //public DbSet<DateDrivenTerms> DateDrivenTerms { get; set; }
        //public DbSet<Deposit> Deposits { get; set; }
        //public DbSet<DepositLine> DepositLines { get; set; }
        //public DbSet<Employee> Employees { get; set; }
        //public DbSet<EmployeeEarning> EmployeeEarnings { get; set; }
        public DbSet<Entity> Entities { get; set; }
        //public DbSet<Estimate> Estimates { get; set; }
        //public DbSet<EstimateLine> EstimateLines { get; set; }
        //public DbSet<EstimateLinkedTxn> EstimateLinkedTxns { get; set; }
        //public DbSet<Host> Hosts { get; set; }
        //public DbSet<HostMetaData> HostMetaDatas { get; set; }
        //public DbSet<HostSupportedVersions> HostSupportedVersions { get; set; }
        //public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }
        //public DbSet<InventoryAdjustmentLine> InventoryAdjustmentLines { get; set; }
        //public DbSet<Invoice> Invoices { get; set; }
        //public DbSet<InvoiceLine> InvoiceLines { get; set; }
        //public DbSet<InvoiceLinkedTxn> InvoiceLinkedTxns { get; set; }
        //public DbSet<Item> Items { get; set; }
        //public DbSet<ItemAssembliesCanBuild> ItemAssembliesCanBuilds { get; set; }
        //public DbSet<ItemDiscount> ItemDiscounts { get; set; }
        //public DbSet<ItemFixedAsset> ItemFixedAssets { get; set; }
        //public DbSet<ItemGroup> ItemGroups { get; set; }
        //public DbSet<ItemGroupLine> ItemGroupLines { get; set; }
        //public DbSet<ItemInventory> ItemInventories { get; set; }
        //public DbSet<ItemInventoryAssembly> ItemInventoryAssemblies { get; set; }
        //public DbSet<ItemInventoryAssemblyLine> ItemInventoryAssemblyLines { get; set; }
        //public DbSet<ItemNonInventory> ItemNonInventories { get; set; }
        //public DbSet<ItemOtherCharge> ItemOtherCharges { get; set; }
        //public DbSet<ItemPayment> ItemPayments { get; set; }
        //public DbSet<ItemReceipt> ItemReceipts { get; set; }
        //public DbSet<ItemReceiptExpenseLine> ItemReceiptExpenseLines { get; set; }
        //public DbSet<ItemReceiptItemLine> ItemReceiptItemLines { get; set; }
        //public DbSet<ItemReceiptLinkedTxn> ItemReceiptLinkedTxns { get; set; }
        //public DbSet<ItemSalesTax> ItemSalesTaxes { get; set; }
        //public DbSet<ItemSalesTaxGroup> ItemSalesTaxGroups { get; set; }
        //public DbSet<ItemSalesTaxGroupLine> ItemSalesTaxGroupLines { get; set; }
        //public DbSet<ItemService> ItemServices { get; set; }
        //public DbSet<ItemSubtotal> ItemSubtotals { get; set; }
        //public DbSet<JobType> JobTypes { get; set; }
        //public DbSet<JournalEntry> JournalEntries { get; set; }
        //public DbSet<JournalEntryCreditLine> JournalEntryCreditLines { get; set; }
        //public DbSet<JournalEntryDebitLine> JournalEntryDebitLines { get; set; }
        //public DbSet<JournalEntryLine> JournalEntryLines { get; set; }
        //public DbSet<ListDeleted> ListDeleteds { get; set; }
        //public DbSet<OtherName> OtherNames { get; set; }
        //public DbSet<PaymentMethod> PaymentMethods { get; set; }
        //public DbSet<PayrollItemNonWage> PayrollItemNonWages { get; set; }
        //public DbSet<PayrollItemWage> PayrollItemWages { get; set; }
        //public DbSet<Preferences> Preferences { get; set; }
        //public DbSet<PriceLevel> PriceLevels { get; set; }
        //public DbSet<PriceLevelPerItem> PriceLevelPerItems { get; set; }
        //public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        //public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        //public DbSet<PurchaseOrderLinkedTxn> PurchaseOrderLinkedTxns { get; set; }
        //public DbSet<ReceivePayment> ReceivePayments { get; set; }
        //public DbSet<ReceivePaymentLine> ReceivePaymentLines { get; set; }
        //public DbSet<ReceivePaymentToDeposit> ReceivePaymentToDeposits { get; set; }
        //public DbSet<Sales> Sales { get; set; }
        //public DbSet<SalesLine> SalesLines { get; set; }
        //public DbSet<SalesOrder> SalesOrders { get; set; }
        //public DbSet<SalesOrderLine> SalesOrderLines { get; set; }
        //public DbSet<SalesOrderLinkedTxn> SalesOrderLinkedTxns { get; set; }
        //public DbSet<SalesReceipt> SalesReceipts { get; set; }
        //public DbSet<SalesReceiptLine> SalesReceiptLines { get; set; }
        //public DbSet<SalesRep> SalesReps { get; set; }
        //public DbSet<SalesTaxCode> SalesTaxCodes { get; set; }
        //public DbSet<SalesTaxPaymentCheck> SalesTaxPaymentChecks { get; set; }
        //public DbSet<SalesTaxPaymentCheckLine> SalesTaxPaymentCheckLines { get; set; }
        //public DbSet<ShipMethod> ShipMethods { get; set; }
        //public DbSet<SpecialAccount> SpecialAccounts { get; set; }
        //public DbSet<SpecialItem> SpecialItems { get; set; }
        //public DbSet<StandardTerms> StandardTerms { get; set; }
        //public DbSet<TaxCode> TaxCodes { get; set; }
        //public DbSet<Template> Templates { get; set; }
        //public DbSet<Terms> Terms { get; set; }
        //public DbSet<TimeTracking> TimeTrackings { get; set; }
        //public DbSet<ToDo> ToDoes { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }
        //public DbSet<TxnDeleted> TxnDeleteds { get; set; }
        //public DbSet<UnitOfMeasureSet> UnitOfMeasureSets { get; set; }
        //public DbSet<UnitOfMeasureSetRelatedUnit> UnitOfMeasureSetRelatedUnits { get; set; }
        //public DbSet<UnitOfMeasureSetDefaultUnit> UnitOfMeasureSetDefaultUnits { get; set; }
        //public DbSet<Vehicle> Vehicles { get; set; }
        //public DbSet<VehicleMileage> VehicleMileages { get; set; }
        //public DbSet<Vendor> Vendors { get; set; }
        //public DbSet<VendorCredit> VendorCredits { get; set; }
        //public DbSet<VendorCreditExpenseLine> VendorCreditExpenseLines { get; set; }
        //public DbSet<VendorCreditItemLine> VendorCreditItemLines { get; set; }
        //public DbSet<VendorCreditLinkedTxn> VendorCreditLinkedTxns { get; set; }
        //public DbSet<VendorType> VendorTypes { get; set; }
        //public DbSet<WorkersCompCode> WorkersCompCodes { get; set; }
        //public DbSet<WorkersCompCodeRateHistory> WorkersCompCodeRateHistories { get; set; }
    }
    public partial class Account
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [Required, MaxLength(21)]
        public string AccountType { get; set; }
        [MaxLength(30)]
        public string SpecialAccountType { get; set; }
        public bool IsTaxAccount { get; set; }
        [MaxLength(7)]
        public string AccountNumber { get; set; }
        [MaxLength(25)]
        public string BankNumber { get; set; }
        [MaxLength(200)]
        public string Desc { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal OpenBalance { get; set; }
        public DateTime OpenBalanceDate { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Account -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: Account -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public int TaxLineInfoRetTaxLineID { get; set; } // Relates To: AccountTaxLineInfo.TaxLineID
        [MaxLength(256)]
        public string TaxLineInfoRetTaxLineName { get; set; } // Relates To: AccountTaxLineInfo.TaxLineName
        [MaxLength(13)]
        public string CashFlowClassification { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: Account -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
    }
    public partial class AccountTaxLineInfo
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required]
        public int TaxLineID { get; set; }
        [MaxLength(256)]
        public string TaxLineName { get; set; }
    }
    public partial class ARRefundCreditCard
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: ARRefundCreditCard -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string RefundFromAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account RefundFromAccountRefAccount { get; set; } // Navigation: ARRefundCreditCard -> Account
        [MaxLength(159)]
        public string RefundFromAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: ARRefundCreditCard -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: ARRefundCreditCard -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(25)]
        public string CreditCardTxnInfoInputCreditCardNumber { get; set; }
        public int CreditCardTxnInfoInputExpirationMonth { get; set; }
        public int CreditCardTxnInfoInputExpirationYear { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputNameOnCard { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardAddress { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardPostalCode { get; set; }
        [MaxLength(50)]
        public string CreditCardTxnInfoInputCommercialCardCode { get; set; }
        [MaxLength(14)]
        public string CreditCardTxnInfoInputTransactionMode { get; set; }
        [MaxLength(18)]
        public string CreditCardTxnInfoInputCreditCardTxnType { get; set; }
        public int CreditCardTxnInfoResultResultCode { get; set; }
        [MaxLength(500)]
        public string CreditCardTxnInfoResultResultMessage { get; set; }
        [MaxLength(24)]
        public string CreditCardTxnInfoResultCreditCardTransID { get; set; }
        [MaxLength(32)]
        public string CreditCardTxnInfoResultMerchantAccountNumber { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAuthorizationCode { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSStreet { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSZip { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultCardSecurityCodeMatch { get; set; }
        [MaxLength(84)]
        public string CreditCardTxnInfoResultReconBatchID { get; set; }
        public int CreditCardTxnInfoResultPaymentGroupingCode { get; set; }
        [MaxLength(9)]
        public string CreditCardTxnInfoResultPaymentStatus { get; set; }
        public DateTime CreditCardTxnInfoResultTxnAuthorizationTime { get; set; }
        public int CreditCardTxnInfoResultTxnAuthorizationStamp { get; set; }
        [MaxLength(16)]
        public string CreditCardTxnInfoResultClientTransID { get; set; }
    }
    public partial class ARRefundCreditCardRefundAppliedTo
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: ARRefundCreditCard.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: ARRefundCreditCardRefundAppliedTo -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string RefundFromAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account RefundFromAccountRefAccount { get; set; } // Navigation: ARRefundCreditCardRefundAppliedTo -> Account
        [MaxLength(159)]
        public string RefundFromAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: ARRefundCreditCardRefundAppliedTo -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: ARRefundCreditCardRefundAppliedTo -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(25)]
        public string CreditCardTxnInfoInputCreditCardNumber { get; set; }
        public int CreditCardTxnInfoInputExpirationMonth { get; set; }
        public int CreditCardTxnInfoInputExpirationYear { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputNameOnCard { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardAddress { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardPostalCode { get; set; }
        [MaxLength(50)]
        public string CreditCardTxnInfoInputCommercialCardCode { get; set; }
        [MaxLength(14)]
        public string CreditCardTxnInfoInputTransactionMode { get; set; }
        [MaxLength(18)]
        public string CreditCardTxnInfoInputCreditCardTxnType { get; set; }
        public int CreditCardTxnInfoResultResultCode { get; set; }
        [MaxLength(500)]
        public string CreditCardTxnInfoResultResultMessage { get; set; }
        [MaxLength(24)]
        public string CreditCardTxnInfoResultCreditCardTransID { get; set; }
        [MaxLength(32)]
        public string CreditCardTxnInfoResultMerchantAccountNumber { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAuthorizationCode { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSStreet { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSZip { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultCardSecurityCodeMatch { get; set; }
        [MaxLength(84)]
        public string CreditCardTxnInfoResultReconBatchID { get; set; }
        public int CreditCardTxnInfoResultPaymentGroupingCode { get; set; }
        [MaxLength(9)]
        public string CreditCardTxnInfoResultPaymentStatus { get; set; }
        public DateTime CreditCardTxnInfoResultTxnAuthorizationTime { get; set; }
        public int CreditCardTxnInfoResultTxnAuthorizationStamp { get; set; }
        [MaxLength(16)]
        public string CreditCardTxnInfoResultClientTransID { get; set; }
        [Required, MaxLength(36)]
        public string RefundAppliedToTxnTxnID { get; set; }
        [MaxLength(14)]
        public string RefundAppliedToTxnTxnType { get; set; }
        public DateTime RefundAppliedToTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string RefundAppliedToTxnRefNumber { get; set; }
        public decimal RefundAppliedToTxnRefCreditRemaining { get; set; }
        public decimal RefundAppliedToTxnRefRefundAmount { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Bill
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: Bill -> Vendor
        [Required, MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: Bill -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: Bill -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Bill -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: Bill -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal ExchangeRate { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public bool IsPaid { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class BillExpenseLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Bill.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: BillExpenseLine -> Vendor
        [Required, MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillExpenseLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: BillExpenseLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: BillExpenseLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: BillExpenseLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal ExchangeRate { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public bool IsPaid { get; set; }
        public bool ExpenseLineClearExpenseLines { get; set; }
        public int ExpenseLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ExpenseLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseLineAccountRefAccount { get; set; } // Navigation: BillExpenseLine -> Account
        [MaxLength(159)]
        public string ExpenseLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ExpenseLineAmount { get; set; }
        public decimal ExpenseLineTaxAmount { get; set; }
        public decimal ExpenseLineTax1Amount { get; set; }
        [MaxLength(4095)]
        public string ExpenseLineMemo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ExpenseLineCustomerRefCustomer { get; set; } // Navigation: BillExpenseLine -> Customer
        [MaxLength(209)]
        public string ExpenseLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ExpenseLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ExpenseLineClassRefClass { get; set; } // Navigation: BillExpenseLine -> Class
        [MaxLength(159)]
        public string ExpenseLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ExpenseLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ExpenseLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: BillExpenseLine -> SalesTaxCode
        [MaxLength(36)]
        public string ExpenseLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ExpenseLineTaxCodeRefTaxCode { get; set; } // Navigation: BillExpenseLine -> TaxCode
        [MaxLength(3)]
        public string ExpenseLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ExpenseLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ExpenseLineBillableStatus { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class BillItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Bill.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: BillItemLine -> Vendor
        [Required, MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillItemLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: BillItemLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: BillItemLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: BillItemLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal ExchangeRate { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(11)]
        public string ItemLineType { get; set; }
        public int ItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemGroupRefItem { get; set; } // Navigation: BillItemLine -> Item
        [MaxLength(31)]
        public string ItemGroupLineItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        public decimal ItemGroupLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: BillItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemGroupLineTotalAmount { get; set; }
        public int ItemGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemLineItemRefItem { get; set; } // Navigation: BillItemLine -> Item
        [MaxLength(159)]
        public string ItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemLineDesc { get; set; }
        public decimal ItemLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: BillItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemLineCost { get; set; }
        public decimal ItemLineAmount { get; set; }
        public decimal ItemLineTaxAmount { get; set; }
        public decimal ItemLineTax1Amount { get; set; }
        [MaxLength(36)]
        public string ItemLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ItemLineCustomerRefCustomer { get; set; } // Navigation: BillItemLine -> Customer
        [MaxLength(209)]
        public string ItemLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ItemLineClassRefClass { get; set; } // Navigation: BillItemLine -> Class
        [MaxLength(159)]
        public string ItemLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ItemLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: BillItemLine -> SalesTaxCode
        [MaxLength(36)]
        public string ItemLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ItemLineTaxCodeRefTaxCode { get; set; } // Navigation: BillItemLine -> TaxCode
        [MaxLength(3)]
        public string ItemLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ItemLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ItemLineBillableStatus { get; set; }
        [MaxLength(36)]
        public string ItemLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ItemLineOverrideItemAccountRefAccount { get; set; } // Navigation: BillItemLine -> Account
        [MaxLength(159)]
        public string ItemLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string ItemLineLinkToTxnTxnID { get; set; }
        [MaxLength(36)]
        public string ItemLineLinkToTxnTxnLineID { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class BillLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Bill.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: BillLinkedTxn -> Vendor
        [Required, MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillLinkedTxn -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: BillLinkedTxn -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: BillLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: BillLinkedTxn -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal ExchangeRate { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public bool IsPaid { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(8)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class BillingRate
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(9)]
        public string BillingRateType { get; set; }
        public decimal FixedBillingRate { get; set; }
    }
    public partial class BillingRateLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(9)]
        public string BillingRateType { get; set; }
        public decimal FixedBillingRate { get; set; }
        [MaxLength(36)]
        public string BillingRateLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item BillingRateLineItemRefItem { get; set; } // Navigation: BillingRateLine -> Item
        [MaxLength(159)]
        public string BillingRateLineItemRefFullName { get; set; } // Relates To: Item.FullName
        public decimal BillingRateLineCustomRate { get; set; }
        public decimal BillingRateLineCustomRatePercent { get; set; }
        public decimal BillingRateLineAdjustPercentage { get; set; }
        [MaxLength(9)]
        public string BillingRateLineAdjustBillingRateRelativeTo { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class BillPaymentCheck
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: BillPaymentCheck -> Entity
        [Required, MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillPaymentCheck -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string BankAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account BankAccountRefAccount { get; set; } // Navigation: BillPaymentCheck -> Account
        [Required, MaxLength(159)]
        public string BankAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Amount { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: BillPaymentCheck -> Currency
        [MaxLength(159)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.FullName
        public decimal ExchangeRate { get; set; }
        public decimal AmountInHomeCurrency { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public bool IsToBePrinted { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
    }
    public partial class BillPaymentCheckLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: BillPaymentCheck.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: BillPaymentCheckLine -> Entity
        [Required, MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillPaymentCheckLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string BankAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account BankAccountRefAccount { get; set; } // Navigation: BillPaymentCheckLine -> Account
        [Required, MaxLength(159)]
        public string BankAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Amount { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: BillPaymentCheckLine -> Currency
        [MaxLength(159)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.FullName
        public decimal ExchangeRate { get; set; }
        public decimal AmountInHomeCurrency { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public bool IsToBePrinted { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public int AppliedToTxnSeqNo { get; set; }
        [Required, MaxLength(36)]
        public string AppliedToTxnTxnID { get; set; }
        public decimal AppliedToTxnPaymentAmount { get; set; }
        [MaxLength(14)]
        public string AppliedToTxnTxnType { get; set; }
        public DateTime AppliedToTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string AppliedToTxnRefNumber { get; set; }
        public decimal AppliedToTxnBalanceRemaining { get; set; }
        public decimal AppliedToTxnAmount { get; set; }
        [MaxLength(36)]
        public string AppliedToTxnSetCreditCreditTxnID { get; set; }
        public decimal AppliedToTxnSetCreditAppliedAmount { get; set; }
        public decimal AppliedToTxnDiscountAmount { get; set; }
        [MaxLength(36)]
        public string AppliedToTxnDiscountAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AppliedToTxnDiscountAccountRefAccount { get; set; } // Navigation: BillPaymentCheckLine -> Account
        [MaxLength(159)]
        public string AppliedToTxnDiscountAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string AppliedToTxnDiscountClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class AppliedToTxnDiscountClassRefClass { get; set; } // Navigation: BillPaymentCheckLine -> Class
        [MaxLength(159)]
        public string AppliedToTxnDiscountClassRefFullName { get; set; } // Relates To: Class.FullName
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class BillPaymentCreditCard
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: BillPaymentCreditCard -> Entity
        [Required, MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillPaymentCreditCard -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string CreditCardAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account CreditCardAccountRefAccount { get; set; } // Navigation: BillPaymentCreditCard -> Account
        [Required, MaxLength(159)]
        public string CreditCardAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
    }
    public partial class BillPaymentCreditCardLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: BillPaymentCreditCard.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: BillPaymentCreditCardLine -> Entity
        [Required, MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: BillPaymentCreditCardLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string CreditCardAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account CreditCardAccountRefAccount { get; set; } // Navigation: BillPaymentCreditCardLine -> Account
        [Required, MaxLength(159)]
        public string CreditCardAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public int AppliedToTxnSeqNo { get; set; }
        [Required, MaxLength(36)]
        public string AppliedToTxnTxnID { get; set; }
        public decimal AppliedToTxnPaymentAmount { get; set; }
        [MaxLength(14)]
        public string AppliedToTxnTxnType { get; set; }
        public DateTime AppliedToTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string AppliedToTxnRefNumber { get; set; }
        public decimal AppliedToTxnBalanceRemaining { get; set; }
        public decimal AppliedToTxnAmount { get; set; }
        [MaxLength(36)]
        public string AppliedToTxnSetCreditCreditTxnID { get; set; }
        public decimal AppliedToTxnSetCreditAppliedAmount { get; set; }
        public decimal AppliedToTxnDiscountAmount { get; set; }
        [MaxLength(36)]
        public string AppliedToTxnDiscountAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AppliedToTxnDiscountAccountRefAccount { get; set; } // Navigation: BillPaymentCreditCardLine -> Account
        [MaxLength(159)]
        public string AppliedToTxnDiscountAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string AppliedToTxnDiscountClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class AppliedToTxnDiscountClassRefClass { get; set; } // Navigation: BillPaymentCreditCardLine -> Class
        [MaxLength(159)]
        public string AppliedToTxnDiscountClassRefFullName { get; set; } // Relates To: Class.FullName
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class BillToPay
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: BillToPay -> Entity
        [MaxLength(159)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime DueDateCutoff { get; set; }
        [MaxLength(36)]
        public string BillToPayTxnID { get; set; }
        [MaxLength(14)]
        public string BillToPayTxnType { get; set; }
        public int BillToPayTxnNumber { get; set; }
        [MaxLength(36)]
        public string BillToPayAPAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account BillToPayAPAccountRefAccount { get; set; } // Navigation: BillToPay -> Account
        [MaxLength(159)]
        public string BillToPayAPAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime BillToPayTxnDate { get; set; }
        [MaxLength(20)]
        public string BillToPayRefNumber { get; set; }
        public DateTime BillToPayDueDate { get; set; }
        public decimal BillToPayAmountDue { get; set; }
        [MaxLength(36)]
        public string CreditToApplyTxnID { get; set; }
        [MaxLength(14)]
        public string CreditToApplyTxnType { get; set; }
        public int CreditToApplyTxnNumber { get; set; }
        [MaxLength(36)]
        public string CreditToApplyAPAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account CreditToApplyAPAccountRefAccount { get; set; } // Navigation: BillToPay -> Account
        [MaxLength(159)]
        public string CreditToApplyAPAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime CreditToApplyTxnDate { get; set; }
        [MaxLength(20)]
        public string CreditToApplyRefNumber { get; set; }
        public decimal CreditToApplyCreditRemaining { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class BuildAssembly
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string ItemInventoryAssemblyRefListID { get; set; } // Relates To: ItemInventoryAssembly.ListID
        public virtual ItemInventoryAssembly ItemInventoryAssemblyRefItemInventoryAssembly { get; set; } // Navigation: BuildAssembly -> ItemInventoryAssembly
        [Required, MaxLength(159)]
        public string ItemInventoryAssemblyRefFullName { get; set; } // Relates To: ItemInventoryAssembly.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPending { get; set; }
        [Required]
        public decimal QuantityToBuild { get; set; }
        public decimal QuantityCanBuild { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityOnSalesOrder { get; set; }
        public bool MarkPendingIfRequired { get; set; }
        public bool RemovePending { get; set; }
    }
    public partial class BuildAssemblyComponentItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: BuildAssembly.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string ItemInventoryAssemblyRefListID { get; set; } // Relates To: ItemInventoryAssembly.ListID
        public virtual ItemInventoryAssembly ItemInventoryAssemblyRefItemInventoryAssembly { get; set; } // Navigation: BuildAssemblyComponentItemLine -> ItemInventoryAssembly
        [Required, MaxLength(159)]
        public string ItemInventoryAssemblyRefFullName { get; set; } // Relates To: ItemInventoryAssembly.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPending { get; set; }
        [Required]
        public decimal QuantityToBuild { get; set; }
        public decimal QuantityCanBuild { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityOnSalesOrder { get; set; }
        public bool MarkPendingIfRequired { get; set; }
        public bool RemovePending { get; set; }
        public int ComponentItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ComponentItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ComponentItemLineItemRefItem { get; set; } // Navigation: BuildAssemblyComponentItemLine -> Item
        [MaxLength(159)]
        public string ComponentItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ComponentItemLineDesc { get; set; }
        public decimal ComponentItemLineQuantityOnHand { get; set; }
        public decimal ComponentItemLineQuantityNeeded { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Charge
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: Charge -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [Required, MaxLength(36)]
        public string ItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemRefItem { get; set; } // Navigation: Charge -> Item
        [Required, MaxLength(159)]
        public string ItemRefFullName { get; set; } // Relates To: Item.FullName
        public decimal Quantity { get; set; }
        [MaxLength(31)]
        public string UnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string OverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet OverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: Charge -> UnitOfMeasureSet
        [MaxLength(31)]
        public string OverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceRemaining { get; set; }
        [MaxLength(4095)]
        public string Desc { get; set; }
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: Charge -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: Charge -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        public DateTime BilledDate { get; set; }
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string OverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account OverrideItemAccountRefAccount { get; set; } // Navigation: Charge -> Account
        [MaxLength(159)]
        public string OverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool IsPaid { get; set; }
    }
    public partial class ChargeLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Charge.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: ChargeLinkedTxn -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [Required, MaxLength(36)]
        public string ItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemRefItem { get; set; } // Navigation: ChargeLinkedTxn -> Item
        [Required, MaxLength(159)]
        public string ItemRefFullName { get; set; } // Relates To: Item.FullName
        public decimal Quantity { get; set; }
        [MaxLength(31)]
        public string UnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string OverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet OverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: ChargeLinkedTxn -> UnitOfMeasureSet
        [MaxLength(31)]
        public string OverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceRemaining { get; set; }
        [MaxLength(4095)]
        public string Desc { get; set; }
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: ChargeLinkedTxn -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: ChargeLinkedTxn -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        public DateTime BilledDate { get; set; }
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string OverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account OverrideItemAccountRefAccount { get; set; } // Navigation: ChargeLinkedTxn -> Account
        [MaxLength(159)]
        public string OverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool IsPaid { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(8)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Check
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: Check -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: Check -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: Check -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal AmountInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Check -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: Check -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class CheckApplyCheckToTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Check.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CheckApplyCheckToTxn -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CheckApplyCheckToTxn -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: CheckApplyCheckToTxn -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal AmountInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CheckApplyCheckToTxn -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: CheckApplyCheckToTxn -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public int ApplyCheckToTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string ApplyCheckToTxnTxnID { get; set; }
        public decimal ApplyCheckToTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class CheckExpenseLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Check.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CheckExpenseLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CheckExpenseLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: CheckExpenseLine -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal AmountInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CheckExpenseLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: CheckExpenseLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public bool ExpenseLineClearExpenseLines { get; set; }
        public int ExpenseLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ExpenseLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseLineAccountRefAccount { get; set; } // Navigation: CheckExpenseLine -> Account
        [MaxLength(159)]
        public string ExpenseLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ExpenseLineAmount { get; set; }
        public decimal ExpenseLineTaxAmount { get; set; }
        public decimal ExpenseLineTax1Amount { get; set; }
        [MaxLength(4095)]
        public string ExpenseLineMemo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ExpenseLineCustomerRefCustomer { get; set; } // Navigation: CheckExpenseLine -> Customer
        [MaxLength(209)]
        public string ExpenseLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ExpenseLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ExpenseLineClassRefClass { get; set; } // Navigation: CheckExpenseLine -> Class
        [MaxLength(159)]
        public string ExpenseLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ExpenseLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ExpenseLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CheckExpenseLine -> SalesTaxCode
        [MaxLength(36)]
        public string ExpenseLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ExpenseLineTaxCodeRefTaxCode { get; set; } // Navigation: CheckExpenseLine -> TaxCode
        [MaxLength(3)]
        public string ExpenseLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ExpenseLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ExpenseLineBillableStatus { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class CheckItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Check.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CheckItemLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CheckItemLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: CheckItemLine -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal AmountInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CheckItemLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: CheckItemLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public bool ItemLineClearItemLines { get; set; }
        [MaxLength(11)]
        public string ItemLineType { get; set; }
        public int ItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemRefItem { get; set; } // Navigation: CheckItemLine -> Item
        [MaxLength(31)]
        public string ItemGroupLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemGroupLineDesc { get; set; }
        public decimal ItemGroupLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemGroupLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemGroupLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CheckItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemGroupLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemGroupLineTotalAmount { get; set; }
        public int ItemGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemLineTxnLineID { get; set; }
        [Required, MaxLength(36)]
        public string ItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemLineItemRefItem { get; set; } // Navigation: CheckItemLine -> Item
        [Required, MaxLength(159)]
        public string ItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemLineDesc { get; set; }
        public decimal ItemLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CheckItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemLineCost { get; set; }
        public decimal ItemLineAmount { get; set; }
        public decimal ItemLineTaxAmount { get; set; }
        public decimal ItemLineTax1Amount { get; set; }
        [MaxLength(36)]
        public string ItemLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ItemLineCustomerRefCustomer { get; set; } // Navigation: CheckItemLine -> Customer
        [MaxLength(209)]
        public string ItemLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ItemLineClassRefClass { get; set; } // Navigation: CheckItemLine -> Class
        [MaxLength(159)]
        public string ItemLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ItemLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CheckItemLine -> SalesTaxCode
        [MaxLength(36)]
        public string ItemLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ItemLineTaxCodeRefTaxCode { get; set; } // Navigation: CheckItemLine -> TaxCode
        [MaxLength(3)]
        public string ItemLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ItemLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ItemLineBillableStatus { get; set; }
        [MaxLength(36)]
        public string ItemLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ItemLineOverrideItemAccountRefAccount { get; set; } // Navigation: CheckItemLine -> Account
        [MaxLength(159)]
        public string ItemLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class Class
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
    }
    public partial class ClearedStatus
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        [MaxLength(36)]
        public string TxnLineID { get; set; }
        [Required, MaxLength(10)]
        public string ClearedStatusString { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Company
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required]
        public int Id { get; set; }
        public bool IsSampleCompany { get; set; }
        [MaxLength(59)]
        public string CompanyName { get; set; }
        [MaxLength(59)]
        public string LegalCompanyName { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        [MaxLength(41)]
        public string LegalAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string LegalAddressAddr2 { get; set; }
        [MaxLength(41)]
        public string LegalAddressAddr3 { get; set; }
        [MaxLength(41)]
        public string LegalAddressAddr4 { get; set; }
        [MaxLength(41)]
        public string LegalAddressAddr5 { get; set; }
        [MaxLength(31)]
        public string LegalAddressCity { get; set; }
        [MaxLength(21)]
        public string LegalAddressState { get; set; }
        [MaxLength(21)]
        public string LegalAddressProvince { get; set; }
        [MaxLength(21)]
        public string LegalAddressCounty { get; set; }
        [MaxLength(13)]
        public string LegalAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string LegalAddressCountry { get; set; }
        [MaxLength(41)]
        public string LegalAddressNote { get; set; }
        [MaxLength(41)]
        public string CompanyAddressForCustomerAddr1 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressForCustomerAddr2 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressForCustomerAddr3 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressForCustomerAddr4 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressForCustomerAddr5 { get; set; }
        [MaxLength(31)]
        public string CompanyAddressForCustomerCity { get; set; }
        [MaxLength(21)]
        public string CompanyAddressForCustomerState { get; set; }
        [MaxLength(21)]
        public string CompanyAddressForCustomerProvince { get; set; }
        [MaxLength(21)]
        public string CompanyAddressForCustomerCounty { get; set; }
        [MaxLength(13)]
        public string CompanyAddressForCustomerPostalCode { get; set; }
        [MaxLength(31)]
        public string CompanyAddressForCustomerCountry { get; set; }
        [MaxLength(41)]
        public string CompanyAddressForCustomerNote { get; set; }
        [MaxLength(41)]
        public string CompanyAddressBlockForCustomerAddr1 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressBlockForCustomerAddr2 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressBlockForCustomerAddr3 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressBlockForCustomerAddr4 { get; set; }
        [MaxLength(41)]
        public string CompanyAddressBlockForCustomerAddr5 { get; set; }
        [MaxLength(51)]
        public string Phone { get; set; }
        [MaxLength(51)]
        public string Fax { get; set; }
        [MaxLength(99)]
        public string Email { get; set; }
        [MaxLength(128)]
        public string CompanyWebSite { get; set; }
        [MaxLength(9)]
        public string FirstMonthFiscalYear { get; set; }
        [MaxLength(9)]
        public string FirstMonthIncomeTaxYear { get; set; }
        [MaxLength(255)]
        public string CompanyType { get; set; }
        [MaxLength(20)]
        public string EIN { get; set; }
        [MaxLength(11)]
        public string SSN { get; set; }
        [MaxLength(11)]
        public string TaxForm { get; set; }
        [MaxLength(16)]
        public string BusinessNumber { get; set; }
        [MaxLength(50)]
        public string SubscribedServicesServiceName { get; set; }
        [MaxLength(50)]
        public string SubscribedServicesServiceDomain { get; set; }
        [MaxLength(10)]
        public string SubscribedServicesServiceServiceStatus { get; set; }
    }
    public partial class CompanyActivity
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required]
        public int Id { get; set; }
        public DateTime LastRestoreTime { get; set; }
        public DateTime LastCondenseTime { get; set; }
    }
    public partial class CreditCardCharge
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CreditCardCharge -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CreditCardCharge -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditCardCharge -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: CreditCardCharge -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class CreditCardChargeExpenseLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: CreditCardCharge.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CreditCardChargeExpenseLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CreditCardChargeExpenseLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditCardChargeExpenseLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: CreditCardChargeExpenseLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public bool ExpenseLineClearExpenseLines { get; set; }
        public int ExpenseLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ExpenseLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseLineAccountRefAccount { get; set; } // Navigation: CreditCardChargeExpenseLine -> Account
        [MaxLength(159)]
        public string ExpenseLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ExpenseLineAmount { get; set; }
        public decimal ExpenseLineTaxAmount { get; set; }
        public decimal ExpenseLineTax1Amount { get; set; }
        [MaxLength(4095)]
        public string ExpenseLineMemo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ExpenseLineCustomerRefCustomer { get; set; } // Navigation: CreditCardChargeExpenseLine -> Customer
        [MaxLength(209)]
        public string ExpenseLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ExpenseLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ExpenseLineClassRefClass { get; set; } // Navigation: CreditCardChargeExpenseLine -> Class
        [MaxLength(159)]
        public string ExpenseLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ExpenseLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ExpenseLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditCardChargeExpenseLine -> SalesTaxCode
        [MaxLength(36)]
        public string ExpenseLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ExpenseLineTaxCodeRefTaxCode { get; set; } // Navigation: CreditCardChargeExpenseLine -> TaxCode
        [MaxLength(3)]
        public string ExpenseLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ExpenseLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ExpenseLineBillableStatus { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class CreditCardChargeItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: CreditCardCharge.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CreditCardChargeItemLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CreditCardChargeItemLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditCardChargeItemLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: CreditCardChargeItemLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public bool ItemLineClearItemLines { get; set; }
        [MaxLength(11)]
        public string ItemLineType { get; set; }
        public int ItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemRefItem { get; set; } // Navigation: CreditCardChargeItemLine -> Item
        [MaxLength(31)]
        public string ItemGroupLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemGroupLineDesc { get; set; }
        public decimal ItemGroupLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CreditCardChargeItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemGroupLineTotalAmount { get; set; }
        public int ItemGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemLineTxnLineID { get; set; }
        [Required, MaxLength(36)]
        public string ItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemLineItemRefItem { get; set; } // Navigation: CreditCardChargeItemLine -> Item
        [Required, MaxLength(159)]
        public string ItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemLineDesc { get; set; }
        public decimal ItemLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CreditCardChargeItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemLineCost { get; set; }
        public decimal ItemLineAmount { get; set; }
        public decimal ItemLineTaxAmount { get; set; }
        public decimal ItemLineTax1Amount { get; set; }
        [MaxLength(36)]
        public string ItemLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ItemLineCustomerRefCustomer { get; set; } // Navigation: CreditCardChargeItemLine -> Customer
        [MaxLength(209)]
        public string ItemLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ItemLineClassRefClass { get; set; } // Navigation: CreditCardChargeItemLine -> Class
        [MaxLength(159)]
        public string ItemLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ItemLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditCardChargeItemLine -> SalesTaxCode
        [MaxLength(36)]
        public string ItemLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ItemLineTaxCodeRefTaxCode { get; set; } // Navigation: CreditCardChargeItemLine -> TaxCode
        [MaxLength(3)]
        public string ItemLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ItemLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ItemLineBillableStatus { get; set; }
        [MaxLength(36)]
        public string ItemLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ItemLineOverrideItemAccountRefAccount { get; set; } // Navigation: CreditCardChargeItemLine -> Account
        [MaxLength(159)]
        public string ItemLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class CreditCardCredit
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CreditCardCredit -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CreditCardCredit -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class CreditCardCreditExpenseLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: CreditCardCredit.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CreditCardCreditExpenseLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CreditCardCreditExpenseLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool ExpenseLineClearExpenseLines { get; set; }
        public int ExpenseLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ExpenseLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseLineAccountRefAccount { get; set; } // Navigation: CreditCardCreditExpenseLine -> Account
        [MaxLength(159)]
        public string ExpenseLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ExpenseLineAmount { get; set; }
        public decimal ExpenseLineTaxAmount { get; set; }
        public decimal ExpenseLineTax1Amount { get; set; }
        [MaxLength(4095)]
        public string ExpenseLineMemo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ExpenseLineCustomerRefCustomer { get; set; } // Navigation: CreditCardCreditExpenseLine -> Customer
        [MaxLength(209)]
        public string ExpenseLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ExpenseLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ExpenseLineClassRefClass { get; set; } // Navigation: CreditCardCreditExpenseLine -> Class
        [MaxLength(159)]
        public string ExpenseLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ExpenseLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ExpenseLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditCardCreditExpenseLine -> SalesTaxCode
        [MaxLength(36)]
        public string ExpenseLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ExpenseLineTaxCodeRefTaxCode { get; set; } // Navigation: CreditCardCreditExpenseLine -> TaxCode
        [MaxLength(3)]
        public string ExpenseLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ExpenseLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ExpenseLineBillableStatus { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class CreditCardCreditItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: CreditCardCredit.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: CreditCardCreditItemLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: CreditCardCreditItemLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool ItemLineClearItemLines { get; set; }
        [MaxLength(11)]
        public string ItemLineType { get; set; }
        public int ItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemRefItem { get; set; } // Navigation: CreditCardCreditItemLine -> Item
        [MaxLength(31)]
        public string ItemGroupLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemGroupLineDesc { get; set; }
        public decimal ItemGroupLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CreditCardCreditItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemGroupLineTotalAmount { get; set; }
        public int ItemGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemLineTxnLineID { get; set; }
        [Required, MaxLength(36)]
        public string ItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemLineItemRefItem { get; set; } // Navigation: CreditCardCreditItemLine -> Item
        [Required, MaxLength(159)]
        public string ItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemLineDesc { get; set; }
        public decimal ItemLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CreditCardCreditItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemLineCost { get; set; }
        public decimal ItemLineAmount { get; set; }
        public decimal ItemLineTaxAmount { get; set; }
        public decimal ItemLineTax1Amount { get; set; }
        [MaxLength(36)]
        public string ItemLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ItemLineCustomerRefCustomer { get; set; } // Navigation: CreditCardCreditItemLine -> Customer
        [MaxLength(209)]
        public string ItemLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ItemLineClassRefClass { get; set; } // Navigation: CreditCardCreditItemLine -> Class
        [MaxLength(159)]
        public string ItemLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(13)]
        public string ItemLineBillableStatus { get; set; }
        [MaxLength(36)]
        public string ItemLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ItemLineOverrideItemAccountRefAccount { get; set; } // Navigation: CreditCardCreditItemLine -> Account
        [MaxLength(159)]
        public string ItemLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class CreditMemo
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: CreditMemo -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: CreditMemo -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: CreditMemo -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: CreditMemo -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: CreditMemo -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: CreditMemo -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: CreditMemo -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: CreditMemo -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CreditRemaining { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: CreditMemo -> Currency
        [MaxLength(101)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: CreditMemo -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditMemo -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: CreditMemo -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class CreditMemoLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: CreditMemo.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: CreditMemoLine -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: CreditMemoLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: CreditMemoLine -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: CreditMemoLine -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: CreditMemoLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: CreditMemoLine -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: CreditMemoLine -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: CreditMemoLine -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CreditRemaining { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: CreditMemoLine -> Currency
        [MaxLength(101)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: CreditMemoLine -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditMemoLine -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: CreditMemoLine -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        [MaxLength(11)]
        public string CreditMemoLineType { get; set; }
        public int CreditMemoLineSeqNo { get; set; }
        [MaxLength(36)]
        public string CreditMemoLineGroupLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string CreditMemoLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item CreditMemoLineGroupItemGroupRefItem { get; set; } // Navigation: CreditMemoLine -> Item
        [MaxLength(159)]
        public string CreditMemoLineGroupItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string CreditMemoLineGroupDesc { get; set; }
        public decimal CreditMemoLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string CreditMemoLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string CreditMemoLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CreditMemoLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string CreditMemoLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool CreditMemoLineGroupIsPrintItemsInGroup { get; set; }
        public decimal CreditMemoLineGroupTotalAmount { get; set; }
        public DateTime CreditMemoLineGroupServiceDate { get; set; }
        public int CreditMemoLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string CreditMemoLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string CreditMemoLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item CreditMemoLineItemRefItem { get; set; } // Navigation: CreditMemoLine -> Item
        [MaxLength(159)]
        public string CreditMemoLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string CreditMemoLineDesc { get; set; }
        public decimal CreditMemoLineQuantity { get; set; }
        [MaxLength(31)]
        public string CreditMemoLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string CreditMemoLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: CreditMemoLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string CreditMemoLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal CreditMemoLineRate { get; set; }
        public decimal CreditMemoLineRatePercent { get; set; }
        [MaxLength(36)]
        public string CreditMemoLinePriceLevelRefListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel CreditMemoLinePriceLevelRefPriceLevel { get; set; } // Navigation: CreditMemoLine -> PriceLevel
        [MaxLength(159)]
        public string CreditMemoLinePriceLevelRefFullName { get; set; } // Relates To: PriceLevel.Name
        [MaxLength(36)]
        public string CreditMemoLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class CreditMemoLineClassRefClass { get; set; } // Navigation: CreditMemoLine -> Class
        [MaxLength(159)]
        public string CreditMemoLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal CreditMemoLineAmount { get; set; }
        public decimal CreditMemoLineTaxAmount { get; set; }
        public DateTime CreditMemoLineServiceDate { get; set; }
        [MaxLength(36)]
        public string CreditMemoLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CreditMemoLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditMemoLine -> SalesTaxCode
        [MaxLength(3)]
        public string CreditMemoLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CreditMemoLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CreditMemoLineTaxCodeRefTaxCode { get; set; } // Navigation: CreditMemoLine -> TaxCode
        [MaxLength(3)]
        public string CreditMemoLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(36)]
        public string CreditMemoLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account CreditMemoLineOverrideItemAccountRefAccount { get; set; } // Navigation: CreditMemoLine -> Account
        [MaxLength(159)]
        public string CreditMemoLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(29)]
        public string CustomFieldCreditMemoLineOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldCreditMemoLineOther2 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class CreditMemoLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: CreditMemo.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: CreditMemoLinkedTxn -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: CreditMemoLinkedTxn -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: CreditMemoLinkedTxn -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: CreditMemoLinkedTxn -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: CreditMemoLinkedTxn -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: CreditMemoLinkedTxn -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: CreditMemoLinkedTxn -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: CreditMemoLinkedTxn -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CreditRemaining { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: CreditMemoLinkedTxn -> Currency
        [MaxLength(101)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: CreditMemoLinkedTxn -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: CreditMemoLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: CreditMemoLinkedTxn -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Currency
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; }
        [MaxLength(10)]
        public string CurrencyFormatThousandSeparator { get; set; }
        [MaxLength(11)]
        public string CurrencyFormatThousandSeparatorGrouping { get; set; }
        [MaxLength(1)]
        public string CurrencyFormatDecimalPlaces { get; set; }
        [MaxLength(6)]
        public string CurrencyFormatDecimalSeparator { get; set; }
        public bool IsUserDefinedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime AsOfDate { get; set; }
    }
    public partial class Customer
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity Entity { get; set; } // Navigation: Customer -> Entity
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(41)]
        public string Name { get; set; }
        [MaxLength(209)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(209)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
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
        public string CustomerTypeRefListID { get; set; } // Relates To: CustomerType.ListID
        public virtual CustomerType CustomerTypeRefCustomerType { get; set; } // Navigation: Customer -> CustomerType
        [MaxLength(159)]
        public string CustomerTypeRefFullName { get; set; } // Relates To: CustomerType.FullName
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: Customer -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: Customer -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        public decimal Balance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal OpenBalance { get; set; }
        public DateTime OpenBalanceDate { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Customer -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: Customer -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: Customer -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
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
        public string PreferredPaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PreferredPaymentMethodRefPaymentMethod { get; set; } // Navigation: Customer -> PaymentMethod
        [MaxLength(31)]
        public string PreferredPaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
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
        public string JobTypeRefListID { get; set; } // Relates To: JobType.ListID
        public virtual JobType JobTypeRefJobType { get; set; } // Navigation: Customer -> JobType
        [MaxLength(159)]
        public string JobTypeRefFullName { get; set; } // Relates To: JobType.FullName
        [MaxLength(4095)]
        public string Notes { get; set; }
        public bool IsUsingCustomerTaxCode { get; set; }
        [MaxLength(36)]
        public string PriceLevelRefListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel PriceLevelRefPriceLevel { get; set; } // Navigation: Customer -> PriceLevel
        [MaxLength(31)]
        public string PriceLevelRefFullName { get; set; } // Relates To: PriceLevel.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        [MaxLength(30)]
        public string TaxRegistrationNumber { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: Customer -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
    }
    public partial class CustomerMsg
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(101)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public partial class CustomerType
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
    }
    public partial class CustomField
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(50)]
        public string OwnerID { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(23)]
        public string EntityType { get; set; }
        [MaxLength(36)]
        public string EntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity EntityRefEntity { get; set; } // Navigation: CustomField -> Entity
        [MaxLength(159)]
        public string EntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(23)]
        public string TxnType { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; }
        [MaxLength(36)]
        public string TxnLineID { get; set; }
        [MaxLength(7)]
        public string OtherType { get; set; }
        [MaxLength(4095)]
        public string Value { get; set; }
    }
    public partial class DateDrivenTerms
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms Terms { get; set; } // Navigation: DateDrivenTerms -> Terms
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int DayOfMonthDue { get; set; }
        public int DueNextMonthDays { get; set; }
        public int DiscountDayOfMonth { get; set; }
        public decimal DiscountPct { get; set; }
    }
    public partial class Deposit
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: Deposit -> Account
        [Required, MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string Memo { get; set; }
        public decimal DepositTotal { get; set; }
        [MaxLength(36)]
        public string CashBackInfoTxnLineID { get; set; }
        [MaxLength(36)]
        public string CashBackInfoAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account CashBackInfoAccountRefAccount { get; set; } // Navigation: Deposit -> Account
        [MaxLength(159)]
        public string CashBackInfoAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string CashBackInfoMemo { get; set; }
        public decimal CashBackInfoAmount { get; set; }
    }
    public partial class DepositLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Deposit.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: DepositLine -> Account
        [Required, MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string Memo { get; set; }
        public decimal DepositTotal { get; set; }
        [MaxLength(36)]
        public string CashBackInfoTxnLineID { get; set; }
        [MaxLength(36)]
        public string CashBackInfoAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account CashBackInfoAccountRefAccount { get; set; } // Navigation: DepositLine -> Account
        [MaxLength(159)]
        public string CashBackInfoAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string CashBackInfoMemo { get; set; }
        public decimal CashBackInfoAmount { get; set; }
        public int DepositLineSeqNo { get; set; }
        [MaxLength(22)]
        public string DepositLineTxnType { get; set; }
        [MaxLength(36)]
        public string DepositLineTxnID { get; set; }
        [MaxLength(36)]
        public string DepositLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string DepositLinePaymentTxnID { get; set; }
        [MaxLength(36)]
        public string DepositLinePaymentTxnLineID { get; set; }
        [MaxLength(36)]
        public string DepositLineEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity DepositLineEntityRefEntity { get; set; } // Navigation: DepositLine -> Entity
        [MaxLength(209)]
        public string DepositLineEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [Required, MaxLength(36)]
        public string DepositLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositLineAccountRefAccount { get; set; } // Navigation: DepositLine -> Account
        [Required, MaxLength(159)]
        public string DepositLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string DepositLineMemo { get; set; }
        [MaxLength(25)]
        public string DepositLineCheckNumber { get; set; }
        [MaxLength(36)]
        public string DepositLinePaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod DepositLinePaymentMethodRefPaymentMethod { get; set; } // Navigation: DepositLine -> PaymentMethod
        [MaxLength(31)]
        public string DepositLinePaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        [MaxLength(36)]
        public string DepositLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class DepositLineClassRefClass { get; set; } // Navigation: DepositLine -> Class
        [MaxLength(159)]
        public string DepositLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal DepositLineAmount { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Employee
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity Entity { get; set; } // Navigation: Employee -> Entity
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(41)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string Salutation { get; set; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(5)]
        public string MiddleName { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        [MaxLength(41)]
        public string EmployeeAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string EmployeeAddressAddr2 { get; set; }
        [MaxLength(31)]
        public string EmployeeAddressCity { get; set; }
        [MaxLength(21)]
        public string EmployeeAddressState { get; set; }
        [MaxLength(21)]
        public string EmployeeAddressProvince { get; set; }
        [MaxLength(21)]
        public string EmployeeAddressCounty { get; set; }
        [MaxLength(13)]
        public string EmployeeAddressPostalCode { get; set; }
        [MaxLength(41)]
        public string PrintAs { get; set; }
        [MaxLength(21)]
        public string Phone { get; set; }
        [MaxLength(21)]
        public string Mobile { get; set; }
        [MaxLength(21)]
        public string Pager { get; set; }
        [MaxLength(10)]
        public string PagerPIN { get; set; }
        [MaxLength(21)]
        public string AltPhone { get; set; }
        [MaxLength(21)]
        public string Fax { get; set; }
        [MaxLength(11)]
        public string SSN { get; set; }
        [MaxLength(11)]
        public string SIN { get; set; }
        [MaxLength(11)]
        public string NiNumber { get; set; }
        [MaxLength(6)]
        public string MaritalStatus { get; set; }
        [MaxLength(1023)]
        public string Email { get; set; }
        [MaxLength(9)]
        public string EmployeeType { get; set; }
        [MaxLength(6)]
        public string Gender { get; set; }
        [MaxLength(6)]
        public string Sex { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime ReleasedDate { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(99)]
        public string AccountNumber { get; set; }
        [MaxLength(4096)]
        public string Notes { get; set; }
        [MaxLength(36)]
        public string BillingRateRefListID { get; set; } // Relates To: BillingRate.ListID
        public virtual BillingRate BillingRateRefBillingRate { get; set; } // Navigation: Employee -> BillingRate
        [MaxLength(31)]
        public string BillingRateRefFullName { get; set; } // Relates To: BillingRate.FullName
        [MaxLength(11)]
        public string PayrollInfoPayPeriod { get; set; }
        [MaxLength(36)]
        public string PayrollInfoClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class PayrollInfoClassRefClass { get; set; } // Navigation: Employee -> Class
        [MaxLength(159)]
        public string PayrollInfoClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(16)]
        public string PayrollInfoUseTimeDataToCreatePaychecks { get; set; }
        public double PayrollInfoSickHoursHoursAvailable { get; set; }
        [MaxLength(19)]
        public string PayrollInfoSickHoursAccrualPeriod { get; set; }
        public double PayrollInfoSickHoursHoursAccrued { get; set; }
        public double PayrollInfoSickHoursMaximumHours { get; set; }
        public bool PayrollInfoSickHoursIsResettingHoursEachNewYear { get; set; }
        public double PayrollInfoSickHoursHoursUsed { get; set; }
        public DateTime PayrollInfoSickHoursAccrualStartDate { get; set; }
        public double PayrollInfoVacationHoursHoursAvailable { get; set; }
        [MaxLength(19)]
        public string PayrollInfoVacationHoursAccrualPeriod { get; set; }
        public double PayrollInfoVacationHoursHoursAccrued { get; set; }
        public double PayrollInfoVacationHoursMaximumHours { get; set; }
        public bool PayrollInfoVacationHoursIsResetHoursEachNewYear { get; set; }
        public double PayrollInfoVacationHoursHoursUsed { get; set; }
        public DateTime PayrollInfoVacationHoursAccrualStartDate { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class EmployeeEarning
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: Employee.ListID
        public virtual Employee Employee { get; set; } // Navigation: EmployeeEarning -> Employee
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(41)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string Salutation { get; set; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(5)]
        public string MiddleName { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        [MaxLength(41)]
        public string EmployeeAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string EmployeeAddressAddr2 { get; set; }
        [MaxLength(31)]
        public string EmployeeAddressCity { get; set; }
        [MaxLength(21)]
        public string EmployeeAddressState { get; set; }
        [MaxLength(21)]
        public string EmployeeAddressProvince { get; set; }
        [MaxLength(21)]
        public string EmployeeAddressCounty { get; set; }
        [MaxLength(13)]
        public string EmployeeAddressPostalCode { get; set; }
        [MaxLength(41)]
        public string PrintAs { get; set; }
        [MaxLength(21)]
        public string Phone { get; set; }
        [MaxLength(21)]
        public string Mobile { get; set; }
        [MaxLength(21)]
        public string Pager { get; set; }
        [MaxLength(10)]
        public string PagerPIN { get; set; }
        [MaxLength(21)]
        public string AltPhone { get; set; }
        [MaxLength(21)]
        public string Fax { get; set; }
        [MaxLength(11)]
        public string SSN { get; set; }
        [MaxLength(11)]
        public string SIN { get; set; }
        [MaxLength(11)]
        public string NiNumber { get; set; }
        [MaxLength(6)]
        public string MaritalStatus { get; set; }
        [MaxLength(1023)]
        public string Email { get; set; }
        [MaxLength(9)]
        public string EmployeeType { get; set; }
        [MaxLength(6)]
        public string Gender { get; set; }
        [MaxLength(6)]
        public string Sex { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime ReleasedDate { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(99)]
        public string AccountNumber { get; set; }
        [MaxLength(4096)]
        public string Notes { get; set; }
        [MaxLength(36)]
        public string BillingRateRefListID { get; set; } // Relates To: BillingRate.ListID
        public virtual BillingRate BillingRateRefBillingRate { get; set; } // Navigation: EmployeeEarning -> BillingRate
        [MaxLength(31)]
        public string BillingRateRefFullName { get; set; } // Relates To: BillingRate.FullName
        [MaxLength(11)]
        public string PayrollInfoPayPeriod { get; set; }
        [MaxLength(36)]
        public string PayrollInfoClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class PayrollInfoClassRefClass { get; set; } // Navigation: EmployeeEarning -> Class
        [MaxLength(159)]
        public string PayrollInfoClassRefFullName { get; set; } // Relates To: Class.FullName
        public bool PayrollInfoEarningsClearEarnings { get; set; }
        public int PayrollInfoEarningsSeqNo { get; set; }
        [MaxLength(36)]
        public string PayrollInfoEarningsPayrollItemWageRefListID { get; set; } // Relates To: PayrollItemWage.ListID
        public virtual PayrollItemWage PayrollInfoEarningsPayrollItemWageRefPayrollItemWage { get; set; } // Navigation: EmployeeEarning -> PayrollItemWage
        [MaxLength(31)]
        public string PayrollInfoEarningsPayrollItemWageRefFullName { get; set; } // Relates To: PayrollItemWage.Name
        public decimal PayrollInfoEarningsRate { get; set; }
        public decimal PayrollInfoEarningsRatePercent { get; set; }
        [MaxLength(16)]
        public string PayrollInfoUseTimeDataToCreatePaychecks { get; set; }
        public double PayrollInfoSickHoursHoursAvailable { get; set; }
        [MaxLength(19)]
        public string PayrollInfoSickHoursAccrualPeriod { get; set; }
        public double PayrollInfoSickHoursHoursAccrued { get; set; }
        public double PayrollInfoSickHoursMaximumHours { get; set; }
        public bool PayrollInfoSickHoursIsResettingHoursEachNewYear { get; set; }
        public double PayrollInfoSickHoursHoursUsed { get; set; }
        public DateTime PayrollInfoSickHoursAccrualStartDate { get; set; }
        public double PayrollInfoVacationHoursHoursAvailable { get; set; }
        [MaxLength(19)]
        public string PayrollInfoVacationHoursAccrualPeriod { get; set; }
        public double PayrollInfoVacationHoursHoursAccrued { get; set; }
        public double PayrollInfoVacationHoursMaximumHours { get; set; }
        public bool PayrollInfoVacationHoursIsResetHoursEachNewYear { get; set; }
        public double PayrollInfoVacationHoursHoursUsed { get; set; }
        public DateTime PayrollInfoVacationHoursAccrualStartDate { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Entity
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(209)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(209)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
    }
    public partial class Estimate
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: Estimate -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: Estimate -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: Estimate -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsActive { get; set; }
        public bool CreateChangeOrder { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: Estimate -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: Estimate -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: Estimate -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: Estimate -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Estimate -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: Estimate -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class EstimateLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Estimate.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: EstimateLine -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: EstimateLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: EstimateLine -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsActive { get; set; }
        public bool CreateChangeOrder { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: EstimateLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: EstimateLine -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: EstimateLine -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: EstimateLine -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: EstimateLine -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: EstimateLine -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        [MaxLength(11)]
        public string EstimateLineType { get; set; }
        public int EstimateLineSeqNo { get; set; }
        [MaxLength(36)]
        public string EstimateLineGroupTxnLineID { get; set; }
        [MaxLength(36)]
        public string EstimateLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item EstimateLineGroupItemGroupRefItem { get; set; } // Navigation: EstimateLine -> Item
        [MaxLength(159)]
        public string EstimateLineGroupItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string EstimateLineGroupDesc { get; set; }
        public decimal EstimateLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string EstimateLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string EstimateLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: EstimateLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string EstimateLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool EstimateLineGroupIsPrintItemsInGroup { get; set; }
        public decimal EstimateLineGroupTotalAmount { get; set; }
        public int EstimateLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string EstimateLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string EstimateLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item EstimateLineItemRefItem { get; set; } // Navigation: EstimateLine -> Item
        [MaxLength(159)]
        public string EstimateLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string EstimateLineDesc { get; set; }
        public decimal EstimateLineQuantity { get; set; }
        [MaxLength(31)]
        public string EstimateLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string EstimateLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet EstimateLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: EstimateLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string EstimateLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal EstimateLineRate { get; set; }
        public decimal EstimateLineRatePercent { get; set; }
        [MaxLength(36)]
        public string EstimateLinePriceLevelRefListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel EstimateLinePriceLevelRefPriceLevel { get; set; } // Navigation: EstimateLine -> PriceLevel
        [MaxLength(159)]
        public string EstimateLinePriceLevelRefFullName { get; set; } // Relates To: PriceLevel.Name
        [MaxLength(36)]
        public string EstimateLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class EstimateLineClassRefClass { get; set; } // Navigation: EstimateLine -> Class
        [MaxLength(159)]
        public string EstimateLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal EstimateLineAmount { get; set; }
        public decimal EstimateLineTaxAmount { get; set; }
        [MaxLength(36)]
        public string EstimateLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode EstimateLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: EstimateLine -> SalesTaxCode
        [MaxLength(3)]
        public string EstimateLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string EstimateLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode EstimateLineTaxCodeRefTaxCode { get; set; } // Navigation: EstimateLine -> TaxCode
        [MaxLength(3)]
        public string EstimateLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal EstimateLineMarkupRate { get; set; }
        public decimal EstimateLineMarkupRatePercent { get; set; }
        [MaxLength(36)]
        public string EstimateLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account EstimateLineOverrideItemAccountRefAccount { get; set; } // Navigation: EstimateLine -> Account
        [MaxLength(159)]
        public string EstimateLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(29)]
        public string CustomFieldEstimateLineOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldEstimateLineOther2 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class EstimateLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Estimate.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: EstimateLinkedTxn -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: EstimateLinkedTxn -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: EstimateLinkedTxn -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsActive { get; set; }
        public bool CreateChangeOrder { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: EstimateLinkedTxn -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: EstimateLinkedTxn -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: EstimateLinkedTxn -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: EstimateLinkedTxn -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: EstimateLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: EstimateLinkedTxn -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(11)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Host
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required]
        public int ID { get; set; }
        [MaxLength(255)]
        public string ProductName { get; set; }
        [MaxLength(5)]
        public string MajorVersion { get; set; }
        [MaxLength(5)]
        public string MinorVersion { get; set; }
        [MaxLength(2)]
        public string Country { get; set; }
        public bool IsAutomaticLogin { get; set; }
        [MaxLength(10)]
        public string QBFileMode { get; set; }
        [MaxLength(5)]
        public string QODBCMajorVersion { get; set; }
        [MaxLength(5)]
        public string QODBCMinorVersion { get; set; }
        [MaxLength(5)]
        public string QODBCBuildNumber { get; set; }
        [MaxLength(5)]
        public string QODBCRegion { get; set; }
        [MaxLength(12)]
        public string QODBCSerialNo { get; set; }
        [MaxLength(50)]
        public string QODBCEdition { get; set; }
        [MaxLength(50)]
        public string QODBCEditionQBES { get; set; }
        [MaxLength(50)]
        public string QODBCEditionRunning { get; set; }
    }
    public partial class HostMetaData
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required]
        public int ID { get; set; } // Relates To: Host.ID
        [MaxLength(255)]
        public string ProductName { get; set; }
        [MaxLength(5)]
        public string MajorVersion { get; set; }
        [MaxLength(5)]
        public string MinorVersion { get; set; }
        [MaxLength(2)]
        public string Country { get; set; }
        public bool IsAutomaticLogin { get; set; }
        [MaxLength(10)]
        public string QBFileMode { get; set; }
        [MaxLength(5)]
        public string QODBCMajorVersion { get; set; }
        [MaxLength(5)]
        public string QODBCMinorVersion { get; set; }
        [MaxLength(5)]
        public string QODBCBuildNumber { get; set; }
        [MaxLength(5)]
        public string QODBCRegion { get; set; }
        [MaxLength(12)]
        public string QODBCSerialNo { get; set; }
        [MaxLength(50)]
        public string QODBCEdition { get; set; }
        [MaxLength(50)]
        public string QODBCEditionQBES { get; set; }
        [MaxLength(50)]
        public string QODBCEditionRunning { get; set; }
        public int AccountMetaDataMaxCapacity { get; set; }
        public int BillingRateMetaDataMaxCapacity { get; set; }
        public int ClassMetaDataMaxCapacity { get; set; }
        public int CustomerMsgMetaDataMaxCapacity { get; set; }
        public int CustomerTypeMetaDataMaxCapacity { get; set; }
        public int EntityMetaDataMaxCapacity { get; set; }
        public int ItemMetaDataMaxCapacity { get; set; }
        public int JobTypeMetaDataMaxCapacity { get; set; }
        public int PaymentMethodMetaDataMaxCapacity { get; set; }
        public int PayrollItemMetaData { get; set; }
        public int PriceLevelMetaData { get; set; }
        public int SalesRepMetaDataMaxCapacity { get; set; }
        public int SalesTaxCodeMetaDataMaxCapacity { get; set; }
        public int ShipMethodMetaDataMaxCapacity { get; set; }
        public int TemplateMetaDataMaxCapacity { get; set; }
        public int TermsMetaDataMaxCapacity { get; set; }
        public int ToDoMetaDataMaxCapacity { get; set; }
        public int VehicleMetaDataMaxCapacity { get; set; }
        public int VendorTypeMetaDataMaxCapacity { get; set; }
    }
    public partial class HostSupportedVersions
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        public int ID { get; set; } // Relates To: Host.ID
        [MaxLength(255)]
        public string ProductName { get; set; }
        [MaxLength(5)]
        public string MajorVersion { get; set; }
        [MaxLength(5)]
        public string MinorVersion { get; set; }
        [MaxLength(2)]
        public string Country { get; set; }
        [MaxLength(10)]
        public string SupportedQBXMLVersion { get; set; }
        public bool IsAutomaticLogin { get; set; }
        [MaxLength(10)]
        public string QBFileMode { get; set; }
        [MaxLength(5)]
        public string QODBCMajorVersion { get; set; }
        [MaxLength(5)]
        public string QODBCMinorVersion { get; set; }
        [MaxLength(5)]
        public string QODBCBuildNumber { get; set; }
        [MaxLength(5)]
        public string QODBCRegion { get; set; }
        [MaxLength(12)]
        public string QODBCSerialNo { get; set; }
        [MaxLength(50)]
        public string QODBCEdition { get; set; }
        [MaxLength(50)]
        public string QODBCEditionQBES { get; set; }
        [MaxLength(50)]
        public string QODBCEditionRunning { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class InventoryAdjustment
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: InventoryAdjustment -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: InventoryAdjustment -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: InventoryAdjustment -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(4095)]
        public string Memo { get; set; }
    }
    public partial class InventoryAdjustmentLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: InventoryAdjustment.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: InventoryAdjustmentLine -> Account
        [Required, MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: InventoryAdjustmentLine -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: InventoryAdjustmentLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(4095)]
        public string Memo { get; set; }
        public int InventoryAdjustmentSeqNo { get; set; }
        [MaxLength(36)]
        public string InventoryAdjustmentLineTxnLineID { get; set; }
        [Required, MaxLength(36)]
        public string InventoryAdjustmentLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item InventoryAdjustmentLineItemRefItem { get; set; } // Navigation: InventoryAdjustmentLine -> Item
        [Required, MaxLength(159)]
        public string InventoryAdjustmentLineItemRefFullName { get; set; } // Relates To: Item.FullName
        public decimal InventoryAdjustmentLineQuantityDifference { get; set; }
        public decimal InventoryAdjustmentLineValueDifference { get; set; }
        public decimal InventoryAdjustmentLineQuantityAdjustmentNewQuantity { get; set; }
        public decimal InventoryAdjustmentLineQuantityAdjustmentQuantityDifference { get; set; }
        public decimal InventoryAdjustmentLineValueAdjustmentNewQuantity { get; set; }
        public decimal InventoryAdjustmentLineValueAdjustmentNewValue { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Invoice
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: Invoice -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: Invoice -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: Invoice -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: Invoice -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        public bool IsFinanceCharge { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: Invoice -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: Invoice -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: Invoice -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: Invoice -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal BalanceRemaining { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: Invoice -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal BalanceRemainingInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: Invoice -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Invoice -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: Invoice -> TaxCode
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
    public partial class InvoiceLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Invoice.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: InvoiceLine -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: InvoiceLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: InvoiceLine -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: InvoiceLine -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        public bool IsFinanceCharge { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: InvoiceLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: InvoiceLine -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: InvoiceLine -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: InvoiceLine -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal BalanceRemaining { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: InvoiceLine -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal BalanceRemainingInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: InvoiceLine -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: InvoiceLine -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: InvoiceLine -> TaxCode
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
        public string InvoiceLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item InvoiceLineGroupItemGroupRefItem { get; set; } // Navigation: InvoiceLine -> Item
        [MaxLength(31)]
        public string InvoiceLineGroupItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string InvoiceLineGroupDesc { get; set; }
        public decimal InvoiceLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string InvoiceLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string InvoiceLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: InvoiceLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string InvoiceLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool InvoiceLineGroupIsPrintItemsInGroup { get; set; }
        public decimal InvoiceLineGroupTotalAmount { get; set; }
        public DateTime InvoiceLineGroupServiceDate { get; set; }
        public int InvoiceLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string InvoiceLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string InvoiceLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item InvoiceLineItemRefItem { get; set; } // Navigation: InvoiceLine -> Item
        [MaxLength(159)]
        public string InvoiceLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string InvoiceLineDesc { get; set; }
        public decimal InvoiceLineQuantity { get; set; }
        [MaxLength(31)]
        public string InvoiceLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string InvoiceLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet InvoiceLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: InvoiceLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string InvoiceLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal InvoiceLineRate { get; set; }
        public decimal InvoiceLineRatePercent { get; set; }
        [MaxLength(36)]
        public string InvoiceLinePriceLevelRefListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel InvoiceLinePriceLevelRefPriceLevel { get; set; } // Navigation: InvoiceLine -> PriceLevel
        [MaxLength(159)]
        public string InvoiceLinePriceLevelRefFullName { get; set; } // Relates To: PriceLevel.Name
        [MaxLength(36)]
        public string InvoiceLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class InvoiceLineClassRefClass { get; set; } // Navigation: InvoiceLine -> Class
        [MaxLength(159)]
        public string InvoiceLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal InvoiceLineAmount { get; set; }
        public decimal InvoiceLineTaxAmount { get; set; }
        public DateTime InvoiceLineServiceDate { get; set; }
        [MaxLength(36)]
        public string InvoiceLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode InvoiceLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: InvoiceLine -> SalesTaxCode
        [MaxLength(3)]
        public string InvoiceLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string InvoiceLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode InvoiceLineTaxCodeRefTaxCode { get; set; } // Navigation: InvoiceLine -> TaxCode
        [MaxLength(3)]
        public string InvoiceLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(36)]
        public string InvoiceLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account InvoiceLineOverrideItemAccountRefAccount { get; set; } // Navigation: InvoiceLine -> Account
        [MaxLength(159)]
        public string InvoiceLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
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
    public partial class InvoiceLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Invoice.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: InvoiceLinkedTxn -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: InvoiceLinkedTxn -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: InvoiceLinkedTxn -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: InvoiceLinkedTxn -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        public bool IsFinanceCharge { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: InvoiceLinkedTxn -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: InvoiceLinkedTxn -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: InvoiceLinkedTxn -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: InvoiceLinkedTxn -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal BalanceRemaining { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: InvoiceLinkedTxn -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
        public decimal ExchangeRate { get; set; }
        public decimal BalanceRemainingInHomeCurrency { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: InvoiceLinkedTxn -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: InvoiceLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: InvoiceLinkedTxn -> TaxCode
        public decimal SuggestedDiscountAmount { get; set; }
        public DateTime SuggestedDiscountDate { get; set; }
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(11)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Item
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(209)]
        public string FullName { get; set; }
        [MaxLength(4095)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(209)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(31)]
        public string ManufacturerPartNumber { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: Item -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(25)]
        public string Type { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Item -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesOrPurchaseDesc { get; set; }
        public decimal SalesOrPurchasePrice { get; set; }
        public decimal SalesOrPurchasePricePercent { get; set; }
        [MaxLength(36)]
        public string SalesOrPurchaseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesOrPurchaseAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string SalesOrPurchaseAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string SalesAndPurchaseSalesDesc { get; set; }
        public decimal SalesAndPurchaseSalesPrice { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseIncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseIncomeAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseIncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string SalesAndPurchasePurchaseDesc { get; set; }
        public decimal SalesAndPurchasePurchaseCost { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseExpenseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseExpenseAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseExpenseAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string SalesAndPurchasePrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor SalesAndPurchasePrefVendorRefVendor { get; set; } // Navigation: Item -> Vendor
        [MaxLength(41)]
        public string SalesAndPurchasePrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(4095)]
        public string SalesDesc { get; set; }
        public decimal SalesPrice { get; set; }
        [MaxLength(36)]
        public string IncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account IncomeAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string IncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(4095)]
        public string PurchaseDesc { get; set; }
        public decimal PurchaseCost { get; set; }
        [MaxLength(36)]
        public string COGSAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account COGSAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string COGSAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor PrefVendorRefVendor { get; set; } // Navigation: Item -> Vendor
        [MaxLength(41)]
        public string PrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string AssetAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AssetAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string AssetAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ReorderBuildPoint { get; set; }
        public DateTime InventoryDate { get; set; }
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: Item -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: Item -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        public decimal TaxRate { get; set; }
        [MaxLength(36)]
        public string TaxVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor TaxVendorRefVendor { get; set; } // Navigation: Item -> Vendor
        [MaxLength(41)]
        public string TaxVendorRefFullName { get; set; } // Relates To: Vendor.Name
    }
    public partial class ItemAssembliesCanBuild
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string ItemInventoryAssemblyRefListID { get; set; } // Relates To: ItemInventoryAssembly.ListID
        public virtual ItemInventoryAssembly ItemInventoryAssemblyRefItemInventoryAssembly { get; set; } // Navigation: ItemAssembliesCanBuild -> ItemInventoryAssembly
        [MaxLength(159)]
        public string ItemInventoryAssemblyRefFullName { get; set; } // Relates To: ItemInventoryAssembly.FullName
        public DateTime TxnDate { get; set; }
        public decimal QuantityCanBuild { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ItemDiscount
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemDiscount -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode TaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemDiscount -> SalesTaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        public decimal DiscountRate { get; set; }
        public decimal DiscountRatePercent { get; set; }
        [MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: ItemDiscount -> Account
        [MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public bool ApplyAccountRefToExistingTxns { get; set; }
    }
    public partial class ItemFixedAsset
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(3)]
        public string AcquiredAs { get; set; }
        [Required, MaxLength(50)]
        public string PurchaseDesc { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseCost { get; set; }
        [MaxLength(50)]
        public string VendorOrPayeeName { get; set; }
        [Required, MaxLength(36)]
        public string AssetAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AssetAccountRefAccount { get; set; } // Navigation: ItemFixedAsset -> Account
        [Required, MaxLength(159)]
        public string AssetAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(50)]
        public string FixedAssetSalesInfoSalesDesc { get; set; }
        public DateTime FixedAssetSalesInfoSalesDate { get; set; }
        public decimal FixedAssetSalesInfoSalesPrice { get; set; }
        public decimal FixedAssetSalesInfoSalesExpense { get; set; }
        [MaxLength(50)]
        public string AssetDesc { get; set; }
        [MaxLength(50)]
        public string Location { get; set; }
        [MaxLength(30)]
        public string PONumber { get; set; }
        [MaxLength(30)]
        public string SerialNumber { get; set; }
        public DateTime WarrantyExpDate { get; set; }
        [MaxLength(4096)]
        public string Notes { get; set; }
        [MaxLength(10)]
        public string AssetNumber { get; set; }
        public decimal CostBasis { get; set; }
        public decimal YearEndAccumulatedDepreciation { get; set; }
        public decimal YearEndBookValue { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemGroup
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemGroup -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        public bool IsPrintItemsInGroup { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        [MaxLength(24)]
        public string SpecialItemType { get; set; } // Relates To: SpecialItem.SpecialItemType
    }
    public partial class ItemGroupLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: ItemGroup.ListID
        public virtual ItemGroup ItemGroup { get; set; } // Navigation: ItemGroupLine -> ItemGroup
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemGroupLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        public bool IsPrintItemsInGroup { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        [MaxLength(24)]
        public string SpecialItemType { get; set; } // Relates To: SpecialItem.SpecialItemType
        public bool ClearItemsInGroup { get; set; }
        public int ItemGroupLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemRefItem { get; set; } // Navigation: ItemGroupLine -> Item
        [MaxLength(159)]
        public string ItemGroupLineItemRefFullName { get; set; } // Relates To: Item.FullName
        public decimal ItemGroupLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemGroupLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ItemInventory
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(31)]
        public string ManufacturerPartNumber { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemInventory -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemInventory -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesDesc { get; set; }
        public decimal SalesPrice { get; set; }
        [Required, MaxLength(36)]
        public string IncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account IncomeAccountRefAccount { get; set; } // Navigation: ItemInventory -> Account
        [Required, MaxLength(159)]
        public string IncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool ApplyIncomeAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string PurchaseDesc { get; set; }
        public decimal PurchaseCost { get; set; }
        [Required, MaxLength(36)]
        public string COGSAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account COGSAccountRefAccount { get; set; } // Navigation: ItemInventory -> Account
        [Required, MaxLength(159)]
        public string COGSAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor PrefVendorRefVendor { get; set; } // Navigation: ItemInventory -> Vendor
        [MaxLength(41)]
        public string PrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [Required, MaxLength(36)]
        public string AssetAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AssetAccountRefAccount { get; set; } // Navigation: ItemInventory -> Account
        [Required, MaxLength(159)]
        public string AssetAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ReorderPoint { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime InventoryDate { get; set; }
        public decimal AverageCost { get; set; }
        public decimal QuantityOnOrder { get; set; }
        public decimal QuantityOnSalesOrder { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemInventoryAssembly
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemInventoryAssembly -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemInventoryAssembly -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesDesc { get; set; }
        public decimal SalesPrice { get; set; }
        [Required, MaxLength(36)]
        public string IncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account IncomeAccountRefAccount { get; set; } // Navigation: ItemInventoryAssembly -> Account
        [Required, MaxLength(159)]
        public string IncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool ApplyIncomeAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string PurchaseDesc { get; set; }
        public decimal PurchaseCost { get; set; }
        [Required, MaxLength(36)]
        public string COGSAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account COGSAccountRefAccount { get; set; } // Navigation: ItemInventoryAssembly -> Account
        [Required, MaxLength(159)]
        public string COGSAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor PrefVendorRefVendor { get; set; } // Navigation: ItemInventoryAssembly -> Vendor
        [MaxLength(41)]
        public string PrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [Required, MaxLength(36)]
        public string AssetAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AssetAccountRefAccount { get; set; } // Navigation: ItemInventoryAssembly -> Account
        [Required, MaxLength(159)]
        public string AssetAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal BuildPoint { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime InventoryDate { get; set; }
        public decimal AverageCost { get; set; }
        public decimal QuantityOnOrder { get; set; }
        public decimal QuantityOnSalesOrder { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemInventoryAssemblyLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: ItemInventoryAssembly.ListID
        public virtual ItemInventoryAssembly ItemInventoryAssembly { get; set; } // Navigation: ItemInventoryAssemblyLine -> ItemInventoryAssembly
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemInventoryAssemblyLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemInventoryAssemblyLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesDesc { get; set; }
        public decimal SalesPrice { get; set; }
        [Required, MaxLength(36)]
        public string IncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account IncomeAccountRefAccount { get; set; } // Navigation: ItemInventoryAssemblyLine -> Account
        [Required, MaxLength(159)]
        public string IncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool ApplyIncomeAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string PurchaseDesc { get; set; }
        public decimal PurchaseCost { get; set; }
        [Required, MaxLength(36)]
        public string COGSAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account COGSAccountRefAccount { get; set; } // Navigation: ItemInventoryAssemblyLine -> Account
        [Required, MaxLength(159)]
        public string COGSAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor PrefVendorRefVendor { get; set; } // Navigation: ItemInventoryAssemblyLine -> Vendor
        [MaxLength(41)]
        public string PrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [Required, MaxLength(36)]
        public string AssetAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AssetAccountRefAccount { get; set; } // Navigation: ItemInventoryAssemblyLine -> Account
        [Required, MaxLength(159)]
        public string AssetAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal BuildPoint { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime InventoryDate { get; set; }
        public decimal AverageCost { get; set; }
        public decimal QuantityOnOrder { get; set; }
        public decimal QuantityOnSalesOrder { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public bool ClearItemsInGroup { get; set; }
        public int ItemInventoryAssemblyLnSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemInventoryAssemblyLnItemInventoryRefListID { get; set; } // Relates To: ItemInventory.ListID
        public virtual ItemInventory ItemInventoryAssemblyLnItemInventoryRefItemInventory { get; set; } // Navigation: ItemInventoryAssemblyLine -> ItemInventory
        [MaxLength(159)]
        public string ItemInventoryAssemblyLnItemInventoryRefFullName { get; set; } // Relates To: ItemInventory.FullName
        public decimal ItemInventoryAssemblyLnQuantity { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ItemNonInventory
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(31)]
        public string ManufacturerPartNumber { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemNonInventory -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemNonInventory -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesOrPurchaseDesc { get; set; }
        public decimal SalesOrPurchasePrice { get; set; }
        public decimal SalesOrPurchasePricePercent { get; set; }
        [MaxLength(36)]
        public string SalesOrPurchaseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesOrPurchaseAccountRefAccount { get; set; } // Navigation: ItemNonInventory -> Account
        [MaxLength(159)]
        public string SalesOrPurchaseAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesOrPurchaseApplyAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string SalesAndPurchaseSalesDesc { get; set; }
        public decimal SalesAndPurchaseSalesPrice { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseIncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseIncomeAccountRefAccount { get; set; } // Navigation: ItemNonInventory -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseIncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesAndPurchaseApplyIncomeAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string SalesAndPurchasePurchaseDesc { get; set; }
        public decimal SalesAndPurchasePurchaseCost { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseExpenseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseExpenseAccountRefAccount { get; set; } // Navigation: ItemNonInventory -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseExpenseAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesAndPurchaseApplyExpenseAccountRefToExistingTxns { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchasePrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor SalesAndPurchasePrefVendorRefVendor { get; set; } // Navigation: ItemNonInventory -> Vendor
        [MaxLength(41)]
        public string SalesAndPurchasePrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemOtherCharge
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemOtherCharge -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesOrPurchaseDesc { get; set; }
        public decimal SalesOrPurchasePrice { get; set; }
        public decimal SalesOrPurchasePricePercent { get; set; }
        [MaxLength(36)]
        public string SalesOrPurchaseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesOrPurchaseAccountRefAccount { get; set; } // Navigation: ItemOtherCharge -> Account
        [MaxLength(159)]
        public string SalesOrPurchaseAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesOrPurchaseApplyAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string SalesAndPurchaseSalesDesc { get; set; }
        public decimal SalesAndPurchaseSalesPrice { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseIncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseIncomeAccountRefAccount { get; set; } // Navigation: ItemOtherCharge -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseIncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesAndPurchaseApplyIncomeAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string SalesAndPurchasePurchaseDesc { get; set; }
        public decimal SalesAndPurchasePurchaseCost { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseExpenseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseExpenseAccountRefAccount { get; set; } // Navigation: ItemOtherCharge -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseExpenseAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesAndPurchaseApplyExpenseAccountRefToExistingTxns { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchasePrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor SalesAndPurchasePrefVendorRefVendor { get; set; } // Navigation: ItemOtherCharge -> Vendor
        [MaxLength(41)]
        public string SalesAndPurchasePrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(24)]
        public string SpecialItemType { get; set; } // Relates To: SpecialItem.SpecialItemType
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemPayment
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: ItemPayment -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: ItemPayment -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemReceipt
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: ItemReceipt -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: ItemReceipt -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string LiabilityAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account LiabilityAccountRefAccount { get; set; } // Navigation: ItemReceipt -> Account
        [MaxLength(159)]
        public string LiabilityAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class ItemReceiptExpenseLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: ItemReceipt.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: ItemReceiptExpenseLine -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: ItemReceiptExpenseLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string LiabilityAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account LiabilityAccountRefAccount { get; set; } // Navigation: ItemReceiptExpenseLine -> Account
        [MaxLength(159)]
        public string LiabilityAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public bool ExpenseLineClearExpenseLines { get; set; }
        [MaxLength(36)]
        public string ExpenseLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ExpenseLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseLineAccountRefAccount { get; set; } // Navigation: ItemReceiptExpenseLine -> Account
        [MaxLength(159)]
        public string ExpenseLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ExpenseLineAmount { get; set; }
        public decimal ExpenseLineTaxAmount { get; set; }
        public decimal ExpenseLineTax1Amount { get; set; }
        [MaxLength(4095)]
        public string ExpenseLineMemo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ExpenseLineCustomerRefCustomer { get; set; } // Navigation: ItemReceiptExpenseLine -> Customer
        [MaxLength(209)]
        public string ExpenseLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ExpenseLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ExpenseLineClassRefClass { get; set; } // Navigation: ItemReceiptExpenseLine -> Class
        [MaxLength(159)]
        public string ExpenseLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ExpenseLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ExpenseLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemReceiptExpenseLine -> SalesTaxCode
        [MaxLength(36)]
        public string ExpenseLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ExpenseLineTaxCodeRefTaxCode { get; set; } // Navigation: ItemReceiptExpenseLine -> TaxCode
        [MaxLength(3)]
        public string ExpenseLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ExpenseLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ExpenseLineBillableStatus { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ItemReceiptItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: ItemReceipt.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: ItemReceiptItemLine -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: ItemReceiptItemLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string LiabilityAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account LiabilityAccountRefAccount { get; set; } // Navigation: ItemReceiptItemLine -> Account
        [MaxLength(159)]
        public string LiabilityAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        [MaxLength(11)]
        public string ItemLineType { get; set; }
        public int ItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemGroupRefItem { get; set; } // Navigation: ItemReceiptItemLine -> Item
        [MaxLength(31)]
        public string ItemGroupLineItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemGroupLineDesc { get; set; }
        public decimal ItemGroupLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemReceiptItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemGroupLineTotalAmount { get; set; }
        public int ItemGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemLineItemRefItem { get; set; } // Navigation: ItemReceiptItemLine -> Item
        [MaxLength(159)]
        public string ItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemLineDesc { get; set; }
        public decimal ItemLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemReceiptItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemLineCost { get; set; }
        public decimal ItemLineAmount { get; set; }
        public decimal ItemLineTaxAmount { get; set; }
        public decimal ItemLineTax1Amount { get; set; }
        [MaxLength(36)]
        public string ItemLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ItemLineCustomerRefCustomer { get; set; } // Navigation: ItemReceiptItemLine -> Customer
        [MaxLength(209)]
        public string ItemLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ItemLineClassRefClass { get; set; } // Navigation: ItemReceiptItemLine -> Class
        [MaxLength(159)]
        public string ItemLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ItemLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemReceiptItemLine -> SalesTaxCode
        [MaxLength(36)]
        public string ItemLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ItemLineTaxCodeRefTaxCode { get; set; } // Navigation: ItemReceiptItemLine -> TaxCode
        [MaxLength(3)]
        public string ItemLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ItemLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ItemLineBillableStatus { get; set; }
        [MaxLength(36)]
        public string ItemLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ItemLineOverrideItemAccountRefAccount { get; set; } // Navigation: ItemReceiptItemLine -> Account
        [MaxLength(159)]
        public string ItemLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string ItemLineLinkToTxnTxnID { get; set; }
        [MaxLength(36)]
        public string ItemLineLinkToTxnTxnLineID { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class ItemReceiptLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: ItemReceipt.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: ItemReceiptLinkedTxn -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: ItemReceiptLinkedTxn -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string LiabilityAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account LiabilityAccountRefAccount { get; set; } // Navigation: ItemReceiptLinkedTxn -> Account
        [MaxLength(159)]
        public string LiabilityAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string LinkToTxnID1 { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(8)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ItemSalesTax
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsUsedOnPurchaseTransaction { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        public decimal TaxRate { get; set; }
        [MaxLength(36)]
        public string TaxVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor TaxVendorRefVendor { get; set; } // Navigation: ItemSalesTax -> Vendor
        [MaxLength(41)]
        public string TaxVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string SalesTaxReturnLineRefListID { get; set; }
        [MaxLength(41)]
        public string SalesTaxReturnLineRefFullName { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemSalesTaxGroup
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemSalesTaxGroupLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: ItemSalesTaxGroup.ListID
        public virtual ItemSalesTaxGroup ItemSalesTaxGroup { get; set; } // Navigation: ItemSalesTaxGroupLine -> ItemSalesTaxGroup
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public int ItemSalesTaxSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemSalesTaxRefSalesTaxCode { get; set; } // Navigation: ItemSalesTaxGroupLine -> SalesTaxCode
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ItemService
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
        [MaxLength(36)]
        public string UnitOfMeasureSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSetRefUnitOfMeasureSet { get; set; } // Navigation: ItemService -> UnitOfMeasureSet
        [MaxLength(31)]
        public string UnitOfMeasureSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool ForceUOMChange { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: ItemService -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(4095)]
        public string SalesOrPurchaseDesc { get; set; }
        public decimal SalesOrPurchasePrice { get; set; }
        public decimal SalesOrPurchasePricePercent { get; set; }
        [MaxLength(36)]
        public string SalesOrPurchaseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesOrPurchaseAccountRefAccount { get; set; } // Navigation: ItemService -> Account
        [MaxLength(159)]
        public string SalesOrPurchaseAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesOrPurchaseApplyAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string SalesAndPurchaseSalesDesc { get; set; }
        public decimal SalesAndPurchaseSalesPrice { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseIncomeAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseIncomeAccountRefAccount { get; set; } // Navigation: ItemService -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseIncomeAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesAndPurchaseApplyIncomeAccountRefToExistingTxns { get; set; }
        [MaxLength(4095)]
        public string SalesAndPurchasePurchaseDesc { get; set; }
        public decimal SalesAndPurchasePurchaseCost { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchaseExpenseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesAndPurchaseExpenseAccountRefAccount { get; set; } // Navigation: ItemService -> Account
        [MaxLength(159)]
        public string SalesAndPurchaseExpenseAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool SalesAndPurchaseApplyExpenseAccountRefToExistingTxns { get; set; }
        [MaxLength(36)]
        public string SalesAndPurchasePrefVendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor SalesAndPurchasePrefVendorRefVendor { get; set; } // Navigation: ItemService -> Vendor
        [MaxLength(41)]
        public string SalesAndPurchasePrefVendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class ItemSubtotal
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(4095)]
        public string ItemDesc { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        [MaxLength(24)]
        public string SpecialItemType { get; set; } // Relates To: SpecialItem.SpecialItemType
    }
    public partial class JobType
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(41)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
    }
    public partial class JournalEntry
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public bool IsAdjustment { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; }
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }
        public decimal ExchangeRate { get; set; }
    }
    public partial class JournalEntryCreditLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: JournalEntry.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public bool IsAdjustment { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; }
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }
        [MaxLength(36)]
        public string JournalCreditLineTxnLineID { get; set; }
        [MaxLength(14)]
        public string JournalCreditLineType { get; set; }
        [MaxLength(36)]
        public string JournalCreditLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account JournalCreditLineAccountRefAccount { get; set; } // Navigation: JournalEntryCreditLine -> Account
        [MaxLength(159)]
        public string JournalCreditLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal JournalCreditLineAmount { get; set; }
        public decimal JournalCreditLineTaxAmount { get; set; }
        [MaxLength(4095)]
        public string JournalCreditLineMemo { get; set; }
        [MaxLength(36)]
        public string JournalCreditLineEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity JournalCreditLineEntityRefEntity { get; set; } // Navigation: JournalEntryCreditLine -> Entity
        [MaxLength(209)]
        public string JournalCreditLineEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string JournalCreditLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class JournalCreditLineClassRefClass { get; set; } // Navigation: JournalEntryCreditLine -> Class
        [MaxLength(159)]
        public string JournalCreditLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(13)]
        public string JournalCreditLineBillableStatus { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool FQSaveToCache { get; set; }
        [MaxLength(73)]
        public string FQTransactionLinkKey { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class JournalEntryDebitLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: JournalEntry.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public bool IsAdjustment { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; }
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }
        [MaxLength(36)]
        public string JournalDebitLineTxnLineID { get; set; }
        [MaxLength(14)]
        public string JournalDebitLineType { get; set; }
        [MaxLength(36)]
        public string JournalDebitLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account JournalDebitLineAccountRefAccount { get; set; } // Navigation: JournalEntryDebitLine -> Account
        [MaxLength(159)]
        public string JournalDebitLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal JournalDebitLineAmount { get; set; }
        public decimal JournalDebitLineTaxAmount { get; set; }
        [MaxLength(4095)]
        public string JournalDebitLineMemo { get; set; }
        [MaxLength(36)]
        public string JournalDebitLineEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity JournalDebitLineEntityRefEntity { get; set; } // Navigation: JournalEntryDebitLine -> Entity
        [MaxLength(209)]
        public string JournalDebitLineEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string JournalDebitLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class JournalDebitLineClassRefClass { get; set; } // Navigation: JournalEntryDebitLine -> Class
        [MaxLength(159)]
        public string JournalDebitLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(13)]
        public string JournalDebitLineBillableStatus { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool FQSaveToCache { get; set; }
        [MaxLength(73)]
        public string FQTransactionLinkKey { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class JournalEntryLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: JournalEntry.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        public bool IsAdjustment { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; }
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }
        public int JournalLineSeqNo { get; set; }
        [MaxLength(25)]
        public string JournalLineType { get; set; }
        [MaxLength(36)]
        public string JournalLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string JournalLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account JournalLineAccountRefAccount { get; set; } // Navigation: JournalEntryLine -> Account
        [MaxLength(159)]
        public string JournalLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal JournalLineAmount { get; set; }
        public decimal JournalLineCreditAmount { get; set; }
        public decimal JournalLineDebitAmount { get; set; }
        [MaxLength(4095)]
        public string JournalLineMemo { get; set; }
        [MaxLength(36)]
        public string JournalLineEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity JournalLineEntityRefEntity { get; set; } // Navigation: JournalEntryLine -> Entity
        [MaxLength(209)]
        public string JournalLineEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string JournalLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class JournalLineClassRefClass { get; set; } // Navigation: JournalEntryLine -> Class
        [MaxLength(159)]
        public string JournalLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(13)]
        public string JournalLineBillableStatus { get; set; }
        public decimal ExchangeRate { get; set; }
        [MaxLength(73)]
        public string FQTransactionLinkKey { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ListDeleted
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(20)]
        public string ListDelType { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeDeleted { get; set; }
        public DateTime TimeModified { get; set; }
        [MaxLength(41)]
        public string FullName { get; set; }
    }
    public partial class OtherName
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity Entity { get; set; } // Navigation: OtherName -> Entity
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(41)]
        public string Name { get; set; }
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
        public string OtherNameAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressAddr2 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressAddr3 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressAddr4 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressAddr5 { get; set; }
        [MaxLength(31)]
        public string OtherNameAddressCity { get; set; }
        [MaxLength(21)]
        public string OtherNameAddressState { get; set; }
        [MaxLength(21)]
        public string OtherNameAddressProvince { get; set; }
        [MaxLength(21)]
        public string OtherNameAddressCounty { get; set; }
        [MaxLength(13)]
        public string OtherNameAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string OtherNameAddressCountry { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressNote { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string OtherNameAddressBlockAddr5 { get; set; }
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
        [MaxLength(99)]
        public string AccountNumber { get; set; }
        [MaxLength(99)]
        public string BusinessNumber { get; set; }
        [MaxLength(4095)]
        public string Notes { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; }
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class PaymentMethod
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(41)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string PaymentMethodType { get; set; }
    }
    public partial class PayrollItemNonWage
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(19)]
        public string NonWageType { get; set; }
        [MaxLength(36)]
        public string ExpenseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseAccountRefAccount { get; set; } // Navigation: PayrollItemNonWage -> Account
        [MaxLength(159)]
        public string ExpenseAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string LiabilityAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account LiabilityAccountRefAccount { get; set; } // Navigation: PayrollItemNonWage -> Account
        [MaxLength(159)]
        public string LiabilityAccountRefFullName { get; set; } // Relates To: Account.FullName
    }
    public partial class PayrollItemWage
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(14)]
        public string WageType { get; set; }
        [MaxLength(36)]
        public string ExpenseAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseAccountRefAccount { get; set; } // Navigation: PayrollItemWage -> Account
        [MaxLength(159)]
        public string ExpenseAccountRefFullName { get; set; } // Relates To: Account.FullName
    }
    public partial class Preferences
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required]
        public int ID { get; set; }
        public bool AccountingPrefsIsUsingAccountNumbers { get; set; }
        public bool AccountingPrefsIsRequiringAccounts { get; set; }
        public bool AccountingPrefsIsUsingClassTracking { get; set; }
        public bool AccountingPrefsIsUsingAuditTrail { get; set; }
        public bool AccountingPrefsIsAssigningJournalEntryNumbers { get; set; }
        public DateTime AccountingPrefsClosingDate { get; set; }
        public decimal FinanceChargePrefsAnnualInterestRate { get; set; }
        public decimal FinanceChargePrefsMinFinanceCharge { get; set; }
        public int FinanceChargePrefsGracePeriod { get; set; }
        [MaxLength(36)]
        public string FinanceChargePrefsFinanceChargeAcctRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account FinanceChargePrefsFinanceChargeAcctRefAccount { get; set; } // Navigation: Preferences -> Account
        [MaxLength(159)]
        public string FinanceChargePrefsFinanceChargeAcctRefFullName { get; set; } // Relates To: Account.FullName
        public bool FinanceChargePrefsIsAssessingForOverdueCharges { get; set; }
        [MaxLength(20)]
        public string FinanceChargePrefsCalculateChargesFrom { get; set; }
        public bool FinanceChargePrefsIsMarkedToBePrinted { get; set; }
        public bool JobsAndEstimatesPrefsIsUsingEstimates { get; set; }
        public bool JobsAndEstimatesPrefsIsUsingProgressInvoicing { get; set; }
        public bool JobsAndEstimatesPrefsIsPrintItemsWithZeroAmt { get; set; }
        public bool MultiCurrencyPrefsIsMultiCurrencyOn { get; set; }
        [MaxLength(36)]
        public string MultiCurrencyPrefsHomeCurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency MultiCurrencyPrefsHomeCurrencyRefCurrency { get; set; } // Navigation: Preferences -> Currency
        [MaxLength(159)]
        public string MultiCurrencyPrefsHomeCurrencyRefFullName { get; set; } // Relates To: Currency.FullName
        public bool PurchasesAndVendorsPrefsIsUsingInventory { get; set; }
        public int PurchasesAndVendorsPrefsDaysBillsAreDue { get; set; }
        public bool PurchasesAndVendorsPrefsIsAutomaticUsingDis { get; set; }
        [MaxLength(36)]
        public string PurchasesAndVendorsPrefDefaultDisARefListID { get; set; } // Relates To: Account.ListID
        public virtual Account PurchasesAndVendorsPrefDefaultDisARefAccount { get; set; } // Navigation: Preferences -> Account
        [MaxLength(159)]
        public string PurchasesAndVendorsPrefDefaultDisARefFullName { get; set; } // Relates To: Account.FullName
        public bool PurchasesAndVendorsPrefsIsUsingUnitsOfMeasure { get; set; }
        [MaxLength(22)]
        public string ReportsPrefsAgingReportBasis { get; set; }
        [MaxLength(7)]
        public string ReportsPrefsSummaryReportBasis { get; set; }
        [MaxLength(36)]
        public string SalesAndCustomersPrefsDeftShipMethRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod SalesAndCustomersPrefsDeftShipMethRefShipMethod { get; set; } // Navigation: Preferences -> ShipMethod
        [MaxLength(15)]
        public string SalesAndCustomersPrefsDeftShipMethRefFullName { get; set; } // Relates To: ShipMethod.Name
        [MaxLength(13)]
        public string SalesAndCustomersPrefsDefaultFOB { get; set; }
        public decimal SalesAndCustomersPrefsDefaultMarkup { get; set; }
        public bool SalesAndCustomersPrefsIsTrackingRembrsdExpInc { get; set; }
        public bool SalesAndCustomersPrefsIsAutoApplyingPayments { get; set; }
        [MaxLength(36)]
        public string SalesTaxPrefsDefaultItemSalesTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode { get; set; } // Navigation: Preferences -> SalesTaxCode
        [MaxLength(31)]
        public string SalesTaxPrefsDefaultItemSalesTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(9)]
        public string SalesTaxPrefsPaySalesTax { get; set; }
        [MaxLength(36)]
        public string SalesTaxPrefsDefaultTaxableSaleTCRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode { get; set; } // Navigation: Preferences -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxPrefsDefaultTaxableSaleTCRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string SalesTaxPrefsDefaultNonTaxableSaleTCRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode { get; set; } // Navigation: Preferences -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxPrefsDefaultNonTaxableSaleTCRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(9)]
        public string TimeTrackingPrefsFirstDayOfWeek { get; set; }
        public bool CurrentAppAccessRightsIsAutomaticLoginAllowed { get; set; }
        [MaxLength(29)]
        public string CurrentAppAccessRightsAutomaticLoginUserName { get; set; }
        public bool CurrentAppAccessRightsIsPersonalDataAccAllowed { get; set; }
        public bool ItemsAndInventoryPrefsEnhancedInventoryReceivingEnabled { get; set; }
        [MaxLength(12)]
        public string ItemsAndInventoryPrefsIsTrackingSerialOrLotNumber { get; set; }
        public bool ItemsAndInventoryPrefsIsTrackingOnSalesTransactionsEnabled { get; set; }
        public bool ItemsAndInventoryPrefsIsTrackingOnPurchaseTransactionsEnabled { get; set; }
        public bool ItemsAndInventoryPrefsIsTrackingOnInventoryAdjustmentEnabled { get; set; }
        public bool ItemsAndInventoryPrefsIsTrackingOnBuildAssemblyEnabled { get; set; }
        public bool ItemsAndInventoryPrefsFIFOEnabled { get; set; }
        public DateTime ItemsAndInventoryPrefsFIFOEffectiveDate { get; set; }
    }
    public partial class PriceLevel
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string PriceLevelType { get; set; }
        public decimal PriceLevelFixedPercentage { get; set; }
    }
    public partial class PriceLevelPerItem
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel PriceLevel { get; set; } // Navigation: PriceLevelPerItem -> PriceLevel
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string PriceLevelType { get; set; }
        public decimal PriceLevelFixedPercentage { get; set; }
        public int PriceLevelPerItemSeqNo { get; set; }
        [MaxLength(36)]
        public string PriceLevelPerItemItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item PriceLevelPerItemItemRefItem { get; set; } // Navigation: PriceLevelPerItem -> Item
        [MaxLength(159)]
        public string PriceLevelPerItemItemRefFullName { get; set; } // Relates To: Item.FullName
        public decimal PriceLevelPerItemCustomPrice { get; set; }
        public decimal PriceLevelPerItemCustomPricePercent { get; set; }
        public decimal PriceLevelPerItemAdjustPercentage { get; set; }
        [MaxLength(15)]
        public string PriceLevelPerItemAdjustRelativeTo { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class PurchaseOrder
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: PurchaseOrder -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: PurchaseOrder -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ShipToEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity ShipToEntityRefEntity { get; set; } // Navigation: PurchaseOrder -> Entity
        [MaxLength(209)]
        public string ShipToEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: PurchaseOrder -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr5 { get; set; }
        [MaxLength(31)]
        public string VendorAddressCity { get; set; }
        [MaxLength(21)]
        public string VendorAddressState { get; set; }
        [MaxLength(21)]
        public string VendorAddressProvince { get; set; }
        [MaxLength(21)]
        public string VendorAddressCounty { get; set; }
        [MaxLength(13)]
        public string VendorAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string VendorAddressCountry { get; set; }
        [MaxLength(41)]
        public string VendorAddressNote { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr5 { get; set; }
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
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: PurchaseOrder -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: PurchaseOrder -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsManuallyClosed { get; set; }
        public bool IsFullyReceived { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(99)]
        public string VendorMsg { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: PurchaseOrder -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: PurchaseOrder -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(29)]
        public string CustomFieldOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldOther2 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class PurchaseOrderLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: PurchaseOrder.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: PurchaseOrderLine -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: PurchaseOrderLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ShipToEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity ShipToEntityRefEntity { get; set; } // Navigation: PurchaseOrderLine -> Entity
        [MaxLength(209)]
        public string ShipToEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: PurchaseOrderLine -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr5 { get; set; }
        [MaxLength(31)]
        public string VendorAddressCity { get; set; }
        [MaxLength(21)]
        public string VendorAddressState { get; set; }
        [MaxLength(21)]
        public string VendorAddressProvince { get; set; }
        [MaxLength(21)]
        public string VendorAddressCounty { get; set; }
        [MaxLength(13)]
        public string VendorAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string VendorAddressCountry { get; set; }
        [MaxLength(41)]
        public string VendorAddressNote { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr5 { get; set; }
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
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: PurchaseOrderLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: PurchaseOrderLine -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsManuallyClosed { get; set; }
        public bool IsFullyReceived { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(99)]
        public string VendorMsg { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: PurchaseOrderLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: PurchaseOrderLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(29)]
        public string CustomFieldOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldOther2 { get; set; }
        [MaxLength(11)]
        public string PurchaseOrderLineType { get; set; }
        public int PurchaseOrderLineSeqNo { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineGroupTxnLineID { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item PurchaseOrderLineGroupItemGroupRefItem { get; set; } // Navigation: PurchaseOrderLine -> Item
        [MaxLength(159)]
        public string PurchaseOrderLineGroupItemGroupFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string PurchaseOrderLineGroupDesc { get; set; }
        public decimal PurchaseOrderLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string PurchaseOrderLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string PurchaseOrderLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: PurchaseOrderLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string PurchaseOrderLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool PurchaseOrderLineGroupIsPrintItemsInGroup { get; set; }
        public decimal PurchaseOrderLineGroupTotalAmount { get; set; }
        public DateTime PurchaseOrderLineGroupServiceDate { get; set; }
        public int PurchaseOrderLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item PurchaseOrderLineItemRefItem { get; set; } // Navigation: PurchaseOrderLine -> Item
        [MaxLength(159)]
        public string PurchaseOrderLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(31)]
        public string PurchaseOrderLineManufacturerPartNumber { get; set; }
        [MaxLength(4095)]
        public string PurchaseOrderLineDesc { get; set; }
        public decimal PurchaseOrderLineQuantity { get; set; }
        [MaxLength(31)]
        public string PurchaseOrderLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string PurchaseOrderLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: PurchaseOrderLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string PurchaseOrderLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal PurchaseOrderLineRate { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class PurchaseOrderLineClassRefClass { get; set; } // Navigation: PurchaseOrderLine -> Class
        [MaxLength(159)]
        public string PurchaseOrderLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal PurchaseOrderLineAmount { get; set; }
        public decimal PurchaseOrderLineTaxAmount { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer PurchaseOrderLineCustomerRefCustomer { get; set; } // Navigation: PurchaseOrderLine -> Customer
        [MaxLength(209)]
        public string PurchaseOrderLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        public DateTime PurchaseOrderLineServiceDate { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode PurchaseOrderLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: PurchaseOrderLine -> SalesTaxCode
        [MaxLength(36)]
        public string PurchaseOrderLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode PurchaseOrderLineTaxCodeRefTaxCode { get; set; } // Navigation: PurchaseOrderLine -> TaxCode
        [MaxLength(3)]
        public string PurchaseOrderLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string PurchaseOrderLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal PurchaseOrderLineReceivedQuantity { get; set; }
        public decimal PurchaseOrderLineUnbilledQuantity { get; set; }
        public bool PurchaseOrderLineIsBilled { get; set; }
        public bool PurchaseOrderLineIsManuallyClosed { get; set; }
        [MaxLength(36)]
        public string PurchaseOrderLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account PurchaseOrderLineOverrideItemAccountRefAccount { get; set; } // Navigation: PurchaseOrderLine -> Account
        [MaxLength(159)]
        public string PurchaseOrderLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(29)]
        public string CustomFieldPurchaseOrderLineOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldPurchaseOrderLineOther2 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class PurchaseOrderLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: PurchaseOrder.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: PurchaseOrderLinkedTxn -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: PurchaseOrderLinkedTxn -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ShipToEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity ShipToEntityRefEntity { get; set; } // Navigation: PurchaseOrderLinkedTxn -> Entity
        [MaxLength(209)]
        public string ShipToEntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: PurchaseOrderLinkedTxn -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr5 { get; set; }
        [MaxLength(31)]
        public string VendorAddressCity { get; set; }
        [MaxLength(21)]
        public string VendorAddressState { get; set; }
        [MaxLength(21)]
        public string VendorAddressProvince { get; set; }
        [MaxLength(21)]
        public string VendorAddressCounty { get; set; }
        [MaxLength(13)]
        public string VendorAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string VendorAddressCountry { get; set; }
        [MaxLength(41)]
        public string VendorAddressNote { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr5 { get; set; }
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
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: PurchaseOrderLinkedTxn -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: PurchaseOrderLinkedTxn -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsManuallyClosed { get; set; }
        public bool IsFullyReceived { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(99)]
        public string VendorMsg { get; set; }
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: PurchaseOrderLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: PurchaseOrderLinkedTxn -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(29)]
        public string CustomFieldOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldOther2 { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(11)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ReceivePayment
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: ReceivePayment -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: ReceivePayment -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: ReceivePayment -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: ReceivePayment -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(25)]
        public string CreditCardTxnInfoInputCreditCardNumber { get; set; }
        public int CreditCardTxnInfoInputExpirationMonth { get; set; }
        public int CreditCardTxnInfoInputExpirationYear { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputNameOnCard { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardAddress { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardPostalCode { get; set; }
        [MaxLength(50)]
        public string CreditCardTxnInfoInputCommercialCardCode { get; set; }
        [MaxLength(14)]
        public string CreditCardTxnInfoInputTransactionMode { get; set; }
        [MaxLength(18)]
        public string CreditCardTxnInfoInputCreditCardTxnType { get; set; }
        public int CreditCardTxnInfoResultResultCode { get; set; }
        [MaxLength(500)]
        public string CreditCardTxnInfoResultResultMessage { get; set; }
        [MaxLength(24)]
        public string CreditCardTxnInfoResultCreditCardTransID { get; set; }
        [MaxLength(32)]
        public string CreditCardTxnInfoResultMerchantAccountNumber { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAuthorizationCode { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSStreet { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSZip { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultCardSecurityCodeMatch { get; set; }
        [MaxLength(84)]
        public string CreditCardTxnInfoResultReconBatchID { get; set; }
        public int CreditCardTxnInfoResultPaymentGroupingCode { get; set; }
        [MaxLength(9)]
        public string CreditCardTxnInfoResultPaymentStatus { get; set; }
        public DateTime CreditCardTxnInfoResultTxnAuthorizationTime { get; set; }
        public int CreditCardTxnInfoResultTxnAuthorizationStamp { get; set; }
        [MaxLength(16)]
        public string CreditCardTxnInfoResultClientTransID { get; set; }
        public bool IsAutoApply { get; set; }
        public decimal UnusedPayment { get; set; }
        public decimal UnusedCredits { get; set; }
        public decimal ExchangeRate { get; set; }
    }
    public partial class ReceivePaymentLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: ReceivePayment.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: ReceivePaymentLine -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: ReceivePaymentLine -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: ReceivePaymentLine -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: ReceivePaymentLine -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(25)]
        public string CreditCardTxnInfoInputCreditCardNumber { get; set; }
        public int CreditCardTxnInfoInputExpirationMonth { get; set; }
        public int CreditCardTxnInfoInputExpirationYear { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputNameOnCard { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardAddress { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardPostalCode { get; set; }
        [MaxLength(50)]
        public string CreditCardTxnInfoInputCommercialCardCode { get; set; }
        [MaxLength(14)]
        public string CreditCardTxnInfoInputTransactionMode { get; set; }
        [MaxLength(18)]
        public string CreditCardTxnInfoInputCreditCardTxnType { get; set; }
        public int CreditCardTxnInfoResultResultCode { get; set; }
        [MaxLength(500)]
        public string CreditCardTxnInfoResultResultMessage { get; set; }
        [MaxLength(24)]
        public string CreditCardTxnInfoResultCreditCardTransID { get; set; }
        [MaxLength(32)]
        public string CreditCardTxnInfoResultMerchantAccountNumber { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAuthorizationCode { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSStreet { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSZip { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultCardSecurityCodeMatch { get; set; }
        [MaxLength(84)]
        public string CreditCardTxnInfoResultReconBatchID { get; set; }
        public int CreditCardTxnInfoResultPaymentGroupingCode { get; set; }
        [MaxLength(9)]
        public string CreditCardTxnInfoResultPaymentStatus { get; set; }
        public DateTime CreditCardTxnInfoResultTxnAuthorizationTime { get; set; }
        public int CreditCardTxnInfoResultTxnAuthorizationStamp { get; set; }
        [MaxLength(16)]
        public string CreditCardTxnInfoResultClientTransID { get; set; }
        public bool IsAutoApply { get; set; }
        public decimal UnusedPayment { get; set; }
        public decimal UnusedCredits { get; set; }
        [Required, MaxLength(36)]
        public string AppliedToTxnTxnID { get; set; }
        public decimal AppliedToTxnPaymentAmount { get; set; }
        [MaxLength(14)]
        public string AppliedToTxnTxnType { get; set; }
        public DateTime AppliedToTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string AppliedToTxnRefNumber { get; set; }
        public decimal AppliedToTxnBalanceRemaining { get; set; }
        public decimal AppliedToTxnAmount { get; set; }
        [MaxLength(36)]
        public string AppliedToTxnSetCreditCreditTxnID { get; set; }
        public decimal AppliedToTxnSetCreditAppliedAmount { get; set; }
        public decimal AppliedToTxnDiscountAmount { get; set; }
        [MaxLength(36)]
        public string AppliedToTxnDiscountAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AppliedToTxnDiscountAccountRefAccount { get; set; } // Navigation: ReceivePaymentLine -> Account
        [MaxLength(159)]
        public string AppliedToTxnDiscountAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(36)]
        public string AppliedToTxnDiscountClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class AppliedToTxnDiscountClassRefClass { get; set; } // Navigation: ReceivePaymentLine -> Class
        [MaxLength(159)]
        public string AppliedToTxnDiscountClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal ExchangeRate { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ReceivePaymentToDeposit
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; }
        [MaxLength(21)]
        public string TxnLineID { get; set; }
        [MaxLength(14)]
        public string TxnType { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: ReceivePaymentToDeposit -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        public decimal Amount { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Sales
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: Sales -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: Sales -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: Sales -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string CheckNumber { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: Sales -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        public bool IsFinanceCharge { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: Sales -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: Sales -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: Sales -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: Sales -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal Amount { get; set; }
        public decimal Remaining { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: Sales -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Sales -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: Sales -> TaxCode
        public decimal SuggestedDiscountAmount { get; set; }
        public DateTime SuggestedDiscountDate { get; set; }
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: Sales -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool CreditMemoLineGroupIsPrintItemsInGroup { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class SalesLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: Sales.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: SalesLine -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: SalesLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ARAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ARAccountRefAccount { get; set; } // Navigation: SalesLine -> Account
        [MaxLength(159)]
        public string ARAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string CheckNumber { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: SalesLine -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        public bool IsFinanceCharge { get; set; }
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: SalesLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: SalesLine -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: SalesLine -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: SalesLine -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal Amount { get; set; }
        public decimal Remaining { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsPaid { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: SalesLine -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesLine -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: SalesLine -> TaxCode
        public decimal SuggestedDiscountAmount { get; set; }
        public DateTime SuggestedDiscountDate { get; set; }
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: SalesLine -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        public bool CreditMemoLineGroupIsPrintItemsInGroup { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
        [MaxLength(11)]
        public string SalesLineType { get; set; }
        public int SalesLineSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesLineGroupTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item SalesLineGroupItemGroupRefItem { get; set; } // Navigation: SalesLine -> Item
        [MaxLength(31)]
        public string SalesLineGroupItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string SalesLineGroupDesc { get; set; }
        public decimal SalesLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string SalesLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool SalesLineGroupIsPrintItemsInGroup { get; set; }
        public decimal SalesLineGroupTotalAmount { get; set; }
        public int SalesLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item SalesLineItemRefItem { get; set; } // Navigation: SalesLine -> Item
        [MaxLength(159)]
        public string SalesLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string SalesLineDesc { get; set; }
        public decimal SalesLineQuantity { get; set; }
        [MaxLength(31)]
        public string SalesLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal SalesLineRate { get; set; }
        public decimal SalesLineRatePercent { get; set; }
        [MaxLength(36)]
        public string SalesLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class SalesLineClassRefClass { get; set; } // Navigation: SalesLine -> Class
        [MaxLength(159)]
        public string SalesLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal SalesLineAmount { get; set; }
        public DateTime SalesLineServiceDate { get; set; }
        [MaxLength(36)]
        public string SalesLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string SalesLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode SalesLineTaxCodeRefTaxCode { get; set; } // Navigation: SalesLine -> TaxCode
        [MaxLength(3)]
        public string SalesLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class SalesOrder
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: SalesOrder -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: SalesOrder -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: SalesOrder -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: SalesOrder -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: SalesOrder -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: SalesOrder -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: SalesOrder -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsManuallyClosed { get; set; }
        public bool IsFullyInvoiced { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: SalesOrder -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesOrder -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: SalesOrder -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class SalesOrderLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: SalesOrder.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: SalesOrderLine -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: SalesOrderLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: SalesOrderLine -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: SalesOrderLine -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: SalesOrderLine -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: SalesOrderLine -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: SalesOrderLine -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsManuallyClosed { get; set; }
        public bool IsFullyInvoiced { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: SalesOrderLine -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesOrderLine -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: SalesOrderLine -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public int SalesOrderLineSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesOrderLineGroupTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesOrderLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item SalesOrderLineGroupItemGroupRefItem { get; set; } // Navigation: SalesOrderLine -> Item
        [MaxLength(159)]
        public string SalesOrderLineGroupItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string SalesOrderLineGroupDesc { get; set; }
        public decimal SalesOrderLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string SalesOrderLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string SalesOrderLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: SalesOrderLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string SalesOrderLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool SalesOrderLineGroupIsPrintItemsInGroup { get; set; }
        public decimal SalesOrderLineGroupTotalAmount { get; set; }
        public int SalesOrderLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesOrderLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesOrderLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item SalesOrderLineItemRefItem { get; set; } // Navigation: SalesOrderLine -> Item
        [MaxLength(159)]
        public string SalesOrderLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string SalesOrderLineDesc { get; set; }
        public decimal SalesOrderLineQuantity { get; set; }
        [MaxLength(31)]
        public string SalesOrderLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string SalesOrderLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: SalesOrderLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string SalesOrderLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal SalesOrderLineRate { get; set; }
        public decimal SalesOrderLineRatePercent { get; set; }
        [MaxLength(36)]
        public string SalesOrderLinePriceLevelRefListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel SalesOrderLinePriceLevelRefPriceLevel { get; set; } // Navigation: SalesOrderLine -> PriceLevel
        [MaxLength(159)]
        public string SalesOrderLinePriceLevelRefFullName { get; set; } // Relates To: PriceLevel.Name
        [MaxLength(36)]
        public string SalesOrderLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class SalesOrderLineClassRefClass { get; set; } // Navigation: SalesOrderLine -> Class
        [MaxLength(159)]
        public string SalesOrderLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal SalesOrderLineAmount { get; set; }
        public decimal SalesOrderLineTaxAmount { get; set; }
        [MaxLength(36)]
        public string SalesOrderLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesOrderLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesOrderLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesOrderLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string SalesOrderLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode SalesOrderLineTaxCodeRefTaxCode { get; set; } // Navigation: SalesOrderLine -> TaxCode
        [MaxLength(3)]
        public string SalesOrderLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        public decimal SalesOrderLineInvoiced { get; set; }
        public bool SalesOrderLineIsManuallyClosed { get; set; }
        [MaxLength(29)]
        public string CustomFieldSalesOrderLineOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldSalesOrderLineOther2 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class SalesOrderLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: SalesOrder.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [Required, MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: SalesOrderLinkedTxn -> Customer
        [Required, MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: SalesOrderLinkedTxn -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: SalesOrderLinkedTxn -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        [MaxLength(25)]
        public string PONumber { get; set; }
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: SalesOrderLinkedTxn -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: SalesOrderLinkedTxn -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        [MaxLength(13)]
        public string FOB { get; set; }
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: SalesOrderLinkedTxn -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: SalesOrderLinkedTxn -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsManuallyClosed { get; set; }
        public bool IsFullyInvoiced { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: SalesOrderLinkedTxn -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesOrderLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: SalesOrderLinkedTxn -> TaxCode
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(8)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class SalesReceipt
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: SalesReceipt -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: SalesReceipt -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: SalesReceipt -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string CheckNumber { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: SalesReceipt -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: SalesReceipt -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: SalesReceipt -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: SalesReceipt -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: SalesReceipt -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesReceipt -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: SalesReceipt -> TaxCode
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: SalesReceipt -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(25)]
        public string CreditCardTxnInfoInputCreditCardNumber { get; set; }
        public int CreditCardTxnInfoInputExpirationMonth { get; set; }
        public int CreditCardTxnInfoInputExpirationYear { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputNameOnCard { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardAddress { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardPostalCode { get; set; }
        [MaxLength(50)]
        public string CreditCardTxnInfoInputCommercialCardCode { get; set; }
        [MaxLength(14)]
        public string CreditCardTxnInfoInputTransactionMode { get; set; }
        [MaxLength(18)]
        public string CreditCardTxnInfoInputCreditCardTxnType { get; set; }
        public int CreditCardTxnInfoResultResultCode { get; set; }
        [MaxLength(500)]
        public string CreditCardTxnInfoResultResultMessage { get; set; }
        [MaxLength(24)]
        public string CreditCardTxnInfoResultCreditCardTransID { get; set; }
        [MaxLength(32)]
        public string CreditCardTxnInfoResultMerchantAccountNumber { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAuthorizationCode { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSStreet { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSZip { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultCardSecurityCodeMatch { get; set; }
        [MaxLength(84)]
        public string CreditCardTxnInfoResultReconBatchID { get; set; }
        public int CreditCardTxnInfoResultPaymentGroupingCode { get; set; }
        [MaxLength(9)]
        public string CreditCardTxnInfoResultPaymentStatus { get; set; }
        public DateTime CreditCardTxnInfoResultTxnAuthorizationTime { get; set; }
        public int CreditCardTxnInfoResultTxnAuthorizationStamp { get; set; }
        [MaxLength(16)]
        public string CreditCardTxnInfoResultClientTransID { get; set; }
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class SalesReceiptLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: SalesReceipt.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: SalesReceiptLine -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: SalesReceiptLine -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string TemplateRefListID { get; set; } // Relates To: Template.ListID
        public virtual Template TemplateRefTemplate { get; set; } // Navigation: SalesReceiptLine -> Template
        [MaxLength(159)]
        public string TemplateRefFullName { get; set; } // Relates To: Template.Name
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
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
        public bool IsPending { get; set; }
        [MaxLength(25)]
        public string CheckNumber { get; set; }
        [MaxLength(36)]
        public string PaymentMethodRefListID { get; set; } // Relates To: PaymentMethod.ListID
        public virtual PaymentMethod PaymentMethodRefPaymentMethod { get; set; } // Navigation: SalesReceiptLine -> PaymentMethod
        [MaxLength(31)]
        public string PaymentMethodRefFullName { get; set; } // Relates To: PaymentMethod.Name
        public DateTime DueDate { get; set; }
        [MaxLength(36)]
        public string SalesRepRefListID { get; set; } // Relates To: SalesRep.ListID
        public virtual SalesRep SalesRepRefSalesRep { get; set; } // Navigation: SalesReceiptLine -> SalesRep
        [MaxLength(5)]
        public string SalesRepRefFullName { get; set; } // Relates To: SalesRep.Initial
        public DateTime ShipDate { get; set; }
        [MaxLength(36)]
        public string ShipMethodRefListID { get; set; } // Relates To: ShipMethod.ListID
        public virtual ShipMethod ShipMethodRefShipMethod { get; set; } // Navigation: SalesReceiptLine -> ShipMethod
        [MaxLength(15)]
        public string ShipMethodRefFullName { get; set; } // Relates To: ShipMethod.Name
        [MaxLength(13)]
        public string FOB { get; set; }
        public decimal Subtotal { get; set; }
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: ItemSalesTax.ListID
        public virtual ItemSalesTax ItemSalesTaxRefItemSalesTax { get; set; } // Navigation: SalesReceiptLine -> ItemSalesTax
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: ItemSalesTax.Name
        public decimal SalesTaxPercentage { get; set; }
        public decimal SalesTaxTotal { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(36)]
        public string CustomerMsgRefListID { get; set; } // Relates To: CustomerMsg.ListID
        public virtual CustomerMsg CustomerMsgRefCustomerMsg { get; set; } // Navigation: SalesReceiptLine -> CustomerMsg
        [MaxLength(101)]
        public string CustomerMsgRefFullName { get; set; } // Relates To: CustomerMsg.Name
        public bool IsToBePrinted { get; set; }
        public bool IsToBeEmailed { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string CustomerSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode CustomerSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesReceiptLine -> SalesTaxCode
        [MaxLength(3)]
        public string CustomerSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CustomerTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode CustomerTaxCodeRefTaxCode { get; set; } // Navigation: SalesReceiptLine -> TaxCode
        [MaxLength(36)]
        public string DepositToAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account DepositToAccountRefAccount { get; set; } // Navigation: SalesReceiptLine -> Account
        [MaxLength(159)]
        public string DepositToAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(25)]
        public string CreditCardTxnInfoInputCreditCardNumber { get; set; }
        public int CreditCardTxnInfoInputExpirationMonth { get; set; }
        public int CreditCardTxnInfoInputExpirationYear { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputNameOnCard { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardAddress { get; set; }
        [MaxLength(41)]
        public string CreditCardTxnInfoInputCreditCardPostalCode { get; set; }
        [MaxLength(50)]
        public string CreditCardTxnInfoInputCommercialCardCode { get; set; }
        [MaxLength(14)]
        public string CreditCardTxnInfoInputTransactionMode { get; set; }
        [MaxLength(18)]
        public string CreditCardTxnInfoInputCreditCardTxnType { get; set; }
        public int CreditCardTxnInfoResultResultCode { get; set; }
        [MaxLength(500)]
        public string CreditCardTxnInfoResultResultMessage { get; set; }
        [MaxLength(24)]
        public string CreditCardTxnInfoResultCreditCardTransID { get; set; }
        [MaxLength(32)]
        public string CreditCardTxnInfoResultMerchantAccountNumber { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAuthorizationCode { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSStreet { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultAVSZip { get; set; }
        [MaxLength(12)]
        public string CreditCardTxnInfoResultCardSecurityCodeMatch { get; set; }
        [MaxLength(84)]
        public string CreditCardTxnInfoResultReconBatchID { get; set; }
        public int CreditCardTxnInfoResultPaymentGroupingCode { get; set; }
        [MaxLength(9)]
        public string CreditCardTxnInfoResultPaymentStatus { get; set; }
        public DateTime CreditCardTxnInfoResultTxnAuthorizationTime { get; set; }
        public int CreditCardTxnInfoResultTxnAuthorizationStamp { get; set; }
        [MaxLength(16)]
        public string CreditCardTxnInfoResultClientTransID { get; set; }
        [MaxLength(29)]
        public string CustomFieldOther { get; set; }
        [MaxLength(11)]
        public string SalesReceiptLineItemLineType { get; set; }
        public int SalesReceiptLineSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesReceiptLineGroupTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesReceiptLineGroupItemGroupRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item SalesReceiptLineGroupItemGroupRefItem { get; set; } // Navigation: SalesReceiptLine -> Item
        [MaxLength(159)]
        public string SalesReceiptLineGroupItemGroupRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string SalesReceiptLineGroupDesc { get; set; }
        public decimal SalesReceiptLineGroupQuantity { get; set; }
        [MaxLength(31)]
        public string SalesReceiptLineGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string SalesReceiptLineGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: SalesReceiptLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string SalesReceiptLineGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public bool SalesReceiptLineGroupIsPrintItemsInGroup { get; set; }
        public decimal SalesReceiptLineGroupTotalAmount { get; set; }
        public DateTime SalesReceiptLineGroupServiceDate { get; set; }
        public int SalesReceiptLineGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesReceiptLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesReceiptLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item SalesReceiptLineItemRefItem { get; set; } // Navigation: SalesReceiptLine -> Item
        [MaxLength(159)]
        public string SalesReceiptLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string SalesReceiptLineDesc { get; set; }
        public decimal SalesReceiptLineQuantity { get; set; }
        [MaxLength(31)]
        public string SalesReceiptLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string SalesReceiptLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: SalesReceiptLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string SalesReceiptLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal SalesReceiptLineRate { get; set; }
        public decimal SalesReceiptLineRatePercent { get; set; }
        [MaxLength(36)]
        public string SalesReceiptLinePriceLevelRefListID { get; set; } // Relates To: PriceLevel.ListID
        public virtual PriceLevel SalesReceiptLinePriceLevelRefPriceLevel { get; set; } // Navigation: SalesReceiptLine -> PriceLevel
        [MaxLength(159)]
        public string SalesReceiptLinePriceLevelRefFullName { get; set; } // Relates To: PriceLevel.Name
        [MaxLength(36)]
        public string SalesReceiptLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class SalesReceiptLineClassRefClass { get; set; } // Navigation: SalesReceiptLine -> Class
        [MaxLength(159)]
        public string SalesReceiptLineClassRefFullName { get; set; } // Relates To: Class.FullName
        public decimal SalesReceiptLineAmount { get; set; }
        public decimal SalesReceiptTaxAmount { get; set; }
        public DateTime SalesReceiptLineServiceDate { get; set; }
        [MaxLength(36)]
        public string SalesReceiptLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesReceiptLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: SalesReceiptLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesReceiptLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string SalesReceiptLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode SalesReceiptLineTaxCodeRefTaxCode { get; set; } // Navigation: SalesReceiptLine -> TaxCode
        [MaxLength(3)]
        public string SalesReceiptLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(36)]
        public string SalesReceiptLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account SalesReceiptLineOverrideItemAccountRefAccount { get; set; } // Navigation: SalesReceiptLine -> Account
        [MaxLength(159)]
        public string SalesReceiptLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        [MaxLength(29)]
        public string CustomFieldSalesReceiptLineOther1 { get; set; }
        [MaxLength(29)]
        public string CustomFieldSalesReceiptLineOther2 { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class SalesRep
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(5)]
        public string Initial { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(36)]
        public string SalesRepEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity SalesRepEntityRefEntity { get; set; } // Navigation: SalesRep -> Entity
        [Required, MaxLength(209)]
        public string SalesRepEntityRefFullName { get; set; } // Relates To: Entity.FullName
    }
    public partial class SalesTaxCode
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(3)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public bool IsTaxable { get; set; }
        [MaxLength(31)]
        public string Desc { get; set; }
        [MaxLength(36)]
        public string ItemPurchaseTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemPurchaseTaxRefSalesTaxCode { get; set; } // Navigation: SalesTaxCode -> SalesTaxCode
        [MaxLength(31)]
        public string ItemPurchaseTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemSalesTaxRefSalesTaxCode { get; set; } // Navigation: SalesTaxCode -> SalesTaxCode
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
    }
    public partial class SalesTaxPaymentCheck
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: SalesTaxPaymentCheck -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(36)]
        public string BankAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account BankAccountRefAccount { get; set; } // Navigation: SalesTaxPaymentCheck -> Account
        [MaxLength(159)]
        public string BankAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public bool IsToBePrinted { get; set; }
    }
    public partial class SalesTaxPaymentCheckLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: SalesTaxPaymentCheck.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string PayeeEntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity PayeeEntityRefEntity { get; set; } // Navigation: SalesTaxPaymentCheckLine -> Entity
        [MaxLength(209)]
        public string PayeeEntityRefFullName { get; set; } // Relates To: Entity.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(36)]
        public string BankAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account BankAccountRefAccount { get; set; } // Navigation: SalesTaxPaymentCheckLine -> Account
        [MaxLength(159)]
        public string BankAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Amount { get; set; }
        [MaxLength(11)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(41)]
        public string AddressAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressAddr5 { get; set; }
        [MaxLength(31)]
        public string AddressCity { get; set; }
        [MaxLength(21)]
        public string AddressState { get; set; }
        [MaxLength(21)]
        public string AddressProvince { get; set; }
        [MaxLength(21)]
        public string AddressCounty { get; set; }
        [MaxLength(13)]
        public string AddressPostalCode { get; set; }
        [MaxLength(31)]
        public string AddressCountry { get; set; }
        [MaxLength(41)]
        public string AddressNote { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string AddressBlockAddr5 { get; set; }
        public bool IsToBePrinted { get; set; }
        public int SalesTaxPaymentCheckLineSeqNo { get; set; }
        [MaxLength(36)]
        public string SalesTaxPaymentCheckLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string SalesTaxPaymentCheckLineItemSalesTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode { get; set; } // Navigation: SalesTaxPaymentCheckLine -> SalesTaxCode
        [MaxLength(31)]
        public string SalesTaxPaymentCheckLineItemSalesTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        public decimal SalesTaxPaymentCheckLineAmount { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class ShipMethod
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(15)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public partial class SpecialAccount
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(13)]
        public string SpecialAccountType { get; set; }
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: SpecialAccount -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
    }
    public partial class SpecialItem
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(13)]
        public string SpecialItemType { get; set; }
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
    }
    public partial class StandardTerms
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms Terms { get; set; } // Navigation: StandardTerms -> Terms
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(41)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int StdDueDays { get; set; }
        public int StdDiscountDays { get; set; }
        public decimal DiscountPct { get; set; }
    }
    public partial class TaxCode
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(3)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public bool IsTaxable { get; set; }
        [MaxLength(31)]
        public string Desc { get; set; }
        [MaxLength(36)]
        public string ItemPurchaseTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemPurchaseTaxRefSalesTaxCode { get; set; } // Navigation: TaxCode -> SalesTaxCode
        [MaxLength(31)]
        public string ItemPurchaseTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string ItemSalesTaxRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemSalesTaxRefSalesTaxCode { get; set; } // Navigation: TaxCode -> SalesTaxCode
        [MaxLength(31)]
        public string ItemSalesTaxRefFullName { get; set; } // Relates To: SalesTaxCode.Name
    }
    public partial class Template
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(13)]
        public string TemplateType { get; set; }
    }
    public partial class Terms
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int DayOfMonthDue { get; set; }
        public int DueNextMonthDays { get; set; }
        public int DiscountDayOfMonth { get; set; }
        public decimal DiscountPct { get; set; }
        public int StdDueDays { get; set; }
        public int StdDiscountDays { get; set; }
        public decimal StdDiscountPct { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
    }
    public partial class TimeTracking
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [Required, MaxLength(36)]
        public string EntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity EntityRefEntity { get; set; } // Navigation: TimeTracking -> Entity
        [Required, MaxLength(209)]
        public string EntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: TimeTracking -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemServiceRefListID { get; set; } // Relates To: ItemService.ListID
        public virtual ItemService ItemServiceRefItemService { get; set; } // Navigation: TimeTracking -> ItemService
        [MaxLength(159)]
        public string ItemServiceRefFullName { get; set; } // Relates To: ItemService.FullName
        public decimal Rate { get; set; }
        [Required]
        public int DurationMinutes { get; set; }
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: TimeTracking -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string PayrollItemWageRefListID { get; set; } // Relates To: PayrollItemWage.ListID
        public virtual PayrollItemWage PayrollItemWageRefPayrollItemWage { get; set; } // Navigation: TimeTracking -> PayrollItemWage
        [MaxLength(31)]
        public string PayrollItemWageRefFullName { get; set; } // Relates To: PayrollItemWage.Name
        [MaxLength(4095)]
        public string Notes { get; set; }
        [MaxLength(23)]
        public string BillableStatus { get; set; }
    }
    public partial class ToDo
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(4095)]
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsDone { get; set; }
        public DateTime ReminderDate { get; set; }
    }
    public partial class Transaction
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(25)]
        public string TxnType { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; }
        [MaxLength(36)]
        public string TxnLineID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [MaxLength(36)]
        public string EntityRefListID { get; set; } // Relates To: Entity.ListID
        public virtual Entity EntityRefEntity { get; set; } // Navigation: Transaction -> Entity
        [MaxLength(209)]
        public string EntityRefFullName { get; set; } // Relates To: Entity.FullName
        [MaxLength(36)]
        public string AccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account AccountRefAccount { get; set; } // Navigation: Transaction -> Account
        [MaxLength(159)]
        public string AccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        [MaxLength(23)]
        public string TransactionDetailLevelFilter { get; set; }
        [MaxLength(23)]
        public string TransactionPostingStatusFilter { get; set; }
        [MaxLength(23)]
        public string TransactionPaidStatusFilter { get; set; }
        [MaxLength(25)]
        public string Empty { get; set; }
        [MaxLength(184)]
        public string FQTxnLinkKey { get; set; }
        [MaxLength(184)]
        public string FQJournalEntryLinkKey { get; set; }
        [Required, MaxLength(184)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class TxnDeleted
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(20)]
        public string TxnDelType { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeDeleted { get; set; }
        public DateTime TimeModified { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
    }
    public partial class UnitOfMeasureSet
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string UnitOfMeasureType { get; set; }
        [MaxLength(31)]
        public string BaseUnitName { get; set; }
        [MaxLength(31)]
        public string BaseUnitAbbreviation { get; set; }
    }
    public partial class UnitOfMeasureSetRelatedUnit
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSet { get; set; } // Navigation: UnitOfMeasureSetRelatedUnit -> UnitOfMeasureSet
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string UnitOfMeasureType { get; set; }
        [MaxLength(31)]
        public string BaseUnitName { get; set; }
        [MaxLength(31)]
        public string BaseUnitAbbreviation { get; set; }
        public int RelatedUnitSeqNo { get; set; }
        [MaxLength(31)]
        public string RelatedUnitName { get; set; }
        [MaxLength(31)]
        public string RelatedUnitAbbreviation { get; set; }
        public decimal RelatedUnitConversionRatio { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class UnitOfMeasureSetDefaultUnit
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet UnitOfMeasureSet { get; set; } // Navigation: UnitOfMeasureSetDefaultUnit -> UnitOfMeasureSet
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(15)]
        public string UnitOfMeasureType { get; set; }
        [MaxLength(31)]
        public string BaseUnitName { get; set; }
        [MaxLength(31)]
        public string BaseUnitAbbreviation { get; set; }
        public int DefaultUnitUnitSeqNo { get; set; }
        [MaxLength(15)]
        public string DefaultUnitUnitUsedFor { get; set; }
        [MaxLength(31)]
        public string DefaultUnitUnit { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class Vehicle
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(256)]
        public string Desc { get; set; }
    }
    public partial class VehicleMileage
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [MaxLength(36)]
        public string VehicleRefListID { get; set; } // Relates To: Vehicle.ListID
        public virtual Vehicle VehicleRefVehicle { get; set; } // Navigation: VehicleMileage -> Vehicle
        [MaxLength(31)]
        public string VehicleRefFullName { get; set; } // Relates To: Vehicle.FullName
        [MaxLength(36)]
        public string CustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer CustomerRefCustomer { get; set; } // Navigation: VehicleMileage -> Customer
        [MaxLength(209)]
        public string CustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemRefItem { get; set; } // Navigation: VehicleMileage -> Item
        [MaxLength(31)]
        public string ItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(36)]
        public string ClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ClassRefClass { get; set; } // Navigation: VehicleMileage -> Class
        [MaxLength(159)]
        public string ClassRefFullName { get; set; } // Relates To: Class.FullName
        public DateTime TripStartDate { get; set; }
        public DateTime TripEndDate { get; set; }
        public decimal OdometerStart { get; set; }
        public decimal OdometerEnd { get; set; }
        public decimal TotalMiles { get; set; }
        [MaxLength(4095)]
        public string Notes { get; set; }
        [MaxLength(13)]
        public string BillableStatus { get; set; }
        public decimal StandardMileageRate { get; set; }
        public decimal StandardMileageTotalAmount { get; set; }
        public decimal BillableRate { get; set; }
        public decimal BillableAmount { get; set; }
    }
    public partial class Vendor
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(41)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsTaxAgency { get; set; }
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
        public string VendorAddressAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressAddr5 { get; set; }
        [MaxLength(31)]
        public string VendorAddressCity { get; set; }
        [MaxLength(21)]
        public string VendorAddressState { get; set; }
        [MaxLength(21)]
        public string VendorAddressProvince { get; set; }
        [MaxLength(21)]
        public string VendorAddressCounty { get; set; }
        [MaxLength(13)]
        public string VendorAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string VendorAddressCountry { get; set; }
        [MaxLength(41)]
        public string VendorAddressNote { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr1 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr2 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr3 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr4 { get; set; }
        [MaxLength(41)]
        public string VendorAddressBlockAddr5 { get; set; }
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
        [MaxLength(13)]
        public string ShipAddressPostalCode { get; set; }
        [MaxLength(31)]
        public string ShipAddressCountry { get; set; }
        [MaxLength(41)]
        public string ShipAddressNote { get; set; }
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
        [MaxLength(41)]
        public string NameOnCheck { get; set; }
        [MaxLength(99)]
        public string AccountNumber { get; set; }
        [MaxLength(4095)]
        public string Notes { get; set; }
        [MaxLength(36)]
        public string VendorTypeRefListID { get; set; } // Relates To: VendorType.ListID
        public virtual VendorType VendorTypeRefVendorType { get; set; } // Navigation: Vendor -> VendorType
        [MaxLength(159)]
        public string VendorTypeRefFullName { get; set; } // Relates To: VendorType.FullName
        [MaxLength(36)]
        public string TermsRefListID { get; set; } // Relates To: Terms.ListID
        public virtual Terms TermsRefTerms { get; set; } // Navigation: Vendor -> Terms
        [MaxLength(31)]
        public string TermsRefFullName { get; set; } // Relates To: Terms.Name
        public decimal CreditLimit { get; set; }
        [MaxLength(15)]
        public string VendorTaxIdent { get; set; }
        public bool IsVendorEligibleFor1099 { get; set; }
        public bool IsVendorEligibleForT4A { get; set; }
        public decimal OpenBalance { get; set; }
        public DateTime OpenBalanceDate { get; set; }
        public decimal Balance { get; set; }
        [MaxLength(36)]
        public string BillingRateRefListID { get; set; } // Relates To: BillingRate.ListID
        public virtual BillingRate BillingRateRefBillingRate { get; set; } // Navigation: Vendor -> BillingRate
        [MaxLength(31)]
        public string BillingRateRefFullName { get; set; } // Relates To: BillingRate.FullName
        [MaxLength(36)]
        public string PrefillAccount1RefListID { get; set; } // Relates To: PrefillAccount.ListID
        [MaxLength(31)]
        public string PrefillAccount1RefFullName { get; set; } // Relates To: PrefillAccount.FullName
        [MaxLength(36)]
        public string PrefillAccount2RefListID { get; set; } // Relates To: PrefillAccount.ListID
        [MaxLength(31)]
        public string PrefillAccount2RefFullName { get; set; } // Relates To: PrefillAccount.FullName
        [MaxLength(36)]
        public string PrefillAccount3RefListID { get; set; } // Relates To: PrefillAccount.ListID
        [MaxLength(31)]
        public string PrefillAccount3RefFullName { get; set; } // Relates To: PrefillAccount.FullName
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        [MaxLength(99)]
        public string BusinessNumber { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: Vendor -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(100)]
        public string SalesTaxCountry { get; set; }
        public bool IsSalesTaxAgency { get; set; }
        [MaxLength(36)]
        public string SalesTaxReturnRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxReturnRefSalesTaxCode { get; set; } // Navigation: Vendor -> SalesTaxCode
        [MaxLength(159)]
        public string SalesTaxReturnRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string CurrencyRefListID { get; set; } // Relates To: Currency.ListID
        public virtual Currency CurrencyRefCurrency { get; set; } // Navigation: Vendor -> Currency
        [MaxLength(64)]
        public string CurrencyRefFullName { get; set; } // Relates To: Currency.Name
    }
    public partial class VendorCredit
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36)]
        public string TxnID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: VendorCredit -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: VendorCredit -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: VendorCredit -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: VendorCredit -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate1 { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
    }
    public partial class VendorCreditExpenseLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: VendorCredit.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: VendorCreditExpenseLine -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: VendorCreditExpenseLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: VendorCreditExpenseLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: VendorCreditExpenseLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public int ExpenseLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ExpenseLineAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ExpenseLineAccountRefAccount { get; set; } // Navigation: VendorCreditExpenseLine -> Account
        [MaxLength(159)]
        public string ExpenseLineAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal ExpenseLineAmount { get; set; }
        public decimal ExpenseLineTaxAmount { get; set; }
        public decimal ExpenseLineTax1Amount { get; set; }
        [MaxLength(4095)]
        public string ExpenseLineMemo { get; set; }
        [MaxLength(36)]
        public string ExpenseLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ExpenseLineCustomerRefCustomer { get; set; } // Navigation: VendorCreditExpenseLine -> Customer
        [MaxLength(209)]
        public string ExpenseLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ExpenseLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ExpenseLineClassRefClass { get; set; } // Navigation: VendorCreditExpenseLine -> Class
        [MaxLength(159)]
        public string ExpenseLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ExpenseLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ExpenseLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: VendorCreditExpenseLine -> SalesTaxCode
        [MaxLength(36)]
        public string ExpenseLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ExpenseLineTaxCodeRefTaxCode { get; set; } // Navigation: VendorCreditExpenseLine -> TaxCode
        [MaxLength(3)]
        public string ExpenseLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(13)]
        public string ExpenseLineBillableStatus { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate1 { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class VendorCreditItemLine
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: VendorCredit.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: VendorCreditItemLine -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: VendorCreditItemLine -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: VendorCreditItemLine -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: VendorCreditItemLine -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        [MaxLength(11)]
        public string ItemLineType { get; set; }
        public int ItemLineSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemGroupTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemGroupLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemGroupLineItemRefItem { get; set; } // Navigation: VendorCreditItemLine -> Item
        [MaxLength(31)]
        public string ItemGroupLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemGroupLineDesc { get; set; }
        [MaxLength(31)]
        public string ItemGroupUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemGroupOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemGroupOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: VendorCreditItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemGroupOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemGroupLineQuantity { get; set; }
        public decimal ItemGroupLineTotalAmount { get; set; }
        public int ItemGroupSeqNo { get; set; }
        [MaxLength(36)]
        public string ItemLineTxnLineID { get; set; }
        [MaxLength(36)]
        public string ItemLineItemRefListID { get; set; } // Relates To: Item.ListID
        public virtual Item ItemLineItemRefItem { get; set; } // Navigation: VendorCreditItemLine -> Item
        [MaxLength(159)]
        public string ItemLineItemRefFullName { get; set; } // Relates To: Item.FullName
        [MaxLength(4095)]
        public string ItemLineDesc { get; set; }
        public decimal ItemLineQuantity { get; set; }
        [MaxLength(31)]
        public string ItemLineUnitOfMeasure { get; set; } // Relates To: UnitOfMeasureSet.Name
        [MaxLength(36)]
        public string ItemLineOverrideUOMSetRefListID { get; set; } // Relates To: UnitOfMeasureSet.ListID
        public virtual UnitOfMeasureSet ItemLineOverrideUOMSetRefUnitOfMeasureSet { get; set; } // Navigation: VendorCreditItemLine -> UnitOfMeasureSet
        [MaxLength(31)]
        public string ItemLineOverrideUOMSetRefFullName { get; set; } // Relates To: UnitOfMeasureSet.Name
        public decimal ItemLineCost { get; set; }
        public decimal ItemLineAmount { get; set; }
        public decimal ItemLineTaxAmount { get; set; }
        public decimal ItemLineTax1Amount { get; set; }
        [MaxLength(36)]
        public string ItemLineCustomerRefListID { get; set; } // Relates To: Customer.ListID
        public virtual Customer ItemLineCustomerRefCustomer { get; set; } // Navigation: VendorCreditItemLine -> Customer
        [MaxLength(209)]
        public string ItemLineCustomerRefFullName { get; set; } // Relates To: Customer.FullName
        [MaxLength(36)]
        public string ItemLineClassRefListID { get; set; } // Relates To: Class.ListID
        public virtual Class ItemLineClassRefClass { get; set; } // Navigation: VendorCreditItemLine -> Class
        [MaxLength(159)]
        public string ItemLineClassRefFullName { get; set; } // Relates To: Class.FullName
        [MaxLength(36)]
        public string ItemLineSalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode ItemLineSalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: VendorCreditItemLine -> SalesTaxCode
        [MaxLength(36)]
        public string ItemLineTaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode ItemLineTaxCodeRefTaxCode { get; set; } // Navigation: VendorCreditItemLine -> TaxCode
        [MaxLength(3)]
        public string ItemLineSalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(3)]
        public string ItemLineTaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(13)]
        public string ItemLineBillableStatus { get; set; }
        [MaxLength(36)]
        public string ItemLineOverrideItemAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account ItemLineOverrideItemAccountRefAccount { get; set; } // Navigation: VendorCreditItemLine -> Account
        [MaxLength(159)]
        public string ItemLineOverrideItemAccountRefFullName { get; set; } // Relates To: Account.FullName
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate1 { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
        public bool FQSaveToCache { get; set; }
        [Required, MaxLength(110)]
        public string FQPrimaryKey { get; set; }
        [MaxLength(110)]
        public string FQTxnLinkKey { get; set; }
    }
    public partial class VendorCreditLinkedTxn
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36)]
        public string TxnID { get; set; } // Relates To: VendorCredit.TxnID
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        public int TxnNumber { get; set; }
        [MaxLength(36)]
        public string VendorRefListID { get; set; } // Relates To: Vendor.ListID
        public virtual Vendor VendorRefVendor { get; set; } // Navigation: VendorCreditLinkedTxn -> Vendor
        [MaxLength(41)]
        public string VendorRefFullName { get; set; } // Relates To: Vendor.Name
        [MaxLength(36)]
        public string APAccountRefListID { get; set; } // Relates To: Account.ListID
        public virtual Account APAccountRefAccount { get; set; } // Navigation: VendorCreditLinkedTxn -> Account
        [MaxLength(159)]
        public string APAccountRefFullName { get; set; } // Relates To: Account.FullName
        public DateTime TxnDate { get; set; }
        [MaxLength(23)]
        public string TxnDateMacro { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        [MaxLength(20)]
        public string RefNumber { get; set; }
        [MaxLength(4095)]
        public string Memo { get; set; }
        public bool IsTaxIncluded { get; set; }
        [MaxLength(36)]
        public string SalesTaxCodeRefListID { get; set; } // Relates To: SalesTaxCode.ListID
        public virtual SalesTaxCode SalesTaxCodeRefSalesTaxCode { get; set; } // Navigation: VendorCreditLinkedTxn -> SalesTaxCode
        [MaxLength(3)]
        public string SalesTaxCodeRefFullName { get; set; } // Relates To: SalesTaxCode.Name
        [MaxLength(36)]
        public string TaxCodeRefListID { get; set; } // Relates To: TaxCode.ListID
        public virtual TaxCode TaxCodeRefTaxCode { get; set; } // Navigation: VendorCreditLinkedTxn -> TaxCode
        [MaxLength(3)]
        public string TaxCodeRefFullName { get; set; } // Relates To: TaxCode.Name
        [MaxLength(40)]
        public string ExternalGUID { get; set; }
        public int LinkedTxnSeqNo { get; set; }
        [MaxLength(36)]
        public string LinkedTxnTxnID { get; set; }
        [MaxLength(21)]
        public string LinkedTxnTxnType { get; set; }
        public DateTime LinkedTxnTxnDate { get; set; }
        [MaxLength(20)]
        public string LinkedTxnRefNumber { get; set; }
        [MaxLength(8)]
        public string LinkedTxnLinkType { get; set; }
        public decimal LinkedTxnAmount { get; set; }
        public decimal Tax1Total { get; set; }
        public decimal Tax2Total { get; set; }
        public decimal ExchangeRate1 { get; set; }
        public decimal OpenAmount { get; set; }
        public bool AmountIncludesVAT { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    public partial class VendorType
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        [MaxLength(159)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(36)]
        public string ParentRefListID { get; set; }
        [MaxLength(159)]
        public string ParentRefFullName { get; set; }
        public int Sublevel { get; set; }
    }
    public partial class WorkersCompCode
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [Required, MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(31)]
        public string Desc { get; set; }
        public decimal CurrentRate { get; set; }
        public DateTime CurrentEffectiveDate { get; set; }
        public decimal NextRate { get; set; }
        public DateTime NextEffectiveDate { get; set; }
    }
    public partial class WorkersCompCodeRateHistory
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [MaxLength(36), Key, Column(Order = 1)]
        public string ListID { get; set; } // Relates To: WorkersCompCode.ListID
        public virtual WorkersCompCode WorkersCompCode { get; set; } // Navigation: WorkersCompCodeRateHistory -> WorkersCompCode
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Required, MaxLength(16)]
        public string EditSequence { get; set; }
        [Required, MaxLength(31)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(31)]
        public string Desc { get; set; }
        public decimal CurrentRate { get; set; }
        public DateTime CurrentEffectiveDate { get; set; }
        public decimal NextRate { get; set; }
        public DateTime NextEffectiveDate { get; set; }
        public int RateHistorySeqNo { get; set; }
        public decimal RateHistoryRate { get; set; }
        public DateTime RateHistoryEffectiveDate { get; set; }
        [Required, MaxLength(73)]
        public string FQPrimaryKey { get; set; }
    }
    #endregion
}
