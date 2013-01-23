
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/17/2012 11:55:08
-- Generated from EDMX file: C:\Code2011\da2\AccountingBackupWeb.Models\QuickBooks\TextTemplate11.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QuickBooksData];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[AccountTaxLineInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountTaxLineInfo];
GO
IF OBJECT_ID(N'[dbo].[ARRefundCreditCard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ARRefundCreditCard];
GO
IF OBJECT_ID(N'[dbo].[ARRefundCreditCardRefundAppliedTo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ARRefundCreditCardRefundAppliedTo];
GO
IF OBJECT_ID(N'[dbo].[Bill]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bill];
GO
IF OBJECT_ID(N'[dbo].[BillExpenseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillExpenseLine];
GO
IF OBJECT_ID(N'[dbo].[BillItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillItemLine];
GO
IF OBJECT_ID(N'[dbo].[BillLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[BillingRate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillingRate];
GO
IF OBJECT_ID(N'[dbo].[BillingRateLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillingRateLine];
GO
IF OBJECT_ID(N'[dbo].[BillPaymentCheck]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillPaymentCheck];
GO
IF OBJECT_ID(N'[dbo].[BillPaymentCheckLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillPaymentCheckLine];
GO
IF OBJECT_ID(N'[dbo].[BillPaymentCreditCard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillPaymentCreditCard];
GO
IF OBJECT_ID(N'[dbo].[BillPaymentCreditCardLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillPaymentCreditCardLine];
GO
IF OBJECT_ID(N'[dbo].[BillToPay]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BillToPay];
GO
IF OBJECT_ID(N'[dbo].[BuildAssembly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BuildAssembly];
GO
IF OBJECT_ID(N'[dbo].[BuildAssemblyComponentItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BuildAssemblyComponentItemLine];
GO
IF OBJECT_ID(N'[dbo].[Charge]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Charge];
GO
IF OBJECT_ID(N'[dbo].[ChargeLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChargeLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[Check]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Check];
GO
IF OBJECT_ID(N'[dbo].[CheckApplyCheckToTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CheckApplyCheckToTxn];
GO
IF OBJECT_ID(N'[dbo].[CheckExpenseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CheckExpenseLine];
GO
IF OBJECT_ID(N'[dbo].[CheckItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CheckItemLine];
GO
IF OBJECT_ID(N'[dbo].[Class]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Class];
GO
IF OBJECT_ID(N'[dbo].[Company]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Company];
GO
IF OBJECT_ID(N'[dbo].[CompanyActivity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyActivity];
GO
IF OBJECT_ID(N'[dbo].[CreditCardCharge]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardCharge];
GO
IF OBJECT_ID(N'[dbo].[CreditCardChargeExpenseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardChargeExpenseLine];
GO
IF OBJECT_ID(N'[dbo].[CreditCardChargeItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardChargeItemLine];
GO
IF OBJECT_ID(N'[dbo].[CreditCardCredit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardCredit];
GO
IF OBJECT_ID(N'[dbo].[CreditCardCreditExpenseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardCreditExpenseLine];
GO
IF OBJECT_ID(N'[dbo].[CreditCardCreditItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditCardCreditItemLine];
GO
IF OBJECT_ID(N'[dbo].[CreditMemo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditMemo];
GO
IF OBJECT_ID(N'[dbo].[CreditMemoLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditMemoLine];
GO
IF OBJECT_ID(N'[dbo].[CreditMemoLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditMemoLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[Currency]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Currency];
GO
IF OBJECT_ID(N'[dbo].[Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer];
GO
IF OBJECT_ID(N'[dbo].[CustomerMsg]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerMsg];
GO
IF OBJECT_ID(N'[dbo].[CustomerType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerType];
GO
IF OBJECT_ID(N'[dbo].[CustomField]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomField];
GO
IF OBJECT_ID(N'[dbo].[DateDrivenTerms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DateDrivenTerms];
GO
IF OBJECT_ID(N'[dbo].[Deposit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Deposit];
GO
IF OBJECT_ID(N'[dbo].[DepositLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepositLine];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[EmployeeEarning]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeEarning];
GO
IF OBJECT_ID(N'[dbo].[Entity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entity];
GO
IF OBJECT_ID(N'[dbo].[Estimate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Estimate];
GO
IF OBJECT_ID(N'[dbo].[EstimateLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EstimateLine];
GO
IF OBJECT_ID(N'[dbo].[EstimateLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EstimateLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[Host]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Host];
GO
IF OBJECT_ID(N'[dbo].[HostMetaData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HostMetaData];
GO
IF OBJECT_ID(N'[dbo].[HostSupportedVersions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HostSupportedVersions];
GO
IF OBJECT_ID(N'[dbo].[InventoryAdjustment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryAdjustment];
GO
IF OBJECT_ID(N'[dbo].[InventoryAdjustmentLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryAdjustmentLine];
GO
IF OBJECT_ID(N'[dbo].[Invoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoice];
GO
IF OBJECT_ID(N'[dbo].[InvoiceLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceLine];
GO
IF OBJECT_ID(N'[dbo].[InvoiceLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[Item]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item];
GO
IF OBJECT_ID(N'[dbo].[ItemAssembliesCanBuild]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemAssembliesCanBuild];
GO
IF OBJECT_ID(N'[dbo].[ItemDiscount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemDiscount];
GO
IF OBJECT_ID(N'[dbo].[ItemFixedAsset]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemFixedAsset];
GO
IF OBJECT_ID(N'[dbo].[ItemGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemGroup];
GO
IF OBJECT_ID(N'[dbo].[ItemGroupLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemGroupLine];
GO
IF OBJECT_ID(N'[dbo].[ItemInventory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemInventory];
GO
IF OBJECT_ID(N'[dbo].[ItemInventoryAssembly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemInventoryAssembly];
GO
IF OBJECT_ID(N'[dbo].[ItemInventoryAssemblyLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemInventoryAssemblyLine];
GO
IF OBJECT_ID(N'[dbo].[ItemNonInventory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemNonInventory];
GO
IF OBJECT_ID(N'[dbo].[ItemOtherCharge]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemOtherCharge];
GO
IF OBJECT_ID(N'[dbo].[ItemPayment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemPayment];
GO
IF OBJECT_ID(N'[dbo].[ItemReceipt]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemReceipt];
GO
IF OBJECT_ID(N'[dbo].[ItemReceiptExpenseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemReceiptExpenseLine];
GO
IF OBJECT_ID(N'[dbo].[ItemReceiptItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemReceiptItemLine];
GO
IF OBJECT_ID(N'[dbo].[ItemReceiptLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemReceiptLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[ItemSalesTax]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemSalesTax];
GO
IF OBJECT_ID(N'[dbo].[ItemSalesTaxGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemSalesTaxGroup];
GO
IF OBJECT_ID(N'[dbo].[ItemSalesTaxGroupLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemSalesTaxGroupLine];
GO
IF OBJECT_ID(N'[dbo].[ItemService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemService];
GO
IF OBJECT_ID(N'[dbo].[ItemSubtotal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemSubtotal];
GO
IF OBJECT_ID(N'[dbo].[JobType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobType];
GO
IF OBJECT_ID(N'[dbo].[JournalEntry]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalEntry];
GO
IF OBJECT_ID(N'[dbo].[JournalEntryCreditLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalEntryCreditLine];
GO
IF OBJECT_ID(N'[dbo].[JournalEntryDebitLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalEntryDebitLine];
GO
IF OBJECT_ID(N'[dbo].[JournalEntryLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalEntryLine];
GO
IF OBJECT_ID(N'[dbo].[ListDeleted]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ListDeleted];
GO
IF OBJECT_ID(N'[dbo].[OtherName]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OtherName];
GO
IF OBJECT_ID(N'[dbo].[PaymentMethod]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentMethod];
GO
IF OBJECT_ID(N'[dbo].[PayrollItemNonWage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayrollItemNonWage];
GO
IF OBJECT_ID(N'[dbo].[PayrollItemWage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayrollItemWage];
GO
IF OBJECT_ID(N'[dbo].[Preferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Preferences];
GO
IF OBJECT_ID(N'[dbo].[PriceLevel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PriceLevel];
GO
IF OBJECT_ID(N'[dbo].[PriceLevelPerItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PriceLevelPerItem];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrder];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderLine];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[ReceivePayment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceivePayment];
GO
IF OBJECT_ID(N'[dbo].[ReceivePaymentLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceivePaymentLine];
GO
IF OBJECT_ID(N'[dbo].[ReceivePaymentToDeposit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceivePaymentToDeposit];
GO
IF OBJECT_ID(N'[dbo].[Sales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sales];
GO
IF OBJECT_ID(N'[dbo].[SalesLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLine];
GO
IF OBJECT_ID(N'[dbo].[SalesOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesOrder];
GO
IF OBJECT_ID(N'[dbo].[SalesOrderLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesOrderLine];
GO
IF OBJECT_ID(N'[dbo].[SalesOrderLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesOrderLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[SalesReceipt]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceipt];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptLine];
GO
IF OBJECT_ID(N'[dbo].[SalesRep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesRep];
GO
IF OBJECT_ID(N'[dbo].[SalesTaxCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesTaxCode];
GO
IF OBJECT_ID(N'[dbo].[SalesTaxPaymentCheck]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesTaxPaymentCheck];
GO
IF OBJECT_ID(N'[dbo].[SalesTaxPaymentCheckLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesTaxPaymentCheckLine];
GO
IF OBJECT_ID(N'[dbo].[ShipMethod]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShipMethod];
GO
IF OBJECT_ID(N'[dbo].[SpecialAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialAccount];
GO
IF OBJECT_ID(N'[dbo].[SpecialItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpecialItem];
GO
IF OBJECT_ID(N'[dbo].[StandardTerms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StandardTerms];
GO
IF OBJECT_ID(N'[dbo].[TaxCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaxCode];
GO
IF OBJECT_ID(N'[dbo].[Template]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Template];
GO
IF OBJECT_ID(N'[dbo].[Terms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms];
GO
IF OBJECT_ID(N'[dbo].[TimeTracking]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TimeTracking];
GO
IF OBJECT_ID(N'[dbo].[ToDo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ToDo];
GO
IF OBJECT_ID(N'[dbo].[Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transaction];
GO
IF OBJECT_ID(N'[dbo].[TxnDeleted]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TxnDeleted];
GO
IF OBJECT_ID(N'[dbo].[UnitOfMeasureSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitOfMeasureSet];
GO
IF OBJECT_ID(N'[dbo].[UnitOfMeasureSetRelatedUnit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitOfMeasureSetRelatedUnit];
GO
IF OBJECT_ID(N'[dbo].[UnitOfMeasureSetDefaultUnit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitOfMeasureSetDefaultUnit];
GO
IF OBJECT_ID(N'[dbo].[Vehicle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vehicle];
GO
IF OBJECT_ID(N'[dbo].[VehicleMileage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleMileage];
GO
IF OBJECT_ID(N'[dbo].[Vendor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vendor];
GO
IF OBJECT_ID(N'[dbo].[VendorCredit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VendorCredit];
GO
IF OBJECT_ID(N'[dbo].[VendorCreditExpenseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VendorCreditExpenseLine];
GO
IF OBJECT_ID(N'[dbo].[VendorCreditItemLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VendorCreditItemLine];
GO
IF OBJECT_ID(N'[dbo].[VendorCreditLinkedTxn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VendorCreditLinkedTxn];
GO
IF OBJECT_ID(N'[dbo].[VendorType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VendorType];
GO
IF OBJECT_ID(N'[dbo].[WorkersCompCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkersCompCode];
GO
IF OBJECT_ID(N'[dbo].[WorkersCompCodeRateHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkersCompCodeRateHistory];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Account'
CREATE TABLE [dbo].[Account] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [AccountType] nvarchar(max)  NULL,
    [SpecialAccountType] nvarchar(max)  NULL,
    [IsTaxAccount] bit  NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [BankNumber] nvarchar(max)  NULL,
    [Desc] nvarchar(max)  NULL,
    [Balance] decimal(18,0)  NULL,
    [TotalBalance] decimal(18,0)  NULL,
    [OpenBalance] decimal(18,0)  NULL,
    [OpenBalanceDate] datetime  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [TaxLineInfoRetTaxLineID] int  NULL,
    [TaxLineInfoRetTaxLineName] nvarchar(max)  NULL,
    [CashFlowClassification] nvarchar(max)  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'AccountTaxLineInfo'
CREATE TABLE [dbo].[AccountTaxLineInfo] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TaxLineID] int  NULL,
    [TaxLineName] nvarchar(max)  NULL
);
GO

-- Creating table 'ARRefundCreditCard'
CREATE TABLE [dbo].[ARRefundCreditCard] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [RefundFromAccountRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL
);
GO

-- Creating table 'ARRefundCreditCardRefundAppliedTo'
CREATE TABLE [dbo].[ARRefundCreditCardRefundAppliedTo] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [RefundFromAccountRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [RefundAppliedToTxnTxnID] nvarchar(max)  NULL,
    [RefundAppliedToTxnTxnType] nvarchar(max)  NULL,
    [RefundAppliedToTxnTxnDate] datetime  NULL,
    [RefundAppliedToTxnRefNumber] nvarchar(max)  NULL,
    [RefundAppliedToTxnRefCreditRemaining] decimal(18,0)  NULL,
    [RefundAppliedToTxnRefRefundAmount] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Bill'
CREATE TABLE [dbo].[Bill] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [AmountDue] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'BillExpenseLine'
CREATE TABLE [dbo].[BillExpenseLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [AmountDue] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [ExpenseLineClearExpenseLines] bit  NULL,
    [ExpenseLineSeqNo] int  NULL,
    [ExpenseLineTxnLineID] nvarchar(max)  NULL,
    [ExpenseLineAccountRefListID] nvarchar(max)  NULL,
    [ExpenseLineAmount] decimal(18,0)  NULL,
    [ExpenseLineTaxAmount] decimal(18,0)  NULL,
    [ExpenseLineTax1Amount] decimal(18,0)  NULL,
    [ExpenseLineMemo] nvarchar(max)  NULL,
    [ExpenseLineCustomerRefListID] nvarchar(max)  NULL,
    [ExpenseLineClassRefListID] nvarchar(max)  NULL,
    [ExpenseLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineBillableStatus] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BillItemLine'
CREATE TABLE [dbo].[BillItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [AmountDue] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [ItemLineType] nvarchar(max)  NULL,
    [ItemLineSeqNo] int  NULL,
    [ItemGroupLineTxnLineID] nvarchar(max)  NULL,
    [ItemGroupLineItemGroupRefListID] nvarchar(max)  NULL,
    [ItemGroupLineDesc] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemGroupUnitOfMeasure] nvarchar(max)  NULL,
    [ItemGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemGroupLineTotalAmount] decimal(18,0)  NULL,
    [ItemGroupSeqNo] int  NULL,
    [ItemLineTxnLineID] nvarchar(max)  NULL,
    [ItemLineItemRefListID] nvarchar(max)  NULL,
    [ItemLineDesc] nvarchar(max)  NULL,
    [ItemLineQuantity] decimal(18,0)  NULL,
    [ItemLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemLineCost] decimal(18,0)  NULL,
    [ItemLineAmount] decimal(18,0)  NULL,
    [ItemLineTaxAmount] decimal(18,0)  NULL,
    [ItemLineTax1Amount] decimal(18,0)  NULL,
    [ItemLineCustomerRefListID] nvarchar(max)  NULL,
    [ItemLineClassRefListID] nvarchar(max)  NULL,
    [ItemLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineBillableStatus] nvarchar(max)  NULL,
    [ItemLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [ItemLineLinkToTxnTxnID] nvarchar(max)  NULL,
    [ItemLineLinkToTxnTxnLineID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BillLinkedTxn'
CREATE TABLE [dbo].[BillLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [AmountDue] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BillingRate'
CREATE TABLE [dbo].[BillingRate] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [BillingRateType] nvarchar(max)  NULL,
    [FixedBillingRate] decimal(18,0)  NULL
);
GO

-- Creating table 'BillingRateLine'
CREATE TABLE [dbo].[BillingRateLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [BillingRateType] nvarchar(max)  NULL,
    [FixedBillingRate] decimal(18,0)  NULL,
    [BillingRateLineItemRefListID] nvarchar(max)  NULL,
    [BillingRateLineCustomRate] decimal(18,0)  NULL,
    [BillingRateLineCustomRatePercent] decimal(18,0)  NULL,
    [BillingRateLineAdjustPercentage] decimal(18,0)  NULL,
    [BillingRateLineAdjustBillingRateRelativeTo] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BillPaymentCheck'
CREATE TABLE [dbo].[BillPaymentCheck] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [BankAccountRefListID] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [Memo] nvarchar(max)  NULL
);
GO

-- Creating table 'BillPaymentCheckLine'
CREATE TABLE [dbo].[BillPaymentCheckLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [BankAccountRefListID] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [AppliedToTxnSeqNo] int  NULL,
    [AppliedToTxnTxnID] nvarchar(max)  NULL,
    [AppliedToTxnPaymentAmount] decimal(18,0)  NULL,
    [AppliedToTxnTxnType] nvarchar(max)  NULL,
    [AppliedToTxnTxnDate] datetime  NULL,
    [AppliedToTxnRefNumber] nvarchar(max)  NULL,
    [AppliedToTxnBalanceRemaining] decimal(18,0)  NULL,
    [AppliedToTxnAmount] decimal(18,0)  NULL,
    [AppliedToTxnSetCreditCreditTxnID] nvarchar(max)  NULL,
    [AppliedToTxnSetCreditAppliedAmount] decimal(18,0)  NULL,
    [AppliedToTxnDiscountAmount] decimal(18,0)  NULL,
    [AppliedToTxnDiscountAccountRefListID] nvarchar(max)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BillPaymentCreditCard'
CREATE TABLE [dbo].[BillPaymentCreditCard] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL
);
GO

-- Creating table 'BillPaymentCreditCardLine'
CREATE TABLE [dbo].[BillPaymentCreditCardLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [AppliedToTxnSeqNo] int  NULL,
    [AppliedToTxnTxnID] nvarchar(max)  NULL,
    [AppliedToTxnPaymentAmount] decimal(18,0)  NULL,
    [AppliedToTxnTxnType] nvarchar(max)  NULL,
    [AppliedToTxnTxnDate] datetime  NULL,
    [AppliedToTxnRefNumber] nvarchar(max)  NULL,
    [AppliedToTxnBalanceRemaining] decimal(18,0)  NULL,
    [AppliedToTxnAmount] decimal(18,0)  NULL,
    [AppliedToTxnSetCreditCreditTxnID] nvarchar(max)  NULL,
    [AppliedToTxnSetCreditAppliedAmount] decimal(18,0)  NULL,
    [AppliedToTxnDiscountAmount] decimal(18,0)  NULL,
    [AppliedToTxnDiscountAccountRefListID] nvarchar(max)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BillToPay'
CREATE TABLE [dbo].[BillToPay] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [DueDateCutoff] datetime  NULL,
    [BillToPayTxnID] nvarchar(max)  NULL,
    [BillToPayTxnType] nvarchar(max)  NULL,
    [BillToPayTxnNumber] int  NULL,
    [BillToPayAPAccountRefListID] nvarchar(max)  NULL,
    [BillToPayTxnDate] datetime  NULL,
    [BillToPayRefNumber] nvarchar(max)  NULL,
    [BillToPayDueDate] datetime  NULL,
    [BillToPayAmountDue] decimal(18,0)  NULL,
    [CreditToApplyTxnID] nvarchar(max)  NULL,
    [CreditToApplyTxnType] nvarchar(max)  NULL,
    [CreditToApplyTxnNumber] int  NULL,
    [CreditToApplyAPAccountRefListID] nvarchar(max)  NULL,
    [CreditToApplyTxnDate] datetime  NULL,
    [CreditToApplyRefNumber] nvarchar(max)  NULL,
    [CreditToApplyCreditRemaining] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'BuildAssembly'
CREATE TABLE [dbo].[BuildAssembly] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [ItemInventoryAssemblyRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [QuantityToBuild] decimal(18,0)  NULL,
    [QuantityCanBuild] decimal(18,0)  NULL,
    [QuantityOnHand] decimal(18,0)  NULL,
    [QuantityOnSalesOrder] decimal(18,0)  NULL,
    [MarkPendingIfRequired] bit  NULL,
    [RemovePending] bit  NULL
);
GO

-- Creating table 'BuildAssemblyComponentItemLine'
CREATE TABLE [dbo].[BuildAssemblyComponentItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [ItemInventoryAssemblyRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [QuantityToBuild] decimal(18,0)  NULL,
    [QuantityCanBuild] decimal(18,0)  NULL,
    [QuantityOnHand] decimal(18,0)  NULL,
    [QuantityOnSalesOrder] decimal(18,0)  NULL,
    [MarkPendingIfRequired] bit  NULL,
    [RemovePending] bit  NULL,
    [ComponentItemLineSeqNo] int  NULL,
    [ComponentItemLineItemRefListID] nvarchar(max)  NULL,
    [ComponentItemLineDesc] nvarchar(max)  NULL,
    [ComponentItemLineQuantityOnHand] decimal(18,0)  NULL,
    [ComponentItemLineQuantityNeeded] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Charge'
CREATE TABLE [dbo].[Charge] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [ItemRefListID] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [UnitOfMeasure] nvarchar(max)  NULL,
    [OverrideUOMSetRefListID] nvarchar(max)  NULL,
    [Rate] decimal(18,0)  NULL,
    [Amount] decimal(18,0)  NULL,
    [BalanceRemaining] decimal(18,0)  NULL,
    [Desc] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [BilledDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [OverrideItemAccountRefListID] nvarchar(max)  NULL,
    [IsPaid] bit  NULL
);
GO

-- Creating table 'ChargeLinkedTxn'
CREATE TABLE [dbo].[ChargeLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [ItemRefListID] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [UnitOfMeasure] nvarchar(max)  NULL,
    [OverrideUOMSetRefListID] nvarchar(max)  NULL,
    [Rate] decimal(18,0)  NULL,
    [Amount] decimal(18,0)  NULL,
    [BalanceRemaining] decimal(18,0)  NULL,
    [Desc] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [BilledDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [OverrideItemAccountRefListID] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Check'
CREATE TABLE [dbo].[Check] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'CheckApplyCheckToTxn'
CREATE TABLE [dbo].[CheckApplyCheckToTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [ApplyCheckToTxnSeqNo] int  NULL,
    [ApplyCheckToTxnTxnID] nvarchar(max)  NULL,
    [ApplyCheckToTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'CheckExpenseLine'
CREATE TABLE [dbo].[CheckExpenseLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineClearExpenseLines] bit  NULL,
    [ExpenseLineSeqNo] int  NULL,
    [ExpenseLineTxnLineID] nvarchar(max)  NULL,
    [ExpenseLineAccountRefListID] nvarchar(max)  NULL,
    [ExpenseLineAmount] decimal(18,0)  NULL,
    [ExpenseLineTaxAmount] decimal(18,0)  NULL,
    [ExpenseLineTax1Amount] decimal(18,0)  NULL,
    [ExpenseLineMemo] nvarchar(max)  NULL,
    [ExpenseLineCustomerRefListID] nvarchar(max)  NULL,
    [ExpenseLineClassRefListID] nvarchar(max)  NULL,
    [ExpenseLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineBillableStatus] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'CheckItemLine'
CREATE TABLE [dbo].[CheckItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineClearItemLines] bit  NULL,
    [ItemLineType] nvarchar(max)  NULL,
    [ItemLineSeqNo] int  NULL,
    [ItemGroupLineTxnLineID] nvarchar(max)  NULL,
    [ItemGroupLineItemRefListID] nvarchar(max)  NULL,
    [ItemGroupLineDesc] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemGroupLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemGroupLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemGroupLineTotalAmount] decimal(18,0)  NULL,
    [ItemGroupSeqNo] int  NULL,
    [ItemLineTxnLineID] nvarchar(max)  NULL,
    [ItemLineItemRefListID] nvarchar(max)  NULL,
    [ItemLineDesc] nvarchar(max)  NULL,
    [ItemLineQuantity] decimal(18,0)  NULL,
    [ItemLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemLineCost] decimal(18,0)  NULL,
    [ItemLineAmount] decimal(18,0)  NULL,
    [ItemLineTaxAmount] decimal(18,0)  NULL,
    [ItemLineTax1Amount] decimal(18,0)  NULL,
    [ItemLineCustomerRefListID] nvarchar(max)  NULL,
    [ItemLineClassRefListID] nvarchar(max)  NULL,
    [ItemLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineBillableStatus] nvarchar(max)  NULL,
    [ItemLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Class'
CREATE TABLE [dbo].[Class] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL
);
GO

-- Creating table 'Company'
CREATE TABLE [dbo].[Company] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [Id] int  NULL,
    [IsSampleCompany] bit  NULL,
    [CompanyName] nvarchar(max)  NULL,
    [LegalCompanyName] nvarchar(max)  NULL,
    [LegalAddressAddr1] nvarchar(max)  NULL,
    [LegalAddressAddr2] nvarchar(max)  NULL,
    [LegalAddressAddr3] nvarchar(max)  NULL,
    [LegalAddressAddr4] nvarchar(max)  NULL,
    [LegalAddressAddr5] nvarchar(max)  NULL,
    [LegalAddressCity] nvarchar(max)  NULL,
    [LegalAddressState] nvarchar(max)  NULL,
    [LegalAddressProvince] nvarchar(max)  NULL,
    [LegalAddressCounty] nvarchar(max)  NULL,
    [LegalAddressPostalCode] nvarchar(max)  NULL,
    [LegalAddressCountry] nvarchar(max)  NULL,
    [LegalAddressNote] nvarchar(max)  NULL,
    [CompanyAddressForCustomerAddr1] nvarchar(max)  NULL,
    [CompanyAddressForCustomerAddr2] nvarchar(max)  NULL,
    [CompanyAddressForCustomerAddr3] nvarchar(max)  NULL,
    [CompanyAddressForCustomerAddr4] nvarchar(max)  NULL,
    [CompanyAddressForCustomerAddr5] nvarchar(max)  NULL,
    [CompanyAddressForCustomerCity] nvarchar(max)  NULL,
    [CompanyAddressForCustomerState] nvarchar(max)  NULL,
    [CompanyAddressForCustomerProvince] nvarchar(max)  NULL,
    [CompanyAddressForCustomerCounty] nvarchar(max)  NULL,
    [CompanyAddressForCustomerPostalCode] nvarchar(max)  NULL,
    [CompanyAddressForCustomerCountry] nvarchar(max)  NULL,
    [CompanyAddressForCustomerNote] nvarchar(max)  NULL,
    [CompanyAddressBlockForCustomerAddr1] nvarchar(max)  NULL,
    [CompanyAddressBlockForCustomerAddr2] nvarchar(max)  NULL,
    [CompanyAddressBlockForCustomerAddr3] nvarchar(max)  NULL,
    [CompanyAddressBlockForCustomerAddr4] nvarchar(max)  NULL,
    [CompanyAddressBlockForCustomerAddr5] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [CompanyWebSite] nvarchar(max)  NULL,
    [FirstMonthFiscalYear] nvarchar(max)  NULL,
    [FirstMonthIncomeTaxYear] nvarchar(max)  NULL,
    [CompanyType] nvarchar(max)  NULL,
    [EIN] nvarchar(max)  NULL,
    [SSN] nvarchar(max)  NULL,
    [TaxForm] nvarchar(max)  NULL,
    [BusinessNumber] nvarchar(max)  NULL,
    [SubscribedServicesServiceName] nvarchar(max)  NULL,
    [SubscribedServicesServiceDomain] nvarchar(max)  NULL,
    [SubscribedServicesServiceServiceStatus] nvarchar(max)  NULL
);
GO

-- Creating table 'CompanyActivity'
CREATE TABLE [dbo].[CompanyActivity] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [Id] int  NULL,
    [LastRestoreTime] datetime  NULL,
    [LastCondenseTime] datetime  NULL
);
GO

-- Creating table 'CreditCardCharge'
CREATE TABLE [dbo].[CreditCardCharge] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'CreditCardChargeExpenseLine'
CREATE TABLE [dbo].[CreditCardChargeExpenseLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineClearExpenseLines] bit  NULL,
    [ExpenseLineSeqNo] int  NULL,
    [ExpenseLineTxnLineID] nvarchar(max)  NULL,
    [ExpenseLineAccountRefListID] nvarchar(max)  NULL,
    [ExpenseLineAmount] decimal(18,0)  NULL,
    [ExpenseLineTaxAmount] decimal(18,0)  NULL,
    [ExpenseLineTax1Amount] decimal(18,0)  NULL,
    [ExpenseLineMemo] nvarchar(max)  NULL,
    [ExpenseLineCustomerRefListID] nvarchar(max)  NULL,
    [ExpenseLineClassRefListID] nvarchar(max)  NULL,
    [ExpenseLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineBillableStatus] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'CreditCardChargeItemLine'
CREATE TABLE [dbo].[CreditCardChargeItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineClearItemLines] bit  NULL,
    [ItemLineType] nvarchar(max)  NULL,
    [ItemLineSeqNo] int  NULL,
    [ItemGroupLineTxnLineID] nvarchar(max)  NULL,
    [ItemGroupLineItemRefListID] nvarchar(max)  NULL,
    [ItemGroupLineDesc] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemGroupLineTotalAmount] decimal(18,0)  NULL,
    [ItemGroupSeqNo] int  NULL,
    [ItemLineTxnLineID] nvarchar(max)  NULL,
    [ItemLineItemRefListID] nvarchar(max)  NULL,
    [ItemLineDesc] nvarchar(max)  NULL,
    [ItemLineQuantity] decimal(18,0)  NULL,
    [ItemLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemLineCost] decimal(18,0)  NULL,
    [ItemLineAmount] decimal(18,0)  NULL,
    [ItemLineTaxAmount] decimal(18,0)  NULL,
    [ItemLineTax1Amount] decimal(18,0)  NULL,
    [ItemLineCustomerRefListID] nvarchar(max)  NULL,
    [ItemLineClassRefListID] nvarchar(max)  NULL,
    [ItemLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineBillableStatus] nvarchar(max)  NULL,
    [ItemLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'CreditCardCredit'
CREATE TABLE [dbo].[CreditCardCredit] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'CreditCardCreditExpenseLine'
CREATE TABLE [dbo].[CreditCardCreditExpenseLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [ExpenseLineClearExpenseLines] bit  NULL,
    [ExpenseLineSeqNo] int  NULL,
    [ExpenseLineTxnLineID] nvarchar(max)  NULL,
    [ExpenseLineAccountRefListID] nvarchar(max)  NULL,
    [ExpenseLineAmount] decimal(18,0)  NULL,
    [ExpenseLineTaxAmount] decimal(18,0)  NULL,
    [ExpenseLineTax1Amount] decimal(18,0)  NULL,
    [ExpenseLineMemo] nvarchar(max)  NULL,
    [ExpenseLineCustomerRefListID] nvarchar(max)  NULL,
    [ExpenseLineClassRefListID] nvarchar(max)  NULL,
    [ExpenseLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineBillableStatus] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'CreditCardCreditItemLine'
CREATE TABLE [dbo].[CreditCardCreditItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [ItemLineClearItemLines] bit  NULL,
    [ItemLineType] nvarchar(max)  NULL,
    [ItemLineSeqNo] int  NULL,
    [ItemGroupLineTxnLineID] nvarchar(max)  NULL,
    [ItemGroupLineItemRefListID] nvarchar(max)  NULL,
    [ItemGroupLineDesc] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemGroupLineTotalAmount] decimal(18,0)  NULL,
    [ItemGroupSeqNo] int  NULL,
    [ItemLineTxnLineID] nvarchar(max)  NULL,
    [ItemLineItemRefListID] nvarchar(max)  NULL,
    [ItemLineDesc] nvarchar(max)  NULL,
    [ItemLineQuantity] decimal(18,0)  NULL,
    [ItemLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemLineCost] decimal(18,0)  NULL,
    [ItemLineAmount] decimal(18,0)  NULL,
    [ItemLineTaxAmount] decimal(18,0)  NULL,
    [ItemLineTax1Amount] decimal(18,0)  NULL,
    [ItemLineCustomerRefListID] nvarchar(max)  NULL,
    [ItemLineClassRefListID] nvarchar(max)  NULL,
    [ItemLineBillableStatus] nvarchar(max)  NULL,
    [ItemLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'CreditMemo'
CREATE TABLE [dbo].[CreditMemo] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [CreditRemaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'CreditMemoLine'
CREATE TABLE [dbo].[CreditMemoLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [CreditRemaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [CreditMemoLineType] nvarchar(max)  NULL,
    [CreditMemoLineSeqNo] int  NULL,
    [CreditMemoLineGroupLineTxnLineID] nvarchar(max)  NULL,
    [CreditMemoLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [CreditMemoLineGroupDesc] nvarchar(max)  NULL,
    [CreditMemoLineGroupQuantity] decimal(18,0)  NULL,
    [CreditMemoLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [CreditMemoLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [CreditMemoLineGroupIsPrintItemsInGroup] bit  NULL,
    [CreditMemoLineGroupTotalAmount] decimal(18,0)  NULL,
    [CreditMemoLineGroupServiceDate] datetime  NULL,
    [CreditMemoLineGroupSeqNo] int  NULL,
    [CreditMemoLineTxnLineID] nvarchar(max)  NULL,
    [CreditMemoLineItemRefListID] nvarchar(max)  NULL,
    [CreditMemoLineDesc] nvarchar(max)  NULL,
    [CreditMemoLineQuantity] decimal(18,0)  NULL,
    [CreditMemoLineUnitOfMeasure] nvarchar(max)  NULL,
    [CreditMemoLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [CreditMemoLineRate] decimal(18,0)  NULL,
    [CreditMemoLineRatePercent] decimal(18,0)  NULL,
    [CreditMemoLinePriceLevelRefListID] nvarchar(max)  NULL,
    [CreditMemoLineClassRefListID] nvarchar(max)  NULL,
    [CreditMemoLineAmount] decimal(18,0)  NULL,
    [CreditMemoLineTaxAmount] decimal(18,0)  NULL,
    [CreditMemoLineServiceDate] datetime  NULL,
    [CreditMemoLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CreditMemoLineTaxCodeRefListID] nvarchar(max)  NULL,
    [CreditMemoLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldCreditMemoLineOther1] nvarchar(max)  NULL,
    [CustomFieldCreditMemoLineOther2] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [CustomFieldCreditMemoLineGroupOther1] nvarchar(max)  NULL,
    [CustomFieldCreditMemoLineGroupOther2] nvarchar(max)  NULL
);
GO

-- Creating table 'CreditMemoLinkedTxn'
CREATE TABLE [dbo].[CreditMemoLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [CreditRemaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Currency'
CREATE TABLE [dbo].[Currency] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [CurrencyCode] nvarchar(max)  NULL,
    [CurrencyFormatThousandSeparator] nvarchar(max)  NULL,
    [CurrencyFormatThousandSeparatorGrouping] nvarchar(max)  NULL,
    [CurrencyFormatDecimalPlaces] nvarchar(max)  NULL,
    [CurrencyFormatDecimalSeparator] nvarchar(max)  NULL,
    [IsUserDefinedCurrency] bit  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AsOfDate] datetime  NULL
);
GO

-- Creating table 'Customer'
CREATE TABLE [dbo].[Customer] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [CompanyName] nvarchar(max)  NULL,
    [Salutation] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [AltPhone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [AltContact] nvarchar(max)  NULL,
    [CustomerTypeRefListID] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [Balance] decimal(18,0)  NULL,
    [TotalBalance] decimal(18,0)  NULL,
    [OpenBalance] decimal(18,0)  NULL,
    [OpenBalanceDate] datetime  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxCountry] nvarchar(max)  NULL,
    [ResaleNumber] nvarchar(max)  NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [BusinessNumber] nvarchar(max)  NULL,
    [CreditLimit] decimal(18,0)  NULL,
    [PreferredPaymentMethodRefListID] nvarchar(max)  NULL,
    [JobStatus] nvarchar(max)  NULL,
    [JobStartDate] datetime  NULL,
    [JobProjectedEndDate] datetime  NULL,
    [JobEndDate] datetime  NULL,
    [JobDesc] nvarchar(max)  NULL,
    [JobTypeRefListID] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL,
    [IsUsingCustomerTaxCode] bit  NULL,
    [PriceLevelRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'CustomerMsg'
CREATE TABLE [dbo].[CustomerMsg] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'CustomerType'
CREATE TABLE [dbo].[CustomerType] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL
);
GO

-- Creating table 'CustomField'
CREATE TABLE [dbo].[CustomField] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [OwnerID] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [EntityType] nvarchar(max)  NULL,
    [EntityRefListID] nvarchar(max)  NULL,
    [TxnType] nvarchar(max)  NULL,
    [TxnID] nvarchar(max)  NULL,
    [TxnLineID] nvarchar(max)  NULL,
    [OtherType] nvarchar(max)  NULL,
    [Value] nvarchar(max)  NULL
);
GO

-- Creating table 'DateDrivenTerms'
CREATE TABLE [dbo].[DateDrivenTerms] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [DayOfMonthDue] int  NULL,
    [DueNextMonthDays] int  NULL,
    [DiscountDayOfMonth] int  NULL,
    [DiscountPct] decimal(18,0)  NULL
);
GO

-- Creating table 'Deposit'
CREATE TABLE [dbo].[Deposit] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [DepositTotal] decimal(18,0)  NULL,
    [CashBackInfoTxnLineID] nvarchar(max)  NULL,
    [CashBackInfoAccountRefListID] nvarchar(max)  NULL,
    [CashBackInfoMemo] nvarchar(max)  NULL,
    [CashBackInfoAmount] decimal(18,0)  NULL
);
GO

-- Creating table 'DepositLine'
CREATE TABLE [dbo].[DepositLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [DepositTotal] decimal(18,0)  NULL,
    [CashBackInfoTxnLineID] nvarchar(max)  NULL,
    [CashBackInfoAccountRefListID] nvarchar(max)  NULL,
    [CashBackInfoMemo] nvarchar(max)  NULL,
    [CashBackInfoAmount] decimal(18,0)  NULL,
    [DepositLineSeqNo] int  NULL,
    [DepositLineTxnType] nvarchar(max)  NULL,
    [DepositLineTxnID] nvarchar(max)  NULL,
    [DepositLineTxnLineID] nvarchar(max)  NULL,
    [DepositLinePaymentTxnID] nvarchar(max)  NULL,
    [DepositLinePaymentTxnLineID] nvarchar(max)  NULL,
    [DepositLineEntityRefListID] nvarchar(max)  NULL,
    [DepositLineAccountRefListID] nvarchar(max)  NULL,
    [DepositLineMemo] nvarchar(max)  NULL,
    [DepositLineCheckNumber] nvarchar(max)  NULL,
    [DepositLinePaymentMethodRefListID] nvarchar(max)  NULL,
    [DepositLineClassRefListID] nvarchar(max)  NULL,
    [DepositLineAmount] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Employee'
CREATE TABLE [dbo].[Employee] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [Salutation] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [EmployeeAddressAddr1] nvarchar(max)  NULL,
    [EmployeeAddressAddr2] nvarchar(max)  NULL,
    [EmployeeAddressCity] nvarchar(max)  NULL,
    [EmployeeAddressState] nvarchar(max)  NULL,
    [EmployeeAddressProvince] nvarchar(max)  NULL,
    [EmployeeAddressCounty] nvarchar(max)  NULL,
    [EmployeeAddressPostalCode] nvarchar(max)  NULL,
    [PrintAs] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Mobile] nvarchar(max)  NULL,
    [Pager] nvarchar(max)  NULL,
    [PagerPIN] nvarchar(max)  NULL,
    [AltPhone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [SSN] nvarchar(max)  NULL,
    [SIN] nvarchar(max)  NULL,
    [NiNumber] nvarchar(max)  NULL,
    [MaritalStatus] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [EmployeeType] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NULL,
    [Sex] nvarchar(max)  NULL,
    [HiredDate] datetime  NULL,
    [ReleasedDate] datetime  NULL,
    [BirthDate] datetime  NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [BillingRateRefListID] nvarchar(max)  NULL,
    [PayrollInfoPayPeriod] nvarchar(max)  NULL,
    [PayrollInfoClassRefListID] nvarchar(max)  NULL,
    [PayrollInfoUseTimeDataToCreatePaychecks] nvarchar(max)  NULL,
    [PayrollInfoSickHoursHoursAvailable] float  NULL,
    [PayrollInfoSickHoursAccrualPeriod] nvarchar(max)  NULL,
    [PayrollInfoSickHoursHoursAccrued] float  NULL,
    [PayrollInfoSickHoursMaximumHours] float  NULL,
    [PayrollInfoSickHoursIsResettingHoursEachNewYear] bit  NULL,
    [PayrollInfoSickHoursHoursUsed] float  NULL,
    [PayrollInfoSickHoursAccrualStartDate] datetime  NULL,
    [PayrollInfoVacationHoursHoursAvailable] float  NULL,
    [PayrollInfoVacationHoursAccrualPeriod] nvarchar(max)  NULL,
    [PayrollInfoVacationHoursHoursAccrued] float  NULL,
    [PayrollInfoVacationHoursMaximumHours] float  NULL,
    [PayrollInfoVacationHoursIsResetHoursEachNewYear] bit  NULL,
    [PayrollInfoVacationHoursHoursUsed] float  NULL,
    [PayrollInfoVacationHoursAccrualStartDate] datetime  NULL
);
GO

-- Creating table 'EmployeeEarning'
CREATE TABLE [dbo].[EmployeeEarning] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [Salutation] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [EmployeeAddressAddr1] nvarchar(max)  NULL,
    [EmployeeAddressAddr2] nvarchar(max)  NULL,
    [EmployeeAddressCity] nvarchar(max)  NULL,
    [EmployeeAddressState] nvarchar(max)  NULL,
    [EmployeeAddressProvince] nvarchar(max)  NULL,
    [EmployeeAddressCounty] nvarchar(max)  NULL,
    [EmployeeAddressPostalCode] nvarchar(max)  NULL,
    [PrintAs] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Mobile] nvarchar(max)  NULL,
    [Pager] nvarchar(max)  NULL,
    [PagerPIN] nvarchar(max)  NULL,
    [AltPhone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [SSN] nvarchar(max)  NULL,
    [SIN] nvarchar(max)  NULL,
    [NiNumber] nvarchar(max)  NULL,
    [MaritalStatus] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [EmployeeType] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NULL,
    [Sex] nvarchar(max)  NULL,
    [HiredDate] datetime  NULL,
    [ReleasedDate] datetime  NULL,
    [BirthDate] datetime  NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [BillingRateRefListID] nvarchar(max)  NULL,
    [PayrollInfoPayPeriod] nvarchar(max)  NULL,
    [PayrollInfoClassRefListID] nvarchar(max)  NULL,
    [PayrollInfoEarningsClearEarnings] bit  NULL,
    [PayrollInfoEarningsSeqNo] int  NULL,
    [PayrollInfoEarningsPayrollItemWageRefListID] nvarchar(max)  NULL,
    [PayrollInfoEarningsRate] decimal(18,0)  NULL,
    [PayrollInfoEarningsRatePercent] decimal(18,0)  NULL,
    [PayrollInfoUseTimeDataToCreatePaychecks] nvarchar(max)  NULL,
    [PayrollInfoSickHoursHoursAvailable] float  NULL,
    [PayrollInfoSickHoursAccrualPeriod] nvarchar(max)  NULL,
    [PayrollInfoSickHoursHoursAccrued] float  NULL,
    [PayrollInfoSickHoursMaximumHours] float  NULL,
    [PayrollInfoSickHoursIsResettingHoursEachNewYear] bit  NULL,
    [PayrollInfoSickHoursHoursUsed] float  NULL,
    [PayrollInfoSickHoursAccrualStartDate] datetime  NULL,
    [PayrollInfoVacationHoursHoursAvailable] float  NULL,
    [PayrollInfoVacationHoursAccrualPeriod] nvarchar(max)  NULL,
    [PayrollInfoVacationHoursHoursAccrued] float  NULL,
    [PayrollInfoVacationHoursMaximumHours] float  NULL,
    [PayrollInfoVacationHoursIsResetHoursEachNewYear] bit  NULL,
    [PayrollInfoVacationHoursHoursUsed] float  NULL,
    [PayrollInfoVacationHoursAccrualStartDate] datetime  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Entity'
CREATE TABLE [dbo].[Entity] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [Type] nvarchar(max)  NULL
);
GO

-- Creating table 'Estimate'
CREATE TABLE [dbo].[Estimate] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [CreateChangeOrder] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'EstimateLine'
CREATE TABLE [dbo].[EstimateLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [CreateChangeOrder] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [EstimateLineType] nvarchar(max)  NULL,
    [EstimateLineSeqNo] int  NULL,
    [EstimateLineGroupTxnLineID] nvarchar(max)  NULL,
    [EstimateLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [EstimateLineGroupDesc] nvarchar(max)  NULL,
    [EstimateLineGroupQuantity] decimal(18,0)  NULL,
    [EstimateLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [EstimateLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [EstimateLineGroupIsPrintItemsInGroup] bit  NULL,
    [EstimateLineGroupTotalAmount] decimal(18,0)  NULL,
    [EstimateLineGroupSeqNo] int  NULL,
    [EstimateLineTxnLineID] nvarchar(max)  NULL,
    [EstimateLineItemRefListID] nvarchar(max)  NULL,
    [EstimateLineDesc] nvarchar(max)  NULL,
    [EstimateLineQuantity] decimal(18,0)  NULL,
    [EstimateLineUnitOfMeasure] nvarchar(max)  NULL,
    [EstimateLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [EstimateLineRate] decimal(18,0)  NULL,
    [EstimateLineRatePercent] decimal(18,0)  NULL,
    [EstimateLinePriceLevelRefListID] nvarchar(max)  NULL,
    [EstimateLineClassRefListID] nvarchar(max)  NULL,
    [EstimateLineAmount] decimal(18,0)  NULL,
    [EstimateLineTaxAmount] decimal(18,0)  NULL,
    [EstimateLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [EstimateLineTaxCodeRefListID] nvarchar(max)  NULL,
    [EstimateLineMarkupRate] decimal(18,0)  NULL,
    [EstimateLineMarkupRatePercent] decimal(18,0)  NULL,
    [EstimateLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldEstimateLineOther1] nvarchar(max)  NULL,
    [CustomFieldEstimateLineOther2] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [CustomFieldEstimateLineGroupOther1] nvarchar(max)  NULL,
    [CustomFieldEstimateLineGroupOther2] nvarchar(max)  NULL
);
GO

-- Creating table 'EstimateLinkedTxn'
CREATE TABLE [dbo].[EstimateLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [CreateChangeOrder] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Host'
CREATE TABLE [dbo].[Host] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ID] int  NULL,
    [ProductName] nvarchar(max)  NULL,
    [MajorVersion] nvarchar(max)  NULL,
    [MinorVersion] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [IsAutomaticLogin] bit  NULL,
    [QBFileMode] nvarchar(max)  NULL,
    [QODBCMajorVersion] nvarchar(max)  NULL,
    [QODBCMinorVersion] nvarchar(max)  NULL,
    [QODBCBuildNumber] nvarchar(max)  NULL,
    [QODBCRegion] nvarchar(max)  NULL,
    [QODBCSerialNo] nvarchar(max)  NULL,
    [QODBCEdition] nvarchar(max)  NULL,
    [QODBCEditionQBES] nvarchar(max)  NULL,
    [QODBCEditionRunning] nvarchar(max)  NULL
);
GO

-- Creating table 'HostMetaData'
CREATE TABLE [dbo].[HostMetaData] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ID] int  NULL,
    [ProductName] nvarchar(max)  NULL,
    [MajorVersion] nvarchar(max)  NULL,
    [MinorVersion] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [IsAutomaticLogin] bit  NULL,
    [QBFileMode] nvarchar(max)  NULL,
    [QODBCMajorVersion] nvarchar(max)  NULL,
    [QODBCMinorVersion] nvarchar(max)  NULL,
    [QODBCBuildNumber] nvarchar(max)  NULL,
    [QODBCRegion] nvarchar(max)  NULL,
    [QODBCSerialNo] nvarchar(max)  NULL,
    [QODBCEdition] nvarchar(max)  NULL,
    [QODBCEditionQBES] nvarchar(max)  NULL,
    [QODBCEditionRunning] nvarchar(max)  NULL,
    [AccountMetaDataMaxCapacity] int  NULL,
    [BillingRateMetaDataMaxCapacity] int  NULL,
    [ClassMetaDataMaxCapacity] int  NULL,
    [CustomerMsgMetaDataMaxCapacity] int  NULL,
    [CustomerTypeMetaDataMaxCapacity] int  NULL,
    [EntityMetaDataMaxCapacity] int  NULL,
    [ItemMetaDataMaxCapacity] int  NULL,
    [JobTypeMetaDataMaxCapacity] int  NULL,
    [PaymentMethodMetaDataMaxCapacity] int  NULL,
    [PayrollItemMetaData] int  NULL,
    [PriceLevelMetaData] int  NULL,
    [SalesRepMetaDataMaxCapacity] int  NULL,
    [SalesTaxCodeMetaDataMaxCapacity] int  NULL,
    [ShipMethodMetaDataMaxCapacity] int  NULL,
    [TemplateMetaDataMaxCapacity] int  NULL,
    [TermsMetaDataMaxCapacity] int  NULL,
    [ToDoMetaDataMaxCapacity] int  NULL,
    [VehicleMetaDataMaxCapacity] int  NULL,
    [VendorTypeMetaDataMaxCapacity] int  NULL
);
GO

-- Creating table 'HostSupportedVersions'
CREATE TABLE [dbo].[HostSupportedVersions] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ID] int  NULL,
    [ProductName] nvarchar(max)  NULL,
    [MajorVersion] nvarchar(max)  NULL,
    [MinorVersion] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [SupportedQBXMLVersion] nvarchar(max)  NULL,
    [IsAutomaticLogin] bit  NULL,
    [QBFileMode] nvarchar(max)  NULL,
    [QODBCMajorVersion] nvarchar(max)  NULL,
    [QODBCMinorVersion] nvarchar(max)  NULL,
    [QODBCBuildNumber] nvarchar(max)  NULL,
    [QODBCRegion] nvarchar(max)  NULL,
    [QODBCSerialNo] nvarchar(max)  NULL,
    [QODBCEdition] nvarchar(max)  NULL,
    [QODBCEditionQBES] nvarchar(max)  NULL,
    [QODBCEditionRunning] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'InventoryAdjustment'
CREATE TABLE [dbo].[InventoryAdjustment] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL
);
GO

-- Creating table 'InventoryAdjustmentLine'
CREATE TABLE [dbo].[InventoryAdjustmentLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [InventoryAdjustmentSeqNo] int  NULL,
    [InventoryAdjustmentLineTxnLineID] nvarchar(max)  NULL,
    [InventoryAdjustmentLineItemRefListID] nvarchar(max)  NULL,
    [InventoryAdjustmentLineQuantityDifference] decimal(18,0)  NULL,
    [InventoryAdjustmentLineValueDifference] decimal(18,0)  NULL,
    [InventoryAdjustmentLineQuantityAdjustmentNewQuantity] decimal(18,0)  NULL,
    [InventoryAdjustmentLineQuantityAdjustmentQuantityDifference] decimal(18,0)  NULL,
    [InventoryAdjustmentLineValueAdjustmentNewQuantity] decimal(18,0)  NULL,
    [InventoryAdjustmentLineValueAdjustmentNewValue] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Invoice'
CREATE TABLE [dbo].[Invoice] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [IsFinanceCharge] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [AppliedAmount] decimal(18,0)  NULL,
    [BalanceRemaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [SuggestedDiscountAmount] decimal(18,0)  NULL,
    [SuggestedDiscountDate] datetime  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'InvoiceLine'
CREATE TABLE [dbo].[InvoiceLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [IsFinanceCharge] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [AppliedAmount] decimal(18,0)  NULL,
    [BalanceRemaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [SuggestedDiscountAmount] decimal(18,0)  NULL,
    [SuggestedDiscountDate] datetime  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [InvoiceLineType] nvarchar(max)  NULL,
    [InvoiceLineSeqNo] int  NULL,
    [InvoiceLineGroupTxnLineID] nvarchar(max)  NULL,
    [InvoiceLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [InvoiceLineGroupDesc] nvarchar(max)  NULL,
    [InvoiceLineGroupQuantity] decimal(18,0)  NULL,
    [InvoiceLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [InvoiceLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [InvoiceLineGroupIsPrintItemsInGroup] bit  NULL,
    [InvoiceLineGroupTotalAmount] decimal(18,0)  NULL,
    [InvoiceLineGroupServiceDate] datetime  NULL,
    [InvoiceLineGroupSeqNo] int  NULL,
    [InvoiceLineTxnLineID] nvarchar(max)  NULL,
    [InvoiceLineItemRefListID] nvarchar(max)  NULL,
    [InvoiceLineDesc] nvarchar(max)  NULL,
    [InvoiceLineQuantity] decimal(18,0)  NULL,
    [InvoiceLineUnitOfMeasure] nvarchar(max)  NULL,
    [InvoiceLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [InvoiceLineRate] decimal(18,0)  NULL,
    [InvoiceLineRatePercent] decimal(18,0)  NULL,
    [InvoiceLinePriceLevelRefListID] nvarchar(max)  NULL,
    [InvoiceLineClassRefListID] nvarchar(max)  NULL,
    [InvoiceLineAmount] decimal(18,0)  NULL,
    [InvoiceLineTaxAmount] decimal(18,0)  NULL,
    [InvoiceLineServiceDate] datetime  NULL,
    [InvoiceLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [InvoiceLineTaxCodeRefListID] nvarchar(max)  NULL,
    [InvoiceLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldInvoiceLineOther1] nvarchar(max)  NULL,
    [CustomFieldInvoiceLineOther2] nvarchar(max)  NULL,
    [InvoiceLineLinkToTxnTxnID] nvarchar(max)  NULL,
    [InvoiceLineLinkToTxnTxnLineID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [CustomFieldInvoiceLineGroupOther1] nvarchar(max)  NULL,
    [CustomFieldInvoiceLineGroupOther2] nvarchar(max)  NULL
);
GO

-- Creating table 'InvoiceLinkedTxn'
CREATE TABLE [dbo].[InvoiceLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [IsFinanceCharge] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [AppliedAmount] decimal(18,0)  NULL,
    [BalanceRemaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [SuggestedDiscountAmount] decimal(18,0)  NULL,
    [SuggestedDiscountDate] datetime  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Item'
CREATE TABLE [dbo].[Item] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [ManufacturerPartNumber] nvarchar(max)  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [Type] nvarchar(max)  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseDesc] nvarchar(max)  NULL,
    [SalesOrPurchasePrice] decimal(18,0)  NULL,
    [SalesOrPurchasePricePercent] decimal(18,0)  NULL,
    [SalesOrPurchaseAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseSalesDesc] nvarchar(max)  NULL,
    [SalesAndPurchaseSalesPrice] decimal(18,0)  NULL,
    [SalesAndPurchaseIncomeAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchasePurchaseDesc] nvarchar(max)  NULL,
    [SalesAndPurchasePurchaseCost] decimal(18,0)  NULL,
    [SalesAndPurchaseExpenseAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchasePrefVendorRefListID] nvarchar(max)  NULL,
    [SalesDesc] nvarchar(max)  NULL,
    [SalesPrice] decimal(18,0)  NULL,
    [IncomeAccountRefListID] nvarchar(max)  NULL,
    [PurchaseDesc] nvarchar(max)  NULL,
    [PurchaseCost] decimal(18,0)  NULL,
    [COGSAccountRefListID] nvarchar(max)  NULL,
    [PrefVendorRefListID] nvarchar(max)  NULL,
    [AssetAccountRefListID] nvarchar(max)  NULL,
    [ReorderBuildPoint] decimal(18,0)  NULL,
    [InventoryDate] datetime  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [TaxRate] decimal(18,0)  NULL,
    [TaxVendorRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemAssembliesCanBuild'
CREATE TABLE [dbo].[ItemAssembliesCanBuild] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ItemInventoryAssemblyRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [QuantityCanBuild] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemDiscount'
CREATE TABLE [dbo].[ItemDiscount] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [DiscountRate] decimal(18,0)  NULL,
    [DiscountRatePercent] decimal(18,0)  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [ApplyAccountRefToExistingTxns] bit  NULL
);
GO

-- Creating table 'ItemFixedAsset'
CREATE TABLE [dbo].[ItemFixedAsset] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [AcquiredAs] nvarchar(max)  NULL,
    [PurchaseDesc] nvarchar(max)  NULL,
    [PurchaseDate] datetime  NULL,
    [PurchaseCost] decimal(18,0)  NULL,
    [VendorOrPayeeName] nvarchar(max)  NULL,
    [AssetAccountRefListID] nvarchar(max)  NULL,
    [FixedAssetSalesInfoSalesDesc] nvarchar(max)  NULL,
    [FixedAssetSalesInfoSalesDate] datetime  NULL,
    [FixedAssetSalesInfoSalesPrice] decimal(18,0)  NULL,
    [FixedAssetSalesInfoSalesExpense] decimal(18,0)  NULL,
    [AssetDesc] nvarchar(max)  NULL,
    [Location] nvarchar(max)  NULL,
    [PONumber] nvarchar(max)  NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [WarrantyExpDate] datetime  NULL,
    [Notes] nvarchar(max)  NULL,
    [AssetNumber] nvarchar(max)  NULL,
    [CostBasis] decimal(18,0)  NULL,
    [YearEndAccumulatedDepreciation] decimal(18,0)  NULL,
    [YearEndBookValue] decimal(18,0)  NULL
);
GO

-- Creating table 'ItemGroup'
CREATE TABLE [dbo].[ItemGroup] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [IsPrintItemsInGroup] bit  NULL,
    [SpecialItemType] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemGroupLine'
CREATE TABLE [dbo].[ItemGroupLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [IsPrintItemsInGroup] bit  NULL,
    [SpecialItemType] nvarchar(max)  NULL,
    [ClearItemsInGroup] bit  NULL,
    [ItemGroupLineSeqNo] int  NULL,
    [ItemGroupLineItemRefListID] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemGroupLineUnitOfMeasure] nvarchar(max)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemInventory'
CREATE TABLE [dbo].[ItemInventory] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [ManufacturerPartNumber] nvarchar(max)  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesDesc] nvarchar(max)  NULL,
    [SalesPrice] decimal(18,0)  NULL,
    [IncomeAccountRefListID] nvarchar(max)  NULL,
    [ApplyIncomeAccountRefToExistingTxns] bit  NULL,
    [PurchaseDesc] nvarchar(max)  NULL,
    [PurchaseCost] decimal(18,0)  NULL,
    [COGSAccountRefListID] nvarchar(max)  NULL,
    [PrefVendorRefListID] nvarchar(max)  NULL,
    [AssetAccountRefListID] nvarchar(max)  NULL,
    [ReorderPoint] decimal(18,0)  NULL,
    [QuantityOnHand] decimal(18,0)  NULL,
    [TotalValue] decimal(18,0)  NULL,
    [InventoryDate] datetime  NULL,
    [AverageCost] decimal(18,0)  NULL,
    [QuantityOnOrder] decimal(18,0)  NULL,
    [QuantityOnSalesOrder] decimal(18,0)  NULL
);
GO

-- Creating table 'ItemInventoryAssembly'
CREATE TABLE [dbo].[ItemInventoryAssembly] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesDesc] nvarchar(max)  NULL,
    [SalesPrice] decimal(18,0)  NULL,
    [IncomeAccountRefListID] nvarchar(max)  NULL,
    [ApplyIncomeAccountRefToExistingTxns] bit  NULL,
    [PurchaseDesc] nvarchar(max)  NULL,
    [PurchaseCost] decimal(18,0)  NULL,
    [COGSAccountRefListID] nvarchar(max)  NULL,
    [PrefVendorRefListID] nvarchar(max)  NULL,
    [AssetAccountRefListID] nvarchar(max)  NULL,
    [BuildPoint] decimal(18,0)  NULL,
    [QuantityOnHand] decimal(18,0)  NULL,
    [TotalValue] decimal(18,0)  NULL,
    [InventoryDate] datetime  NULL,
    [AverageCost] decimal(18,0)  NULL,
    [QuantityOnOrder] decimal(18,0)  NULL,
    [QuantityOnSalesOrder] decimal(18,0)  NULL
);
GO

-- Creating table 'ItemInventoryAssemblyLine'
CREATE TABLE [dbo].[ItemInventoryAssemblyLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesDesc] nvarchar(max)  NULL,
    [SalesPrice] decimal(18,0)  NULL,
    [IncomeAccountRefListID] nvarchar(max)  NULL,
    [ApplyIncomeAccountRefToExistingTxns] bit  NULL,
    [PurchaseDesc] nvarchar(max)  NULL,
    [PurchaseCost] decimal(18,0)  NULL,
    [COGSAccountRefListID] nvarchar(max)  NULL,
    [PrefVendorRefListID] nvarchar(max)  NULL,
    [AssetAccountRefListID] nvarchar(max)  NULL,
    [BuildPoint] decimal(18,0)  NULL,
    [QuantityOnHand] decimal(18,0)  NULL,
    [TotalValue] decimal(18,0)  NULL,
    [InventoryDate] datetime  NULL,
    [AverageCost] decimal(18,0)  NULL,
    [QuantityOnOrder] decimal(18,0)  NULL,
    [QuantityOnSalesOrder] decimal(18,0)  NULL,
    [ClearItemsInGroup] bit  NULL,
    [ItemInventoryAssemblyLnSeqNo] int  NULL,
    [ItemInventoryAssemblyLnItemInventoryRefListID] nvarchar(max)  NULL,
    [ItemInventoryAssemblyLnQuantity] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemNonInventory'
CREATE TABLE [dbo].[ItemNonInventory] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [ManufacturerPartNumber] nvarchar(max)  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseDesc] nvarchar(max)  NULL,
    [SalesOrPurchasePrice] decimal(18,0)  NULL,
    [SalesOrPurchasePricePercent] decimal(18,0)  NULL,
    [SalesOrPurchaseAccountRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseApplyAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchaseSalesDesc] nvarchar(max)  NULL,
    [SalesAndPurchaseSalesPrice] decimal(18,0)  NULL,
    [SalesAndPurchaseIncomeAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseApplyIncomeAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchasePurchaseDesc] nvarchar(max)  NULL,
    [SalesAndPurchasePurchaseCost] decimal(18,0)  NULL,
    [SalesAndPurchaseExpenseAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseApplyExpenseAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchasePrefVendorRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemOtherCharge'
CREATE TABLE [dbo].[ItemOtherCharge] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseDesc] nvarchar(max)  NULL,
    [SalesOrPurchasePrice] decimal(18,0)  NULL,
    [SalesOrPurchasePricePercent] decimal(18,0)  NULL,
    [SalesOrPurchaseAccountRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseApplyAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchaseSalesDesc] nvarchar(max)  NULL,
    [SalesAndPurchaseSalesPrice] decimal(18,0)  NULL,
    [SalesAndPurchaseIncomeAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseApplyIncomeAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchasePurchaseDesc] nvarchar(max)  NULL,
    [SalesAndPurchasePurchaseCost] decimal(18,0)  NULL,
    [SalesAndPurchaseExpenseAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseApplyExpenseAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchasePrefVendorRefListID] nvarchar(max)  NULL,
    [SpecialItemType] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemPayment'
CREATE TABLE [dbo].[ItemPayment] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemReceipt'
CREATE TABLE [dbo].[ItemReceipt] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'ItemReceiptExpenseLine'
CREATE TABLE [dbo].[ItemReceiptExpenseLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [ExpenseLineClearExpenseLines] bit  NULL,
    [ExpenseLineTxnLineID] nvarchar(max)  NULL,
    [ExpenseLineAccountRefListID] nvarchar(max)  NULL,
    [ExpenseLineAmount] decimal(18,0)  NULL,
    [ExpenseLineTaxAmount] decimal(18,0)  NULL,
    [ExpenseLineTax1Amount] decimal(18,0)  NULL,
    [ExpenseLineMemo] nvarchar(max)  NULL,
    [ExpenseLineCustomerRefListID] nvarchar(max)  NULL,
    [ExpenseLineClassRefListID] nvarchar(max)  NULL,
    [ExpenseLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineBillableStatus] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemReceiptItemLine'
CREATE TABLE [dbo].[ItemReceiptItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [ItemLineType] nvarchar(max)  NULL,
    [ItemLineSeqNo] int  NULL,
    [ItemGroupLineTxnLineID] nvarchar(max)  NULL,
    [ItemGroupLineItemGroupRefListID] nvarchar(max)  NULL,
    [ItemGroupLineDesc] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemGroupUnitOfMeasure] nvarchar(max)  NULL,
    [ItemGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemGroupLineTotalAmount] decimal(18,0)  NULL,
    [ItemGroupSeqNo] int  NULL,
    [ItemLineTxnLineID] nvarchar(max)  NULL,
    [ItemLineItemRefListID] nvarchar(max)  NULL,
    [ItemLineDesc] nvarchar(max)  NULL,
    [ItemLineQuantity] decimal(18,0)  NULL,
    [ItemLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemLineCost] decimal(18,0)  NULL,
    [ItemLineAmount] decimal(18,0)  NULL,
    [ItemLineTaxAmount] decimal(18,0)  NULL,
    [ItemLineTax1Amount] decimal(18,0)  NULL,
    [ItemLineCustomerRefListID] nvarchar(max)  NULL,
    [ItemLineClassRefListID] nvarchar(max)  NULL,
    [ItemLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineBillableStatus] nvarchar(max)  NULL,
    [ItemLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [ItemLineLinkToTxnTxnID] nvarchar(max)  NULL,
    [ItemLineLinkToTxnTxnLineID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemReceiptLinkedTxn'
CREATE TABLE [dbo].[ItemReceiptLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [LinkToTxnID1] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemSalesTax'
CREATE TABLE [dbo].[ItemSalesTax] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [IsUsedOnPurchaseTransaction] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [TaxRate] decimal(18,0)  NULL,
    [TaxVendorRefListID] nvarchar(max)  NULL,
    [SalesTaxReturnLineNumber] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemSalesTaxGroup'
CREATE TABLE [dbo].[ItemSalesTaxGroup] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemSalesTaxGroupLine'
CREATE TABLE [dbo].[ItemSalesTaxGroupLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [ItemSalesTaxSeqNo] int  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemService'
CREATE TABLE [dbo].[ItemService] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL,
    [UnitOfMeasureSetRefListID] nvarchar(max)  NULL,
    [ForceUOMChange] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseDesc] nvarchar(max)  NULL,
    [SalesOrPurchasePrice] decimal(18,0)  NULL,
    [SalesOrPurchasePricePercent] decimal(18,0)  NULL,
    [SalesOrPurchaseAccountRefListID] nvarchar(max)  NULL,
    [SalesOrPurchaseApplyAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchaseSalesDesc] nvarchar(max)  NULL,
    [SalesAndPurchaseSalesPrice] decimal(18,0)  NULL,
    [SalesAndPurchaseIncomeAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseApplyIncomeAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchasePurchaseDesc] nvarchar(max)  NULL,
    [SalesAndPurchasePurchaseCost] decimal(18,0)  NULL,
    [SalesAndPurchaseExpenseAccountRefListID] nvarchar(max)  NULL,
    [SalesAndPurchaseApplyExpenseAccountRefToExistingTxns] bit  NULL,
    [SalesAndPurchasePrefVendorRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'ItemSubtotal'
CREATE TABLE [dbo].[ItemSubtotal] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [SpecialItemType] nvarchar(max)  NULL
);
GO

-- Creating table 'JobType'
CREATE TABLE [dbo].[JobType] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL
);
GO

-- Creating table 'JournalEntry'
CREATE TABLE [dbo].[JournalEntry] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsAdjustment] bit  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL,
    [ExchangeRate] decimal(18,0)  NULL
);
GO

-- Creating table 'JournalEntryCreditLine'
CREATE TABLE [dbo].[JournalEntryCreditLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsAdjustment] bit  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL,
    [JournalCreditLineTxnLineID] nvarchar(max)  NULL,
    [JournalCreditLineType] nvarchar(max)  NULL,
    [JournalCreditLineAccountRefListID] nvarchar(max)  NULL,
    [JournalCreditLineAmount] decimal(18,0)  NULL,
    [JournalCreditLineTaxAmount] decimal(18,0)  NULL,
    [JournalCreditLineMemo] nvarchar(max)  NULL,
    [JournalCreditLineEntityRefListID] nvarchar(max)  NULL,
    [JournalCreditLineClassRefListID] nvarchar(max)  NULL,
    [JournalCreditLineBillableStatus] nvarchar(max)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQTransactionLinkKey] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'JournalEntryDebitLine'
CREATE TABLE [dbo].[JournalEntryDebitLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsAdjustment] bit  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL,
    [JournalDebitLineTxnLineID] nvarchar(max)  NULL,
    [JournalDebitLineType] nvarchar(max)  NULL,
    [JournalDebitLineAccountRefListID] nvarchar(max)  NULL,
    [JournalDebitLineAmount] decimal(18,0)  NULL,
    [JournalDebitLineTaxAmount] decimal(18,0)  NULL,
    [JournalDebitLineMemo] nvarchar(max)  NULL,
    [JournalDebitLineEntityRefListID] nvarchar(max)  NULL,
    [JournalDebitLineClassRefListID] nvarchar(max)  NULL,
    [JournalDebitLineBillableStatus] nvarchar(max)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQTransactionLinkKey] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'JournalEntryLine'
CREATE TABLE [dbo].[JournalEntryLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsAdjustment] bit  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL,
    [JournalLineSeqNo] int  NULL,
    [JournalLineType] nvarchar(max)  NULL,
    [JournalLineTxnLineID] nvarchar(max)  NULL,
    [JournalLineAccountRefListID] nvarchar(max)  NULL,
    [JournalLineAmount] decimal(18,0)  NULL,
    [JournalLineCreditAmount] decimal(18,0)  NULL,
    [JournalLineDebitAmount] decimal(18,0)  NULL,
    [JournalLineMemo] nvarchar(max)  NULL,
    [JournalLineEntityRefListID] nvarchar(max)  NULL,
    [JournalLineClassRefListID] nvarchar(max)  NULL,
    [JournalLineBillableStatus] nvarchar(max)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [FQTransactionLinkKey] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ListDeleted'
CREATE TABLE [dbo].[ListDeleted] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListDelType] nvarchar(max)  NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeDeleted] datetime  NULL,
    [TimeModified] datetime  NULL,
    [FullName] nvarchar(max)  NULL
);
GO

-- Creating table 'OtherName'
CREATE TABLE [dbo].[OtherName] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [CompanyName] nvarchar(max)  NULL,
    [Salutation] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [OtherNameAddressAddr1] nvarchar(max)  NULL,
    [OtherNameAddressAddr2] nvarchar(max)  NULL,
    [OtherNameAddressAddr3] nvarchar(max)  NULL,
    [OtherNameAddressAddr4] nvarchar(max)  NULL,
    [OtherNameAddressAddr5] nvarchar(max)  NULL,
    [OtherNameAddressCity] nvarchar(max)  NULL,
    [OtherNameAddressState] nvarchar(max)  NULL,
    [OtherNameAddressProvince] nvarchar(max)  NULL,
    [OtherNameAddressCounty] nvarchar(max)  NULL,
    [OtherNameAddressPostalCode] nvarchar(max)  NULL,
    [OtherNameAddressCountry] nvarchar(max)  NULL,
    [OtherNameAddressNote] nvarchar(max)  NULL,
    [OtherNameAddressBlockAddr1] nvarchar(max)  NULL,
    [OtherNameAddressBlockAddr2] nvarchar(max)  NULL,
    [OtherNameAddressBlockAddr3] nvarchar(max)  NULL,
    [OtherNameAddressBlockAddr4] nvarchar(max)  NULL,
    [OtherNameAddressBlockAddr5] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [AltPhone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [AltContact] nvarchar(max)  NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [BusinessNumber] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'PaymentMethod'
CREATE TABLE [dbo].[PaymentMethod] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [PaymentMethodType] nvarchar(max)  NULL
);
GO

-- Creating table 'PayrollItemNonWage'
CREATE TABLE [dbo].[PayrollItemNonWage] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [NonWageType] nvarchar(max)  NULL,
    [ExpenseAccountRefListID] nvarchar(max)  NULL,
    [LiabilityAccountRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'PayrollItemWage'
CREATE TABLE [dbo].[PayrollItemWage] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [WageType] nvarchar(max)  NULL,
    [ExpenseAccountRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'Preferences'
CREATE TABLE [dbo].[Preferences] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ID] int  NULL,
    [AccountingPrefsIsUsingAccountNumbers] bit  NULL,
    [AccountingPrefsIsRequiringAccounts] bit  NULL,
    [AccountingPrefsIsUsingClassTracking] bit  NULL,
    [AccountingPrefsIsUsingAuditTrail] bit  NULL,
    [AccountingPrefsIsAssigningJournalEntryNumbers] bit  NULL,
    [AccountingPrefsClosingDate] datetime  NULL,
    [AccountingPrefsIsUsingMulticurrency] bit  NULL,
    [AccountingPrefsHomeCurrencyRefListID] nvarchar(max)  NULL,
    [AccountingPrefsIsUsingForeignPricesOnItems] bit  NULL,
    [AccountingPrefsForeignCurrencyRefListID] nvarchar(max)  NULL,
    [FinanceChargePrefsAnnualInterestRate] decimal(18,0)  NULL,
    [FinanceChargePrefsMinFinanceCharge] decimal(18,0)  NULL,
    [FinanceChargePrefsGracePeriod] int  NULL,
    [FinanceChargePrefsFinanceChargeAcctRefListID] nvarchar(max)  NULL,
    [FinanceChargePrefsIsAssessingForOverdueCharges] bit  NULL,
    [FinanceChargePrefsCalculateChargesFrom] nvarchar(max)  NULL,
    [FinanceChargePrefsIsMarkedToBePrinted] bit  NULL,
    [JobsAndEstimatesPrefsIsUsingEstimates] bit  NULL,
    [JobsAndEstimatesPrefsIsUsingProgressInvoicing] bit  NULL,
    [JobsAndEstimatesPrefsIsPrintItemsWithZeroAmt] bit  NULL,
    [PurchasesAndVendorsPrefsIsUsingInventory] bit  NULL,
    [PurchasesAndVendorsPrefsDaysBillsAreDue] int  NULL,
    [PurchasesAndVendorsPrefsIsAutomaticUsingDis] bit  NULL,
    [PurchasesAndVendorsPrefDefaultDisARefListID] nvarchar(max)  NULL,
    [PurchasesAndVendorsPrefsIsUsingUnitsOfMeasure] bit  NULL,
    [ReportsPrefsAgingReportBasis] nvarchar(max)  NULL,
    [ReportsPrefsSummaryReportBasis] nvarchar(max)  NULL,
    [SalesAndCustomersPrefsDeftShipMethRefListID] nvarchar(max)  NULL,
    [SalesAndCustomersPrefsDefaultFOB] nvarchar(max)  NULL,
    [SalesAndCustomersPrefsDefaultMarkup] decimal(18,0)  NULL,
    [SalesAndCustomersPrefsIsTrackingRembrsdExpInc] bit  NULL,
    [SalesAndCustomersPrefsIsAutoApplyingPayments] bit  NULL,
    [SalesTaxPrefsDefaultItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPrefsPaySalesTax] nvarchar(max)  NULL,
    [SalesTaxPrefsDefaultTaxableSaleTCRefListID] nvarchar(max)  NULL,
    [SalesTaxPrefsDefaultNonTaxableSaleTCRefListID] nvarchar(max)  NULL,
    [TimeTrackingPrefsFirstDayOfWeek] nvarchar(max)  NULL,
    [CurrentAppAccessRightsIsAutomaticLoginAllowed] bit  NULL,
    [CurrentAppAccessRightsAutomaticLoginUserName] nvarchar(max)  NULL,
    [CurrentAppAccessRightsIsPersonalDataAccAllowed] bit  NULL
);
GO

-- Creating table 'PriceLevel'
CREATE TABLE [dbo].[PriceLevel] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [PriceLevelType] nvarchar(max)  NULL,
    [PriceLevelFixedPercentage] decimal(18,0)  NULL
);
GO

-- Creating table 'PriceLevelPerItem'
CREATE TABLE [dbo].[PriceLevelPerItem] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [PriceLevelType] nvarchar(max)  NULL,
    [PriceLevelFixedPercentage] decimal(18,0)  NULL,
    [PriceLevelPerItemSeqNo] int  NULL,
    [PriceLevelPerItemItemRefListID] nvarchar(max)  NULL,
    [PriceLevelPerItemCustomPrice] decimal(18,0)  NULL,
    [PriceLevelPerItemCustomPricePercent] decimal(18,0)  NULL,
    [PriceLevelPerItemAdjustPercentage] decimal(18,0)  NULL,
    [PriceLevelPerItemAdjustRelativeTo] nvarchar(max)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'PurchaseOrder'
CREATE TABLE [dbo].[PurchaseOrder] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ShipToEntityRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [VendorAddressAddr1] nvarchar(max)  NULL,
    [VendorAddressAddr2] nvarchar(max)  NULL,
    [VendorAddressAddr3] nvarchar(max)  NULL,
    [VendorAddressAddr4] nvarchar(max)  NULL,
    [VendorAddressAddr5] nvarchar(max)  NULL,
    [VendorAddressCity] nvarchar(max)  NULL,
    [VendorAddressState] nvarchar(max)  NULL,
    [VendorAddressProvince] nvarchar(max)  NULL,
    [VendorAddressCounty] nvarchar(max)  NULL,
    [VendorAddressPostalCode] nvarchar(max)  NULL,
    [VendorAddressCountry] nvarchar(max)  NULL,
    [VendorAddressNote] nvarchar(max)  NULL,
    [VendorAddressBlockAddr1] nvarchar(max)  NULL,
    [VendorAddressBlockAddr2] nvarchar(max)  NULL,
    [VendorAddressBlockAddr3] nvarchar(max)  NULL,
    [VendorAddressBlockAddr4] nvarchar(max)  NULL,
    [VendorAddressBlockAddr5] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [ExpectedDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [IsManuallyClosed] bit  NULL,
    [IsFullyReceived] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [VendorMsg] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther1] nvarchar(max)  NULL,
    [CustomFieldOther2] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'PurchaseOrderLine'
CREATE TABLE [dbo].[PurchaseOrderLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ShipToEntityRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [VendorAddressAddr1] nvarchar(max)  NULL,
    [VendorAddressAddr2] nvarchar(max)  NULL,
    [VendorAddressAddr3] nvarchar(max)  NULL,
    [VendorAddressAddr4] nvarchar(max)  NULL,
    [VendorAddressAddr5] nvarchar(max)  NULL,
    [VendorAddressCity] nvarchar(max)  NULL,
    [VendorAddressState] nvarchar(max)  NULL,
    [VendorAddressProvince] nvarchar(max)  NULL,
    [VendorAddressCounty] nvarchar(max)  NULL,
    [VendorAddressPostalCode] nvarchar(max)  NULL,
    [VendorAddressCountry] nvarchar(max)  NULL,
    [VendorAddressNote] nvarchar(max)  NULL,
    [VendorAddressBlockAddr1] nvarchar(max)  NULL,
    [VendorAddressBlockAddr2] nvarchar(max)  NULL,
    [VendorAddressBlockAddr3] nvarchar(max)  NULL,
    [VendorAddressBlockAddr4] nvarchar(max)  NULL,
    [VendorAddressBlockAddr5] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [ExpectedDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [IsManuallyClosed] bit  NULL,
    [IsFullyReceived] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [VendorMsg] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther1] nvarchar(max)  NULL,
    [CustomFieldOther2] nvarchar(max)  NULL,
    [PurchaseOrderLineType] nvarchar(max)  NULL,
    [PurchaseOrderLineSeqNo] int  NULL,
    [PurchaseOrderLineGroupTxnLineID] nvarchar(max)  NULL,
    [PurchaseOrderLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineGroupItemGroupFullName] nvarchar(max)  NULL,
    [PurchaseOrderLineGroupDesc] nvarchar(max)  NULL,
    [PurchaseOrderLineGroupQuantity] decimal(18,0)  NULL,
    [PurchaseOrderLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [PurchaseOrderLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineGroupIsPrintItemsInGroup] bit  NULL,
    [PurchaseOrderLineGroupTotalAmount] decimal(18,0)  NULL,
    [PurchaseOrderLineGroupServiceDate] datetime  NULL,
    [PurchaseOrderLineGroupSeqNo] int  NULL,
    [PurchaseOrderLineTxnLineID] nvarchar(max)  NULL,
    [PurchaseOrderLineItemRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineManufacturerPartNumber] nvarchar(max)  NULL,
    [PurchaseOrderLineDesc] nvarchar(max)  NULL,
    [PurchaseOrderLineQuantity] decimal(18,0)  NULL,
    [PurchaseOrderLineUnitOfMeasure] nvarchar(max)  NULL,
    [PurchaseOrderLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineRate] decimal(18,0)  NULL,
    [PurchaseOrderLineClassRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineAmount] decimal(18,0)  NULL,
    [PurchaseOrderLineTaxAmount] decimal(18,0)  NULL,
    [PurchaseOrderLineCustomerRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineServiceDate] datetime  NULL,
    [PurchaseOrderLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineTaxCodeRefListID] nvarchar(max)  NULL,
    [PurchaseOrderLineReceivedQuantity] decimal(18,0)  NULL,
    [PurchaseOrderLineIsManuallyClosed] bit  NULL,
    [PurchaseOrderLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldPurchaseOrderLineOther1] nvarchar(max)  NULL,
    [CustomFieldPurchaseOrderLineOther2] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [CustomFieldPurchaseOrderLineGroupOther1] nvarchar(max)  NULL,
    [CustomFieldPurchaseOrderLineGroupOther2] nvarchar(max)  NULL
);
GO

-- Creating table 'PurchaseOrderLinkedTxn'
CREATE TABLE [dbo].[PurchaseOrderLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ShipToEntityRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [VendorAddressAddr1] nvarchar(max)  NULL,
    [VendorAddressAddr2] nvarchar(max)  NULL,
    [VendorAddressAddr3] nvarchar(max)  NULL,
    [VendorAddressAddr4] nvarchar(max)  NULL,
    [VendorAddressAddr5] nvarchar(max)  NULL,
    [VendorAddressCity] nvarchar(max)  NULL,
    [VendorAddressState] nvarchar(max)  NULL,
    [VendorAddressProvince] nvarchar(max)  NULL,
    [VendorAddressCounty] nvarchar(max)  NULL,
    [VendorAddressPostalCode] nvarchar(max)  NULL,
    [VendorAddressCountry] nvarchar(max)  NULL,
    [VendorAddressNote] nvarchar(max)  NULL,
    [VendorAddressBlockAddr1] nvarchar(max)  NULL,
    [VendorAddressBlockAddr2] nvarchar(max)  NULL,
    [VendorAddressBlockAddr3] nvarchar(max)  NULL,
    [VendorAddressBlockAddr4] nvarchar(max)  NULL,
    [VendorAddressBlockAddr5] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [ExpectedDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [IsManuallyClosed] bit  NULL,
    [IsFullyReceived] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [VendorMsg] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [SalesTaxCodeRefListID] nvarchar(max)  NULL,
    [TaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther1] nvarchar(max)  NULL,
    [CustomFieldOther2] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ReceivePayment'
CREATE TABLE [dbo].[ReceivePayment] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [IsAutoApply] bit  NULL,
    [UnusedPayment] decimal(18,0)  NULL,
    [UnusedCredits] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL
);
GO

-- Creating table 'ReceivePaymentLine'
CREATE TABLE [dbo].[ReceivePaymentLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [IsAutoApply] bit  NULL,
    [UnusedPayment] decimal(18,0)  NULL,
    [UnusedCredits] decimal(18,0)  NULL,
    [AppliedToTxnTxnID] nvarchar(max)  NULL,
    [AppliedToTxnPaymentAmount] decimal(18,0)  NULL,
    [AppliedToTxnTxnType] nvarchar(max)  NULL,
    [AppliedToTxnTxnDate] datetime  NULL,
    [AppliedToTxnRefNumber] nvarchar(max)  NULL,
    [AppliedToTxnBalanceRemaining] decimal(18,0)  NULL,
    [AppliedToTxnAmount] decimal(18,0)  NULL,
    [AppliedToTxnSetCreditCreditTxnID] nvarchar(max)  NULL,
    [AppliedToTxnSetCreditAppliedAmount] decimal(18,0)  NULL,
    [AppliedToTxnDiscountAmount] decimal(18,0)  NULL,
    [AppliedToTxnDiscountAccountRefListID] nvarchar(max)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ReceivePaymentToDeposit'
CREATE TABLE [dbo].[ReceivePaymentToDeposit] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TxnLineID] nvarchar(max)  NULL,
    [TxnType] nvarchar(max)  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Sales'
CREATE TABLE [dbo].[Sales] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [CheckNumber] nvarchar(max)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [IsFinanceCharge] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [Amount] decimal(18,0)  NULL,
    [Remaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [SuggestedDiscountAmount] decimal(18,0)  NULL,
    [SuggestedDiscountDate] datetime  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [CreditMemoLineGroupIsPrintItemsInGroup] bit  NULL,
    [Type] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'SalesLine'
CREATE TABLE [dbo].[SalesLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [ARAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [CheckNumber] nvarchar(max)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [IsFinanceCharge] bit  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [Amount] decimal(18,0)  NULL,
    [Remaining] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsPaid] bit  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [SuggestedDiscountAmount] decimal(18,0)  NULL,
    [SuggestedDiscountDate] datetime  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [CreditMemoLineGroupIsPrintItemsInGroup] bit  NULL,
    [Type] nvarchar(max)  NULL,
    [SalesLineType] nvarchar(max)  NULL,
    [SalesLineSeqNo] int  NULL,
    [SalesLineGroupTxnLineID] nvarchar(max)  NULL,
    [SalesLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [SalesLineGroupDesc] nvarchar(max)  NULL,
    [SalesLineGroupQuantity] decimal(18,0)  NULL,
    [SalesLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [SalesLineGroupIsPrintItemsInGroup] bit  NULL,
    [SalesLineGroupTotalAmount] decimal(18,0)  NULL,
    [SalesLineGroupSeqNo] int  NULL,
    [SalesLineTxnLineID] nvarchar(max)  NULL,
    [SalesLineItemRefListID] nvarchar(max)  NULL,
    [SalesLineDesc] nvarchar(max)  NULL,
    [SalesLineQuantity] decimal(18,0)  NULL,
    [SalesLineUnitOfMeasure] nvarchar(max)  NULL,
    [SalesLineRate] decimal(18,0)  NULL,
    [SalesLineRatePercent] decimal(18,0)  NULL,
    [SalesLineClassRefListID] nvarchar(max)  NULL,
    [SalesLineAmount] decimal(18,0)  NULL,
    [SalesLineServiceDate] datetime  NULL,
    [SalesLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesLineTaxCodeRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesOrder'
CREATE TABLE [dbo].[SalesOrder] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [IsManuallyClosed] bit  NULL,
    [IsFullyInvoiced] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'SalesOrderLine'
CREATE TABLE [dbo].[SalesOrderLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [IsManuallyClosed] bit  NULL,
    [IsFullyInvoiced] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [SalesOrderLineSeqNo] int  NULL,
    [SalesOrderLineGroupTxnLineID] nvarchar(max)  NULL,
    [SalesOrderLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [SalesOrderLineGroupDesc] nvarchar(max)  NULL,
    [SalesOrderLineGroupQuantity] decimal(18,0)  NULL,
    [SalesOrderLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [SalesOrderLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [SalesOrderLineGroupIsPrintItemsInGroup] bit  NULL,
    [SalesOrderLineGroupTotalAmount] decimal(18,0)  NULL,
    [SalesOrderLineGroupSeqNo] int  NULL,
    [SalesOrderLineTxnLineID] nvarchar(max)  NULL,
    [SalesOrderLineItemRefListID] nvarchar(max)  NULL,
    [SalesOrderLineDesc] nvarchar(max)  NULL,
    [SalesOrderLineQuantity] decimal(18,0)  NULL,
    [SalesOrderLineUnitOfMeasure] nvarchar(max)  NULL,
    [SalesOrderLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [SalesOrderLineRate] decimal(18,0)  NULL,
    [SalesOrderLineRatePercent] decimal(18,0)  NULL,
    [SalesOrderLinePriceLevelRefListID] nvarchar(max)  NULL,
    [SalesOrderLineClassRefListID] nvarchar(max)  NULL,
    [SalesOrderLineAmount] decimal(18,0)  NULL,
    [SalesOrderLineTaxAmount] decimal(18,0)  NULL,
    [SalesOrderLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesOrderLineTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesOrderLineInvoiced] decimal(18,0)  NULL,
    [SalesOrderLineIsManuallyClosed] bit  NULL,
    [CustomFieldSalesOrderLineOther1] nvarchar(max)  NULL,
    [CustomFieldSalesOrderLineOther2] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [CustomFieldSalesOrderLineGroupOther1] nvarchar(max)  NULL,
    [CustomFieldSalesOrderLineGroupOther2] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesOrderLinkedTxn'
CREATE TABLE [dbo].[SalesOrderLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [PONumber] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [IsManuallyClosed] bit  NULL,
    [IsFullyInvoiced] bit  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesReceipt'
CREATE TABLE [dbo].[SalesReceipt] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [CheckNumber] nvarchar(max)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'SalesReceiptLine'
CREATE TABLE [dbo].[SalesReceiptLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TemplateRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [IsPending] bit  NULL,
    [CheckNumber] nvarchar(max)  NULL,
    [PaymentMethodRefListID] nvarchar(max)  NULL,
    [DueDate] datetime  NULL,
    [SalesRepRefListID] nvarchar(max)  NULL,
    [ShipDate] datetime  NULL,
    [ShipMethodRefListID] nvarchar(max)  NULL,
    [FOB] nvarchar(max)  NULL,
    [Subtotal] decimal(18,0)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPercentage] decimal(18,0)  NULL,
    [SalesTaxTotal] decimal(18,0)  NULL,
    [TotalAmount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [CustomerMsgRefListID] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [IsToBeEmailed] bit  NULL,
    [IsTaxIncluded] bit  NULL,
    [CustomerSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [CustomerTaxCodeRefListID] nvarchar(max)  NULL,
    [DepositToAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldOther] nvarchar(max)  NULL,
    [SalesReceiptLineItemLineType] nvarchar(max)  NULL,
    [SalesReceiptLineSeqNo] int  NULL,
    [SalesReceiptLineGroupTxnLineID] nvarchar(max)  NULL,
    [SalesReceiptLineGroupItemGroupRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineGroupDesc] nvarchar(max)  NULL,
    [SalesReceiptLineGroupQuantity] decimal(18,0)  NULL,
    [SalesReceiptLineGroupUnitOfMeasure] nvarchar(max)  NULL,
    [SalesReceiptLineGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineGroupIsPrintItemsInGroup] bit  NULL,
    [SalesReceiptLineGroupTotalAmount] decimal(18,0)  NULL,
    [SalesReceiptLineGroupServiceDate] datetime  NULL,
    [SalesReceiptLineGroupSeqNo] int  NULL,
    [SalesReceiptLineTxnLineID] nvarchar(max)  NULL,
    [SalesReceiptLineItemRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineDesc] nvarchar(max)  NULL,
    [SalesReceiptLineQuantity] decimal(18,0)  NULL,
    [SalesReceiptLineUnitOfMeasure] nvarchar(max)  NULL,
    [SalesReceiptLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineRate] decimal(18,0)  NULL,
    [SalesReceiptLineRatePercent] decimal(18,0)  NULL,
    [SalesReceiptLinePriceLevelRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineClassRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineAmount] decimal(18,0)  NULL,
    [SalesReceiptTaxAmount] decimal(18,0)  NULL,
    [SalesReceiptLineServiceDate] datetime  NULL,
    [SalesReceiptLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineTaxCodeRefListID] nvarchar(max)  NULL,
    [SalesReceiptLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [CustomFieldSalesReceiptLineOther1] nvarchar(max)  NULL,
    [CustomFieldSalesReceiptLineOther2] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [CustomFieldSalesReceiptLineGroupOther1] nvarchar(max)  NULL,
    [CustomFieldSalesReceiptLineGroupOther2] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesRep'
CREATE TABLE [dbo].[SalesRep] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Initial] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [SalesRepEntityRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesTaxCode'
CREATE TABLE [dbo].[SalesTaxCode] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [IsTaxable] bit  NULL,
    [Desc] nvarchar(max)  NULL,
    [ItemPurchaseTaxRefListID] nvarchar(max)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesTaxPaymentCheck'
CREATE TABLE [dbo].[SalesTaxPaymentCheck] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [BankAccountRefListID] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL
);
GO

-- Creating table 'SalesTaxPaymentCheckLine'
CREATE TABLE [dbo].[SalesTaxPaymentCheckLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [PayeeEntityRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [BankAccountRefListID] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [IsToBePrinted] bit  NULL,
    [SalesTaxPaymentCheckLineSeqNo] int  NULL,
    [SalesTaxPaymentCheckLineTxnLineID] nvarchar(max)  NULL,
    [SalesTaxPaymentCheckLineItemSalesTaxRefListID] nvarchar(max)  NULL,
    [SalesTaxPaymentCheckLineAmount] decimal(18,0)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ShipMethod'
CREATE TABLE [dbo].[ShipMethod] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'SpecialAccount'
CREATE TABLE [dbo].[SpecialAccount] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [SpecialAccountType] nvarchar(max)  NULL
);
GO

-- Creating table 'SpecialItem'
CREATE TABLE [dbo].[SpecialItem] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [SpecialItemType] nvarchar(max)  NULL
);
GO

-- Creating table 'StandardTerms'
CREATE TABLE [dbo].[StandardTerms] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [StdDueDays] int  NULL,
    [StdDiscountDays] int  NULL,
    [DiscountPct] decimal(18,0)  NULL
);
GO

-- Creating table 'TaxCode'
CREATE TABLE [dbo].[TaxCode] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [IsTaxable] bit  NULL,
    [Desc] nvarchar(max)  NULL,
    [ItemPurchaseTaxRefListID] nvarchar(max)  NULL,
    [ItemSalesTaxRefListID] nvarchar(max)  NULL
);
GO

-- Creating table 'Template'
CREATE TABLE [dbo].[Template] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [TemplateType] nvarchar(max)  NULL
);
GO

-- Creating table 'Terms'
CREATE TABLE [dbo].[Terms] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [DayOfMonthDue] int  NULL,
    [DueNextMonthDays] int  NULL,
    [DiscountDayOfMonth] int  NULL,
    [DiscountPct] decimal(18,0)  NULL,
    [StdDueDays] int  NULL,
    [StdDiscountDays] int  NULL,
    [StdDiscountPct] decimal(18,0)  NULL,
    [Type] nvarchar(max)  NULL
);
GO

-- Creating table 'TimeTracking'
CREATE TABLE [dbo].[TimeTracking] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [TxnDate] datetime  NULL,
    [EntityRefListID] nvarchar(max)  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ItemServiceRefListID] nvarchar(max)  NULL,
    [Rate] decimal(18,0)  NULL,
    [DurationMinutes] int  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [PayrollItemWageRefListID] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [BillableStatus] nvarchar(max)  NULL
);
GO

-- Creating table 'ToDo'
CREATE TABLE [dbo].[ToDo] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [IsDone] bit  NULL,
    [ReminderDate] datetime  NULL
);
GO

-- Creating table 'Transaction'
CREATE TABLE [dbo].[Transaction] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnType] nvarchar(max)  NULL,
    [TxnID] nvarchar(max)  NULL,
    [TxnLineID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EntityRefListID] nvarchar(max)  NULL,
    [AccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL,
    [Memo] nvarchar(max)  NULL,
    [TransactionDetailLevelFilter] nvarchar(max)  NULL,
    [TransactionPostingStatusFilter] nvarchar(max)  NULL,
    [TransactionPaidStatusFilter] nvarchar(max)  NULL,
    [Empty] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL,
    [FQJournalEntryLinkKey] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'TxnDeleted'
CREATE TABLE [dbo].[TxnDeleted] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnDelType] nvarchar(max)  NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeDeleted] datetime  NULL,
    [TimeModified] datetime  NULL,
    [RefNumber] nvarchar(max)  NULL
);
GO

-- Creating table 'UnitOfMeasureSet'
CREATE TABLE [dbo].[UnitOfMeasureSet] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [UnitOfMeasureType] nvarchar(max)  NULL,
    [BaseUnitName] nvarchar(max)  NULL,
    [BaseUnitAbbreviation] nvarchar(max)  NULL
);
GO

-- Creating table 'UnitOfMeasureSetRelatedUnit'
CREATE TABLE [dbo].[UnitOfMeasureSetRelatedUnit] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [UnitOfMeasureType] nvarchar(max)  NULL,
    [BaseUnitName] nvarchar(max)  NULL,
    [BaseUnitAbbreviation] nvarchar(max)  NULL,
    [RelatedUnitSeqNo] int  NULL,
    [RelatedUnitName] nvarchar(max)  NULL,
    [RelatedUnitAbbreviation] nvarchar(max)  NULL,
    [RelatedUnitConversionRatio] decimal(18,0)  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'UnitOfMeasureSetDefaultUnit'
CREATE TABLE [dbo].[UnitOfMeasureSetDefaultUnit] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [UnitOfMeasureType] nvarchar(max)  NULL,
    [BaseUnitName] nvarchar(max)  NULL,
    [BaseUnitAbbreviation] nvarchar(max)  NULL,
    [DefaultUnitUnitSeqNo] int  NULL,
    [DefaultUnitUnitUsedFor] nvarchar(max)  NULL,
    [DefaultUnitUnit] nvarchar(max)  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Vehicle'
CREATE TABLE [dbo].[Vehicle] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [Desc] nvarchar(max)  NULL
);
GO

-- Creating table 'VehicleMileage'
CREATE TABLE [dbo].[VehicleMileage] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [VehicleRefListID] nvarchar(max)  NULL,
    [CustomerRefListID] nvarchar(max)  NULL,
    [ItemRefListID] nvarchar(max)  NULL,
    [ClassRefListID] nvarchar(max)  NULL,
    [TripStartDate] datetime  NULL,
    [TripEndDate] datetime  NULL,
    [OdometerStart] decimal(18,0)  NULL,
    [OdometerEnd] decimal(18,0)  NULL,
    [TotalMiles] decimal(18,0)  NULL,
    [Notes] nvarchar(max)  NULL,
    [BillableStatus] nvarchar(max)  NULL,
    [StandardMileageRate] decimal(18,0)  NULL,
    [StandardMileageTotalAmount] decimal(18,0)  NULL,
    [BillableRate] decimal(18,0)  NULL,
    [BillableAmount] decimal(18,0)  NULL
);
GO

-- Creating table 'Vendor'
CREATE TABLE [dbo].[Vendor] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [IsTaxAgency] bit  NULL,
    [CompanyName] nvarchar(max)  NULL,
    [Salutation] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [VendorAddressAddr1] nvarchar(max)  NULL,
    [VendorAddressAddr2] nvarchar(max)  NULL,
    [VendorAddressAddr3] nvarchar(max)  NULL,
    [VendorAddressAddr4] nvarchar(max)  NULL,
    [VendorAddressAddr5] nvarchar(max)  NULL,
    [VendorAddressCity] nvarchar(max)  NULL,
    [VendorAddressState] nvarchar(max)  NULL,
    [VendorAddressProvince] nvarchar(max)  NULL,
    [VendorAddressCounty] nvarchar(max)  NULL,
    [VendorAddressPostalCode] nvarchar(max)  NULL,
    [VendorAddressCountry] nvarchar(max)  NULL,
    [VendorAddressNote] nvarchar(max)  NULL,
    [VendorAddressBlockAddr1] nvarchar(max)  NULL,
    [VendorAddressBlockAddr2] nvarchar(max)  NULL,
    [VendorAddressBlockAddr3] nvarchar(max)  NULL,
    [VendorAddressBlockAddr4] nvarchar(max)  NULL,
    [VendorAddressBlockAddr5] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [AltPhone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [AltContact] nvarchar(max)  NULL,
    [NameOnCheck] nvarchar(max)  NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [VendorTypeRefListID] nvarchar(max)  NULL,
    [TermsRefListID] nvarchar(max)  NULL,
    [CreditLimit] decimal(18,0)  NULL,
    [VendorTaxIdent] nvarchar(max)  NULL,
    [IsVendorEligibleFor1099] bit  NULL,
    [IsVendorEligibleForT4A] bit  NULL,
    [OpenBalance] decimal(18,0)  NULL,
    [OpenBalanceDate] datetime  NULL,
    [BillingRateRefListID] nvarchar(max)  NULL,
    [Balance] decimal(18,0)  NULL,
    [CurrencyRefListID] nvarchar(max)  NULL,
    [BusinessNumber] nvarchar(max)  NULL
);
GO

-- Creating table 'VendorCredit'
CREATE TABLE [dbo].[VendorCredit] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [CreditAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [OpenAmount] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL
);
GO

-- Creating table 'VendorCreditExpenseLine'
CREATE TABLE [dbo].[VendorCreditExpenseLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [CreditAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [ExpenseLineSeqNo] int  NULL,
    [ExpenseLineTxnLineID] nvarchar(max)  NULL,
    [ExpenseLineAccountRefListID] nvarchar(max)  NULL,
    [ExpenseLineAmount] decimal(18,0)  NULL,
    [ExpenseLineTaxAmount] decimal(18,0)  NULL,
    [ExpenseLineTax1Amount] decimal(18,0)  NULL,
    [ExpenseLineMemo] nvarchar(max)  NULL,
    [ExpenseLineCustomerRefListID] nvarchar(max)  NULL,
    [ExpenseLineClassRefListID] nvarchar(max)  NULL,
    [ExpenseLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ExpenseLineBillableStatus] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [OpenAmount] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'VendorCreditItemLine'
CREATE TABLE [dbo].[VendorCreditItemLine] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [CreditAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [ItemLineType] nvarchar(max)  NULL,
    [ItemLineSeqNo] int  NULL,
    [ItemGroupTxnLineID] nvarchar(max)  NULL,
    [ItemGroupLineItemRefListID] nvarchar(max)  NULL,
    [ItemGroupLineDesc] nvarchar(max)  NULL,
    [ItemGroupUnitOfMeasure] nvarchar(max)  NULL,
    [ItemGroupOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemGroupLineQuantity] decimal(18,0)  NULL,
    [ItemGroupLineTotalAmount] decimal(18,0)  NULL,
    [ItemGroupSeqNo] int  NULL,
    [ItemLineTxnLineID] nvarchar(max)  NULL,
    [ItemLineItemRefListID] nvarchar(max)  NULL,
    [ItemLineDesc] nvarchar(max)  NULL,
    [ItemLineQuantity] decimal(18,0)  NULL,
    [ItemLineUnitOfMeasure] nvarchar(max)  NULL,
    [ItemLineOverrideUOMSetRefListID] nvarchar(max)  NULL,
    [ItemLineCost] decimal(18,0)  NULL,
    [ItemLineAmount] decimal(18,0)  NULL,
    [ItemLineTaxAmount] decimal(18,0)  NULL,
    [ItemLineTax1Amount] decimal(18,0)  NULL,
    [ItemLineCustomerRefListID] nvarchar(max)  NULL,
    [ItemLineClassRefListID] nvarchar(max)  NULL,
    [ItemLineSalesTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineTaxCodeRefListID] nvarchar(max)  NULL,
    [ItemLineBillableStatus] nvarchar(max)  NULL,
    [ItemLineOverrideItemAccountRefListID] nvarchar(max)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [OpenAmount] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQSaveToCache] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL,
    [FQTxnLinkKey] nvarchar(max)  NULL
);
GO

-- Creating table 'VendorCreditLinkedTxn'
CREATE TABLE [dbo].[VendorCreditLinkedTxn] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [TxnID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [TxnNumber] int  NULL,
    [VendorRefListID] nvarchar(max)  NULL,
    [APAccountRefListID] nvarchar(max)  NULL,
    [TxnDate] datetime  NULL,
    [CreditAmount] decimal(18,0)  NULL,
    [RefNumber] nvarchar(max)  NULL,
    [Memo] nvarchar(max)  NULL,
    [LinkedTxnSeqNo] int  NULL,
    [LinkedTxnTxnID] nvarchar(max)  NULL,
    [LinkedTxnTxnType] nvarchar(max)  NULL,
    [LinkedTxnTxnDate] datetime  NULL,
    [LinkedTxnRefNumber] nvarchar(max)  NULL,
    [LinkedTxnLinkType] nvarchar(max)  NULL,
    [LinkedTxnAmount] decimal(18,0)  NULL,
    [Tax1Total] decimal(18,0)  NULL,
    [Tax2Total] decimal(18,0)  NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [OpenAmount] decimal(18,0)  NULL,
    [AmountIncludesVAT] bit  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- Creating table 'VendorType'
CREATE TABLE [dbo].[VendorType] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [ParentRefListID] nvarchar(max)  NULL,
    [Sublevel] int  NULL
);
GO

-- Creating table 'WorkersCompCode'
CREATE TABLE [dbo].[WorkersCompCode] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [Desc] nvarchar(max)  NULL,
    [CurrentRate] decimal(18,0)  NULL,
    [CurrentEffectiveDate] datetime  NULL,
    [NextRate] decimal(18,0)  NULL,
    [NextEffectiveDate] datetime  NULL
);
GO

-- Creating table 'WorkersCompCodeRateHistory'
CREATE TABLE [dbo].[WorkersCompCodeRateHistory] (
    [RowId] int IDENTITY(1,1) NOT NULL,
    [CompanyId] int  NOT NULL,
    [ListID] nvarchar(max)  NULL,
    [TimeCreated] datetime  NULL,
    [TimeModified] datetime  NULL,
    [EditSequence] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [Desc] nvarchar(max)  NULL,
    [CurrentRate] decimal(18,0)  NULL,
    [CurrentEffectiveDate] datetime  NULL,
    [NextRate] decimal(18,0)  NULL,
    [NextEffectiveDate] datetime  NULL,
    [RateHistorySeqNo] int  NULL,
    [RateHistoryRate] decimal(18,0)  NULL,
    [RateHistoryEffectiveDate] datetime  NULL,
    [FQPrimaryKey] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [RowId] in table 'Account'
ALTER TABLE [dbo].[Account]
ADD CONSTRAINT [PK_Account]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'AccountTaxLineInfo'
ALTER TABLE [dbo].[AccountTaxLineInfo]
ADD CONSTRAINT [PK_AccountTaxLineInfo]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ARRefundCreditCard'
ALTER TABLE [dbo].[ARRefundCreditCard]
ADD CONSTRAINT [PK_ARRefundCreditCard]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ARRefundCreditCardRefundAppliedTo'
ALTER TABLE [dbo].[ARRefundCreditCardRefundAppliedTo]
ADD CONSTRAINT [PK_ARRefundCreditCardRefundAppliedTo]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Bill'
ALTER TABLE [dbo].[Bill]
ADD CONSTRAINT [PK_Bill]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillExpenseLine'
ALTER TABLE [dbo].[BillExpenseLine]
ADD CONSTRAINT [PK_BillExpenseLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillItemLine'
ALTER TABLE [dbo].[BillItemLine]
ADD CONSTRAINT [PK_BillItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillLinkedTxn'
ALTER TABLE [dbo].[BillLinkedTxn]
ADD CONSTRAINT [PK_BillLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillingRate'
ALTER TABLE [dbo].[BillingRate]
ADD CONSTRAINT [PK_BillingRate]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillingRateLine'
ALTER TABLE [dbo].[BillingRateLine]
ADD CONSTRAINT [PK_BillingRateLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillPaymentCheck'
ALTER TABLE [dbo].[BillPaymentCheck]
ADD CONSTRAINT [PK_BillPaymentCheck]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillPaymentCheckLine'
ALTER TABLE [dbo].[BillPaymentCheckLine]
ADD CONSTRAINT [PK_BillPaymentCheckLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillPaymentCreditCard'
ALTER TABLE [dbo].[BillPaymentCreditCard]
ADD CONSTRAINT [PK_BillPaymentCreditCard]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillPaymentCreditCardLine'
ALTER TABLE [dbo].[BillPaymentCreditCardLine]
ADD CONSTRAINT [PK_BillPaymentCreditCardLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BillToPay'
ALTER TABLE [dbo].[BillToPay]
ADD CONSTRAINT [PK_BillToPay]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BuildAssembly'
ALTER TABLE [dbo].[BuildAssembly]
ADD CONSTRAINT [PK_BuildAssembly]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'BuildAssemblyComponentItemLine'
ALTER TABLE [dbo].[BuildAssemblyComponentItemLine]
ADD CONSTRAINT [PK_BuildAssemblyComponentItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Charge'
ALTER TABLE [dbo].[Charge]
ADD CONSTRAINT [PK_Charge]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ChargeLinkedTxn'
ALTER TABLE [dbo].[ChargeLinkedTxn]
ADD CONSTRAINT [PK_ChargeLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Check'
ALTER TABLE [dbo].[Check]
ADD CONSTRAINT [PK_Check]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CheckApplyCheckToTxn'
ALTER TABLE [dbo].[CheckApplyCheckToTxn]
ADD CONSTRAINT [PK_CheckApplyCheckToTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CheckExpenseLine'
ALTER TABLE [dbo].[CheckExpenseLine]
ADD CONSTRAINT [PK_CheckExpenseLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CheckItemLine'
ALTER TABLE [dbo].[CheckItemLine]
ADD CONSTRAINT [PK_CheckItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Class'
ALTER TABLE [dbo].[Class]
ADD CONSTRAINT [PK_Class]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Company'
ALTER TABLE [dbo].[Company]
ADD CONSTRAINT [PK_Company]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CompanyActivity'
ALTER TABLE [dbo].[CompanyActivity]
ADD CONSTRAINT [PK_CompanyActivity]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditCardCharge'
ALTER TABLE [dbo].[CreditCardCharge]
ADD CONSTRAINT [PK_CreditCardCharge]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditCardChargeExpenseLine'
ALTER TABLE [dbo].[CreditCardChargeExpenseLine]
ADD CONSTRAINT [PK_CreditCardChargeExpenseLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditCardChargeItemLine'
ALTER TABLE [dbo].[CreditCardChargeItemLine]
ADD CONSTRAINT [PK_CreditCardChargeItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditCardCredit'
ALTER TABLE [dbo].[CreditCardCredit]
ADD CONSTRAINT [PK_CreditCardCredit]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditCardCreditExpenseLine'
ALTER TABLE [dbo].[CreditCardCreditExpenseLine]
ADD CONSTRAINT [PK_CreditCardCreditExpenseLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditCardCreditItemLine'
ALTER TABLE [dbo].[CreditCardCreditItemLine]
ADD CONSTRAINT [PK_CreditCardCreditItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditMemo'
ALTER TABLE [dbo].[CreditMemo]
ADD CONSTRAINT [PK_CreditMemo]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditMemoLine'
ALTER TABLE [dbo].[CreditMemoLine]
ADD CONSTRAINT [PK_CreditMemoLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CreditMemoLinkedTxn'
ALTER TABLE [dbo].[CreditMemoLinkedTxn]
ADD CONSTRAINT [PK_CreditMemoLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Currency'
ALTER TABLE [dbo].[Currency]
ADD CONSTRAINT [PK_Currency]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Customer'
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT [PK_Customer]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CustomerMsg'
ALTER TABLE [dbo].[CustomerMsg]
ADD CONSTRAINT [PK_CustomerMsg]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CustomerType'
ALTER TABLE [dbo].[CustomerType]
ADD CONSTRAINT [PK_CustomerType]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'CustomField'
ALTER TABLE [dbo].[CustomField]
ADD CONSTRAINT [PK_CustomField]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'DateDrivenTerms'
ALTER TABLE [dbo].[DateDrivenTerms]
ADD CONSTRAINT [PK_DateDrivenTerms]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Deposit'
ALTER TABLE [dbo].[Deposit]
ADD CONSTRAINT [PK_Deposit]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'DepositLine'
ALTER TABLE [dbo].[DepositLine]
ADD CONSTRAINT [PK_DepositLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [PK_Employee]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'EmployeeEarning'
ALTER TABLE [dbo].[EmployeeEarning]
ADD CONSTRAINT [PK_EmployeeEarning]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Entity'
ALTER TABLE [dbo].[Entity]
ADD CONSTRAINT [PK_Entity]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Estimate'
ALTER TABLE [dbo].[Estimate]
ADD CONSTRAINT [PK_Estimate]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'EstimateLine'
ALTER TABLE [dbo].[EstimateLine]
ADD CONSTRAINT [PK_EstimateLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'EstimateLinkedTxn'
ALTER TABLE [dbo].[EstimateLinkedTxn]
ADD CONSTRAINT [PK_EstimateLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Host'
ALTER TABLE [dbo].[Host]
ADD CONSTRAINT [PK_Host]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'HostMetaData'
ALTER TABLE [dbo].[HostMetaData]
ADD CONSTRAINT [PK_HostMetaData]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'HostSupportedVersions'
ALTER TABLE [dbo].[HostSupportedVersions]
ADD CONSTRAINT [PK_HostSupportedVersions]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'InventoryAdjustment'
ALTER TABLE [dbo].[InventoryAdjustment]
ADD CONSTRAINT [PK_InventoryAdjustment]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'InventoryAdjustmentLine'
ALTER TABLE [dbo].[InventoryAdjustmentLine]
ADD CONSTRAINT [PK_InventoryAdjustmentLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [PK_Invoice]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'InvoiceLine'
ALTER TABLE [dbo].[InvoiceLine]
ADD CONSTRAINT [PK_InvoiceLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'InvoiceLinkedTxn'
ALTER TABLE [dbo].[InvoiceLinkedTxn]
ADD CONSTRAINT [PK_InvoiceLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Item'
ALTER TABLE [dbo].[Item]
ADD CONSTRAINT [PK_Item]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemAssembliesCanBuild'
ALTER TABLE [dbo].[ItemAssembliesCanBuild]
ADD CONSTRAINT [PK_ItemAssembliesCanBuild]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemDiscount'
ALTER TABLE [dbo].[ItemDiscount]
ADD CONSTRAINT [PK_ItemDiscount]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemFixedAsset'
ALTER TABLE [dbo].[ItemFixedAsset]
ADD CONSTRAINT [PK_ItemFixedAsset]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemGroup'
ALTER TABLE [dbo].[ItemGroup]
ADD CONSTRAINT [PK_ItemGroup]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemGroupLine'
ALTER TABLE [dbo].[ItemGroupLine]
ADD CONSTRAINT [PK_ItemGroupLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemInventory'
ALTER TABLE [dbo].[ItemInventory]
ADD CONSTRAINT [PK_ItemInventory]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemInventoryAssembly'
ALTER TABLE [dbo].[ItemInventoryAssembly]
ADD CONSTRAINT [PK_ItemInventoryAssembly]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemInventoryAssemblyLine'
ALTER TABLE [dbo].[ItemInventoryAssemblyLine]
ADD CONSTRAINT [PK_ItemInventoryAssemblyLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemNonInventory'
ALTER TABLE [dbo].[ItemNonInventory]
ADD CONSTRAINT [PK_ItemNonInventory]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemOtherCharge'
ALTER TABLE [dbo].[ItemOtherCharge]
ADD CONSTRAINT [PK_ItemOtherCharge]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemPayment'
ALTER TABLE [dbo].[ItemPayment]
ADD CONSTRAINT [PK_ItemPayment]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemReceipt'
ALTER TABLE [dbo].[ItemReceipt]
ADD CONSTRAINT [PK_ItemReceipt]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemReceiptExpenseLine'
ALTER TABLE [dbo].[ItemReceiptExpenseLine]
ADD CONSTRAINT [PK_ItemReceiptExpenseLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemReceiptItemLine'
ALTER TABLE [dbo].[ItemReceiptItemLine]
ADD CONSTRAINT [PK_ItemReceiptItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemReceiptLinkedTxn'
ALTER TABLE [dbo].[ItemReceiptLinkedTxn]
ADD CONSTRAINT [PK_ItemReceiptLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemSalesTax'
ALTER TABLE [dbo].[ItemSalesTax]
ADD CONSTRAINT [PK_ItemSalesTax]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemSalesTaxGroup'
ALTER TABLE [dbo].[ItemSalesTaxGroup]
ADD CONSTRAINT [PK_ItemSalesTaxGroup]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemSalesTaxGroupLine'
ALTER TABLE [dbo].[ItemSalesTaxGroupLine]
ADD CONSTRAINT [PK_ItemSalesTaxGroupLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemService'
ALTER TABLE [dbo].[ItemService]
ADD CONSTRAINT [PK_ItemService]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ItemSubtotal'
ALTER TABLE [dbo].[ItemSubtotal]
ADD CONSTRAINT [PK_ItemSubtotal]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'JobType'
ALTER TABLE [dbo].[JobType]
ADD CONSTRAINT [PK_JobType]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'JournalEntry'
ALTER TABLE [dbo].[JournalEntry]
ADD CONSTRAINT [PK_JournalEntry]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'JournalEntryCreditLine'
ALTER TABLE [dbo].[JournalEntryCreditLine]
ADD CONSTRAINT [PK_JournalEntryCreditLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'JournalEntryDebitLine'
ALTER TABLE [dbo].[JournalEntryDebitLine]
ADD CONSTRAINT [PK_JournalEntryDebitLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'JournalEntryLine'
ALTER TABLE [dbo].[JournalEntryLine]
ADD CONSTRAINT [PK_JournalEntryLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ListDeleted'
ALTER TABLE [dbo].[ListDeleted]
ADD CONSTRAINT [PK_ListDeleted]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'OtherName'
ALTER TABLE [dbo].[OtherName]
ADD CONSTRAINT [PK_OtherName]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PaymentMethod'
ALTER TABLE [dbo].[PaymentMethod]
ADD CONSTRAINT [PK_PaymentMethod]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PayrollItemNonWage'
ALTER TABLE [dbo].[PayrollItemNonWage]
ADD CONSTRAINT [PK_PayrollItemNonWage]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PayrollItemWage'
ALTER TABLE [dbo].[PayrollItemWage]
ADD CONSTRAINT [PK_PayrollItemWage]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Preferences'
ALTER TABLE [dbo].[Preferences]
ADD CONSTRAINT [PK_Preferences]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PriceLevel'
ALTER TABLE [dbo].[PriceLevel]
ADD CONSTRAINT [PK_PriceLevel]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PriceLevelPerItem'
ALTER TABLE [dbo].[PriceLevelPerItem]
ADD CONSTRAINT [PK_PriceLevelPerItem]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PurchaseOrder'
ALTER TABLE [dbo].[PurchaseOrder]
ADD CONSTRAINT [PK_PurchaseOrder]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PurchaseOrderLine'
ALTER TABLE [dbo].[PurchaseOrderLine]
ADD CONSTRAINT [PK_PurchaseOrderLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'PurchaseOrderLinkedTxn'
ALTER TABLE [dbo].[PurchaseOrderLinkedTxn]
ADD CONSTRAINT [PK_PurchaseOrderLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ReceivePayment'
ALTER TABLE [dbo].[ReceivePayment]
ADD CONSTRAINT [PK_ReceivePayment]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ReceivePaymentLine'
ALTER TABLE [dbo].[ReceivePaymentLine]
ADD CONSTRAINT [PK_ReceivePaymentLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ReceivePaymentToDeposit'
ALTER TABLE [dbo].[ReceivePaymentToDeposit]
ADD CONSTRAINT [PK_ReceivePaymentToDeposit]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Sales'
ALTER TABLE [dbo].[Sales]
ADD CONSTRAINT [PK_Sales]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesLine'
ALTER TABLE [dbo].[SalesLine]
ADD CONSTRAINT [PK_SalesLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesOrder'
ALTER TABLE [dbo].[SalesOrder]
ADD CONSTRAINT [PK_SalesOrder]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesOrderLine'
ALTER TABLE [dbo].[SalesOrderLine]
ADD CONSTRAINT [PK_SalesOrderLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesOrderLinkedTxn'
ALTER TABLE [dbo].[SalesOrderLinkedTxn]
ADD CONSTRAINT [PK_SalesOrderLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesReceipt'
ALTER TABLE [dbo].[SalesReceipt]
ADD CONSTRAINT [PK_SalesReceipt]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesReceiptLine'
ALTER TABLE [dbo].[SalesReceiptLine]
ADD CONSTRAINT [PK_SalesReceiptLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesRep'
ALTER TABLE [dbo].[SalesRep]
ADD CONSTRAINT [PK_SalesRep]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesTaxCode'
ALTER TABLE [dbo].[SalesTaxCode]
ADD CONSTRAINT [PK_SalesTaxCode]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesTaxPaymentCheck'
ALTER TABLE [dbo].[SalesTaxPaymentCheck]
ADD CONSTRAINT [PK_SalesTaxPaymentCheck]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SalesTaxPaymentCheckLine'
ALTER TABLE [dbo].[SalesTaxPaymentCheckLine]
ADD CONSTRAINT [PK_SalesTaxPaymentCheckLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ShipMethod'
ALTER TABLE [dbo].[ShipMethod]
ADD CONSTRAINT [PK_ShipMethod]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SpecialAccount'
ALTER TABLE [dbo].[SpecialAccount]
ADD CONSTRAINT [PK_SpecialAccount]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'SpecialItem'
ALTER TABLE [dbo].[SpecialItem]
ADD CONSTRAINT [PK_SpecialItem]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'StandardTerms'
ALTER TABLE [dbo].[StandardTerms]
ADD CONSTRAINT [PK_StandardTerms]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'TaxCode'
ALTER TABLE [dbo].[TaxCode]
ADD CONSTRAINT [PK_TaxCode]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Template'
ALTER TABLE [dbo].[Template]
ADD CONSTRAINT [PK_Template]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Terms'
ALTER TABLE [dbo].[Terms]
ADD CONSTRAINT [PK_Terms]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'TimeTracking'
ALTER TABLE [dbo].[TimeTracking]
ADD CONSTRAINT [PK_TimeTracking]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'ToDo'
ALTER TABLE [dbo].[ToDo]
ADD CONSTRAINT [PK_ToDo]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Transaction'
ALTER TABLE [dbo].[Transaction]
ADD CONSTRAINT [PK_Transaction]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'TxnDeleted'
ALTER TABLE [dbo].[TxnDeleted]
ADD CONSTRAINT [PK_TxnDeleted]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'UnitOfMeasureSet'
ALTER TABLE [dbo].[UnitOfMeasureSet]
ADD CONSTRAINT [PK_UnitOfMeasureSet]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'UnitOfMeasureSetRelatedUnit'
ALTER TABLE [dbo].[UnitOfMeasureSetRelatedUnit]
ADD CONSTRAINT [PK_UnitOfMeasureSetRelatedUnit]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'UnitOfMeasureSetDefaultUnit'
ALTER TABLE [dbo].[UnitOfMeasureSetDefaultUnit]
ADD CONSTRAINT [PK_UnitOfMeasureSetDefaultUnit]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Vehicle'
ALTER TABLE [dbo].[Vehicle]
ADD CONSTRAINT [PK_Vehicle]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'VehicleMileage'
ALTER TABLE [dbo].[VehicleMileage]
ADD CONSTRAINT [PK_VehicleMileage]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'Vendor'
ALTER TABLE [dbo].[Vendor]
ADD CONSTRAINT [PK_Vendor]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'VendorCredit'
ALTER TABLE [dbo].[VendorCredit]
ADD CONSTRAINT [PK_VendorCredit]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'VendorCreditExpenseLine'
ALTER TABLE [dbo].[VendorCreditExpenseLine]
ADD CONSTRAINT [PK_VendorCreditExpenseLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'VendorCreditItemLine'
ALTER TABLE [dbo].[VendorCreditItemLine]
ADD CONSTRAINT [PK_VendorCreditItemLine]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'VendorCreditLinkedTxn'
ALTER TABLE [dbo].[VendorCreditLinkedTxn]
ADD CONSTRAINT [PK_VendorCreditLinkedTxn]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'VendorType'
ALTER TABLE [dbo].[VendorType]
ADD CONSTRAINT [PK_VendorType]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'WorkersCompCode'
ALTER TABLE [dbo].[WorkersCompCode]
ADD CONSTRAINT [PK_WorkersCompCode]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- Creating primary key on [RowId] in table 'WorkersCompCodeRateHistory'
ALTER TABLE [dbo].[WorkersCompCodeRateHistory]
ADD CONSTRAINT [PK_WorkersCompCodeRateHistory]
    PRIMARY KEY CLUSTERED ([RowId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------