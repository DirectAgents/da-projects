namespace Accounting.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        AccountType = c.String(nullable: false, maxLength: 21),
                        SpecialAccountType = c.String(maxLength: 30),
                        IsTaxAccount = c.Boolean(nullable: false),
                        AccountNumber = c.String(maxLength: 7),
                        BankNumber = c.String(maxLength: 25),
                        Desc = c.String(maxLength: 200),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenBalanceDate = c.DateTime(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        TaxLineInfoRetTaxLineID = c.Int(nullable: false),
                        TaxLineInfoRetTaxLineName = c.String(maxLength: 256),
                        CashFlowClassification = c.String(maxLength: 13),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID });
            
            CreateTable(
                "dbo.SalesTaxCodes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 3),
                        IsActive = c.Boolean(nullable: false),
                        IsTaxable = c.Boolean(nullable: false),
                        Desc = c.String(maxLength: 31),
                        ItemPurchaseTaxRefListID = c.String(maxLength: 36),
                        ItemPurchaseTaxRefFullName = c.String(maxLength: 31),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CompanyId, t.ItemPurchaseTaxRefListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CompanyId, t.ItemSalesTaxRefListID })
                .Index(t => new { t.CompanyId, t.ItemPurchaseTaxRefListID })
                .Index(t => new { t.CompanyId, t.ItemSalesTaxRefListID });
            
            CreateTable(
                "dbo.TaxCodes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 3),
                        IsActive = c.Boolean(nullable: false),
                        IsTaxable = c.Boolean(nullable: false),
                        Desc = c.String(maxLength: 31),
                        ItemPurchaseTaxRefListID = c.String(maxLength: 36),
                        ItemPurchaseTaxRefFullName = c.String(maxLength: 31),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        ItemPurchaseTaxRefSalesTaxCode_CompanyId = c.Int(),
                        ItemPurchaseTaxRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefSalesTaxCode_CompanyId = c.Int(),
                        ItemSalesTaxRefSalesTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemPurchaseTaxRefSalesTaxCode_CompanyId, t.ItemPurchaseTaxRefSalesTaxCode_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemSalesTaxRefSalesTaxCode_CompanyId, t.ItemSalesTaxRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemPurchaseTaxRefSalesTaxCode_CompanyId, t.ItemPurchaseTaxRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemSalesTaxRefSalesTaxCode_CompanyId, t.ItemSalesTaxRefSalesTaxCode_ListID });
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 64),
                        IsActive = c.Boolean(nullable: false),
                        CurrencyCode = c.String(nullable: false, maxLength: 3),
                        CurrencyFormatThousandSeparator = c.String(maxLength: 10),
                        CurrencyFormatThousandSeparatorGrouping = c.String(maxLength: 11),
                        CurrencyFormatDecimalPlaces = c.String(maxLength: 1),
                        CurrencyFormatDecimalSeparator = c.String(maxLength: 6),
                        IsUserDefinedCurrency = c.Boolean(nullable: false),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AsOfDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.AccountTaxLineInfoes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TaxLineID = c.Int(nullable: false),
                        TaxLineName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.ARRefundCreditCards",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        RefundFromAccountRefListID = c.String(maxLength: 36),
                        RefundFromAccountRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        CreditCardTxnInfoInputCreditCardNumber = c.String(maxLength: 25),
                        CreditCardTxnInfoInputExpirationMonth = c.Int(nullable: false),
                        CreditCardTxnInfoInputExpirationYear = c.Int(nullable: false),
                        CreditCardTxnInfoInputNameOnCard = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardAddress = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardPostalCode = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCommercialCardCode = c.String(maxLength: 50),
                        CreditCardTxnInfoInputTransactionMode = c.String(maxLength: 14),
                        CreditCardTxnInfoInputCreditCardTxnType = c.String(maxLength: 18),
                        CreditCardTxnInfoResultResultCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultResultMessage = c.String(maxLength: 500),
                        CreditCardTxnInfoResultCreditCardTransID = c.String(maxLength: 24),
                        CreditCardTxnInfoResultMerchantAccountNumber = c.String(maxLength: 32),
                        CreditCardTxnInfoResultAuthorizationCode = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSStreet = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSZip = c.String(maxLength: 12),
                        CreditCardTxnInfoResultCardSecurityCodeMatch = c.String(maxLength: 12),
                        CreditCardTxnInfoResultReconBatchID = c.String(maxLength: 84),
                        CreditCardTxnInfoResultPaymentGroupingCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultPaymentStatus = c.String(maxLength: 9),
                        CreditCardTxnInfoResultTxnAuthorizationTime = c.DateTime(nullable: false),
                        CreditCardTxnInfoResultTxnAuthorizationStamp = c.Int(nullable: false),
                        CreditCardTxnInfoResultClientTransID = c.String(maxLength: 16),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        RefundFromAccountRefAccount_CompanyId = c.Int(),
                        RefundFromAccountRefAccount_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.RefundFromAccountRefAccount_CompanyId, t.RefundFromAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.RefundFromAccountRefAccount_CompanyId, t.RefundFromAccountRefAccount_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID });
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 41),
                        FullName = c.String(maxLength: 209),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 209),
                        Sublevel = c.Int(nullable: false),
                        CompanyName = c.String(maxLength: 41),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 25),
                        MiddleName = c.String(maxLength: 5),
                        LastName = c.String(maxLength: 25),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        Phone = c.String(maxLength: 21),
                        AltPhone = c.String(maxLength: 21),
                        Fax = c.String(maxLength: 21),
                        Email = c.String(maxLength: 1023),
                        Contact = c.String(maxLength: 41),
                        AltContact = c.String(maxLength: 41),
                        CustomerTypeRefListID = c.String(maxLength: 36),
                        CustomerTypeRefFullName = c.String(maxLength: 159),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenBalanceDate = c.DateTime(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxCountry = c.String(maxLength: 31),
                        ResaleNumber = c.String(maxLength: 15),
                        AccountNumber = c.String(maxLength: 99),
                        BusinessNumber = c.String(maxLength: 99),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreferredPaymentMethodRefListID = c.String(maxLength: 36),
                        PreferredPaymentMethodRefFullName = c.String(maxLength: 31),
                        CreditCardInfoCreditCardNumber = c.String(maxLength: 25),
                        CreditCardInfoExpirationMonth = c.Int(nullable: false),
                        CreditCardInfoExpirationYear = c.Int(nullable: false),
                        CreditCardInfoNameOnCard = c.String(maxLength: 41),
                        CreditCardInfoCreditCardAddress = c.String(maxLength: 41),
                        CreditCardInfoCreditCardPostalCode = c.String(maxLength: 41),
                        JobStatus = c.String(maxLength: 10),
                        JobStartDate = c.DateTime(nullable: false),
                        JobProjectedEndDate = c.DateTime(nullable: false),
                        JobEndDate = c.DateTime(nullable: false),
                        JobDesc = c.String(maxLength: 99),
                        JobTypeRefListID = c.String(maxLength: 36),
                        JobTypeRefFullName = c.String(maxLength: 159),
                        Notes = c.String(),
                        IsUsingCustomerTaxCode = c.Boolean(nullable: false),
                        PriceLevelRefListID = c.String(maxLength: 36),
                        PriceLevelRefFullName = c.String(maxLength: 31),
                        ExternalGUID = c.String(maxLength: 40),
                        TaxRegistrationNumber = c.String(maxLength: 30),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        Entity_CompanyId = c.Int(),
                        Entity_ListID = c.String(maxLength: 36),
                        CustomerTypeRefCustomerType_CompanyId = c.Int(),
                        CustomerTypeRefCustomerType_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        PreferredPaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PreferredPaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        JobTypeRefJobType_CompanyId = c.Int(),
                        JobTypeRefJobType_ListID = c.String(maxLength: 36),
                        PriceLevelRefPriceLevel_CompanyId = c.Int(),
                        PriceLevelRefPriceLevel_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Entities", t => new { t.Entity_CompanyId, t.Entity_ListID })
                .ForeignKey("dbo.CustomerTypes", t => new { t.CustomerTypeRefCustomerType_CompanyId, t.CustomerTypeRefCustomerType_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PreferredPaymentMethodRefPaymentMethod_CompanyId, t.PreferredPaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.JobTypes", t => new { t.JobTypeRefJobType_CompanyId, t.JobTypeRefJobType_ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.PriceLevelRefPriceLevel_CompanyId, t.PriceLevelRefPriceLevel_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.Entity_CompanyId, t.Entity_ListID })
                .Index(t => new { t.CustomerTypeRefCustomerType_CompanyId, t.CustomerTypeRefCustomerType_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.PreferredPaymentMethodRefPaymentMethod_CompanyId, t.PreferredPaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.JobTypeRefJobType_CompanyId, t.JobTypeRefJobType_ListID })
                .Index(t => new { t.PriceLevelRefPriceLevel_CompanyId, t.PriceLevelRefPriceLevel_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID });
            
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        FullName = c.String(maxLength: 209),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 209),
                        Sublevel = c.Int(nullable: false),
                        Type = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.CustomerTypes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        DayOfMonthDue = c.Int(nullable: false),
                        DueNextMonthDays = c.Int(nullable: false),
                        DiscountDayOfMonth = c.Int(nullable: false),
                        DiscountPct = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StdDueDays = c.Int(nullable: false),
                        StdDiscountDays = c.Int(nullable: false),
                        StdDiscountPct = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.SalesReps",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Initial = c.String(nullable: false, maxLength: 5),
                        IsActive = c.Boolean(nullable: false),
                        SalesRepEntityRefListID = c.String(nullable: false, maxLength: 36),
                        SalesRepEntityRefFullName = c.String(nullable: false, maxLength: 209),
                        SalesRepEntityRefEntity_CompanyId = c.Int(),
                        SalesRepEntityRefEntity_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Entities", t => new { t.SalesRepEntityRefEntity_CompanyId, t.SalesRepEntityRefEntity_ListID })
                .Index(t => new { t.SalesRepEntityRefEntity_CompanyId, t.SalesRepEntityRefEntity_ListID });
            
            CreateTable(
                "dbo.ItemSalesTaxes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        IsUsedOnPurchaseTransaction = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        TaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxVendorRefListID = c.String(maxLength: 36),
                        TaxVendorRefFullName = c.String(maxLength: 41),
                        SalesTaxReturnLineRefListID = c.String(maxLength: 36),
                        SalesTaxReturnLineRefFullName = c.String(maxLength: 41),
                        ExternalGUID = c.String(maxLength: 40),
                        TaxVendorRefVendor_CompanyId = c.Int(),
                        TaxVendorRefVendor_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Vendors", t => new { t.TaxVendorRefVendor_CompanyId, t.TaxVendorRefVendor_ListID })
                .Index(t => new { t.TaxVendorRefVendor_CompanyId, t.TaxVendorRefVendor_ListID });
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        IsTaxAgency = c.Boolean(nullable: false),
                        CompanyName = c.String(maxLength: 41),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 25),
                        MiddleName = c.String(maxLength: 5),
                        LastName = c.String(maxLength: 25),
                        VendorAddressAddr1 = c.String(maxLength: 41),
                        VendorAddressAddr2 = c.String(maxLength: 41),
                        VendorAddressAddr3 = c.String(maxLength: 41),
                        VendorAddressAddr4 = c.String(maxLength: 41),
                        VendorAddressAddr5 = c.String(maxLength: 41),
                        VendorAddressCity = c.String(maxLength: 31),
                        VendorAddressState = c.String(maxLength: 21),
                        VendorAddressProvince = c.String(maxLength: 21),
                        VendorAddressCounty = c.String(maxLength: 21),
                        VendorAddressPostalCode = c.String(maxLength: 13),
                        VendorAddressCountry = c.String(maxLength: 31),
                        VendorAddressNote = c.String(maxLength: 41),
                        VendorAddressBlockAddr1 = c.String(maxLength: 41),
                        VendorAddressBlockAddr2 = c.String(maxLength: 41),
                        VendorAddressBlockAddr3 = c.String(maxLength: 41),
                        VendorAddressBlockAddr4 = c.String(maxLength: 41),
                        VendorAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        Phone = c.String(maxLength: 21),
                        AltPhone = c.String(maxLength: 21),
                        Fax = c.String(maxLength: 21),
                        Email = c.String(maxLength: 1023),
                        Contact = c.String(maxLength: 41),
                        AltContact = c.String(maxLength: 41),
                        NameOnCheck = c.String(maxLength: 41),
                        AccountNumber = c.String(maxLength: 99),
                        Notes = c.String(),
                        VendorTypeRefListID = c.String(maxLength: 36),
                        VendorTypeRefFullName = c.String(maxLength: 159),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VendorTaxIdent = c.String(maxLength: 15),
                        IsVendorEligibleFor1099 = c.Boolean(nullable: false),
                        IsVendorEligibleForT4A = c.Boolean(nullable: false),
                        OpenBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenBalanceDate = c.DateTime(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingRateRefListID = c.String(maxLength: 36),
                        BillingRateRefFullName = c.String(maxLength: 31),
                        PrefillAccount1RefListID = c.String(maxLength: 36),
                        PrefillAccount1RefFullName = c.String(maxLength: 31),
                        PrefillAccount2RefListID = c.String(maxLength: 36),
                        PrefillAccount2RefFullName = c.String(maxLength: 31),
                        PrefillAccount3RefListID = c.String(maxLength: 36),
                        PrefillAccount3RefFullName = c.String(maxLength: 31),
                        ExternalGUID = c.String(maxLength: 40),
                        BusinessNumber = c.String(maxLength: 99),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesTaxCountry = c.String(maxLength: 100),
                        IsSalesTaxAgency = c.Boolean(nullable: false),
                        SalesTaxReturnRefListID = c.String(maxLength: 36),
                        SalesTaxReturnRefFullName = c.String(maxLength: 159),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        VendorTypeRefVendorType_CompanyId = c.Int(),
                        VendorTypeRefVendorType_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        BillingRateRefBillingRate_CompanyId = c.Int(),
                        BillingRateRefBillingRate_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesTaxReturnRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxReturnRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.VendorTypes", t => new { t.VendorTypeRefVendorType_CompanyId, t.VendorTypeRefVendorType_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.BillingRates", t => new { t.BillingRateRefBillingRate_CompanyId, t.BillingRateRefBillingRate_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxReturnRefSalesTaxCode_CompanyId, t.SalesTaxReturnRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.VendorTypeRefVendorType_CompanyId, t.VendorTypeRefVendorType_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.BillingRateRefBillingRate_CompanyId, t.BillingRateRefBillingRate_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesTaxReturnRefSalesTaxCode_CompanyId, t.SalesTaxReturnRefSalesTaxCode_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID });
            
            CreateTable(
                "dbo.VendorTypes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.BillingRates",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        BillingRateType = c.String(maxLength: 9),
                        FixedBillingRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        PaymentMethodType = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.JobTypes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 41),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.PriceLevels",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        PriceLevelType = c.String(maxLength: 15),
                        PriceLevelFixedPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.ARRefundCreditCardRefundAppliedToes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        RefundFromAccountRefListID = c.String(maxLength: 36),
                        RefundFromAccountRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        CreditCardTxnInfoInputCreditCardNumber = c.String(maxLength: 25),
                        CreditCardTxnInfoInputExpirationMonth = c.Int(nullable: false),
                        CreditCardTxnInfoInputExpirationYear = c.Int(nullable: false),
                        CreditCardTxnInfoInputNameOnCard = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardAddress = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardPostalCode = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCommercialCardCode = c.String(maxLength: 50),
                        CreditCardTxnInfoInputTransactionMode = c.String(maxLength: 14),
                        CreditCardTxnInfoInputCreditCardTxnType = c.String(maxLength: 18),
                        CreditCardTxnInfoResultResultCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultResultMessage = c.String(maxLength: 500),
                        CreditCardTxnInfoResultCreditCardTransID = c.String(maxLength: 24),
                        CreditCardTxnInfoResultMerchantAccountNumber = c.String(maxLength: 32),
                        CreditCardTxnInfoResultAuthorizationCode = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSStreet = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSZip = c.String(maxLength: 12),
                        CreditCardTxnInfoResultCardSecurityCodeMatch = c.String(maxLength: 12),
                        CreditCardTxnInfoResultReconBatchID = c.String(maxLength: 84),
                        CreditCardTxnInfoResultPaymentGroupingCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultPaymentStatus = c.String(maxLength: 9),
                        CreditCardTxnInfoResultTxnAuthorizationTime = c.DateTime(nullable: false),
                        CreditCardTxnInfoResultTxnAuthorizationStamp = c.Int(nullable: false),
                        CreditCardTxnInfoResultClientTransID = c.String(maxLength: 16),
                        RefundAppliedToTxnTxnID = c.String(nullable: false, maxLength: 36),
                        RefundAppliedToTxnTxnType = c.String(maxLength: 14),
                        RefundAppliedToTxnTxnDate = c.DateTime(nullable: false),
                        RefundAppliedToTxnRefNumber = c.String(maxLength: 20),
                        RefundAppliedToTxnRefCreditRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefundAppliedToTxnRefRefundAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        RefundFromAccountRefAccount_CompanyId = c.Int(),
                        RefundFromAccountRefAccount_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.RefundFromAccountRefAccount_CompanyId, t.RefundFromAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.RefundFromAccountRefAccount_CompanyId, t.RefundFromAccountRefAccount_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID });
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(nullable: false, maxLength: 36),
                        VendorRefFullName = c.String(nullable: false, maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        DueDate = c.DateTime(nullable: false),
                        AmountDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        IsPaid = c.Boolean(nullable: false),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.BillExpenseLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(nullable: false, maxLength: 36),
                        VendorRefFullName = c.String(nullable: false, maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        DueDate = c.DateTime(nullable: false),
                        AmountDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        IsPaid = c.Boolean(nullable: false),
                        ExpenseLineClearExpenseLines = c.Boolean(nullable: false),
                        ExpenseLineSeqNo = c.Int(nullable: false),
                        ExpenseLineTxnLineID = c.String(maxLength: 36),
                        ExpenseLineAccountRefListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefFullName = c.String(maxLength: 159),
                        ExpenseLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineMemo = c.String(),
                        ExpenseLineCustomerRefListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefFullName = c.String(maxLength: 209),
                        ExpenseLineClassRefListID = c.String(maxLength: 36),
                        ExpenseLineClassRefFullName = c.String(maxLength: 159),
                        ExpenseLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineBillableStatus = c.String(maxLength: 13),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefAccount_CompanyId = c.Int(),
                        ExpenseLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefCustomer_CompanyId = c.Int(),
                        ExpenseLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ExpenseLineClassRefClass_CompanyId = c.Int(),
                        ExpenseLineClassRefClass_ListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ExpenseLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .Index(t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.BillItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(nullable: false, maxLength: 36),
                        VendorRefFullName = c.String(nullable: false, maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        DueDate = c.DateTime(nullable: false),
                        AmountDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        IsPaid = c.Boolean(nullable: false),
                        ItemLineType = c.String(maxLength: 11),
                        ItemLineSeqNo = c.Int(nullable: false),
                        ItemGroupLineTxnLineID = c.String(maxLength: 36),
                        ItemGroupLineItemGroupRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemGroupRefFullName = c.String(maxLength: 31),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupUnitOfMeasure = c.String(maxLength: 31),
                        ItemGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemGroupLineTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupSeqNo = c.Int(nullable: false),
                        ItemLineTxnLineID = c.String(maxLength: 36),
                        ItemLineItemRefListID = c.String(maxLength: 36),
                        ItemLineItemRefFullName = c.String(maxLength: 159),
                        ItemLineDesc = c.String(),
                        ItemLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemLineCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineCustomerRefListID = c.String(maxLength: 36),
                        ItemLineCustomerRefFullName = c.String(maxLength: 209),
                        ItemLineClassRefListID = c.String(maxLength: 36),
                        ItemLineClassRefFullName = c.String(maxLength: 159),
                        ItemLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineBillableStatus = c.String(maxLength: 13),
                        ItemLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        ItemLineLinkToTxnTxnID = c.String(maxLength: 36),
                        ItemLineLinkToTxnTxnLineID = c.String(maxLength: 36),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemGroupRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemGroupRefItem_ListID = c.String(maxLength: 36),
                        ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineItemRefItem_CompanyId = c.Int(),
                        ItemLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineCustomerRefCustomer_CompanyId = c.Int(),
                        ItemLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemLineClassRefClass_CompanyId = c.Int(),
                        ItemLineClassRefClass_ListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ItemLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ItemLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        ItemLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemGroupRefItem_CompanyId, t.ItemGroupLineItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemGroupLineItemGroupRefItem_CompanyId, t.ItemGroupLineItemGroupRefItem_ListID })
                .Index(t => new { t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .Index(t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(maxLength: 31),
                        FullName = c.String(maxLength: 209),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 209),
                        Sublevel = c.Int(nullable: false),
                        ManufacturerPartNumber = c.String(maxLength: 31),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        Type = c.String(maxLength: 25),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesOrPurchaseDesc = c.String(),
                        SalesOrPurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchasePricePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchaseAccountRefListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseSalesDesc = c.String(),
                        SalesAndPurchaseSalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseIncomeAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchasePurchaseDesc = c.String(),
                        SalesAndPurchasePurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseExpenseAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchasePrefVendorRefListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefFullName = c.String(maxLength: 41),
                        SalesDesc = c.String(),
                        SalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomeAccountRefListID = c.String(maxLength: 36),
                        IncomeAccountRefFullName = c.String(maxLength: 159),
                        PurchaseDesc = c.String(),
                        PurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        COGSAccountRefListID = c.String(maxLength: 36),
                        COGSAccountRefFullName = c.String(maxLength: 159),
                        PrefVendorRefListID = c.String(maxLength: 36),
                        PrefVendorRefFullName = c.String(maxLength: 41),
                        AssetAccountRefListID = c.String(maxLength: 36),
                        AssetAccountRefFullName = c.String(maxLength: 159),
                        ReorderBuildPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryDate = c.DateTime(nullable: false),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        TaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxVendorRefListID = c.String(maxLength: 36),
                        TaxVendorRefFullName = c.String(maxLength: 41),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefAccount_CompanyId = c.Int(),
                        SalesOrPurchaseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseIncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseExpenseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefVendor_CompanyId = c.Int(),
                        SalesAndPurchasePrefVendorRefVendor_ListID = c.String(maxLength: 36),
                        IncomeAccountRefAccount_CompanyId = c.Int(),
                        IncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        COGSAccountRefAccount_CompanyId = c.Int(),
                        COGSAccountRefAccount_ListID = c.String(maxLength: 36),
                        PrefVendorRefVendor_CompanyId = c.Int(),
                        PrefVendorRefVendor_ListID = c.String(maxLength: 36),
                        AssetAccountRefAccount_CompanyId = c.Int(),
                        AssetAccountRefAccount_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        TaxVendorRefVendor_CompanyId = c.Int(),
                        TaxVendorRefVendor_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.TaxVendorRefVendor_CompanyId, t.TaxVendorRefVendor_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID })
                .Index(t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .Index(t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .Index(t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .Index(t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.TaxVendorRefVendor_CompanyId, t.TaxVendorRefVendor_ListID });
            
            CreateTable(
                "dbo.UnitOfMeasureSets",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        UnitOfMeasureType = c.String(maxLength: 15),
                        BaseUnitName = c.String(maxLength: 31),
                        BaseUnitAbbreviation = c.String(maxLength: 31),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.BillLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(nullable: false, maxLength: 36),
                        VendorRefFullName = c.String(nullable: false, maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        DueDate = c.DateTime(nullable: false),
                        AmountDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        IsPaid = c.Boolean(nullable: false),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 8),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.BillingRateLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        BillingRateType = c.String(maxLength: 9),
                        FixedBillingRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingRateLineItemRefListID = c.String(maxLength: 36),
                        BillingRateLineItemRefFullName = c.String(maxLength: 159),
                        BillingRateLineCustomRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingRateLineCustomRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingRateLineAdjustPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingRateLineAdjustBillingRateRelativeTo = c.String(maxLength: 9),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        BillingRateLineItemRefItem_CompanyId = c.Int(),
                        BillingRateLineItemRefItem_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Items", t => new { t.BillingRateLineItemRefItem_CompanyId, t.BillingRateLineItemRefItem_ListID })
                .Index(t => new { t.BillingRateLineItemRefItem_CompanyId, t.BillingRateLineItemRefItem_ListID });
            
            CreateTable(
                "dbo.BillPaymentChecks",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        PayeeEntityRefListID = c.String(nullable: false, maxLength: 36),
                        PayeeEntityRefFullName = c.String(nullable: false, maxLength: 209),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        BankAccountRefListID = c.String(nullable: false, maxLength: 36),
                        BankAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 159),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        IsToBePrinted = c.Boolean(nullable: false),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        BankAccountRefAccount_CompanyId = c.Int(),
                        BankAccountRefAccount_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID });
            
            CreateTable(
                "dbo.BillPaymentCheckLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        PayeeEntityRefListID = c.String(nullable: false, maxLength: 36),
                        PayeeEntityRefFullName = c.String(nullable: false, maxLength: 209),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        BankAccountRefListID = c.String(nullable: false, maxLength: 36),
                        BankAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 159),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        IsToBePrinted = c.Boolean(nullable: false),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        AppliedToTxnSeqNo = c.Int(nullable: false),
                        AppliedToTxnTxnID = c.String(nullable: false, maxLength: 36),
                        AppliedToTxnPaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnTxnType = c.String(maxLength: 14),
                        AppliedToTxnTxnDate = c.DateTime(nullable: false),
                        AppliedToTxnRefNumber = c.String(maxLength: 20),
                        AppliedToTxnBalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnSetCreditCreditTxnID = c.String(maxLength: 36),
                        AppliedToTxnSetCreditAppliedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnDiscountAccountRefListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountAccountRefFullName = c.String(maxLength: 159),
                        AppliedToTxnDiscountClassRefListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountClassRefFullName = c.String(maxLength: 159),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        BankAccountRefAccount_CompanyId = c.Int(),
                        BankAccountRefAccount_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountAccountRefAccount_CompanyId = c.Int(),
                        AppliedToTxnDiscountAccountRefAccount_ListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountClassRefClass_CompanyId = c.Int(),
                        AppliedToTxnDiscountClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AppliedToTxnDiscountAccountRefAccount_CompanyId, t.AppliedToTxnDiscountAccountRefAccount_ListID })
                .ForeignKey("dbo.Classes", t => new { t.AppliedToTxnDiscountClassRefClass_CompanyId, t.AppliedToTxnDiscountClassRefClass_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.AppliedToTxnDiscountAccountRefAccount_CompanyId, t.AppliedToTxnDiscountAccountRefAccount_ListID })
                .Index(t => new { t.AppliedToTxnDiscountClassRefClass_CompanyId, t.AppliedToTxnDiscountClassRefClass_ListID });
            
            CreateTable(
                "dbo.BillPaymentCreditCards",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        PayeeEntityRefListID = c.String(nullable: false, maxLength: 36),
                        PayeeEntityRefFullName = c.String(nullable: false, maxLength: 209),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        CreditCardAccountRefListID = c.String(nullable: false, maxLength: 36),
                        CreditCardAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        CreditCardAccountRefAccount_CompanyId = c.Int(),
                        CreditCardAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.CreditCardAccountRefAccount_CompanyId, t.CreditCardAccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.CreditCardAccountRefAccount_CompanyId, t.CreditCardAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.BillPaymentCreditCardLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        PayeeEntityRefListID = c.String(nullable: false, maxLength: 36),
                        PayeeEntityRefFullName = c.String(nullable: false, maxLength: 209),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        CreditCardAccountRefListID = c.String(nullable: false, maxLength: 36),
                        CreditCardAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        AppliedToTxnSeqNo = c.Int(nullable: false),
                        AppliedToTxnTxnID = c.String(nullable: false, maxLength: 36),
                        AppliedToTxnPaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnTxnType = c.String(maxLength: 14),
                        AppliedToTxnTxnDate = c.DateTime(nullable: false),
                        AppliedToTxnRefNumber = c.String(maxLength: 20),
                        AppliedToTxnBalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnSetCreditCreditTxnID = c.String(maxLength: 36),
                        AppliedToTxnSetCreditAppliedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnDiscountAccountRefListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountAccountRefFullName = c.String(maxLength: 159),
                        AppliedToTxnDiscountClassRefListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountClassRefFullName = c.String(maxLength: 159),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        CreditCardAccountRefAccount_CompanyId = c.Int(),
                        CreditCardAccountRefAccount_ListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountAccountRefAccount_CompanyId = c.Int(),
                        AppliedToTxnDiscountAccountRefAccount_ListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountClassRefClass_CompanyId = c.Int(),
                        AppliedToTxnDiscountClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.CreditCardAccountRefAccount_CompanyId, t.CreditCardAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AppliedToTxnDiscountAccountRefAccount_CompanyId, t.AppliedToTxnDiscountAccountRefAccount_ListID })
                .ForeignKey("dbo.Classes", t => new { t.AppliedToTxnDiscountClassRefClass_CompanyId, t.AppliedToTxnDiscountClassRefClass_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.CreditCardAccountRefAccount_CompanyId, t.CreditCardAccountRefAccount_ListID })
                .Index(t => new { t.AppliedToTxnDiscountAccountRefAccount_CompanyId, t.AppliedToTxnDiscountAccountRefAccount_ListID })
                .Index(t => new { t.AppliedToTxnDiscountClassRefClass_CompanyId, t.AppliedToTxnDiscountClassRefClass_ListID });
            
            CreateTable(
                "dbo.BillToPays",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 159),
                        DueDateCutoff = c.DateTime(nullable: false),
                        BillToPayTxnID = c.String(maxLength: 36),
                        BillToPayTxnType = c.String(maxLength: 14),
                        BillToPayTxnNumber = c.Int(nullable: false),
                        BillToPayAPAccountRefListID = c.String(maxLength: 36),
                        BillToPayAPAccountRefFullName = c.String(maxLength: 159),
                        BillToPayTxnDate = c.DateTime(nullable: false),
                        BillToPayRefNumber = c.String(maxLength: 20),
                        BillToPayDueDate = c.DateTime(nullable: false),
                        BillToPayAmountDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditToApplyTxnID = c.String(maxLength: 36),
                        CreditToApplyTxnType = c.String(maxLength: 14),
                        CreditToApplyTxnNumber = c.Int(nullable: false),
                        CreditToApplyAPAccountRefListID = c.String(maxLength: 36),
                        CreditToApplyAPAccountRefFullName = c.String(maxLength: 159),
                        CreditToApplyTxnDate = c.DateTime(nullable: false),
                        CreditToApplyRefNumber = c.String(maxLength: 20),
                        CreditToApplyCreditRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        BillToPayAPAccountRefAccount_CompanyId = c.Int(),
                        BillToPayAPAccountRefAccount_ListID = c.String(maxLength: 36),
                        CreditToApplyAPAccountRefAccount_CompanyId = c.Int(),
                        CreditToApplyAPAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.BillToPayAPAccountRefAccount_CompanyId, t.BillToPayAPAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.CreditToApplyAPAccountRefAccount_CompanyId, t.CreditToApplyAPAccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.BillToPayAPAccountRefAccount_CompanyId, t.BillToPayAPAccountRefAccount_ListID })
                .Index(t => new { t.CreditToApplyAPAccountRefAccount_CompanyId, t.CreditToApplyAPAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.BuildAssemblies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        ItemInventoryAssemblyRefListID = c.String(nullable: false, maxLength: 36),
                        ItemInventoryAssemblyRefFullName = c.String(nullable: false, maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        IsPending = c.Boolean(nullable: false),
                        QuantityToBuild = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityCanBuild = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnSalesOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MarkPendingIfRequired = c.Boolean(nullable: false),
                        RemovePending = c.Boolean(nullable: false),
                        ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId = c.Int(),
                        ItemInventoryAssemblyRefItemInventoryAssembly_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.ItemInventoryAssemblies", t => new { t.ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId, t.ItemInventoryAssemblyRefItemInventoryAssembly_ListID })
                .Index(t => new { t.ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId, t.ItemInventoryAssemblyRefItemInventoryAssembly_ListID });
            
            CreateTable(
                "dbo.ItemInventoryAssemblies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesDesc = c.String(),
                        SalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomeAccountRefListID = c.String(nullable: false, maxLength: 36),
                        IncomeAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        ApplyIncomeAccountRefToExistingTxns = c.Boolean(nullable: false),
                        PurchaseDesc = c.String(),
                        PurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        COGSAccountRefListID = c.String(nullable: false, maxLength: 36),
                        COGSAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PrefVendorRefListID = c.String(maxLength: 36),
                        PrefVendorRefFullName = c.String(maxLength: 41),
                        AssetAccountRefListID = c.String(nullable: false, maxLength: 36),
                        AssetAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        BuildPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryDate = c.DateTime(nullable: false),
                        AverageCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnSalesOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExternalGUID = c.String(maxLength: 40),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        IncomeAccountRefAccount_CompanyId = c.Int(),
                        IncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        COGSAccountRefAccount_CompanyId = c.Int(),
                        COGSAccountRefAccount_ListID = c.String(maxLength: 36),
                        PrefVendorRefVendor_CompanyId = c.Int(),
                        PrefVendorRefVendor_ListID = c.String(maxLength: 36),
                        AssetAccountRefAccount_CompanyId = c.Int(),
                        AssetAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .Index(t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .Index(t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .Index(t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.BuildAssemblyComponentItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        ItemInventoryAssemblyRefListID = c.String(nullable: false, maxLength: 36),
                        ItemInventoryAssemblyRefFullName = c.String(nullable: false, maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        IsPending = c.Boolean(nullable: false),
                        QuantityToBuild = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityCanBuild = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnSalesOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MarkPendingIfRequired = c.Boolean(nullable: false),
                        RemovePending = c.Boolean(nullable: false),
                        ComponentItemLineSeqNo = c.Int(nullable: false),
                        ComponentItemLineItemRefListID = c.String(maxLength: 36),
                        ComponentItemLineItemRefFullName = c.String(maxLength: 159),
                        ComponentItemLineDesc = c.String(),
                        ComponentItemLineQuantityOnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ComponentItemLineQuantityNeeded = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId = c.Int(),
                        ItemInventoryAssemblyRefItemInventoryAssembly_ListID = c.String(maxLength: 36),
                        ComponentItemLineItemRefItem_CompanyId = c.Int(),
                        ComponentItemLineItemRefItem_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.ItemInventoryAssemblies", t => new { t.ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId, t.ItemInventoryAssemblyRefItemInventoryAssembly_ListID })
                .ForeignKey("dbo.Items", t => new { t.ComponentItemLineItemRefItem_CompanyId, t.ComponentItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId, t.ItemInventoryAssemblyRefItemInventoryAssembly_ListID })
                .Index(t => new { t.ComponentItemLineItemRefItem_CompanyId, t.ComponentItemLineItemRefItem_ListID });
            
            CreateTable(
                "dbo.Charges",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        ItemRefListID = c.String(nullable: false, maxLength: 36),
                        ItemRefFullName = c.String(nullable: false, maxLength: 159),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.String(maxLength: 31),
                        OverrideUOMSetRefListID = c.String(maxLength: 36),
                        OverrideUOMSetRefFullName = c.String(maxLength: 31),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Desc = c.String(),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        BilledDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        OverrideItemAccountRefListID = c.String(maxLength: 36),
                        OverrideItemAccountRefFullName = c.String(maxLength: 159),
                        IsPaid = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemRefItem_CompanyId = c.Int(),
                        ItemRefItem_ListID = c.String(maxLength: 36),
                        OverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        OverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        OverrideItemAccountRefAccount_CompanyId = c.Int(),
                        OverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemRefItem_CompanyId, t.ItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.OverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.OverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.OverrideItemAccountRefAccount_CompanyId, t.OverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ItemRefItem_CompanyId, t.ItemRefItem_ListID })
                .Index(t => new { t.OverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.OverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.OverrideItemAccountRefAccount_CompanyId, t.OverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ChargeLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        ItemRefListID = c.String(nullable: false, maxLength: 36),
                        ItemRefFullName = c.String(nullable: false, maxLength: 159),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitOfMeasure = c.String(maxLength: 31),
                        OverrideUOMSetRefListID = c.String(maxLength: 36),
                        OverrideUOMSetRefFullName = c.String(maxLength: 31),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Desc = c.String(),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        BilledDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        OverrideItemAccountRefListID = c.String(maxLength: 36),
                        OverrideItemAccountRefFullName = c.String(maxLength: 159),
                        IsPaid = c.Boolean(nullable: false),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 8),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemRefItem_CompanyId = c.Int(),
                        ItemRefItem_ListID = c.String(maxLength: 36),
                        OverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        OverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        OverrideItemAccountRefAccount_CompanyId = c.Int(),
                        OverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemRefItem_CompanyId, t.ItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.OverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.OverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.OverrideItemAccountRefAccount_CompanyId, t.OverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ItemRefItem_CompanyId, t.ItemRefItem_ListID })
                .Index(t => new { t.OverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.OverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.OverrideItemAccountRefAccount_CompanyId, t.OverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        RefNumber = c.String(maxLength: 11),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CheckApplyCheckToTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        RefNumber = c.String(maxLength: 11),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ApplyCheckToTxnSeqNo = c.Int(nullable: false),
                        ApplyCheckToTxnTxnID = c.String(maxLength: 36),
                        ApplyCheckToTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CheckExpenseLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        RefNumber = c.String(maxLength: 11),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineClearExpenseLines = c.Boolean(nullable: false),
                        ExpenseLineSeqNo = c.Int(nullable: false),
                        ExpenseLineTxnLineID = c.String(maxLength: 36),
                        ExpenseLineAccountRefListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefFullName = c.String(maxLength: 159),
                        ExpenseLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineMemo = c.String(),
                        ExpenseLineCustomerRefListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefFullName = c.String(maxLength: 209),
                        ExpenseLineClassRefListID = c.String(maxLength: 36),
                        ExpenseLineClassRefFullName = c.String(maxLength: 159),
                        ExpenseLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineBillableStatus = c.String(maxLength: 13),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefAccount_CompanyId = c.Int(),
                        ExpenseLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefCustomer_CompanyId = c.Int(),
                        ExpenseLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ExpenseLineClassRefClass_CompanyId = c.Int(),
                        ExpenseLineClassRefClass_ListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ExpenseLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .Index(t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CheckItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        RefNumber = c.String(maxLength: 11),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineClearItemLines = c.Boolean(nullable: false),
                        ItemLineType = c.String(maxLength: 11),
                        ItemLineSeqNo = c.Int(nullable: false),
                        ItemGroupLineTxnLineID = c.String(maxLength: 36),
                        ItemGroupLineItemRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefFullName = c.String(maxLength: 31),
                        ItemGroupLineDesc = c.String(),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemGroupLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemGroupLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemGroupLineTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupSeqNo = c.Int(nullable: false),
                        ItemLineTxnLineID = c.String(maxLength: 36),
                        ItemLineItemRefListID = c.String(nullable: false, maxLength: 36),
                        ItemLineItemRefFullName = c.String(nullable: false, maxLength: 159),
                        ItemLineDesc = c.String(),
                        ItemLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemLineCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineCustomerRefListID = c.String(maxLength: 36),
                        ItemLineCustomerRefFullName = c.String(maxLength: 209),
                        ItemLineClassRefListID = c.String(maxLength: 36),
                        ItemLineClassRefFullName = c.String(maxLength: 159),
                        ItemLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineBillableStatus = c.String(maxLength: 13),
                        ItemLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineItemRefItem_CompanyId = c.Int(),
                        ItemLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineCustomerRefCustomer_CompanyId = c.Int(),
                        ItemLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemLineClassRefClass_CompanyId = c.Int(),
                        ItemLineClassRefClass_ListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ItemLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ItemLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        ItemLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .Index(t => new { t.ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .Index(t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ClearedStatus",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TxnLineID = c.String(maxLength: 36),
                        ClearedStatusString = c.String(nullable: false, maxLength: 10),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        IsSampleCompany = c.Boolean(nullable: false),
                        CompanyName = c.String(maxLength: 59),
                        LegalCompanyName = c.String(maxLength: 59),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        LegalAddressAddr1 = c.String(maxLength: 41),
                        LegalAddressAddr2 = c.String(maxLength: 41),
                        LegalAddressAddr3 = c.String(maxLength: 41),
                        LegalAddressAddr4 = c.String(maxLength: 41),
                        LegalAddressAddr5 = c.String(maxLength: 41),
                        LegalAddressCity = c.String(maxLength: 31),
                        LegalAddressState = c.String(maxLength: 21),
                        LegalAddressProvince = c.String(maxLength: 21),
                        LegalAddressCounty = c.String(maxLength: 21),
                        LegalAddressPostalCode = c.String(maxLength: 13),
                        LegalAddressCountry = c.String(maxLength: 31),
                        LegalAddressNote = c.String(maxLength: 41),
                        CompanyAddressForCustomerAddr1 = c.String(maxLength: 41),
                        CompanyAddressForCustomerAddr2 = c.String(maxLength: 41),
                        CompanyAddressForCustomerAddr3 = c.String(maxLength: 41),
                        CompanyAddressForCustomerAddr4 = c.String(maxLength: 41),
                        CompanyAddressForCustomerAddr5 = c.String(maxLength: 41),
                        CompanyAddressForCustomerCity = c.String(maxLength: 31),
                        CompanyAddressForCustomerState = c.String(maxLength: 21),
                        CompanyAddressForCustomerProvince = c.String(maxLength: 21),
                        CompanyAddressForCustomerCounty = c.String(maxLength: 21),
                        CompanyAddressForCustomerPostalCode = c.String(maxLength: 13),
                        CompanyAddressForCustomerCountry = c.String(maxLength: 31),
                        CompanyAddressForCustomerNote = c.String(maxLength: 41),
                        CompanyAddressBlockForCustomerAddr1 = c.String(maxLength: 41),
                        CompanyAddressBlockForCustomerAddr2 = c.String(maxLength: 41),
                        CompanyAddressBlockForCustomerAddr3 = c.String(maxLength: 41),
                        CompanyAddressBlockForCustomerAddr4 = c.String(maxLength: 41),
                        CompanyAddressBlockForCustomerAddr5 = c.String(maxLength: 41),
                        Phone = c.String(maxLength: 51),
                        Fax = c.String(maxLength: 51),
                        Email = c.String(maxLength: 99),
                        CompanyWebSite = c.String(maxLength: 128),
                        FirstMonthFiscalYear = c.String(maxLength: 9),
                        FirstMonthIncomeTaxYear = c.String(maxLength: 9),
                        CompanyType = c.String(maxLength: 255),
                        EIN = c.String(maxLength: 20),
                        SSN = c.String(maxLength: 11),
                        TaxForm = c.String(maxLength: 11),
                        BusinessNumber = c.String(maxLength: 16),
                        SubscribedServicesServiceName = c.String(maxLength: 50),
                        SubscribedServicesServiceDomain = c.String(maxLength: 50),
                        SubscribedServicesServiceServiceStatus = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.CompanyActivities",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        LastRestoreTime = c.DateTime(nullable: false),
                        LastCondenseTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.CreditCardCharges",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CreditCardChargeExpenseLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineClearExpenseLines = c.Boolean(nullable: false),
                        ExpenseLineSeqNo = c.Int(nullable: false),
                        ExpenseLineTxnLineID = c.String(maxLength: 36),
                        ExpenseLineAccountRefListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefFullName = c.String(maxLength: 159),
                        ExpenseLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineMemo = c.String(),
                        ExpenseLineCustomerRefListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefFullName = c.String(maxLength: 209),
                        ExpenseLineClassRefListID = c.String(maxLength: 36),
                        ExpenseLineClassRefFullName = c.String(maxLength: 159),
                        ExpenseLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineBillableStatus = c.String(maxLength: 13),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefAccount_CompanyId = c.Int(),
                        ExpenseLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefCustomer_CompanyId = c.Int(),
                        ExpenseLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ExpenseLineClassRefClass_CompanyId = c.Int(),
                        ExpenseLineClassRefClass_ListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ExpenseLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .Index(t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CreditCardChargeItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineClearItemLines = c.Boolean(nullable: false),
                        ItemLineType = c.String(maxLength: 11),
                        ItemLineSeqNo = c.Int(nullable: false),
                        ItemGroupLineTxnLineID = c.String(maxLength: 36),
                        ItemGroupLineItemRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefFullName = c.String(maxLength: 31),
                        ItemGroupLineDesc = c.String(),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemGroupLineTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupSeqNo = c.Int(nullable: false),
                        ItemLineTxnLineID = c.String(maxLength: 36),
                        ItemLineItemRefListID = c.String(nullable: false, maxLength: 36),
                        ItemLineItemRefFullName = c.String(nullable: false, maxLength: 159),
                        ItemLineDesc = c.String(),
                        ItemLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemLineCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineCustomerRefListID = c.String(maxLength: 36),
                        ItemLineCustomerRefFullName = c.String(maxLength: 209),
                        ItemLineClassRefListID = c.String(maxLength: 36),
                        ItemLineClassRefFullName = c.String(maxLength: 159),
                        ItemLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineBillableStatus = c.String(maxLength: 13),
                        ItemLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineItemRefItem_CompanyId = c.Int(),
                        ItemLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineCustomerRefCustomer_CompanyId = c.Int(),
                        ItemLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemLineClassRefClass_CompanyId = c.Int(),
                        ItemLineClassRefClass_ListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ItemLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ItemLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        ItemLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .Index(t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.CreditCardCredits",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID });
            
            CreateTable(
                "dbo.CreditCardCreditExpenseLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        ExpenseLineClearExpenseLines = c.Boolean(nullable: false),
                        ExpenseLineSeqNo = c.Int(nullable: false),
                        ExpenseLineTxnLineID = c.String(maxLength: 36),
                        ExpenseLineAccountRefListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefFullName = c.String(maxLength: 159),
                        ExpenseLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineMemo = c.String(),
                        ExpenseLineCustomerRefListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefFullName = c.String(maxLength: 209),
                        ExpenseLineClassRefListID = c.String(maxLength: 36),
                        ExpenseLineClassRefFullName = c.String(maxLength: 159),
                        ExpenseLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineBillableStatus = c.String(maxLength: 13),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefAccount_CompanyId = c.Int(),
                        ExpenseLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefCustomer_CompanyId = c.Int(),
                        ExpenseLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ExpenseLineClassRefClass_CompanyId = c.Int(),
                        ExpenseLineClassRefClass_ListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ExpenseLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .Index(t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CreditCardCreditItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        ItemLineClearItemLines = c.Boolean(nullable: false),
                        ItemLineType = c.String(maxLength: 11),
                        ItemLineSeqNo = c.Int(nullable: false),
                        ItemGroupLineTxnLineID = c.String(maxLength: 36),
                        ItemGroupLineItemRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefFullName = c.String(maxLength: 31),
                        ItemGroupLineDesc = c.String(),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemGroupLineTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupSeqNo = c.Int(nullable: false),
                        ItemLineTxnLineID = c.String(maxLength: 36),
                        ItemLineItemRefListID = c.String(nullable: false, maxLength: 36),
                        ItemLineItemRefFullName = c.String(nullable: false, maxLength: 159),
                        ItemLineDesc = c.String(),
                        ItemLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemLineCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineCustomerRefListID = c.String(maxLength: 36),
                        ItemLineCustomerRefFullName = c.String(maxLength: 209),
                        ItemLineClassRefListID = c.String(maxLength: 36),
                        ItemLineClassRefFullName = c.String(maxLength: 159),
                        ItemLineBillableStatus = c.String(maxLength: 13),
                        ItemLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineItemRefItem_CompanyId = c.Int(),
                        ItemLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineCustomerRefCustomer_CompanyId = c.Int(),
                        ItemLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemLineClassRefClass_CompanyId = c.Int(),
                        ItemLineClassRefClass_ListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        ItemLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .Index(t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.CreditMemoes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 101),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        TemplateType = c.String(maxLength: 13),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.ShipMethods",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 15),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.CustomerMsgs",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 101),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.CreditMemoLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 101),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        CreditMemoLineType = c.String(maxLength: 11),
                        CreditMemoLineSeqNo = c.Int(nullable: false),
                        CreditMemoLineGroupLineTxnLineID = c.String(maxLength: 36),
                        CreditMemoLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        CreditMemoLineGroupItemGroupRefFullName = c.String(maxLength: 159),
                        CreditMemoLineGroupDesc = c.String(),
                        CreditMemoLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        CreditMemoLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        CreditMemoLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        CreditMemoLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        CreditMemoLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLineGroupServiceDate = c.DateTime(nullable: false),
                        CreditMemoLineGroupSeqNo = c.Int(nullable: false),
                        CreditMemoLineTxnLineID = c.String(maxLength: 36),
                        CreditMemoLineItemRefListID = c.String(maxLength: 36),
                        CreditMemoLineItemRefFullName = c.String(maxLength: 159),
                        CreditMemoLineDesc = c.String(),
                        CreditMemoLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLineUnitOfMeasure = c.String(maxLength: 31),
                        CreditMemoLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        CreditMemoLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        CreditMemoLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLineRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLinePriceLevelRefListID = c.String(maxLength: 36),
                        CreditMemoLinePriceLevelRefFullName = c.String(maxLength: 159),
                        CreditMemoLineClassRefListID = c.String(maxLength: 36),
                        CreditMemoLineClassRefFullName = c.String(maxLength: 159),
                        CreditMemoLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditMemoLineServiceDate = c.DateTime(nullable: false),
                        CreditMemoLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CreditMemoLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CreditMemoLineTaxCodeRefListID = c.String(maxLength: 36),
                        CreditMemoLineTaxCodeRefFullName = c.String(maxLength: 3),
                        CreditMemoLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        CreditMemoLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        CustomFieldCreditMemoLineOther1 = c.String(maxLength: 29),
                        CustomFieldCreditMemoLineOther2 = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        CreditMemoLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        CreditMemoLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        CreditMemoLineItemRefItem_CompanyId = c.Int(),
                        CreditMemoLineItemRefItem_ListID = c.String(maxLength: 36),
                        CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        CreditMemoLinePriceLevelRefPriceLevel_CompanyId = c.Int(),
                        CreditMemoLinePriceLevelRefPriceLevel_ListID = c.String(maxLength: 36),
                        CreditMemoLineClassRefClass_CompanyId = c.Int(),
                        CreditMemoLineClassRefClass_ListID = c.String(maxLength: 36),
                        CreditMemoLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CreditMemoLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CreditMemoLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CreditMemoLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        CreditMemoLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        CreditMemoLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.CreditMemoLineGroupItemGroupRefItem_CompanyId, t.CreditMemoLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.CreditMemoLineItemRefItem_CompanyId, t.CreditMemoLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.CreditMemoLinePriceLevelRefPriceLevel_CompanyId, t.CreditMemoLinePriceLevelRefPriceLevel_ListID })
                .ForeignKey("dbo.Classes", t => new { t.CreditMemoLineClassRefClass_CompanyId, t.CreditMemoLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CreditMemoLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.CreditMemoLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CreditMemoLineTaxCodeRefTaxCode_CompanyId, t.CreditMemoLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.CreditMemoLineOverrideItemAccountRefAccount_CompanyId, t.CreditMemoLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CreditMemoLineGroupItemGroupRefItem_CompanyId, t.CreditMemoLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.CreditMemoLineItemRefItem_CompanyId, t.CreditMemoLineItemRefItem_ListID })
                .Index(t => new { t.CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.CreditMemoLinePriceLevelRefPriceLevel_CompanyId, t.CreditMemoLinePriceLevelRefPriceLevel_ListID })
                .Index(t => new { t.CreditMemoLineClassRefClass_CompanyId, t.CreditMemoLineClassRefClass_ListID })
                .Index(t => new { t.CreditMemoLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.CreditMemoLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CreditMemoLineTaxCodeRefTaxCode_CompanyId, t.CreditMemoLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CreditMemoLineOverrideItemAccountRefAccount_CompanyId, t.CreditMemoLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.CreditMemoLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 101),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.CustomFields",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        OwnerID = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 31),
                        EntityType = c.String(maxLength: 23),
                        EntityRefListID = c.String(maxLength: 36),
                        EntityRefFullName = c.String(maxLength: 159),
                        TxnType = c.String(maxLength: 23),
                        TxnID = c.String(maxLength: 36),
                        TxnLineID = c.String(maxLength: 36),
                        OtherType = c.String(maxLength: 7),
                        Value = c.String(),
                        EntityRefEntity_CompanyId = c.Int(),
                        EntityRefEntity_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.EntityRefEntity_CompanyId, t.EntityRefEntity_ListID })
                .Index(t => new { t.EntityRefEntity_CompanyId, t.EntityRefEntity_ListID });
            
            CreateTable(
                "dbo.DateDrivenTerms",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        DayOfMonthDue = c.Int(nullable: false),
                        DueNextMonthDays = c.Int(nullable: false),
                        DiscountDayOfMonth = c.Int(nullable: false),
                        DiscountPct = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Terms_CompanyId = c.Int(),
                        Terms_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Terms", t => new { t.Terms_CompanyId, t.Terms_ListID })
                .Index(t => new { t.Terms_CompanyId, t.Terms_ListID });
            
            CreateTable(
                "dbo.Deposits",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        DepositToAccountRefListID = c.String(nullable: false, maxLength: 36),
                        DepositToAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        Memo = c.String(),
                        DepositTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CashBackInfoTxnLineID = c.String(maxLength: 36),
                        CashBackInfoAccountRefListID = c.String(maxLength: 36),
                        CashBackInfoAccountRefFullName = c.String(maxLength: 159),
                        CashBackInfoMemo = c.String(),
                        CashBackInfoAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        CashBackInfoAccountRefAccount_CompanyId = c.Int(),
                        CashBackInfoAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.CashBackInfoAccountRefAccount_CompanyId, t.CashBackInfoAccountRefAccount_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.CashBackInfoAccountRefAccount_CompanyId, t.CashBackInfoAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.DepositLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        DepositToAccountRefListID = c.String(nullable: false, maxLength: 36),
                        DepositToAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        Memo = c.String(),
                        DepositTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CashBackInfoTxnLineID = c.String(maxLength: 36),
                        CashBackInfoAccountRefListID = c.String(maxLength: 36),
                        CashBackInfoAccountRefFullName = c.String(maxLength: 159),
                        CashBackInfoMemo = c.String(),
                        CashBackInfoAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositLineSeqNo = c.Int(nullable: false),
                        DepositLineTxnType = c.String(maxLength: 22),
                        DepositLineTxnID = c.String(maxLength: 36),
                        DepositLineTxnLineID = c.String(maxLength: 36),
                        DepositLinePaymentTxnID = c.String(maxLength: 36),
                        DepositLinePaymentTxnLineID = c.String(maxLength: 36),
                        DepositLineEntityRefListID = c.String(maxLength: 36),
                        DepositLineEntityRefFullName = c.String(maxLength: 209),
                        DepositLineAccountRefListID = c.String(nullable: false, maxLength: 36),
                        DepositLineAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        DepositLineMemo = c.String(),
                        DepositLineCheckNumber = c.String(maxLength: 25),
                        DepositLinePaymentMethodRefListID = c.String(maxLength: 36),
                        DepositLinePaymentMethodRefFullName = c.String(maxLength: 31),
                        DepositLineClassRefListID = c.String(maxLength: 36),
                        DepositLineClassRefFullName = c.String(maxLength: 159),
                        DepositLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        CashBackInfoAccountRefAccount_CompanyId = c.Int(),
                        CashBackInfoAccountRefAccount_ListID = c.String(maxLength: 36),
                        DepositLineEntityRefEntity_CompanyId = c.Int(),
                        DepositLineEntityRefEntity_ListID = c.String(maxLength: 36),
                        DepositLineAccountRefAccount_CompanyId = c.Int(),
                        DepositLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        DepositLinePaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        DepositLinePaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        DepositLineClassRefClass_CompanyId = c.Int(),
                        DepositLineClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.CashBackInfoAccountRefAccount_CompanyId, t.CashBackInfoAccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.DepositLineEntityRefEntity_CompanyId, t.DepositLineEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositLineAccountRefAccount_CompanyId, t.DepositLineAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.DepositLinePaymentMethodRefPaymentMethod_CompanyId, t.DepositLinePaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.Classes", t => new { t.DepositLineClassRefClass_CompanyId, t.DepositLineClassRefClass_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.CashBackInfoAccountRefAccount_CompanyId, t.CashBackInfoAccountRefAccount_ListID })
                .Index(t => new { t.DepositLineEntityRefEntity_CompanyId, t.DepositLineEntityRefEntity_ListID })
                .Index(t => new { t.DepositLineAccountRefAccount_CompanyId, t.DepositLineAccountRefAccount_ListID })
                .Index(t => new { t.DepositLinePaymentMethodRefPaymentMethod_CompanyId, t.DepositLinePaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.DepositLineClassRefClass_CompanyId, t.DepositLineClassRefClass_ListID });
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 25),
                        MiddleName = c.String(maxLength: 5),
                        LastName = c.String(maxLength: 25),
                        EmployeeAddressAddr1 = c.String(maxLength: 41),
                        EmployeeAddressAddr2 = c.String(maxLength: 41),
                        EmployeeAddressCity = c.String(maxLength: 31),
                        EmployeeAddressState = c.String(maxLength: 21),
                        EmployeeAddressProvince = c.String(maxLength: 21),
                        EmployeeAddressCounty = c.String(maxLength: 21),
                        EmployeeAddressPostalCode = c.String(maxLength: 13),
                        PrintAs = c.String(maxLength: 41),
                        Phone = c.String(maxLength: 21),
                        Mobile = c.String(maxLength: 21),
                        Pager = c.String(maxLength: 21),
                        PagerPIN = c.String(maxLength: 10),
                        AltPhone = c.String(maxLength: 21),
                        Fax = c.String(maxLength: 21),
                        SSN = c.String(maxLength: 11),
                        SIN = c.String(maxLength: 11),
                        NiNumber = c.String(maxLength: 11),
                        MaritalStatus = c.String(maxLength: 6),
                        Email = c.String(maxLength: 1023),
                        EmployeeType = c.String(maxLength: 9),
                        Gender = c.String(maxLength: 6),
                        Sex = c.String(maxLength: 6),
                        HiredDate = c.DateTime(nullable: false),
                        ReleasedDate = c.DateTime(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        AccountNumber = c.String(maxLength: 99),
                        Notes = c.String(),
                        BillingRateRefListID = c.String(maxLength: 36),
                        BillingRateRefFullName = c.String(maxLength: 31),
                        PayrollInfoPayPeriod = c.String(maxLength: 11),
                        PayrollInfoClassRefListID = c.String(maxLength: 36),
                        PayrollInfoClassRefFullName = c.String(maxLength: 159),
                        PayrollInfoUseTimeDataToCreatePaychecks = c.String(maxLength: 16),
                        PayrollInfoSickHoursHoursAvailable = c.Double(nullable: false),
                        PayrollInfoSickHoursAccrualPeriod = c.String(maxLength: 19),
                        PayrollInfoSickHoursHoursAccrued = c.Double(nullable: false),
                        PayrollInfoSickHoursMaximumHours = c.Double(nullable: false),
                        PayrollInfoSickHoursIsResettingHoursEachNewYear = c.Boolean(nullable: false),
                        PayrollInfoSickHoursHoursUsed = c.Double(nullable: false),
                        PayrollInfoSickHoursAccrualStartDate = c.DateTime(nullable: false),
                        PayrollInfoVacationHoursHoursAvailable = c.Double(nullable: false),
                        PayrollInfoVacationHoursAccrualPeriod = c.String(maxLength: 19),
                        PayrollInfoVacationHoursHoursAccrued = c.Double(nullable: false),
                        PayrollInfoVacationHoursMaximumHours = c.Double(nullable: false),
                        PayrollInfoVacationHoursIsResetHoursEachNewYear = c.Boolean(nullable: false),
                        PayrollInfoVacationHoursHoursUsed = c.Double(nullable: false),
                        PayrollInfoVacationHoursAccrualStartDate = c.DateTime(nullable: false),
                        ExternalGUID = c.String(maxLength: 40),
                        Entity_CompanyId = c.Int(),
                        Entity_ListID = c.String(maxLength: 36),
                        BillingRateRefBillingRate_CompanyId = c.Int(),
                        BillingRateRefBillingRate_ListID = c.String(maxLength: 36),
                        PayrollInfoClassRefClass_CompanyId = c.Int(),
                        PayrollInfoClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Entities", t => new { t.Entity_CompanyId, t.Entity_ListID })
                .ForeignKey("dbo.BillingRates", t => new { t.BillingRateRefBillingRate_CompanyId, t.BillingRateRefBillingRate_ListID })
                .ForeignKey("dbo.Classes", t => new { t.PayrollInfoClassRefClass_CompanyId, t.PayrollInfoClassRefClass_ListID })
                .Index(t => new { t.Entity_CompanyId, t.Entity_ListID })
                .Index(t => new { t.BillingRateRefBillingRate_CompanyId, t.BillingRateRefBillingRate_ListID })
                .Index(t => new { t.PayrollInfoClassRefClass_CompanyId, t.PayrollInfoClassRefClass_ListID });
            
            CreateTable(
                "dbo.EmployeeEarnings",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 25),
                        MiddleName = c.String(maxLength: 5),
                        LastName = c.String(maxLength: 25),
                        EmployeeAddressAddr1 = c.String(maxLength: 41),
                        EmployeeAddressAddr2 = c.String(maxLength: 41),
                        EmployeeAddressCity = c.String(maxLength: 31),
                        EmployeeAddressState = c.String(maxLength: 21),
                        EmployeeAddressProvince = c.String(maxLength: 21),
                        EmployeeAddressCounty = c.String(maxLength: 21),
                        EmployeeAddressPostalCode = c.String(maxLength: 13),
                        PrintAs = c.String(maxLength: 41),
                        Phone = c.String(maxLength: 21),
                        Mobile = c.String(maxLength: 21),
                        Pager = c.String(maxLength: 21),
                        PagerPIN = c.String(maxLength: 10),
                        AltPhone = c.String(maxLength: 21),
                        Fax = c.String(maxLength: 21),
                        SSN = c.String(maxLength: 11),
                        SIN = c.String(maxLength: 11),
                        NiNumber = c.String(maxLength: 11),
                        MaritalStatus = c.String(maxLength: 6),
                        Email = c.String(maxLength: 1023),
                        EmployeeType = c.String(maxLength: 9),
                        Gender = c.String(maxLength: 6),
                        Sex = c.String(maxLength: 6),
                        HiredDate = c.DateTime(nullable: false),
                        ReleasedDate = c.DateTime(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        AccountNumber = c.String(maxLength: 99),
                        Notes = c.String(),
                        BillingRateRefListID = c.String(maxLength: 36),
                        BillingRateRefFullName = c.String(maxLength: 31),
                        PayrollInfoPayPeriod = c.String(maxLength: 11),
                        PayrollInfoClassRefListID = c.String(maxLength: 36),
                        PayrollInfoClassRefFullName = c.String(maxLength: 159),
                        PayrollInfoEarningsClearEarnings = c.Boolean(nullable: false),
                        PayrollInfoEarningsSeqNo = c.Int(nullable: false),
                        PayrollInfoEarningsPayrollItemWageRefListID = c.String(maxLength: 36),
                        PayrollInfoEarningsPayrollItemWageRefFullName = c.String(maxLength: 31),
                        PayrollInfoEarningsRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayrollInfoEarningsRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayrollInfoUseTimeDataToCreatePaychecks = c.String(maxLength: 16),
                        PayrollInfoSickHoursHoursAvailable = c.Double(nullable: false),
                        PayrollInfoSickHoursAccrualPeriod = c.String(maxLength: 19),
                        PayrollInfoSickHoursHoursAccrued = c.Double(nullable: false),
                        PayrollInfoSickHoursMaximumHours = c.Double(nullable: false),
                        PayrollInfoSickHoursIsResettingHoursEachNewYear = c.Boolean(nullable: false),
                        PayrollInfoSickHoursHoursUsed = c.Double(nullable: false),
                        PayrollInfoSickHoursAccrualStartDate = c.DateTime(nullable: false),
                        PayrollInfoVacationHoursHoursAvailable = c.Double(nullable: false),
                        PayrollInfoVacationHoursAccrualPeriod = c.String(maxLength: 19),
                        PayrollInfoVacationHoursHoursAccrued = c.Double(nullable: false),
                        PayrollInfoVacationHoursMaximumHours = c.Double(nullable: false),
                        PayrollInfoVacationHoursIsResetHoursEachNewYear = c.Boolean(nullable: false),
                        PayrollInfoVacationHoursHoursUsed = c.Double(nullable: false),
                        PayrollInfoVacationHoursAccrualStartDate = c.DateTime(nullable: false),
                        ExternalGUID = c.String(maxLength: 40),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        Employee_CompanyId = c.Int(),
                        Employee_ListID = c.String(maxLength: 36),
                        BillingRateRefBillingRate_CompanyId = c.Int(),
                        BillingRateRefBillingRate_ListID = c.String(maxLength: 36),
                        PayrollInfoClassRefClass_CompanyId = c.Int(),
                        PayrollInfoClassRefClass_ListID = c.String(maxLength: 36),
                        PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_CompanyId = c.Int(),
                        PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Employees", t => new { t.Employee_CompanyId, t.Employee_ListID })
                .ForeignKey("dbo.BillingRates", t => new { t.BillingRateRefBillingRate_CompanyId, t.BillingRateRefBillingRate_ListID })
                .ForeignKey("dbo.Classes", t => new { t.PayrollInfoClassRefClass_CompanyId, t.PayrollInfoClassRefClass_ListID })
                .ForeignKey("dbo.PayrollItemWages", t => new { t.PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_CompanyId, t.PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_ListID })
                .Index(t => new { t.Employee_CompanyId, t.Employee_ListID })
                .Index(t => new { t.BillingRateRefBillingRate_CompanyId, t.BillingRateRefBillingRate_ListID })
                .Index(t => new { t.PayrollInfoClassRefClass_CompanyId, t.PayrollInfoClassRefClass_ListID })
                .Index(t => new { t.PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_CompanyId, t.PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_ListID });
            
            CreateTable(
                "dbo.PayrollItemWages",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        WageType = c.String(maxLength: 14),
                        ExpenseAccountRefListID = c.String(maxLength: 36),
                        ExpenseAccountRefFullName = c.String(maxLength: 159),
                        ExpenseAccountRefAccount_CompanyId = c.Int(),
                        ExpenseAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseAccountRefAccount_CompanyId, t.ExpenseAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseAccountRefAccount_CompanyId, t.ExpenseAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.Estimates",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        CreateChangeOrder = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.EstimateLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        CreateChangeOrder = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        EstimateLineType = c.String(maxLength: 11),
                        EstimateLineSeqNo = c.Int(nullable: false),
                        EstimateLineGroupTxnLineID = c.String(maxLength: 36),
                        EstimateLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        EstimateLineGroupItemGroupRefFullName = c.String(maxLength: 159),
                        EstimateLineGroupDesc = c.String(),
                        EstimateLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        EstimateLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        EstimateLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        EstimateLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        EstimateLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineGroupSeqNo = c.Int(nullable: false),
                        EstimateLineTxnLineID = c.String(maxLength: 36),
                        EstimateLineItemRefListID = c.String(maxLength: 36),
                        EstimateLineItemRefFullName = c.String(maxLength: 159),
                        EstimateLineDesc = c.String(),
                        EstimateLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineUnitOfMeasure = c.String(maxLength: 31),
                        EstimateLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        EstimateLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        EstimateLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLinePriceLevelRefListID = c.String(maxLength: 36),
                        EstimateLinePriceLevelRefFullName = c.String(maxLength: 159),
                        EstimateLineClassRefListID = c.String(maxLength: 36),
                        EstimateLineClassRefFullName = c.String(maxLength: 159),
                        EstimateLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        EstimateLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        EstimateLineTaxCodeRefListID = c.String(maxLength: 36),
                        EstimateLineTaxCodeRefFullName = c.String(maxLength: 3),
                        EstimateLineMarkupRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineMarkupRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimateLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        EstimateLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        CustomFieldEstimateLineOther1 = c.String(maxLength: 29),
                        CustomFieldEstimateLineOther2 = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        EstimateLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        EstimateLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        EstimateLineItemRefItem_CompanyId = c.Int(),
                        EstimateLineItemRefItem_ListID = c.String(maxLength: 36),
                        EstimateLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        EstimateLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        EstimateLinePriceLevelRefPriceLevel_CompanyId = c.Int(),
                        EstimateLinePriceLevelRefPriceLevel_ListID = c.String(maxLength: 36),
                        EstimateLineClassRefClass_CompanyId = c.Int(),
                        EstimateLineClassRefClass_ListID = c.String(maxLength: 36),
                        EstimateLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        EstimateLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        EstimateLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        EstimateLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        EstimateLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        EstimateLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.EstimateLineGroupItemGroupRefItem_CompanyId, t.EstimateLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.EstimateLineItemRefItem_CompanyId, t.EstimateLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.EstimateLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.EstimateLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.EstimateLinePriceLevelRefPriceLevel_CompanyId, t.EstimateLinePriceLevelRefPriceLevel_ListID })
                .ForeignKey("dbo.Classes", t => new { t.EstimateLineClassRefClass_CompanyId, t.EstimateLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.EstimateLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.EstimateLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.EstimateLineTaxCodeRefTaxCode_CompanyId, t.EstimateLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.EstimateLineOverrideItemAccountRefAccount_CompanyId, t.EstimateLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.EstimateLineGroupItemGroupRefItem_CompanyId, t.EstimateLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.EstimateLineItemRefItem_CompanyId, t.EstimateLineItemRefItem_ListID })
                .Index(t => new { t.EstimateLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.EstimateLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.EstimateLinePriceLevelRefPriceLevel_CompanyId, t.EstimateLinePriceLevelRefPriceLevel_ListID })
                .Index(t => new { t.EstimateLineClassRefClass_CompanyId, t.EstimateLineClassRefClass_ListID })
                .Index(t => new { t.EstimateLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.EstimateLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.EstimateLineTaxCodeRefTaxCode_CompanyId, t.EstimateLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.EstimateLineOverrideItemAccountRefAccount_CompanyId, t.EstimateLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.EstimateLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        CreateChangeOrder = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 11),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.Hosts",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        ID = c.Int(nullable: false),
                        ProductName = c.String(maxLength: 255),
                        MajorVersion = c.String(maxLength: 5),
                        MinorVersion = c.String(maxLength: 5),
                        Country = c.String(maxLength: 2),
                        IsAutomaticLogin = c.Boolean(nullable: false),
                        QBFileMode = c.String(maxLength: 10),
                        QODBCMajorVersion = c.String(maxLength: 5),
                        QODBCMinorVersion = c.String(maxLength: 5),
                        QODBCBuildNumber = c.String(maxLength: 5),
                        QODBCRegion = c.String(maxLength: 5),
                        QODBCSerialNo = c.String(maxLength: 12),
                        QODBCEdition = c.String(maxLength: 50),
                        QODBCEditionQBES = c.String(maxLength: 50),
                        QODBCEditionRunning = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.HostMetaDatas",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        ID = c.Int(nullable: false),
                        ProductName = c.String(maxLength: 255),
                        MajorVersion = c.String(maxLength: 5),
                        MinorVersion = c.String(maxLength: 5),
                        Country = c.String(maxLength: 2),
                        IsAutomaticLogin = c.Boolean(nullable: false),
                        QBFileMode = c.String(maxLength: 10),
                        QODBCMajorVersion = c.String(maxLength: 5),
                        QODBCMinorVersion = c.String(maxLength: 5),
                        QODBCBuildNumber = c.String(maxLength: 5),
                        QODBCRegion = c.String(maxLength: 5),
                        QODBCSerialNo = c.String(maxLength: 12),
                        QODBCEdition = c.String(maxLength: 50),
                        QODBCEditionQBES = c.String(maxLength: 50),
                        QODBCEditionRunning = c.String(maxLength: 50),
                        AccountMetaDataMaxCapacity = c.Int(nullable: false),
                        BillingRateMetaDataMaxCapacity = c.Int(nullable: false),
                        ClassMetaDataMaxCapacity = c.Int(nullable: false),
                        CustomerMsgMetaDataMaxCapacity = c.Int(nullable: false),
                        CustomerTypeMetaDataMaxCapacity = c.Int(nullable: false),
                        EntityMetaDataMaxCapacity = c.Int(nullable: false),
                        ItemMetaDataMaxCapacity = c.Int(nullable: false),
                        JobTypeMetaDataMaxCapacity = c.Int(nullable: false),
                        PaymentMethodMetaDataMaxCapacity = c.Int(nullable: false),
                        PayrollItemMetaData = c.Int(nullable: false),
                        PriceLevelMetaData = c.Int(nullable: false),
                        SalesRepMetaDataMaxCapacity = c.Int(nullable: false),
                        SalesTaxCodeMetaDataMaxCapacity = c.Int(nullable: false),
                        ShipMethodMetaDataMaxCapacity = c.Int(nullable: false),
                        TemplateMetaDataMaxCapacity = c.Int(nullable: false),
                        TermsMetaDataMaxCapacity = c.Int(nullable: false),
                        ToDoMetaDataMaxCapacity = c.Int(nullable: false),
                        VehicleMetaDataMaxCapacity = c.Int(nullable: false),
                        VendorTypeMetaDataMaxCapacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.HostSupportedVersions",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        ID = c.Int(nullable: false),
                        ProductName = c.String(maxLength: 255),
                        MajorVersion = c.String(maxLength: 5),
                        MinorVersion = c.String(maxLength: 5),
                        Country = c.String(maxLength: 2),
                        SupportedQBXMLVersion = c.String(maxLength: 10),
                        IsAutomaticLogin = c.Boolean(nullable: false),
                        QBFileMode = c.String(maxLength: 10),
                        QODBCMajorVersion = c.String(maxLength: 5),
                        QODBCMinorVersion = c.String(maxLength: 5),
                        QODBCBuildNumber = c.String(maxLength: 5),
                        QODBCRegion = c.String(maxLength: 5),
                        QODBCSerialNo = c.String(maxLength: 12),
                        QODBCEdition = c.String(maxLength: 50),
                        QODBCEditionQBES = c.String(maxLength: 50),
                        QODBCEditionRunning = c.String(maxLength: 50),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.InventoryAdjustments",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        Memo = c.String(),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID });
            
            CreateTable(
                "dbo.InventoryAdjustmentLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        AccountRefListID = c.String(nullable: false, maxLength: 36),
                        AccountRefFullName = c.String(nullable: false, maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        Memo = c.String(),
                        InventoryAdjustmentSeqNo = c.Int(nullable: false),
                        InventoryAdjustmentLineTxnLineID = c.String(maxLength: 36),
                        InventoryAdjustmentLineItemRefListID = c.String(nullable: false, maxLength: 36),
                        InventoryAdjustmentLineItemRefFullName = c.String(nullable: false, maxLength: 159),
                        InventoryAdjustmentLineQuantityDifference = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryAdjustmentLineValueDifference = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryAdjustmentLineQuantityAdjustmentNewQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryAdjustmentLineQuantityAdjustmentQuantityDifference = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryAdjustmentLineValueAdjustmentNewQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryAdjustmentLineValueAdjustmentNewValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        InventoryAdjustmentLineItemRefItem_CompanyId = c.Int(),
                        InventoryAdjustmentLineItemRefItem_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Items", t => new { t.InventoryAdjustmentLineItemRefItem_CompanyId, t.InventoryAdjustmentLineItemRefItem_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.InventoryAdjustmentLineItemRefItem_CompanyId, t.InventoryAdjustmentLineItemRefItem_ListID });
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        IsFinanceCharge = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemainingInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        SuggestedDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SuggestedDiscountDate = c.DateTime(nullable: false),
                        CustomFieldOther = c.String(maxLength: 29),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.InvoiceLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        IsFinanceCharge = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemainingInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        SuggestedDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SuggestedDiscountDate = c.DateTime(nullable: false),
                        CustomFieldOther = c.String(maxLength: 29),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        InvoiceLineType = c.String(maxLength: 11),
                        InvoiceLineSeqNo = c.Int(nullable: false),
                        InvoiceLineGroupTxnLineID = c.String(maxLength: 36),
                        InvoiceLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        InvoiceLineGroupItemGroupRefFullName = c.String(maxLength: 31),
                        InvoiceLineGroupDesc = c.String(),
                        InvoiceLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        InvoiceLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        InvoiceLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        InvoiceLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        InvoiceLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLineGroupServiceDate = c.DateTime(nullable: false),
                        InvoiceLineGroupSeqNo = c.Int(nullable: false),
                        InvoiceLineTxnLineID = c.String(maxLength: 36),
                        InvoiceLineItemRefListID = c.String(maxLength: 36),
                        InvoiceLineItemRefFullName = c.String(maxLength: 159),
                        InvoiceLineDesc = c.String(),
                        InvoiceLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLineUnitOfMeasure = c.String(maxLength: 31),
                        InvoiceLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        InvoiceLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        InvoiceLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLineRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLinePriceLevelRefListID = c.String(maxLength: 36),
                        InvoiceLinePriceLevelRefFullName = c.String(maxLength: 159),
                        InvoiceLineClassRefListID = c.String(maxLength: 36),
                        InvoiceLineClassRefFullName = c.String(maxLength: 159),
                        InvoiceLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceLineServiceDate = c.DateTime(nullable: false),
                        InvoiceLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        InvoiceLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        InvoiceLineTaxCodeRefListID = c.String(maxLength: 36),
                        InvoiceLineTaxCodeRefFullName = c.String(maxLength: 3),
                        InvoiceLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        InvoiceLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        CustomFieldInvoiceLineOther1 = c.String(maxLength: 29),
                        CustomFieldInvoiceLineOther2 = c.String(maxLength: 29),
                        InvoiceLineLinkToTxnTxnID = c.String(maxLength: 36),
                        InvoiceLineLinkToTxnTxnLineID = c.String(maxLength: 36),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        InvoiceLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        InvoiceLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        InvoiceLineItemRefItem_CompanyId = c.Int(),
                        InvoiceLineItemRefItem_ListID = c.String(maxLength: 36),
                        InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        InvoiceLinePriceLevelRefPriceLevel_CompanyId = c.Int(),
                        InvoiceLinePriceLevelRefPriceLevel_ListID = c.String(maxLength: 36),
                        InvoiceLineClassRefClass_CompanyId = c.Int(),
                        InvoiceLineClassRefClass_ListID = c.String(maxLength: 36),
                        InvoiceLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        InvoiceLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        InvoiceLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        InvoiceLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        InvoiceLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        InvoiceLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.InvoiceLineGroupItemGroupRefItem_CompanyId, t.InvoiceLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.InvoiceLineItemRefItem_CompanyId, t.InvoiceLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.InvoiceLinePriceLevelRefPriceLevel_CompanyId, t.InvoiceLinePriceLevelRefPriceLevel_ListID })
                .ForeignKey("dbo.Classes", t => new { t.InvoiceLineClassRefClass_CompanyId, t.InvoiceLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.InvoiceLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.InvoiceLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.InvoiceLineTaxCodeRefTaxCode_CompanyId, t.InvoiceLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.InvoiceLineOverrideItemAccountRefAccount_CompanyId, t.InvoiceLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.InvoiceLineGroupItemGroupRefItem_CompanyId, t.InvoiceLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.InvoiceLineItemRefItem_CompanyId, t.InvoiceLineItemRefItem_ListID })
                .Index(t => new { t.InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.InvoiceLinePriceLevelRefPriceLevel_CompanyId, t.InvoiceLinePriceLevelRefPriceLevel_ListID })
                .Index(t => new { t.InvoiceLineClassRefClass_CompanyId, t.InvoiceLineClassRefClass_ListID })
                .Index(t => new { t.InvoiceLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.InvoiceLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.InvoiceLineTaxCodeRefTaxCode_CompanyId, t.InvoiceLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.InvoiceLineOverrideItemAccountRefAccount_CompanyId, t.InvoiceLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.InvoiceLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        IsFinanceCharge = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRemainingInHomeCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        SuggestedDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SuggestedDiscountDate = c.DateTime(nullable: false),
                        CustomFieldOther = c.String(maxLength: 29),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 11),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.ItemAssembliesCanBuilds",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        ItemInventoryAssemblyRefListID = c.String(maxLength: 36),
                        ItemInventoryAssemblyRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        QuantityCanBuild = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId = c.Int(),
                        ItemInventoryAssemblyRefItemInventoryAssembly_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.ItemInventoryAssemblies", t => new { t.ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId, t.ItemInventoryAssemblyRefItemInventoryAssembly_ListID })
                .Index(t => new { t.ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId, t.ItemInventoryAssemblyRefItemInventoryAssembly_ListID });
            
            CreateTable(
                "dbo.ItemDiscounts",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        ItemDesc = c.String(),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        DiscountRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountRefListID = c.String(maxLength: 36),
                        AccountRefFullName = c.String(maxLength: 159),
                        ExternalGUID = c.String(maxLength: 40),
                        ApplyAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        TaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.TaxCodeRefSalesTaxCode_CompanyId, t.TaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefSalesTaxCode_CompanyId, t.TaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ItemFixedAssets",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        AcquiredAs = c.String(nullable: false, maxLength: 3),
                        PurchaseDesc = c.String(nullable: false, maxLength: 50),
                        PurchaseDate = c.DateTime(nullable: false),
                        PurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VendorOrPayeeName = c.String(maxLength: 50),
                        AssetAccountRefListID = c.String(nullable: false, maxLength: 36),
                        AssetAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        FixedAssetSalesInfoSalesDesc = c.String(maxLength: 50),
                        FixedAssetSalesInfoSalesDate = c.DateTime(nullable: false),
                        FixedAssetSalesInfoSalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FixedAssetSalesInfoSalesExpense = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AssetDesc = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        PONumber = c.String(maxLength: 30),
                        SerialNumber = c.String(maxLength: 30),
                        WarrantyExpDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        AssetNumber = c.String(maxLength: 10),
                        CostBasis = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YearEndAccumulatedDepreciation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YearEndBookValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExternalGUID = c.String(maxLength: 40),
                        AssetAccountRefAccount_CompanyId = c.Int(),
                        AssetAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .Index(t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ItemGroups",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        IsPrintItemsInGroup = c.Boolean(nullable: false),
                        ExternalGUID = c.String(maxLength: 40),
                        SpecialItemType = c.String(maxLength: 24),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID });
            
            CreateTable(
                "dbo.ItemGroupLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        IsPrintItemsInGroup = c.Boolean(nullable: false),
                        ExternalGUID = c.String(maxLength: 40),
                        SpecialItemType = c.String(maxLength: 24),
                        ClearItemsInGroup = c.Boolean(nullable: false),
                        ItemGroupLineSeqNo = c.Int(nullable: false),
                        ItemGroupLineItemRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefFullName = c.String(maxLength: 159),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupLineUnitOfMeasure = c.String(maxLength: 31),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        ItemGroup_CompanyId = c.Int(),
                        ItemGroup_ListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemRefItem_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.ItemGroups", t => new { t.ItemGroup_CompanyId, t.ItemGroup_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .Index(t => new { t.ItemGroup_CompanyId, t.ItemGroup_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID });
            
            CreateTable(
                "dbo.ItemInventories",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        ManufacturerPartNumber = c.String(maxLength: 31),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesDesc = c.String(),
                        SalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomeAccountRefListID = c.String(nullable: false, maxLength: 36),
                        IncomeAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        ApplyIncomeAccountRefToExistingTxns = c.Boolean(nullable: false),
                        PurchaseDesc = c.String(),
                        PurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        COGSAccountRefListID = c.String(nullable: false, maxLength: 36),
                        COGSAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PrefVendorRefListID = c.String(maxLength: 36),
                        PrefVendorRefFullName = c.String(maxLength: 41),
                        AssetAccountRefListID = c.String(nullable: false, maxLength: 36),
                        AssetAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        ReorderPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryDate = c.DateTime(nullable: false),
                        AverageCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnSalesOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExternalGUID = c.String(maxLength: 40),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        IncomeAccountRefAccount_CompanyId = c.Int(),
                        IncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        COGSAccountRefAccount_CompanyId = c.Int(),
                        COGSAccountRefAccount_ListID = c.String(maxLength: 36),
                        PrefVendorRefVendor_CompanyId = c.Int(),
                        PrefVendorRefVendor_ListID = c.String(maxLength: 36),
                        AssetAccountRefAccount_CompanyId = c.Int(),
                        AssetAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .Index(t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .Index(t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .Index(t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ItemInventoryAssemblyLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesDesc = c.String(),
                        SalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomeAccountRefListID = c.String(nullable: false, maxLength: 36),
                        IncomeAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        ApplyIncomeAccountRefToExistingTxns = c.Boolean(nullable: false),
                        PurchaseDesc = c.String(),
                        PurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        COGSAccountRefListID = c.String(nullable: false, maxLength: 36),
                        COGSAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        PrefVendorRefListID = c.String(maxLength: 36),
                        PrefVendorRefFullName = c.String(maxLength: 41),
                        AssetAccountRefListID = c.String(nullable: false, maxLength: 36),
                        AssetAccountRefFullName = c.String(nullable: false, maxLength: 159),
                        BuildPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryDate = c.DateTime(nullable: false),
                        AverageCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityOnSalesOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExternalGUID = c.String(maxLength: 40),
                        ClearItemsInGroup = c.Boolean(nullable: false),
                        ItemInventoryAssemblyLnSeqNo = c.Int(nullable: false),
                        ItemInventoryAssemblyLnItemInventoryRefListID = c.String(maxLength: 36),
                        ItemInventoryAssemblyLnItemInventoryRefFullName = c.String(maxLength: 159),
                        ItemInventoryAssemblyLnQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        ItemInventoryAssembly_CompanyId = c.Int(),
                        ItemInventoryAssembly_ListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        IncomeAccountRefAccount_CompanyId = c.Int(),
                        IncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        COGSAccountRefAccount_CompanyId = c.Int(),
                        COGSAccountRefAccount_ListID = c.String(maxLength: 36),
                        PrefVendorRefVendor_CompanyId = c.Int(),
                        PrefVendorRefVendor_ListID = c.String(maxLength: 36),
                        AssetAccountRefAccount_CompanyId = c.Int(),
                        AssetAccountRefAccount_ListID = c.String(maxLength: 36),
                        ItemInventoryAssemblyLnItemInventoryRefItemInventory_CompanyId = c.Int(),
                        ItemInventoryAssemblyLnItemInventoryRefItemInventory_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.ItemInventoryAssemblies", t => new { t.ItemInventoryAssembly_CompanyId, t.ItemInventoryAssembly_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .ForeignKey("dbo.ItemInventories", t => new { t.ItemInventoryAssemblyLnItemInventoryRefItemInventory_CompanyId, t.ItemInventoryAssemblyLnItemInventoryRefItemInventory_ListID })
                .Index(t => new { t.ItemInventoryAssembly_CompanyId, t.ItemInventoryAssembly_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.IncomeAccountRefAccount_CompanyId, t.IncomeAccountRefAccount_ListID })
                .Index(t => new { t.COGSAccountRefAccount_CompanyId, t.COGSAccountRefAccount_ListID })
                .Index(t => new { t.PrefVendorRefVendor_CompanyId, t.PrefVendorRefVendor_ListID })
                .Index(t => new { t.AssetAccountRefAccount_CompanyId, t.AssetAccountRefAccount_ListID })
                .Index(t => new { t.ItemInventoryAssemblyLnItemInventoryRefItemInventory_CompanyId, t.ItemInventoryAssemblyLnItemInventoryRefItemInventory_ListID });
            
            CreateTable(
                "dbo.ItemNonInventories",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        ManufacturerPartNumber = c.String(maxLength: 31),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesOrPurchaseDesc = c.String(),
                        SalesOrPurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchasePricePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchaseAccountRefListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefFullName = c.String(maxLength: 159),
                        SalesOrPurchaseApplyAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchaseSalesDesc = c.String(),
                        SalesAndPurchaseSalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseIncomeAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseApplyIncomeAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchasePurchaseDesc = c.String(),
                        SalesAndPurchasePurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseExpenseAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseApplyExpenseAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchasePrefVendorRefListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefFullName = c.String(maxLength: 41),
                        ExternalGUID = c.String(maxLength: 40),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefAccount_CompanyId = c.Int(),
                        SalesOrPurchaseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseIncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseExpenseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefVendor_CompanyId = c.Int(),
                        SalesAndPurchasePrefVendorRefVendor_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID });
            
            CreateTable(
                "dbo.ItemOtherCharges",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesOrPurchaseDesc = c.String(),
                        SalesOrPurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchasePricePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchaseAccountRefListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefFullName = c.String(maxLength: 159),
                        SalesOrPurchaseApplyAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchaseSalesDesc = c.String(),
                        SalesAndPurchaseSalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseIncomeAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseApplyIncomeAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchasePurchaseDesc = c.String(),
                        SalesAndPurchasePurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseExpenseAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseApplyExpenseAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchasePrefVendorRefListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefFullName = c.String(maxLength: 41),
                        SpecialItemType = c.String(maxLength: 24),
                        ExternalGUID = c.String(maxLength: 40),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefAccount_CompanyId = c.Int(),
                        SalesOrPurchaseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseIncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseExpenseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefVendor_CompanyId = c.Int(),
                        SalesAndPurchasePrefVendorRefVendor_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID });
            
            CreateTable(
                "dbo.ItemPayments",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        ExternalGUID = c.String(maxLength: 40),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID });
            
            CreateTable(
                "dbo.ItemReceipts",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        LiabilityAccountRefListID = c.String(maxLength: 36),
                        LiabilityAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        LiabilityAccountRefAccount_CompanyId = c.Int(),
                        LiabilityAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ItemReceiptExpenseLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        LiabilityAccountRefListID = c.String(maxLength: 36),
                        LiabilityAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        ExpenseLineClearExpenseLines = c.Boolean(nullable: false),
                        ExpenseLineTxnLineID = c.String(maxLength: 36),
                        ExpenseLineAccountRefListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefFullName = c.String(maxLength: 159),
                        ExpenseLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineMemo = c.String(),
                        ExpenseLineCustomerRefListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefFullName = c.String(maxLength: 209),
                        ExpenseLineClassRefListID = c.String(maxLength: 36),
                        ExpenseLineClassRefFullName = c.String(maxLength: 159),
                        ExpenseLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineBillableStatus = c.String(maxLength: 13),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        LiabilityAccountRefAccount_CompanyId = c.Int(),
                        LiabilityAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefAccount_CompanyId = c.Int(),
                        ExpenseLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefCustomer_CompanyId = c.Int(),
                        ExpenseLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ExpenseLineClassRefClass_CompanyId = c.Int(),
                        ExpenseLineClassRefClass_ListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ExpenseLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .Index(t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.ItemReceiptItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        LiabilityAccountRefListID = c.String(maxLength: 36),
                        LiabilityAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        ItemLineType = c.String(maxLength: 11),
                        ItemLineSeqNo = c.Int(nullable: false),
                        ItemGroupLineTxnLineID = c.String(maxLength: 36),
                        ItemGroupLineItemGroupRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemGroupRefFullName = c.String(maxLength: 31),
                        ItemGroupLineDesc = c.String(),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupUnitOfMeasure = c.String(maxLength: 31),
                        ItemGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemGroupLineTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupSeqNo = c.Int(nullable: false),
                        ItemLineTxnLineID = c.String(maxLength: 36),
                        ItemLineItemRefListID = c.String(maxLength: 36),
                        ItemLineItemRefFullName = c.String(maxLength: 159),
                        ItemLineDesc = c.String(),
                        ItemLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemLineCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineCustomerRefListID = c.String(maxLength: 36),
                        ItemLineCustomerRefFullName = c.String(maxLength: 209),
                        ItemLineClassRefListID = c.String(maxLength: 36),
                        ItemLineClassRefFullName = c.String(maxLength: 159),
                        ItemLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineBillableStatus = c.String(maxLength: 13),
                        ItemLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        ItemLineLinkToTxnTxnID = c.String(maxLength: 36),
                        ItemLineLinkToTxnTxnLineID = c.String(maxLength: 36),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        LiabilityAccountRefAccount_CompanyId = c.Int(),
                        LiabilityAccountRefAccount_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemGroupRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemGroupRefItem_ListID = c.String(maxLength: 36),
                        ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineItemRefItem_CompanyId = c.Int(),
                        ItemLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineCustomerRefCustomer_CompanyId = c.Int(),
                        ItemLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemLineClassRefClass_CompanyId = c.Int(),
                        ItemLineClassRefClass_ListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ItemLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ItemLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        ItemLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemGroupRefItem_CompanyId, t.ItemGroupLineItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .Index(t => new { t.ItemGroupLineItemGroupRefItem_CompanyId, t.ItemGroupLineItemGroupRefItem_ListID })
                .Index(t => new { t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .Index(t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ItemReceiptLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        LiabilityAccountRefListID = c.String(maxLength: 36),
                        LiabilityAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        LinkToTxnID1 = c.String(maxLength: 36),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 8),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        LiabilityAccountRefAccount_CompanyId = c.Int(),
                        LiabilityAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ItemSalesTaxGroups",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        ExternalGUID = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.ItemSalesTaxGroupLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        ExternalGUID = c.String(maxLength: 40),
                        ItemSalesTaxSeqNo = c.Int(nullable: false),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        ItemSalesTaxGroup_CompanyId = c.Int(),
                        ItemSalesTaxGroup_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefSalesTaxCode_CompanyId = c.Int(),
                        ItemSalesTaxRefSalesTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.ItemSalesTaxGroups", t => new { t.ItemSalesTaxGroup_CompanyId, t.ItemSalesTaxGroup_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemSalesTaxRefSalesTaxCode_CompanyId, t.ItemSalesTaxRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemSalesTaxGroup_CompanyId, t.ItemSalesTaxGroup_ListID })
                .Index(t => new { t.ItemSalesTaxRefSalesTaxCode_CompanyId, t.ItemSalesTaxRefSalesTaxCode_ListID });
            
            CreateTable(
                "dbo.ItemServices",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        FullName = c.String(maxLength: 159),
                        IsActive = c.Boolean(nullable: false),
                        ParentRefListID = c.String(maxLength: 36),
                        ParentRefFullName = c.String(maxLength: 159),
                        Sublevel = c.Int(nullable: false),
                        UnitOfMeasureSetRefListID = c.String(maxLength: 36),
                        UnitOfMeasureSetRefFullName = c.String(maxLength: 31),
                        ForceUOMChange = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesOrPurchaseDesc = c.String(),
                        SalesOrPurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchasePricePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrPurchaseAccountRefListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefFullName = c.String(maxLength: 159),
                        SalesOrPurchaseApplyAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchaseSalesDesc = c.String(),
                        SalesAndPurchaseSalesPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseIncomeAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseApplyIncomeAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchasePurchaseDesc = c.String(),
                        SalesAndPurchasePurchaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndPurchaseExpenseAccountRefListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefFullName = c.String(maxLength: 159),
                        SalesAndPurchaseApplyExpenseAccountRefToExistingTxns = c.Boolean(nullable: false),
                        SalesAndPurchasePrefVendorRefListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefFullName = c.String(maxLength: 41),
                        ExternalGUID = c.String(maxLength: 40),
                        UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesOrPurchaseAccountRefAccount_CompanyId = c.Int(),
                        SalesOrPurchaseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseIncomeAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseIncomeAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchaseExpenseAccountRefAccount_CompanyId = c.Int(),
                        SalesAndPurchaseExpenseAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesAndPurchasePrefVendorRefVendor_CompanyId = c.Int(),
                        SalesAndPurchasePrefVendorRefVendor_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .ForeignKey("dbo.Vendors", t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID })
                .Index(t => new { t.UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId, t.UnitOfMeasureSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesOrPurchaseAccountRefAccount_CompanyId, t.SalesOrPurchaseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseIncomeAccountRefAccount_CompanyId, t.SalesAndPurchaseIncomeAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchaseExpenseAccountRefAccount_CompanyId, t.SalesAndPurchaseExpenseAccountRefAccount_ListID })
                .Index(t => new { t.SalesAndPurchasePrefVendorRefVendor_CompanyId, t.SalesAndPurchasePrefVendorRefVendor_ListID });
            
            CreateTable(
                "dbo.ItemSubtotals",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        ItemDesc = c.String(),
                        ExternalGUID = c.String(maxLength: 40),
                        SpecialItemType = c.String(maxLength: 24),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.JournalEntries",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        IsAdjustment = c.Boolean(nullable: false),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.JournalEntryCreditLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        IsAdjustment = c.Boolean(nullable: false),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        JournalCreditLineTxnLineID = c.String(maxLength: 36),
                        JournalCreditLineType = c.String(maxLength: 14),
                        JournalCreditLineAccountRefListID = c.String(maxLength: 36),
                        JournalCreditLineAccountRefFullName = c.String(maxLength: 159),
                        JournalCreditLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalCreditLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalCreditLineMemo = c.String(),
                        JournalCreditLineEntityRefListID = c.String(maxLength: 36),
                        JournalCreditLineEntityRefFullName = c.String(maxLength: 209),
                        JournalCreditLineClassRefListID = c.String(maxLength: 36),
                        JournalCreditLineClassRefFullName = c.String(maxLength: 159),
                        JournalCreditLineBillableStatus = c.String(maxLength: 13),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQTransactionLinkKey = c.String(maxLength: 73),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        JournalCreditLineAccountRefAccount_CompanyId = c.Int(),
                        JournalCreditLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        JournalCreditLineEntityRefEntity_CompanyId = c.Int(),
                        JournalCreditLineEntityRefEntity_ListID = c.String(maxLength: 36),
                        JournalCreditLineClassRefClass_CompanyId = c.Int(),
                        JournalCreditLineClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.JournalCreditLineAccountRefAccount_CompanyId, t.JournalCreditLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.JournalCreditLineEntityRefEntity_CompanyId, t.JournalCreditLineEntityRefEntity_ListID })
                .ForeignKey("dbo.Classes", t => new { t.JournalCreditLineClassRefClass_CompanyId, t.JournalCreditLineClassRefClass_ListID })
                .Index(t => new { t.JournalCreditLineAccountRefAccount_CompanyId, t.JournalCreditLineAccountRefAccount_ListID })
                .Index(t => new { t.JournalCreditLineEntityRefEntity_CompanyId, t.JournalCreditLineEntityRefEntity_ListID })
                .Index(t => new { t.JournalCreditLineClassRefClass_CompanyId, t.JournalCreditLineClassRefClass_ListID });
            
            CreateTable(
                "dbo.JournalEntryDebitLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        IsAdjustment = c.Boolean(nullable: false),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        JournalDebitLineTxnLineID = c.String(maxLength: 36),
                        JournalDebitLineType = c.String(maxLength: 14),
                        JournalDebitLineAccountRefListID = c.String(maxLength: 36),
                        JournalDebitLineAccountRefFullName = c.String(maxLength: 159),
                        JournalDebitLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalDebitLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalDebitLineMemo = c.String(),
                        JournalDebitLineEntityRefListID = c.String(maxLength: 36),
                        JournalDebitLineEntityRefFullName = c.String(maxLength: 209),
                        JournalDebitLineClassRefListID = c.String(maxLength: 36),
                        JournalDebitLineClassRefFullName = c.String(maxLength: 159),
                        JournalDebitLineBillableStatus = c.String(maxLength: 13),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQTransactionLinkKey = c.String(maxLength: 73),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        JournalDebitLineAccountRefAccount_CompanyId = c.Int(),
                        JournalDebitLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        JournalDebitLineEntityRefEntity_CompanyId = c.Int(),
                        JournalDebitLineEntityRefEntity_ListID = c.String(maxLength: 36),
                        JournalDebitLineClassRefClass_CompanyId = c.Int(),
                        JournalDebitLineClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.JournalDebitLineAccountRefAccount_CompanyId, t.JournalDebitLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.JournalDebitLineEntityRefEntity_CompanyId, t.JournalDebitLineEntityRefEntity_ListID })
                .ForeignKey("dbo.Classes", t => new { t.JournalDebitLineClassRefClass_CompanyId, t.JournalDebitLineClassRefClass_ListID })
                .Index(t => new { t.JournalDebitLineAccountRefAccount_CompanyId, t.JournalDebitLineAccountRefAccount_ListID })
                .Index(t => new { t.JournalDebitLineEntityRefEntity_CompanyId, t.JournalDebitLineEntityRefEntity_ListID })
                .Index(t => new { t.JournalDebitLineClassRefClass_CompanyId, t.JournalDebitLineClassRefClass_ListID });
            
            CreateTable(
                "dbo.JournalEntryLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        IsAdjustment = c.Boolean(nullable: false),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        JournalLineSeqNo = c.Int(nullable: false),
                        JournalLineType = c.String(maxLength: 25),
                        JournalLineTxnLineID = c.String(maxLength: 36),
                        JournalLineAccountRefListID = c.String(maxLength: 36),
                        JournalLineAccountRefFullName = c.String(maxLength: 159),
                        JournalLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalLineCreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalLineDebitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JournalLineMemo = c.String(),
                        JournalLineEntityRefListID = c.String(maxLength: 36),
                        JournalLineEntityRefFullName = c.String(maxLength: 209),
                        JournalLineClassRefListID = c.String(maxLength: 36),
                        JournalLineClassRefFullName = c.String(maxLength: 159),
                        JournalLineBillableStatus = c.String(maxLength: 13),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQTransactionLinkKey = c.String(maxLength: 73),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        JournalLineAccountRefAccount_CompanyId = c.Int(),
                        JournalLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        JournalLineEntityRefEntity_CompanyId = c.Int(),
                        JournalLineEntityRefEntity_ListID = c.String(maxLength: 36),
                        JournalLineClassRefClass_CompanyId = c.Int(),
                        JournalLineClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.JournalLineAccountRefAccount_CompanyId, t.JournalLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Entities", t => new { t.JournalLineEntityRefEntity_CompanyId, t.JournalLineEntityRefEntity_ListID })
                .ForeignKey("dbo.Classes", t => new { t.JournalLineClassRefClass_CompanyId, t.JournalLineClassRefClass_ListID })
                .Index(t => new { t.JournalLineAccountRefAccount_CompanyId, t.JournalLineAccountRefAccount_ListID })
                .Index(t => new { t.JournalLineEntityRefEntity_CompanyId, t.JournalLineEntityRefEntity_ListID })
                .Index(t => new { t.JournalLineClassRefClass_CompanyId, t.JournalLineClassRefClass_ListID });
            
            CreateTable(
                "dbo.ListDeleteds",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        ListDelType = c.String(maxLength: 20),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeDeleted = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        FullName = c.String(maxLength: 41),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.OtherNames",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        CompanyName = c.String(maxLength: 41),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 25),
                        MiddleName = c.String(maxLength: 5),
                        LastName = c.String(maxLength: 25),
                        OtherNameAddressAddr1 = c.String(maxLength: 41),
                        OtherNameAddressAddr2 = c.String(maxLength: 41),
                        OtherNameAddressAddr3 = c.String(maxLength: 41),
                        OtherNameAddressAddr4 = c.String(maxLength: 41),
                        OtherNameAddressAddr5 = c.String(maxLength: 41),
                        OtherNameAddressCity = c.String(maxLength: 31),
                        OtherNameAddressState = c.String(maxLength: 21),
                        OtherNameAddressProvince = c.String(maxLength: 21),
                        OtherNameAddressCounty = c.String(maxLength: 21),
                        OtherNameAddressPostalCode = c.String(maxLength: 13),
                        OtherNameAddressCountry = c.String(maxLength: 31),
                        OtherNameAddressNote = c.String(maxLength: 41),
                        OtherNameAddressBlockAddr1 = c.String(maxLength: 41),
                        OtherNameAddressBlockAddr2 = c.String(maxLength: 41),
                        OtherNameAddressBlockAddr3 = c.String(maxLength: 41),
                        OtherNameAddressBlockAddr4 = c.String(maxLength: 41),
                        OtherNameAddressBlockAddr5 = c.String(maxLength: 41),
                        Phone = c.String(maxLength: 21),
                        AltPhone = c.String(maxLength: 21),
                        Fax = c.String(maxLength: 21),
                        Email = c.String(maxLength: 1023),
                        Contact = c.String(maxLength: 41),
                        AltContact = c.String(maxLength: 41),
                        AccountNumber = c.String(maxLength: 99),
                        BusinessNumber = c.String(maxLength: 99),
                        Notes = c.String(),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        ExternalGUID = c.String(maxLength: 40),
                        Entity_CompanyId = c.Int(),
                        Entity_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Entities", t => new { t.Entity_CompanyId, t.Entity_ListID })
                .Index(t => new { t.Entity_CompanyId, t.Entity_ListID });
            
            CreateTable(
                "dbo.PayrollItemNonWages",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        NonWageType = c.String(maxLength: 19),
                        ExpenseAccountRefListID = c.String(maxLength: 36),
                        ExpenseAccountRefFullName = c.String(maxLength: 159),
                        LiabilityAccountRefListID = c.String(maxLength: 36),
                        LiabilityAccountRefFullName = c.String(maxLength: 159),
                        ExpenseAccountRefAccount_CompanyId = c.Int(),
                        ExpenseAccountRefAccount_ListID = c.String(maxLength: 36),
                        LiabilityAccountRefAccount_CompanyId = c.Int(),
                        LiabilityAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseAccountRefAccount_CompanyId, t.ExpenseAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseAccountRefAccount_CompanyId, t.ExpenseAccountRefAccount_ListID })
                .Index(t => new { t.LiabilityAccountRefAccount_CompanyId, t.LiabilityAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.Preferences",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        ID = c.Int(nullable: false),
                        AccountingPrefsIsUsingAccountNumbers = c.Boolean(nullable: false),
                        AccountingPrefsIsRequiringAccounts = c.Boolean(nullable: false),
                        AccountingPrefsIsUsingClassTracking = c.Boolean(nullable: false),
                        AccountingPrefsIsUsingAuditTrail = c.Boolean(nullable: false),
                        AccountingPrefsIsAssigningJournalEntryNumbers = c.Boolean(nullable: false),
                        AccountingPrefsClosingDate = c.DateTime(nullable: false),
                        FinanceChargePrefsAnnualInterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinanceChargePrefsMinFinanceCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinanceChargePrefsGracePeriod = c.Int(nullable: false),
                        FinanceChargePrefsFinanceChargeAcctRefListID = c.String(maxLength: 36),
                        FinanceChargePrefsFinanceChargeAcctRefFullName = c.String(maxLength: 159),
                        FinanceChargePrefsIsAssessingForOverdueCharges = c.Boolean(nullable: false),
                        FinanceChargePrefsCalculateChargesFrom = c.String(maxLength: 20),
                        FinanceChargePrefsIsMarkedToBePrinted = c.Boolean(nullable: false),
                        JobsAndEstimatesPrefsIsUsingEstimates = c.Boolean(nullable: false),
                        JobsAndEstimatesPrefsIsUsingProgressInvoicing = c.Boolean(nullable: false),
                        JobsAndEstimatesPrefsIsPrintItemsWithZeroAmt = c.Boolean(nullable: false),
                        MultiCurrencyPrefsIsMultiCurrencyOn = c.Boolean(nullable: false),
                        MultiCurrencyPrefsHomeCurrencyRefListID = c.String(maxLength: 36),
                        MultiCurrencyPrefsHomeCurrencyRefFullName = c.String(maxLength: 159),
                        PurchasesAndVendorsPrefsIsUsingInventory = c.Boolean(nullable: false),
                        PurchasesAndVendorsPrefsDaysBillsAreDue = c.Int(nullable: false),
                        PurchasesAndVendorsPrefsIsAutomaticUsingDis = c.Boolean(nullable: false),
                        PurchasesAndVendorsPrefDefaultDisARefListID = c.String(maxLength: 36),
                        PurchasesAndVendorsPrefDefaultDisARefFullName = c.String(maxLength: 159),
                        PurchasesAndVendorsPrefsIsUsingUnitsOfMeasure = c.Boolean(nullable: false),
                        ReportsPrefsAgingReportBasis = c.String(maxLength: 22),
                        ReportsPrefsSummaryReportBasis = c.String(maxLength: 7),
                        SalesAndCustomersPrefsDeftShipMethRefListID = c.String(maxLength: 36),
                        SalesAndCustomersPrefsDeftShipMethRefFullName = c.String(maxLength: 15),
                        SalesAndCustomersPrefsDefaultFOB = c.String(maxLength: 13),
                        SalesAndCustomersPrefsDefaultMarkup = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesAndCustomersPrefsIsTrackingRembrsdExpInc = c.Boolean(nullable: false),
                        SalesAndCustomersPrefsIsAutoApplyingPayments = c.Boolean(nullable: false),
                        SalesTaxPrefsDefaultItemSalesTaxRefListID = c.String(maxLength: 36),
                        SalesTaxPrefsDefaultItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPrefsPaySalesTax = c.String(maxLength: 9),
                        SalesTaxPrefsDefaultTaxableSaleTCRefListID = c.String(maxLength: 36),
                        SalesTaxPrefsDefaultTaxableSaleTCRefFullName = c.String(maxLength: 3),
                        SalesTaxPrefsDefaultNonTaxableSaleTCRefListID = c.String(maxLength: 36),
                        SalesTaxPrefsDefaultNonTaxableSaleTCRefFullName = c.String(maxLength: 3),
                        TimeTrackingPrefsFirstDayOfWeek = c.String(maxLength: 9),
                        CurrentAppAccessRightsIsAutomaticLoginAllowed = c.Boolean(nullable: false),
                        CurrentAppAccessRightsAutomaticLoginUserName = c.String(maxLength: 29),
                        CurrentAppAccessRightsIsPersonalDataAccAllowed = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsEnhancedInventoryReceivingEnabled = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsIsTrackingSerialOrLotNumber = c.String(maxLength: 12),
                        ItemsAndInventoryPrefsIsTrackingOnSalesTransactionsEnabled = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsIsTrackingOnPurchaseTransactionsEnabled = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsIsTrackingOnInventoryAdjustmentEnabled = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsIsTrackingOnBuildAssemblyEnabled = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsFIFOEnabled = c.Boolean(nullable: false),
                        ItemsAndInventoryPrefsFIFOEffectiveDate = c.DateTime(nullable: false),
                        FinanceChargePrefsFinanceChargeAcctRefAccount_CompanyId = c.Int(),
                        FinanceChargePrefsFinanceChargeAcctRefAccount_ListID = c.String(maxLength: 36),
                        MultiCurrencyPrefsHomeCurrencyRefCurrency_CompanyId = c.Int(),
                        MultiCurrencyPrefsHomeCurrencyRefCurrency_ListID = c.String(maxLength: 36),
                        PurchasesAndVendorsPrefDefaultDisARefAccount_CompanyId = c.Int(),
                        PurchasesAndVendorsPrefDefaultDisARefAccount_ListID = c.String(maxLength: 36),
                        SalesAndCustomersPrefsDeftShipMethRefShipMethod_CompanyId = c.Int(),
                        SalesAndCustomersPrefsDeftShipMethRefShipMethod_ListID = c.String(maxLength: 36),
                        SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => new { t.FinanceChargePrefsFinanceChargeAcctRefAccount_CompanyId, t.FinanceChargePrefsFinanceChargeAcctRefAccount_ListID })
                .ForeignKey("dbo.Currencies", t => new { t.MultiCurrencyPrefsHomeCurrencyRefCurrency_CompanyId, t.MultiCurrencyPrefsHomeCurrencyRefCurrency_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.PurchasesAndVendorsPrefDefaultDisARefAccount_CompanyId, t.PurchasesAndVendorsPrefDefaultDisARefAccount_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.SalesAndCustomersPrefsDeftShipMethRefShipMethod_CompanyId, t.SalesAndCustomersPrefsDeftShipMethRefShipMethod_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_CompanyId, t.SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_CompanyId, t.SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_CompanyId, t.SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_ListID })
                .Index(t => new { t.FinanceChargePrefsFinanceChargeAcctRefAccount_CompanyId, t.FinanceChargePrefsFinanceChargeAcctRefAccount_ListID })
                .Index(t => new { t.MultiCurrencyPrefsHomeCurrencyRefCurrency_CompanyId, t.MultiCurrencyPrefsHomeCurrencyRefCurrency_ListID })
                .Index(t => new { t.PurchasesAndVendorsPrefDefaultDisARefAccount_CompanyId, t.PurchasesAndVendorsPrefDefaultDisARefAccount_ListID })
                .Index(t => new { t.SalesAndCustomersPrefsDeftShipMethRefShipMethod_CompanyId, t.SalesAndCustomersPrefsDeftShipMethRefShipMethod_ListID })
                .Index(t => new { t.SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_CompanyId, t.SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_CompanyId, t.SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_CompanyId, t.SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_ListID });
            
            CreateTable(
                "dbo.PriceLevelPerItems",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        PriceLevelType = c.String(maxLength: 15),
                        PriceLevelFixedPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceLevelPerItemSeqNo = c.Int(nullable: false),
                        PriceLevelPerItemItemRefListID = c.String(maxLength: 36),
                        PriceLevelPerItemItemRefFullName = c.String(maxLength: 159),
                        PriceLevelPerItemCustomPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceLevelPerItemCustomPricePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceLevelPerItemAdjustPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceLevelPerItemAdjustRelativeTo = c.String(maxLength: 15),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        PriceLevel_CompanyId = c.Int(),
                        PriceLevel_ListID = c.String(maxLength: 36),
                        PriceLevelPerItemItemRefItem_CompanyId = c.Int(),
                        PriceLevelPerItemItemRefItem_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.PriceLevel_CompanyId, t.PriceLevel_ListID })
                .ForeignKey("dbo.Items", t => new { t.PriceLevelPerItemItemRefItem_CompanyId, t.PriceLevelPerItemItemRefItem_ListID })
                .Index(t => new { t.PriceLevel_CompanyId, t.PriceLevel_ListID })
                .Index(t => new { t.PriceLevelPerItemItemRefItem_CompanyId, t.PriceLevelPerItemItemRefItem_ListID });
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ShipToEntityRefListID = c.String(maxLength: 36),
                        ShipToEntityRefFullName = c.String(maxLength: 209),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        VendorAddressAddr1 = c.String(maxLength: 41),
                        VendorAddressAddr2 = c.String(maxLength: 41),
                        VendorAddressAddr3 = c.String(maxLength: 41),
                        VendorAddressAddr4 = c.String(maxLength: 41),
                        VendorAddressAddr5 = c.String(maxLength: 41),
                        VendorAddressCity = c.String(maxLength: 31),
                        VendorAddressState = c.String(maxLength: 21),
                        VendorAddressProvince = c.String(maxLength: 21),
                        VendorAddressCounty = c.String(maxLength: 21),
                        VendorAddressPostalCode = c.String(maxLength: 13),
                        VendorAddressCountry = c.String(maxLength: 31),
                        VendorAddressNote = c.String(maxLength: 41),
                        VendorAddressBlockAddr1 = c.String(maxLength: 41),
                        VendorAddressBlockAddr2 = c.String(maxLength: 41),
                        VendorAddressBlockAddr3 = c.String(maxLength: 41),
                        VendorAddressBlockAddr4 = c.String(maxLength: 41),
                        VendorAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        ExpectedDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        FOB = c.String(maxLength: 13),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsManuallyClosed = c.Boolean(nullable: false),
                        IsFullyReceived = c.Boolean(nullable: false),
                        Memo = c.String(),
                        VendorMsg = c.String(maxLength: 99),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        CustomFieldOther1 = c.String(maxLength: 29),
                        CustomFieldOther2 = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ShipToEntityRefEntity_CompanyId = c.Int(),
                        ShipToEntityRefEntity_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Entities", t => new { t.ShipToEntityRefEntity_CompanyId, t.ShipToEntityRefEntity_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ShipToEntityRefEntity_CompanyId, t.ShipToEntityRefEntity_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.PurchaseOrderLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ShipToEntityRefListID = c.String(maxLength: 36),
                        ShipToEntityRefFullName = c.String(maxLength: 209),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        VendorAddressAddr1 = c.String(maxLength: 41),
                        VendorAddressAddr2 = c.String(maxLength: 41),
                        VendorAddressAddr3 = c.String(maxLength: 41),
                        VendorAddressAddr4 = c.String(maxLength: 41),
                        VendorAddressAddr5 = c.String(maxLength: 41),
                        VendorAddressCity = c.String(maxLength: 31),
                        VendorAddressState = c.String(maxLength: 21),
                        VendorAddressProvince = c.String(maxLength: 21),
                        VendorAddressCounty = c.String(maxLength: 21),
                        VendorAddressPostalCode = c.String(maxLength: 13),
                        VendorAddressCountry = c.String(maxLength: 31),
                        VendorAddressNote = c.String(maxLength: 41),
                        VendorAddressBlockAddr1 = c.String(maxLength: 41),
                        VendorAddressBlockAddr2 = c.String(maxLength: 41),
                        VendorAddressBlockAddr3 = c.String(maxLength: 41),
                        VendorAddressBlockAddr4 = c.String(maxLength: 41),
                        VendorAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        ExpectedDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        FOB = c.String(maxLength: 13),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsManuallyClosed = c.Boolean(nullable: false),
                        IsFullyReceived = c.Boolean(nullable: false),
                        Memo = c.String(),
                        VendorMsg = c.String(maxLength: 99),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        CustomFieldOther1 = c.String(maxLength: 29),
                        CustomFieldOther2 = c.String(maxLength: 29),
                        PurchaseOrderLineType = c.String(maxLength: 11),
                        PurchaseOrderLineSeqNo = c.Int(nullable: false),
                        PurchaseOrderLineGroupTxnLineID = c.String(maxLength: 36),
                        PurchaseOrderLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineGroupItemGroupFullName = c.String(maxLength: 159),
                        PurchaseOrderLineGroupDesc = c.String(),
                        PurchaseOrderLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        PurchaseOrderLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        PurchaseOrderLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        PurchaseOrderLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineGroupServiceDate = c.DateTime(nullable: false),
                        PurchaseOrderLineGroupSeqNo = c.Int(nullable: false),
                        PurchaseOrderLineTxnLineID = c.String(maxLength: 36),
                        PurchaseOrderLineItemRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineItemRefFullName = c.String(maxLength: 159),
                        PurchaseOrderLineManufacturerPartNumber = c.String(maxLength: 31),
                        PurchaseOrderLineDesc = c.String(),
                        PurchaseOrderLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineUnitOfMeasure = c.String(maxLength: 31),
                        PurchaseOrderLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        PurchaseOrderLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineClassRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineClassRefFullName = c.String(maxLength: 159),
                        PurchaseOrderLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineCustomerRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineCustomerRefFullName = c.String(maxLength: 209),
                        PurchaseOrderLineServiceDate = c.DateTime(nullable: false),
                        PurchaseOrderLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineTaxCodeRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        PurchaseOrderLineTaxCodeRefFullName = c.String(maxLength: 3),
                        PurchaseOrderLineReceivedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineUnbilledQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseOrderLineIsBilled = c.Boolean(nullable: false),
                        PurchaseOrderLineIsManuallyClosed = c.Boolean(nullable: false),
                        PurchaseOrderLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        PurchaseOrderLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        CustomFieldPurchaseOrderLineOther1 = c.String(maxLength: 29),
                        CustomFieldPurchaseOrderLineOther2 = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ShipToEntityRefEntity_CompanyId = c.Int(),
                        ShipToEntityRefEntity_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        PurchaseOrderLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineItemRefItem_CompanyId = c.Int(),
                        PurchaseOrderLineItemRefItem_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineClassRefClass_CompanyId = c.Int(),
                        PurchaseOrderLineClassRefClass_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineCustomerRefCustomer_CompanyId = c.Int(),
                        PurchaseOrderLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        PurchaseOrderLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        PurchaseOrderLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        PurchaseOrderLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Entities", t => new { t.ShipToEntityRefEntity_CompanyId, t.ShipToEntityRefEntity_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.PurchaseOrderLineGroupItemGroupRefItem_CompanyId, t.PurchaseOrderLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.PurchaseOrderLineItemRefItem_CompanyId, t.PurchaseOrderLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Classes", t => new { t.PurchaseOrderLineClassRefClass_CompanyId, t.PurchaseOrderLineClassRefClass_ListID })
                .ForeignKey("dbo.Customers", t => new { t.PurchaseOrderLineCustomerRefCustomer_CompanyId, t.PurchaseOrderLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.PurchaseOrderLineTaxCodeRefTaxCode_CompanyId, t.PurchaseOrderLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.PurchaseOrderLineOverrideItemAccountRefAccount_CompanyId, t.PurchaseOrderLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ShipToEntityRefEntity_CompanyId, t.ShipToEntityRefEntity_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.PurchaseOrderLineGroupItemGroupRefItem_CompanyId, t.PurchaseOrderLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.PurchaseOrderLineItemRefItem_CompanyId, t.PurchaseOrderLineItemRefItem_ListID })
                .Index(t => new { t.PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.PurchaseOrderLineClassRefClass_CompanyId, t.PurchaseOrderLineClassRefClass_ListID })
                .Index(t => new { t.PurchaseOrderLineCustomerRefCustomer_CompanyId, t.PurchaseOrderLineCustomerRefCustomer_ListID })
                .Index(t => new { t.PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.PurchaseOrderLineTaxCodeRefTaxCode_CompanyId, t.PurchaseOrderLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.PurchaseOrderLineOverrideItemAccountRefAccount_CompanyId, t.PurchaseOrderLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.PurchaseOrderLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ShipToEntityRefListID = c.String(maxLength: 36),
                        ShipToEntityRefFullName = c.String(maxLength: 209),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        VendorAddressAddr1 = c.String(maxLength: 41),
                        VendorAddressAddr2 = c.String(maxLength: 41),
                        VendorAddressAddr3 = c.String(maxLength: 41),
                        VendorAddressAddr4 = c.String(maxLength: 41),
                        VendorAddressAddr5 = c.String(maxLength: 41),
                        VendorAddressCity = c.String(maxLength: 31),
                        VendorAddressState = c.String(maxLength: 21),
                        VendorAddressProvince = c.String(maxLength: 21),
                        VendorAddressCounty = c.String(maxLength: 21),
                        VendorAddressPostalCode = c.String(maxLength: 13),
                        VendorAddressCountry = c.String(maxLength: 31),
                        VendorAddressNote = c.String(maxLength: 41),
                        VendorAddressBlockAddr1 = c.String(maxLength: 41),
                        VendorAddressBlockAddr2 = c.String(maxLength: 41),
                        VendorAddressBlockAddr3 = c.String(maxLength: 41),
                        VendorAddressBlockAddr4 = c.String(maxLength: 41),
                        VendorAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        ExpectedDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        FOB = c.String(maxLength: 13),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsManuallyClosed = c.Boolean(nullable: false),
                        IsFullyReceived = c.Boolean(nullable: false),
                        Memo = c.String(),
                        VendorMsg = c.String(maxLength: 99),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        CustomFieldOther1 = c.String(maxLength: 29),
                        CustomFieldOther2 = c.String(maxLength: 29),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 11),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ShipToEntityRefEntity_CompanyId = c.Int(),
                        ShipToEntityRefEntity_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Entities", t => new { t.ShipToEntityRefEntity_CompanyId, t.ShipToEntityRefEntity_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ShipToEntityRefEntity_CompanyId, t.ShipToEntityRefEntity_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.ReceivePayments",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 20),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        CreditCardTxnInfoInputCreditCardNumber = c.String(maxLength: 25),
                        CreditCardTxnInfoInputExpirationMonth = c.Int(nullable: false),
                        CreditCardTxnInfoInputExpirationYear = c.Int(nullable: false),
                        CreditCardTxnInfoInputNameOnCard = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardAddress = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardPostalCode = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCommercialCardCode = c.String(maxLength: 50),
                        CreditCardTxnInfoInputTransactionMode = c.String(maxLength: 14),
                        CreditCardTxnInfoInputCreditCardTxnType = c.String(maxLength: 18),
                        CreditCardTxnInfoResultResultCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultResultMessage = c.String(maxLength: 500),
                        CreditCardTxnInfoResultCreditCardTransID = c.String(maxLength: 24),
                        CreditCardTxnInfoResultMerchantAccountNumber = c.String(maxLength: 32),
                        CreditCardTxnInfoResultAuthorizationCode = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSStreet = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSZip = c.String(maxLength: 12),
                        CreditCardTxnInfoResultCardSecurityCodeMatch = c.String(maxLength: 12),
                        CreditCardTxnInfoResultReconBatchID = c.String(maxLength: 84),
                        CreditCardTxnInfoResultPaymentGroupingCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultPaymentStatus = c.String(maxLength: 9),
                        CreditCardTxnInfoResultTxnAuthorizationTime = c.DateTime(nullable: false),
                        CreditCardTxnInfoResultTxnAuthorizationStamp = c.Int(nullable: false),
                        CreditCardTxnInfoResultClientTransID = c.String(maxLength: 16),
                        IsAutoApply = c.Boolean(nullable: false),
                        UnusedPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnusedCredits = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.ReceivePaymentLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 20),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        Memo = c.String(),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        CreditCardTxnInfoInputCreditCardNumber = c.String(maxLength: 25),
                        CreditCardTxnInfoInputExpirationMonth = c.Int(nullable: false),
                        CreditCardTxnInfoInputExpirationYear = c.Int(nullable: false),
                        CreditCardTxnInfoInputNameOnCard = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardAddress = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardPostalCode = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCommercialCardCode = c.String(maxLength: 50),
                        CreditCardTxnInfoInputTransactionMode = c.String(maxLength: 14),
                        CreditCardTxnInfoInputCreditCardTxnType = c.String(maxLength: 18),
                        CreditCardTxnInfoResultResultCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultResultMessage = c.String(maxLength: 500),
                        CreditCardTxnInfoResultCreditCardTransID = c.String(maxLength: 24),
                        CreditCardTxnInfoResultMerchantAccountNumber = c.String(maxLength: 32),
                        CreditCardTxnInfoResultAuthorizationCode = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSStreet = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSZip = c.String(maxLength: 12),
                        CreditCardTxnInfoResultCardSecurityCodeMatch = c.String(maxLength: 12),
                        CreditCardTxnInfoResultReconBatchID = c.String(maxLength: 84),
                        CreditCardTxnInfoResultPaymentGroupingCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultPaymentStatus = c.String(maxLength: 9),
                        CreditCardTxnInfoResultTxnAuthorizationTime = c.DateTime(nullable: false),
                        CreditCardTxnInfoResultTxnAuthorizationStamp = c.Int(nullable: false),
                        CreditCardTxnInfoResultClientTransID = c.String(maxLength: 16),
                        IsAutoApply = c.Boolean(nullable: false),
                        UnusedPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnusedCredits = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnTxnID = c.String(nullable: false, maxLength: 36),
                        AppliedToTxnPaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnTxnType = c.String(maxLength: 14),
                        AppliedToTxnTxnDate = c.DateTime(nullable: false),
                        AppliedToTxnRefNumber = c.String(maxLength: 20),
                        AppliedToTxnBalanceRemaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnSetCreditCreditTxnID = c.String(maxLength: 36),
                        AppliedToTxnSetCreditAppliedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedToTxnDiscountAccountRefListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountAccountRefFullName = c.String(maxLength: 159),
                        AppliedToTxnDiscountClassRefListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountClassRefFullName = c.String(maxLength: 159),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountAccountRefAccount_CompanyId = c.Int(),
                        AppliedToTxnDiscountAccountRefAccount_ListID = c.String(maxLength: 36),
                        AppliedToTxnDiscountClassRefClass_CompanyId = c.Int(),
                        AppliedToTxnDiscountClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AppliedToTxnDiscountAccountRefAccount_CompanyId, t.AppliedToTxnDiscountAccountRefAccount_ListID })
                .ForeignKey("dbo.Classes", t => new { t.AppliedToTxnDiscountClassRefClass_CompanyId, t.AppliedToTxnDiscountClassRefClass_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.AppliedToTxnDiscountAccountRefAccount_CompanyId, t.AppliedToTxnDiscountAccountRefAccount_ListID })
                .Index(t => new { t.AppliedToTxnDiscountClassRefClass_CompanyId, t.AppliedToTxnDiscountClassRefClass_ListID });
            
            CreateTable(
                "dbo.ReceivePaymentToDeposits",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TxnLineID = c.String(maxLength: 21),
                        TxnType = c.String(maxLength: 14),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        RefNumber = c.String(maxLength: 20),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID });
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        CheckNumber = c.String(maxLength: 25),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        IsFinanceCharge = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        SuggestedDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SuggestedDiscountDate = c.DateTime(nullable: false),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        CreditMemoLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        Type = c.String(maxLength: 20),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.SalesLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        ARAccountRefListID = c.String(maxLength: 36),
                        ARAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        CheckNumber = c.String(maxLength: 25),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        IsFinanceCharge = c.Boolean(nullable: false),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        SuggestedDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SuggestedDiscountDate = c.DateTime(nullable: false),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        CreditMemoLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        Type = c.String(maxLength: 20),
                        SalesLineType = c.String(maxLength: 11),
                        SalesLineSeqNo = c.Int(nullable: false),
                        SalesLineGroupTxnLineID = c.String(maxLength: 36),
                        SalesLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        SalesLineGroupItemGroupRefFullName = c.String(maxLength: 31),
                        SalesLineGroupDesc = c.String(),
                        SalesLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        SalesLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        SalesLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesLineGroupSeqNo = c.Int(nullable: false),
                        SalesLineTxnLineID = c.String(maxLength: 36),
                        SalesLineItemRefListID = c.String(maxLength: 36),
                        SalesLineItemRefFullName = c.String(maxLength: 159),
                        SalesLineDesc = c.String(),
                        SalesLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesLineUnitOfMeasure = c.String(maxLength: 31),
                        SalesLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesLineRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesLineClassRefListID = c.String(maxLength: 36),
                        SalesLineClassRefFullName = c.String(maxLength: 159),
                        SalesLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesLineServiceDate = c.DateTime(nullable: false),
                        SalesLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesLineTaxCodeRefListID = c.String(maxLength: 36),
                        SalesLineTaxCodeRefFullName = c.String(maxLength: 3),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        ARAccountRefAccount_CompanyId = c.Int(),
                        ARAccountRefAccount_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        SalesLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        SalesLineItemRefItem_CompanyId = c.Int(),
                        SalesLineItemRefItem_ListID = c.String(maxLength: 36),
                        SalesLineClassRefClass_CompanyId = c.Int(),
                        SalesLineClassRefClass_ListID = c.String(maxLength: 36),
                        SalesLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        SalesLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.Items", t => new { t.SalesLineGroupItemGroupRefItem_CompanyId, t.SalesLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.Items", t => new { t.SalesLineItemRefItem_CompanyId, t.SalesLineItemRefItem_ListID })
                .ForeignKey("dbo.Classes", t => new { t.SalesLineClassRefClass_CompanyId, t.SalesLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.SalesLineTaxCodeRefTaxCode_CompanyId, t.SalesLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.ARAccountRefAccount_CompanyId, t.ARAccountRefAccount_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.SalesLineGroupItemGroupRefItem_CompanyId, t.SalesLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.SalesLineItemRefItem_CompanyId, t.SalesLineItemRefItem_ListID })
                .Index(t => new { t.SalesLineClassRefClass_CompanyId, t.SalesLineClassRefClass_ListID })
                .Index(t => new { t.SalesLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesLineTaxCodeRefTaxCode_CompanyId, t.SalesLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.SalesOrders",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsManuallyClosed = c.Boolean(nullable: false),
                        IsFullyInvoiced = c.Boolean(nullable: false),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.SalesOrderLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsManuallyClosed = c.Boolean(nullable: false),
                        IsFullyInvoiced = c.Boolean(nullable: false),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        SalesOrderLineSeqNo = c.Int(nullable: false),
                        SalesOrderLineGroupTxnLineID = c.String(maxLength: 36),
                        SalesOrderLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        SalesOrderLineGroupItemGroupRefFullName = c.String(maxLength: 159),
                        SalesOrderLineGroupDesc = c.String(),
                        SalesOrderLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        SalesOrderLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        SalesOrderLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        SalesOrderLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        SalesOrderLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineGroupSeqNo = c.Int(nullable: false),
                        SalesOrderLineTxnLineID = c.String(maxLength: 36),
                        SalesOrderLineItemRefListID = c.String(maxLength: 36),
                        SalesOrderLineItemRefFullName = c.String(maxLength: 159),
                        SalesOrderLineDesc = c.String(),
                        SalesOrderLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineUnitOfMeasure = c.String(maxLength: 31),
                        SalesOrderLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        SalesOrderLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        SalesOrderLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLinePriceLevelRefListID = c.String(maxLength: 36),
                        SalesOrderLinePriceLevelRefFullName = c.String(maxLength: 159),
                        SalesOrderLineClassRefListID = c.String(maxLength: 36),
                        SalesOrderLineClassRefFullName = c.String(maxLength: 159),
                        SalesOrderLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesOrderLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesOrderLineTaxCodeRefListID = c.String(maxLength: 36),
                        SalesOrderLineTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesOrderLineInvoiced = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderLineIsManuallyClosed = c.Boolean(nullable: false),
                        CustomFieldSalesOrderLineOther1 = c.String(maxLength: 29),
                        CustomFieldSalesOrderLineOther2 = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        SalesOrderLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        SalesOrderLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesOrderLineItemRefItem_CompanyId = c.Int(),
                        SalesOrderLineItemRefItem_ListID = c.String(maxLength: 36),
                        SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesOrderLinePriceLevelRefPriceLevel_CompanyId = c.Int(),
                        SalesOrderLinePriceLevelRefPriceLevel_ListID = c.String(maxLength: 36),
                        SalesOrderLineClassRefClass_CompanyId = c.Int(),
                        SalesOrderLineClassRefClass_ListID = c.String(maxLength: 36),
                        SalesOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesOrderLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesOrderLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        SalesOrderLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.SalesOrderLineGroupItemGroupRefItem_CompanyId, t.SalesOrderLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.SalesOrderLineItemRefItem_CompanyId, t.SalesOrderLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.SalesOrderLinePriceLevelRefPriceLevel_CompanyId, t.SalesOrderLinePriceLevelRefPriceLevel_ListID })
                .ForeignKey("dbo.Classes", t => new { t.SalesOrderLineClassRefClass_CompanyId, t.SalesOrderLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesOrderLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.SalesOrderLineTaxCodeRefTaxCode_CompanyId, t.SalesOrderLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.SalesOrderLineGroupItemGroupRefItem_CompanyId, t.SalesOrderLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesOrderLineItemRefItem_CompanyId, t.SalesOrderLineItemRefItem_ListID })
                .Index(t => new { t.SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesOrderLinePriceLevelRefPriceLevel_CompanyId, t.SalesOrderLinePriceLevelRefPriceLevel_ListID })
                .Index(t => new { t.SalesOrderLineClassRefClass_CompanyId, t.SalesOrderLineClassRefClass_ListID })
                .Index(t => new { t.SalesOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesOrderLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesOrderLineTaxCodeRefTaxCode_CompanyId, t.SalesOrderLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.SalesOrderLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(nullable: false, maxLength: 36),
                        CustomerRefFullName = c.String(nullable: false, maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        PONumber = c.String(maxLength: 25),
                        TermsRefListID = c.String(maxLength: 36),
                        TermsRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        FOB = c.String(maxLength: 13),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsManuallyClosed = c.Boolean(nullable: false),
                        IsFullyInvoiced = c.Boolean(nullable: false),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        CustomFieldOther = c.String(maxLength: 29),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 8),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        TermsRefTerms_CompanyId = c.Int(),
                        TermsRefTerms_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.Terms", t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.TermsRefTerms_CompanyId, t.TermsRefTerms_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.SalesReceipts",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        CheckNumber = c.String(maxLength: 25),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        FOB = c.String(maxLength: 13),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        CreditCardTxnInfoInputCreditCardNumber = c.String(maxLength: 25),
                        CreditCardTxnInfoInputExpirationMonth = c.Int(nullable: false),
                        CreditCardTxnInfoInputExpirationYear = c.Int(nullable: false),
                        CreditCardTxnInfoInputNameOnCard = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardAddress = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardPostalCode = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCommercialCardCode = c.String(maxLength: 50),
                        CreditCardTxnInfoInputTransactionMode = c.String(maxLength: 14),
                        CreditCardTxnInfoInputCreditCardTxnType = c.String(maxLength: 18),
                        CreditCardTxnInfoResultResultCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultResultMessage = c.String(maxLength: 500),
                        CreditCardTxnInfoResultCreditCardTransID = c.String(maxLength: 24),
                        CreditCardTxnInfoResultMerchantAccountNumber = c.String(maxLength: 32),
                        CreditCardTxnInfoResultAuthorizationCode = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSStreet = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSZip = c.String(maxLength: 12),
                        CreditCardTxnInfoResultCardSecurityCodeMatch = c.String(maxLength: 12),
                        CreditCardTxnInfoResultReconBatchID = c.String(maxLength: 84),
                        CreditCardTxnInfoResultPaymentGroupingCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultPaymentStatus = c.String(maxLength: 9),
                        CreditCardTxnInfoResultTxnAuthorizationTime = c.DateTime(nullable: false),
                        CreditCardTxnInfoResultTxnAuthorizationStamp = c.Int(nullable: false),
                        CreditCardTxnInfoResultClientTransID = c.String(maxLength: 16),
                        CustomFieldOther = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.SalesReceiptLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TemplateRefListID = c.String(maxLength: 36),
                        TemplateRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 11),
                        BillAddressAddr1 = c.String(maxLength: 41),
                        BillAddressAddr2 = c.String(maxLength: 41),
                        BillAddressAddr3 = c.String(maxLength: 41),
                        BillAddressAddr4 = c.String(maxLength: 41),
                        BillAddressAddr5 = c.String(maxLength: 41),
                        BillAddressCity = c.String(maxLength: 31),
                        BillAddressState = c.String(maxLength: 21),
                        BillAddressProvince = c.String(maxLength: 21),
                        BillAddressCounty = c.String(maxLength: 21),
                        BillAddressPostalCode = c.String(maxLength: 13),
                        BillAddressCountry = c.String(maxLength: 31),
                        BillAddressNote = c.String(maxLength: 41),
                        BillAddressBlockAddr1 = c.String(maxLength: 41),
                        BillAddressBlockAddr2 = c.String(maxLength: 41),
                        BillAddressBlockAddr3 = c.String(maxLength: 41),
                        BillAddressBlockAddr4 = c.String(maxLength: 41),
                        BillAddressBlockAddr5 = c.String(maxLength: 41),
                        ShipAddressAddr1 = c.String(maxLength: 41),
                        ShipAddressAddr2 = c.String(maxLength: 41),
                        ShipAddressAddr3 = c.String(maxLength: 41),
                        ShipAddressAddr4 = c.String(maxLength: 41),
                        ShipAddressAddr5 = c.String(maxLength: 41),
                        ShipAddressCity = c.String(maxLength: 31),
                        ShipAddressState = c.String(maxLength: 21),
                        ShipAddressProvince = c.String(maxLength: 21),
                        ShipAddressCounty = c.String(maxLength: 21),
                        ShipAddressPostalCode = c.String(maxLength: 13),
                        ShipAddressCountry = c.String(maxLength: 31),
                        ShipAddressNote = c.String(maxLength: 41),
                        ShipAddressBlockAddr1 = c.String(maxLength: 41),
                        ShipAddressBlockAddr2 = c.String(maxLength: 41),
                        ShipAddressBlockAddr3 = c.String(maxLength: 41),
                        ShipAddressBlockAddr4 = c.String(maxLength: 41),
                        ShipAddressBlockAddr5 = c.String(maxLength: 41),
                        IsPending = c.Boolean(nullable: false),
                        CheckNumber = c.String(maxLength: 25),
                        PaymentMethodRefListID = c.String(maxLength: 36),
                        PaymentMethodRefFullName = c.String(maxLength: 31),
                        DueDate = c.DateTime(nullable: false),
                        SalesRepRefListID = c.String(maxLength: 36),
                        SalesRepRefFullName = c.String(maxLength: 5),
                        ShipDate = c.DateTime(nullable: false),
                        ShipMethodRefListID = c.String(maxLength: 36),
                        ShipMethodRefFullName = c.String(maxLength: 15),
                        FOB = c.String(maxLength: 13),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemSalesTaxRefListID = c.String(maxLength: 36),
                        ItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        CustomerMsgRefListID = c.String(maxLength: 36),
                        CustomerMsgRefFullName = c.String(maxLength: 101),
                        IsToBePrinted = c.Boolean(nullable: false),
                        IsToBeEmailed = c.Boolean(nullable: false),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        CustomerSalesTaxCodeRefListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        CustomerTaxCodeRefListID = c.String(maxLength: 36),
                        DepositToAccountRefListID = c.String(maxLength: 36),
                        DepositToAccountRefFullName = c.String(maxLength: 159),
                        CreditCardTxnInfoInputCreditCardNumber = c.String(maxLength: 25),
                        CreditCardTxnInfoInputExpirationMonth = c.Int(nullable: false),
                        CreditCardTxnInfoInputExpirationYear = c.Int(nullable: false),
                        CreditCardTxnInfoInputNameOnCard = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardAddress = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCreditCardPostalCode = c.String(maxLength: 41),
                        CreditCardTxnInfoInputCommercialCardCode = c.String(maxLength: 50),
                        CreditCardTxnInfoInputTransactionMode = c.String(maxLength: 14),
                        CreditCardTxnInfoInputCreditCardTxnType = c.String(maxLength: 18),
                        CreditCardTxnInfoResultResultCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultResultMessage = c.String(maxLength: 500),
                        CreditCardTxnInfoResultCreditCardTransID = c.String(maxLength: 24),
                        CreditCardTxnInfoResultMerchantAccountNumber = c.String(maxLength: 32),
                        CreditCardTxnInfoResultAuthorizationCode = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSStreet = c.String(maxLength: 12),
                        CreditCardTxnInfoResultAVSZip = c.String(maxLength: 12),
                        CreditCardTxnInfoResultCardSecurityCodeMatch = c.String(maxLength: 12),
                        CreditCardTxnInfoResultReconBatchID = c.String(maxLength: 84),
                        CreditCardTxnInfoResultPaymentGroupingCode = c.Int(nullable: false),
                        CreditCardTxnInfoResultPaymentStatus = c.String(maxLength: 9),
                        CreditCardTxnInfoResultTxnAuthorizationTime = c.DateTime(nullable: false),
                        CreditCardTxnInfoResultTxnAuthorizationStamp = c.Int(nullable: false),
                        CreditCardTxnInfoResultClientTransID = c.String(maxLength: 16),
                        CustomFieldOther = c.String(maxLength: 29),
                        SalesReceiptLineItemLineType = c.String(maxLength: 11),
                        SalesReceiptLineSeqNo = c.Int(nullable: false),
                        SalesReceiptLineGroupTxnLineID = c.String(maxLength: 36),
                        SalesReceiptLineGroupItemGroupRefListID = c.String(maxLength: 36),
                        SalesReceiptLineGroupItemGroupRefFullName = c.String(maxLength: 159),
                        SalesReceiptLineGroupDesc = c.String(),
                        SalesReceiptLineGroupQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptLineGroupUnitOfMeasure = c.String(maxLength: 31),
                        SalesReceiptLineGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        SalesReceiptLineGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        SalesReceiptLineGroupIsPrintItemsInGroup = c.Boolean(nullable: false),
                        SalesReceiptLineGroupTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptLineGroupServiceDate = c.DateTime(nullable: false),
                        SalesReceiptLineGroupSeqNo = c.Int(nullable: false),
                        SalesReceiptLineTxnLineID = c.String(maxLength: 36),
                        SalesReceiptLineItemRefListID = c.String(maxLength: 36),
                        SalesReceiptLineItemRefFullName = c.String(maxLength: 159),
                        SalesReceiptLineDesc = c.String(),
                        SalesReceiptLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptLineUnitOfMeasure = c.String(maxLength: 31),
                        SalesReceiptLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        SalesReceiptLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        SalesReceiptLineRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptLineRatePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptLinePriceLevelRefListID = c.String(maxLength: 36),
                        SalesReceiptLinePriceLevelRefFullName = c.String(maxLength: 159),
                        SalesReceiptLineClassRefListID = c.String(maxLength: 36),
                        SalesReceiptLineClassRefFullName = c.String(maxLength: 159),
                        SalesReceiptLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesReceiptLineServiceDate = c.DateTime(nullable: false),
                        SalesReceiptLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesReceiptLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesReceiptLineTaxCodeRefListID = c.String(maxLength: 36),
                        SalesReceiptLineTaxCodeRefFullName = c.String(maxLength: 3),
                        SalesReceiptLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        SalesReceiptLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        CustomFieldSalesReceiptLineOther1 = c.String(maxLength: 29),
                        CustomFieldSalesReceiptLineOther2 = c.String(maxLength: 29),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        TemplateRefTemplate_CompanyId = c.Int(),
                        TemplateRefTemplate_ListID = c.String(maxLength: 36),
                        PaymentMethodRefPaymentMethod_CompanyId = c.Int(),
                        PaymentMethodRefPaymentMethod_ListID = c.String(maxLength: 36),
                        SalesRepRefSalesRep_CompanyId = c.Int(),
                        SalesRepRefSalesRep_ListID = c.String(maxLength: 36),
                        ShipMethodRefShipMethod_CompanyId = c.Int(),
                        ShipMethodRefShipMethod_ListID = c.String(maxLength: 36),
                        ItemSalesTaxRefItemSalesTax_CompanyId = c.Int(),
                        ItemSalesTaxRefItemSalesTax_ListID = c.String(maxLength: 36),
                        CustomerMsgRefCustomerMsg_CompanyId = c.Int(),
                        CustomerMsgRefCustomerMsg_ListID = c.String(maxLength: 36),
                        CustomerSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        CustomerSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        CustomerTaxCodeRefTaxCode_CompanyId = c.Int(),
                        CustomerTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        DepositToAccountRefAccount_CompanyId = c.Int(),
                        DepositToAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesReceiptLineGroupItemGroupRefItem_CompanyId = c.Int(),
                        SalesReceiptLineGroupItemGroupRefItem_ListID = c.String(maxLength: 36),
                        SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesReceiptLineItemRefItem_CompanyId = c.Int(),
                        SalesReceiptLineItemRefItem_ListID = c.String(maxLength: 36),
                        SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        SalesReceiptLinePriceLevelRefPriceLevel_CompanyId = c.Int(),
                        SalesReceiptLinePriceLevelRefPriceLevel_ListID = c.String(maxLength: 36),
                        SalesReceiptLineClassRefClass_CompanyId = c.Int(),
                        SalesReceiptLineClassRefClass_ListID = c.String(maxLength: 36),
                        SalesReceiptLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesReceiptLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        SalesReceiptLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        SalesReceiptLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        SalesReceiptLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        SalesReceiptLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.Templates", t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .ForeignKey("dbo.PaymentMethods", t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .ForeignKey("dbo.SalesReps", t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .ForeignKey("dbo.ShipMethods", t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .ForeignKey("dbo.ItemSalesTaxes", t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .ForeignKey("dbo.CustomerMsgs", t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .ForeignKey("dbo.Items", t => new { t.SalesReceiptLineGroupItemGroupRefItem_CompanyId, t.SalesReceiptLineGroupItemGroupRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.SalesReceiptLineItemRefItem_CompanyId, t.SalesReceiptLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.PriceLevels", t => new { t.SalesReceiptLinePriceLevelRefPriceLevel_CompanyId, t.SalesReceiptLinePriceLevelRefPriceLevel_ListID })
                .ForeignKey("dbo.Classes", t => new { t.SalesReceiptLineClassRefClass_CompanyId, t.SalesReceiptLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesReceiptLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesReceiptLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.SalesReceiptLineTaxCodeRefTaxCode_CompanyId, t.SalesReceiptLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.SalesReceiptLineOverrideItemAccountRefAccount_CompanyId, t.SalesReceiptLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.TemplateRefTemplate_CompanyId, t.TemplateRefTemplate_ListID })
                .Index(t => new { t.PaymentMethodRefPaymentMethod_CompanyId, t.PaymentMethodRefPaymentMethod_ListID })
                .Index(t => new { t.SalesRepRefSalesRep_CompanyId, t.SalesRepRefSalesRep_ListID })
                .Index(t => new { t.ShipMethodRefShipMethod_CompanyId, t.ShipMethodRefShipMethod_ListID })
                .Index(t => new { t.ItemSalesTaxRefItemSalesTax_CompanyId, t.ItemSalesTaxRefItemSalesTax_ListID })
                .Index(t => new { t.CustomerMsgRefCustomerMsg_CompanyId, t.CustomerMsgRefCustomerMsg_ListID })
                .Index(t => new { t.CustomerSalesTaxCodeRefSalesTaxCode_CompanyId, t.CustomerSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.CustomerTaxCodeRefTaxCode_CompanyId, t.CustomerTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.DepositToAccountRefAccount_CompanyId, t.DepositToAccountRefAccount_ListID })
                .Index(t => new { t.SalesReceiptLineGroupItemGroupRefItem_CompanyId, t.SalesReceiptLineGroupItemGroupRefItem_ListID })
                .Index(t => new { t.SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesReceiptLineItemRefItem_CompanyId, t.SalesReceiptLineItemRefItem_ListID })
                .Index(t => new { t.SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.SalesReceiptLinePriceLevelRefPriceLevel_CompanyId, t.SalesReceiptLinePriceLevelRefPriceLevel_ListID })
                .Index(t => new { t.SalesReceiptLineClassRefClass_CompanyId, t.SalesReceiptLineClassRefClass_ListID })
                .Index(t => new { t.SalesReceiptLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesReceiptLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.SalesReceiptLineTaxCodeRefTaxCode_CompanyId, t.SalesReceiptLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.SalesReceiptLineOverrideItemAccountRefAccount_CompanyId, t.SalesReceiptLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.SalesTaxPaymentChecks",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        BankAccountRefListID = c.String(maxLength: 36),
                        BankAccountRefFullName = c.String(maxLength: 159),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        IsToBePrinted = c.Boolean(nullable: false),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        BankAccountRefAccount_CompanyId = c.Int(),
                        BankAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.SalesTaxPaymentCheckLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        PayeeEntityRefListID = c.String(maxLength: 36),
                        PayeeEntityRefFullName = c.String(maxLength: 209),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        BankAccountRefListID = c.String(maxLength: 36),
                        BankAccountRefFullName = c.String(maxLength: 159),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 11),
                        Memo = c.String(),
                        AddressAddr1 = c.String(maxLength: 41),
                        AddressAddr2 = c.String(maxLength: 41),
                        AddressAddr3 = c.String(maxLength: 41),
                        AddressAddr4 = c.String(maxLength: 41),
                        AddressAddr5 = c.String(maxLength: 41),
                        AddressCity = c.String(maxLength: 31),
                        AddressState = c.String(maxLength: 21),
                        AddressProvince = c.String(maxLength: 21),
                        AddressCounty = c.String(maxLength: 21),
                        AddressPostalCode = c.String(maxLength: 13),
                        AddressCountry = c.String(maxLength: 31),
                        AddressNote = c.String(maxLength: 41),
                        AddressBlockAddr1 = c.String(maxLength: 41),
                        AddressBlockAddr2 = c.String(maxLength: 41),
                        AddressBlockAddr3 = c.String(maxLength: 41),
                        AddressBlockAddr4 = c.String(maxLength: 41),
                        AddressBlockAddr5 = c.String(maxLength: 41),
                        IsToBePrinted = c.Boolean(nullable: false),
                        SalesTaxPaymentCheckLineSeqNo = c.Int(nullable: false),
                        SalesTaxPaymentCheckLineTxnLineID = c.String(maxLength: 36),
                        SalesTaxPaymentCheckLineItemSalesTaxRefListID = c.String(maxLength: 36),
                        SalesTaxPaymentCheckLineItemSalesTaxRefFullName = c.String(maxLength: 31),
                        SalesTaxPaymentCheckLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        PayeeEntityRefEntity_CompanyId = c.Int(),
                        PayeeEntityRefEntity_ListID = c.String(maxLength: 36),
                        BankAccountRefAccount_CompanyId = c.Int(),
                        BankAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_CompanyId, t.SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_ListID })
                .Index(t => new { t.PayeeEntityRefEntity_CompanyId, t.PayeeEntityRefEntity_ListID })
                .Index(t => new { t.BankAccountRefAccount_CompanyId, t.BankAccountRefAccount_ListID })
                .Index(t => new { t.SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_CompanyId, t.SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_ListID });
            
            CreateTable(
                "dbo.SpecialAccounts",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        SpecialAccountType = c.String(nullable: false, maxLength: 13),
                        CurrencyRefListID = c.String(maxLength: 36),
                        CurrencyRefFullName = c.String(maxLength: 64),
                        CurrencyRefCurrency_CompanyId = c.Int(),
                        CurrencyRefCurrency_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Currencies", t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID })
                .Index(t => new { t.CurrencyRefCurrency_CompanyId, t.CurrencyRefCurrency_ListID });
            
            CreateTable(
                "dbo.SpecialItems",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        SpecialItemType = c.String(nullable: false, maxLength: 13),
                        ExternalGUID = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.StandardTerms",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 41),
                        IsActive = c.Boolean(nullable: false),
                        StdDueDays = c.Int(nullable: false),
                        StdDiscountDays = c.Int(nullable: false),
                        DiscountPct = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Terms_CompanyId = c.Int(),
                        Terms_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Terms", t => new { t.Terms_CompanyId, t.Terms_ListID })
                .Index(t => new { t.Terms_CompanyId, t.Terms_ListID });
            
            CreateTable(
                "dbo.TimeTrackings",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        EntityRefListID = c.String(nullable: false, maxLength: 36),
                        EntityRefFullName = c.String(nullable: false, maxLength: 209),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ItemServiceRefListID = c.String(maxLength: 36),
                        ItemServiceRefFullName = c.String(maxLength: 159),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DurationMinutes = c.Int(nullable: false),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        PayrollItemWageRefListID = c.String(maxLength: 36),
                        PayrollItemWageRefFullName = c.String(maxLength: 31),
                        Notes = c.String(),
                        BillableStatus = c.String(maxLength: 23),
                        EntityRefEntity_CompanyId = c.Int(),
                        EntityRefEntity_ListID = c.String(maxLength: 36),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemServiceRefItemService_CompanyId = c.Int(),
                        ItemServiceRefItemService_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                        PayrollItemWageRefPayrollItemWage_CompanyId = c.Int(),
                        PayrollItemWageRefPayrollItemWage_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.EntityRefEntity_CompanyId, t.EntityRefEntity_ListID })
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.ItemServices", t => new { t.ItemServiceRefItemService_CompanyId, t.ItemServiceRefItemService_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .ForeignKey("dbo.PayrollItemWages", t => new { t.PayrollItemWageRefPayrollItemWage_CompanyId, t.PayrollItemWageRefPayrollItemWage_ListID })
                .Index(t => new { t.EntityRefEntity_CompanyId, t.EntityRefEntity_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ItemServiceRefItemService_CompanyId, t.ItemServiceRefItemService_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.PayrollItemWageRefPayrollItemWage_CompanyId, t.PayrollItemWageRefPayrollItemWage_ListID });
            
            CreateTable(
                "dbo.ToDoes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Notes = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        ReminderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnType = c.String(maxLength: 25),
                        TxnID = c.String(maxLength: 36),
                        TxnLineID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EntityRefListID = c.String(maxLength: 36),
                        EntityRefFullName = c.String(maxLength: 209),
                        AccountRefListID = c.String(maxLength: 36),
                        AccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        RefNumber = c.String(maxLength: 20),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                        TransactionDetailLevelFilter = c.String(maxLength: 23),
                        TransactionPostingStatusFilter = c.String(maxLength: 23),
                        TransactionPaidStatusFilter = c.String(maxLength: 23),
                        Empty = c.String(maxLength: 25),
                        FQTxnLinkKey = c.String(maxLength: 184),
                        FQJournalEntryLinkKey = c.String(maxLength: 184),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 184),
                        EntityRefEntity_CompanyId = c.Int(),
                        EntityRefEntity_ListID = c.String(maxLength: 36),
                        AccountRefAccount_CompanyId = c.Int(),
                        AccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Entities", t => new { t.EntityRefEntity_CompanyId, t.EntityRefEntity_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID })
                .Index(t => new { t.EntityRefEntity_CompanyId, t.EntityRefEntity_ListID })
                .Index(t => new { t.AccountRefAccount_CompanyId, t.AccountRefAccount_ListID });
            
            CreateTable(
                "dbo.TxnDeleteds",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnDelType = c.String(maxLength: 20),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeDeleted = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        RefNumber = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.UnitOfMeasureSetRelatedUnits",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        UnitOfMeasureType = c.String(maxLength: 15),
                        BaseUnitName = c.String(maxLength: 31),
                        BaseUnitAbbreviation = c.String(maxLength: 31),
                        RelatedUnitSeqNo = c.Int(nullable: false),
                        RelatedUnitName = c.String(maxLength: 31),
                        RelatedUnitAbbreviation = c.String(maxLength: 31),
                        RelatedUnitConversionRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        UnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSet_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSet_CompanyId, t.UnitOfMeasureSet_ListID })
                .Index(t => new { t.UnitOfMeasureSet_CompanyId, t.UnitOfMeasureSet_ListID });
            
            CreateTable(
                "dbo.UnitOfMeasureSetDefaultUnits",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        UnitOfMeasureType = c.String(maxLength: 15),
                        BaseUnitName = c.String(maxLength: 31),
                        BaseUnitAbbreviation = c.String(maxLength: 31),
                        DefaultUnitUnitSeqNo = c.Int(nullable: false),
                        DefaultUnitUnitUsedFor = c.String(maxLength: 15),
                        DefaultUnitUnit = c.String(maxLength: 31),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        UnitOfMeasureSet_CompanyId = c.Int(),
                        UnitOfMeasureSet_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.UnitOfMeasureSet_CompanyId, t.UnitOfMeasureSet_ListID })
                .Index(t => new { t.UnitOfMeasureSet_CompanyId, t.UnitOfMeasureSet_ListID });
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        Desc = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.VehicleMileages",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        VehicleRefListID = c.String(maxLength: 36),
                        VehicleRefFullName = c.String(maxLength: 31),
                        CustomerRefListID = c.String(maxLength: 36),
                        CustomerRefFullName = c.String(maxLength: 209),
                        ItemRefListID = c.String(maxLength: 36),
                        ItemRefFullName = c.String(maxLength: 31),
                        ClassRefListID = c.String(maxLength: 36),
                        ClassRefFullName = c.String(maxLength: 159),
                        TripStartDate = c.DateTime(nullable: false),
                        TripEndDate = c.DateTime(nullable: false),
                        OdometerStart = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OdometerEnd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalMiles = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        BillableStatus = c.String(maxLength: 13),
                        StandardMileageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StandardMileageTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillableRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillableAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VehicleRefVehicle_CompanyId = c.Int(),
                        VehicleRefVehicle_ListID = c.String(maxLength: 36),
                        CustomerRefCustomer_CompanyId = c.Int(),
                        CustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemRefItem_CompanyId = c.Int(),
                        ItemRefItem_ListID = c.String(maxLength: 36),
                        ClassRefClass_CompanyId = c.Int(),
                        ClassRefClass_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vehicles", t => new { t.VehicleRefVehicle_CompanyId, t.VehicleRefVehicle_ListID })
                .ForeignKey("dbo.Customers", t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemRefItem_CompanyId, t.ItemRefItem_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID })
                .Index(t => new { t.VehicleRefVehicle_CompanyId, t.VehicleRefVehicle_ListID })
                .Index(t => new { t.CustomerRefCustomer_CompanyId, t.CustomerRefCustomer_ListID })
                .Index(t => new { t.ItemRefItem_CompanyId, t.ItemRefItem_ListID })
                .Index(t => new { t.ClassRefClass_CompanyId, t.ClassRefClass_ListID });
            
            CreateTable(
                "dbo.VendorCredits",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExternalGUID = c.String(maxLength: 40),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.VendorCreditExpenseLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExternalGUID = c.String(maxLength: 40),
                        ExpenseLineSeqNo = c.Int(nullable: false),
                        ExpenseLineTxnLineID = c.String(maxLength: 36),
                        ExpenseLineAccountRefListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefFullName = c.String(maxLength: 159),
                        ExpenseLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseLineMemo = c.String(),
                        ExpenseLineCustomerRefListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefFullName = c.String(maxLength: 209),
                        ExpenseLineClassRefListID = c.String(maxLength: 36),
                        ExpenseLineClassRefFullName = c.String(maxLength: 159),
                        ExpenseLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ExpenseLineBillableStatus = c.String(maxLength: 13),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineAccountRefAccount_CompanyId = c.Int(),
                        ExpenseLineAccountRefAccount_ListID = c.String(maxLength: 36),
                        ExpenseLineCustomerRefCustomer_CompanyId = c.Int(),
                        ExpenseLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ExpenseLineClassRefClass_CompanyId = c.Int(),
                        ExpenseLineClassRefClass_ListID = c.String(maxLength: 36),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ExpenseLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ExpenseLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ExpenseLineAccountRefAccount_CompanyId, t.ExpenseLineAccountRefAccount_ListID })
                .Index(t => new { t.ExpenseLineCustomerRefCustomer_CompanyId, t.ExpenseLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ExpenseLineClassRefClass_CompanyId, t.ExpenseLineClassRefClass_ListID })
                .Index(t => new { t.ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ExpenseLineTaxCodeRefTaxCode_CompanyId, t.ExpenseLineTaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.VendorCreditItemLines",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExternalGUID = c.String(maxLength: 40),
                        ItemLineType = c.String(maxLength: 11),
                        ItemLineSeqNo = c.Int(nullable: false),
                        ItemGroupTxnLineID = c.String(maxLength: 36),
                        ItemGroupLineItemRefListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefFullName = c.String(maxLength: 31),
                        ItemGroupLineDesc = c.String(),
                        ItemGroupUnitOfMeasure = c.String(maxLength: 31),
                        ItemGroupOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemGroupOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemGroupLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupLineTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemGroupSeqNo = c.Int(nullable: false),
                        ItemLineTxnLineID = c.String(maxLength: 36),
                        ItemLineItemRefListID = c.String(maxLength: 36),
                        ItemLineItemRefFullName = c.String(maxLength: 159),
                        ItemLineDesc = c.String(),
                        ItemLineQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineUnitOfMeasure = c.String(maxLength: 31),
                        ItemLineOverrideUOMSetRefListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefFullName = c.String(maxLength: 31),
                        ItemLineCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineTax1Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemLineCustomerRefListID = c.String(maxLength: 36),
                        ItemLineCustomerRefFullName = c.String(maxLength: 209),
                        ItemLineClassRefListID = c.String(maxLength: 36),
                        ItemLineClassRefFullName = c.String(maxLength: 159),
                        ItemLineSalesTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineTaxCodeRefFullName = c.String(maxLength: 3),
                        ItemLineBillableStatus = c.String(maxLength: 13),
                        ItemLineOverrideItemAccountRefListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefFullName = c.String(maxLength: 159),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQSaveToCache = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 110),
                        FQTxnLinkKey = c.String(maxLength: 110),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemGroupLineItemRefItem_CompanyId = c.Int(),
                        ItemGroupLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineItemRefItem_CompanyId = c.Int(),
                        ItemLineItemRefItem_ListID = c.String(maxLength: 36),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId = c.Int(),
                        ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID = c.String(maxLength: 36),
                        ItemLineCustomerRefCustomer_CompanyId = c.Int(),
                        ItemLineCustomerRefCustomer_ListID = c.String(maxLength: 36),
                        ItemLineClassRefClass_CompanyId = c.Int(),
                        ItemLineClassRefClass_ListID = c.String(maxLength: 36),
                        ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        ItemLineSalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineTaxCodeRefTaxCode_CompanyId = c.Int(),
                        ItemLineTaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                        ItemLineOverrideItemAccountRefAccount_CompanyId = c.Int(),
                        ItemLineOverrideItemAccountRefAccount_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Items", t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .ForeignKey("dbo.UnitOfMeasureSets", t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .ForeignKey("dbo.Customers", t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .ForeignKey("dbo.Classes", t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemGroupLineItemRefItem_CompanyId, t.ItemGroupLineItemRefItem_ListID })
                .Index(t => new { t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineItemRefItem_CompanyId, t.ItemLineItemRefItem_ListID })
                .Index(t => new { t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId, t.ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID })
                .Index(t => new { t.ItemLineCustomerRefCustomer_CompanyId, t.ItemLineCustomerRefCustomer_ListID })
                .Index(t => new { t.ItemLineClassRefClass_CompanyId, t.ItemLineClassRefClass_ListID })
                .Index(t => new { t.ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId, t.ItemLineSalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.ItemLineTaxCodeRefTaxCode_CompanyId, t.ItemLineTaxCodeRefTaxCode_ListID })
                .Index(t => new { t.ItemLineOverrideItemAccountRefAccount_CompanyId, t.ItemLineOverrideItemAccountRefAccount_ListID });
            
            CreateTable(
                "dbo.VendorCreditLinkedTxns",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        TxnID = c.String(maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        TxnNumber = c.Int(nullable: false),
                        VendorRefListID = c.String(maxLength: 36),
                        VendorRefFullName = c.String(maxLength: 41),
                        APAccountRefListID = c.String(maxLength: 36),
                        APAccountRefFullName = c.String(maxLength: 159),
                        TxnDate = c.DateTime(nullable: false),
                        TxnDateMacro = c.String(maxLength: 23),
                        CreditAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefNumber = c.String(maxLength: 20),
                        Memo = c.String(),
                        IsTaxIncluded = c.Boolean(nullable: false),
                        SalesTaxCodeRefListID = c.String(maxLength: 36),
                        SalesTaxCodeRefFullName = c.String(maxLength: 3),
                        TaxCodeRefListID = c.String(maxLength: 36),
                        TaxCodeRefFullName = c.String(maxLength: 3),
                        ExternalGUID = c.String(maxLength: 40),
                        LinkedTxnSeqNo = c.Int(nullable: false),
                        LinkedTxnTxnID = c.String(maxLength: 36),
                        LinkedTxnTxnType = c.String(maxLength: 21),
                        LinkedTxnTxnDate = c.DateTime(nullable: false),
                        LinkedTxnRefNumber = c.String(maxLength: 20),
                        LinkedTxnLinkType = c.String(maxLength: 8),
                        LinkedTxnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax1Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax2Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExchangeRate1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountIncludesVAT = c.Boolean(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        VendorRefVendor_CompanyId = c.Int(),
                        VendorRefVendor_ListID = c.String(maxLength: 36),
                        APAccountRefAccount_CompanyId = c.Int(),
                        APAccountRefAccount_ListID = c.String(maxLength: 36),
                        SalesTaxCodeRefSalesTaxCode_CompanyId = c.Int(),
                        SalesTaxCodeRefSalesTaxCode_ListID = c.String(maxLength: 36),
                        TaxCodeRefTaxCode_CompanyId = c.Int(),
                        TaxCodeRefTaxCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Vendors", t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .ForeignKey("dbo.Accounts", t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .ForeignKey("dbo.SalesTaxCodes", t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .ForeignKey("dbo.TaxCodes", t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID })
                .Index(t => new { t.VendorRefVendor_CompanyId, t.VendorRefVendor_ListID })
                .Index(t => new { t.APAccountRefAccount_CompanyId, t.APAccountRefAccount_ListID })
                .Index(t => new { t.SalesTaxCodeRefSalesTaxCode_CompanyId, t.SalesTaxCodeRefSalesTaxCode_ListID })
                .Index(t => new { t.TaxCodeRefTaxCode_CompanyId, t.TaxCodeRefTaxCode_ListID });
            
            CreateTable(
                "dbo.WorkersCompCodes",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        Desc = c.String(nullable: false, maxLength: 31),
                        CurrentRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentEffectiveDate = c.DateTime(nullable: false),
                        NextRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NextEffectiveDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID });
            
            CreateTable(
                "dbo.WorkersCompCodeRateHistories",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ListID = c.String(nullable: false, maxLength: 36),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeModified = c.DateTime(nullable: false),
                        EditSequence = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 31),
                        IsActive = c.Boolean(nullable: false),
                        Desc = c.String(nullable: false, maxLength: 31),
                        CurrentRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentEffectiveDate = c.DateTime(nullable: false),
                        NextRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NextEffectiveDate = c.DateTime(nullable: false),
                        RateHistorySeqNo = c.Int(nullable: false),
                        RateHistoryRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RateHistoryEffectiveDate = c.DateTime(nullable: false),
                        FQPrimaryKey = c.String(nullable: false, maxLength: 73),
                        WorkersCompCode_CompanyId = c.Int(),
                        WorkersCompCode_ListID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.WorkersCompCodes", t => new { t.WorkersCompCode_CompanyId, t.WorkersCompCode_ListID })
                .Index(t => new { t.WorkersCompCode_CompanyId, t.WorkersCompCode_ListID });
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkersCompCodeRateHistories", new[] { "WorkersCompCode_CompanyId", "WorkersCompCode_ListID" });
            DropIndex("dbo.VendorCreditLinkedTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.VendorCreditLinkedTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.VendorCreditLinkedTxns", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.VendorCreditLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.VendorCreditItemLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.VendorCreditExpenseLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.VendorCredits", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.VendorCredits", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.VendorCredits", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.VendorCredits", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.VehicleMileages", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.VehicleMileages", new[] { "ItemRefItem_CompanyId", "ItemRefItem_ListID" });
            DropIndex("dbo.VehicleMileages", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.VehicleMileages", new[] { "VehicleRefVehicle_CompanyId", "VehicleRefVehicle_ListID" });
            DropIndex("dbo.UnitOfMeasureSetDefaultUnits", new[] { "UnitOfMeasureSet_CompanyId", "UnitOfMeasureSet_ListID" });
            DropIndex("dbo.UnitOfMeasureSetRelatedUnits", new[] { "UnitOfMeasureSet_CompanyId", "UnitOfMeasureSet_ListID" });
            DropIndex("dbo.Transactions", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.Transactions", new[] { "EntityRefEntity_CompanyId", "EntityRefEntity_ListID" });
            DropIndex("dbo.TimeTrackings", new[] { "PayrollItemWageRefPayrollItemWage_CompanyId", "PayrollItemWageRefPayrollItemWage_ListID" });
            DropIndex("dbo.TimeTrackings", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.TimeTrackings", new[] { "ItemServiceRefItemService_CompanyId", "ItemServiceRefItemService_ListID" });
            DropIndex("dbo.TimeTrackings", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.TimeTrackings", new[] { "EntityRefEntity_CompanyId", "EntityRefEntity_ListID" });
            DropIndex("dbo.StandardTerms", new[] { "Terms_CompanyId", "Terms_ListID" });
            DropIndex("dbo.SpecialAccounts", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.SalesTaxPaymentCheckLines", new[] { "SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_CompanyId", "SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesTaxPaymentCheckLines", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" });
            DropIndex("dbo.SalesTaxPaymentCheckLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.SalesTaxPaymentChecks", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" });
            DropIndex("dbo.SalesTaxPaymentChecks", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineOverrideItemAccountRefAccount_CompanyId", "SalesReceiptLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineTaxCodeRefTaxCode_CompanyId", "SalesReceiptLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineSalesTaxCodeRefSalesTaxCode_CompanyId", "SalesReceiptLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineClassRefClass_CompanyId", "SalesReceiptLineClassRefClass_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLinePriceLevelRefPriceLevel_CompanyId", "SalesReceiptLinePriceLevelRefPriceLevel_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineItemRefItem_CompanyId", "SalesReceiptLineItemRefItem_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesReceiptLineGroupItemGroupRefItem_CompanyId", "SalesReceiptLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.SalesReceiptLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.SalesReceipts", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.SalesOrderLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineTaxCodeRefTaxCode_CompanyId", "SalesOrderLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId", "SalesOrderLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineClassRefClass_CompanyId", "SalesOrderLineClassRefClass_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLinePriceLevelRefPriceLevel_CompanyId", "SalesOrderLinePriceLevelRefPriceLevel_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineItemRefItem_CompanyId", "SalesOrderLineItemRefItem_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesOrderLineGroupItemGroupRefItem_CompanyId", "SalesOrderLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.SalesOrderLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.SalesOrders", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.SalesLines", new[] { "SalesLineTaxCodeRefTaxCode_CompanyId", "SalesLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesLines", new[] { "SalesLineSalesTaxCodeRefSalesTaxCode_CompanyId", "SalesLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesLines", new[] { "SalesLineClassRefClass_CompanyId", "SalesLineClassRefClass_ListID" });
            DropIndex("dbo.SalesLines", new[] { "SalesLineItemRefItem_CompanyId", "SalesLineItemRefItem_ListID" });
            DropIndex("dbo.SalesLines", new[] { "SalesLineGroupItemGroupRefItem_CompanyId", "SalesLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.SalesLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.SalesLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.SalesLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.SalesLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.SalesLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.SalesLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.SalesLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.SalesLines", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.SalesLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.SalesLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.SalesLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.Sales", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.Sales", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Sales", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Sales", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.Sales", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.Sales", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.Sales", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.Sales", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.Sales", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.Sales", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.Sales", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.Sales", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.ReceivePaymentToDeposits", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.ReceivePaymentLines", new[] { "AppliedToTxnDiscountClassRefClass_CompanyId", "AppliedToTxnDiscountClassRefClass_ListID" });
            DropIndex("dbo.ReceivePaymentLines", new[] { "AppliedToTxnDiscountAccountRefAccount_CompanyId", "AppliedToTxnDiscountAccountRefAccount_ListID" });
            DropIndex("dbo.ReceivePaymentLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.ReceivePaymentLines", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.ReceivePaymentLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.ReceivePaymentLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.ReceivePayments", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.ReceivePayments", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.ReceivePayments", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.ReceivePayments", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "ShipToEntityRefEntity_CompanyId", "ShipToEntityRefEntity_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.PurchaseOrderLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineOverrideItemAccountRefAccount_CompanyId", "PurchaseOrderLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineTaxCodeRefTaxCode_CompanyId", "PurchaseOrderLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId", "PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineCustomerRefCustomer_CompanyId", "PurchaseOrderLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineClassRefClass_CompanyId", "PurchaseOrderLineClassRefClass_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineItemRefItem_CompanyId", "PurchaseOrderLineItemRefItem_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineGroupItemGroupRefItem_CompanyId", "PurchaseOrderLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "ShipToEntityRefEntity_CompanyId", "ShipToEntityRefEntity_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "ShipToEntityRefEntity_CompanyId", "ShipToEntityRefEntity_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.PurchaseOrders", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.PriceLevelPerItems", new[] { "PriceLevelPerItemItemRefItem_CompanyId", "PriceLevelPerItemItemRefItem_ListID" });
            DropIndex("dbo.PriceLevelPerItems", new[] { "PriceLevel_CompanyId", "PriceLevel_ListID" });
            DropIndex("dbo.Preferences", new[] { "SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_CompanyId", "SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_ListID" });
            DropIndex("dbo.Preferences", new[] { "SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_CompanyId", "SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_ListID" });
            DropIndex("dbo.Preferences", new[] { "SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_CompanyId", "SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_ListID" });
            DropIndex("dbo.Preferences", new[] { "SalesAndCustomersPrefsDeftShipMethRefShipMethod_CompanyId", "SalesAndCustomersPrefsDeftShipMethRefShipMethod_ListID" });
            DropIndex("dbo.Preferences", new[] { "PurchasesAndVendorsPrefDefaultDisARefAccount_CompanyId", "PurchasesAndVendorsPrefDefaultDisARefAccount_ListID" });
            DropIndex("dbo.Preferences", new[] { "MultiCurrencyPrefsHomeCurrencyRefCurrency_CompanyId", "MultiCurrencyPrefsHomeCurrencyRefCurrency_ListID" });
            DropIndex("dbo.Preferences", new[] { "FinanceChargePrefsFinanceChargeAcctRefAccount_CompanyId", "FinanceChargePrefsFinanceChargeAcctRefAccount_ListID" });
            DropIndex("dbo.PayrollItemNonWages", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" });
            DropIndex("dbo.PayrollItemNonWages", new[] { "ExpenseAccountRefAccount_CompanyId", "ExpenseAccountRefAccount_ListID" });
            DropIndex("dbo.OtherNames", new[] { "Entity_CompanyId", "Entity_ListID" });
            DropIndex("dbo.JournalEntryLines", new[] { "JournalLineClassRefClass_CompanyId", "JournalLineClassRefClass_ListID" });
            DropIndex("dbo.JournalEntryLines", new[] { "JournalLineEntityRefEntity_CompanyId", "JournalLineEntityRefEntity_ListID" });
            DropIndex("dbo.JournalEntryLines", new[] { "JournalLineAccountRefAccount_CompanyId", "JournalLineAccountRefAccount_ListID" });
            DropIndex("dbo.JournalEntryDebitLines", new[] { "JournalDebitLineClassRefClass_CompanyId", "JournalDebitLineClassRefClass_ListID" });
            DropIndex("dbo.JournalEntryDebitLines", new[] { "JournalDebitLineEntityRefEntity_CompanyId", "JournalDebitLineEntityRefEntity_ListID" });
            DropIndex("dbo.JournalEntryDebitLines", new[] { "JournalDebitLineAccountRefAccount_CompanyId", "JournalDebitLineAccountRefAccount_ListID" });
            DropIndex("dbo.JournalEntryCreditLines", new[] { "JournalCreditLineClassRefClass_CompanyId", "JournalCreditLineClassRefClass_ListID" });
            DropIndex("dbo.JournalEntryCreditLines", new[] { "JournalCreditLineEntityRefEntity_CompanyId", "JournalCreditLineEntityRefEntity_ListID" });
            DropIndex("dbo.JournalEntryCreditLines", new[] { "JournalCreditLineAccountRefAccount_CompanyId", "JournalCreditLineAccountRefAccount_ListID" });
            DropIndex("dbo.ItemServices", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" });
            DropIndex("dbo.ItemServices", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" });
            DropIndex("dbo.ItemServices", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" });
            DropIndex("dbo.ItemServices", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" });
            DropIndex("dbo.ItemServices", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemServices", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemSalesTaxGroupLines", new[] { "ItemSalesTaxRefSalesTaxCode_CompanyId", "ItemSalesTaxRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemSalesTaxGroupLines", new[] { "ItemSalesTaxGroup_CompanyId", "ItemSalesTaxGroup_ListID" });
            DropIndex("dbo.ItemReceiptLinkedTxns", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptLinkedTxns", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "ItemGroupLineItemGroupRefItem_CompanyId", "ItemGroupLineItemGroupRefItem_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptItemLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceiptExpenseLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.ItemReceipts", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceipts", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.ItemReceipts", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.ItemPayments", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.ItemPayments", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.ItemOtherCharges", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" });
            DropIndex("dbo.ItemOtherCharges", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" });
            DropIndex("dbo.ItemOtherCharges", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" });
            DropIndex("dbo.ItemOtherCharges", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" });
            DropIndex("dbo.ItemOtherCharges", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemNonInventories", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" });
            DropIndex("dbo.ItemNonInventories", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" });
            DropIndex("dbo.ItemNonInventories", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" });
            DropIndex("dbo.ItemNonInventories", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" });
            DropIndex("dbo.ItemNonInventories", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemNonInventories", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "ItemInventoryAssemblyLnItemInventoryRefItemInventory_CompanyId", "ItemInventoryAssemblyLnItemInventoryRefItemInventory_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemInventoryAssemblyLines", new[] { "ItemInventoryAssembly_CompanyId", "ItemInventoryAssembly_ListID" });
            DropIndex("dbo.ItemInventories", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventories", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" });
            DropIndex("dbo.ItemInventories", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventories", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventories", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemInventories", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemGroupLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" });
            DropIndex("dbo.ItemGroupLines", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemGroupLines", new[] { "ItemGroup_CompanyId", "ItemGroup_ListID" });
            DropIndex("dbo.ItemGroups", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ItemFixedAssets", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" });
            DropIndex("dbo.ItemDiscounts", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.ItemDiscounts", new[] { "TaxCodeRefSalesTaxCode_CompanyId", "TaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemDiscounts", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemAssembliesCanBuilds", new[] { "ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId", "ItemInventoryAssemblyRefItemInventoryAssembly_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.InvoiceLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineOverrideItemAccountRefAccount_CompanyId", "InvoiceLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineTaxCodeRefTaxCode_CompanyId", "InvoiceLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineSalesTaxCodeRefSalesTaxCode_CompanyId", "InvoiceLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineClassRefClass_CompanyId", "InvoiceLineClassRefClass_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLinePriceLevelRefPriceLevel_CompanyId", "InvoiceLinePriceLevelRefPriceLevel_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineItemRefItem_CompanyId", "InvoiceLineItemRefItem_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "InvoiceLineGroupItemGroupRefItem_CompanyId", "InvoiceLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.InvoiceLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.Invoices", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Invoices", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Invoices", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.Invoices", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.Invoices", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.Invoices", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.Invoices", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.Invoices", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.Invoices", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.Invoices", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.Invoices", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.Invoices", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.InventoryAdjustmentLines", new[] { "InventoryAdjustmentLineItemRefItem_CompanyId", "InventoryAdjustmentLineItemRefItem_ListID" });
            DropIndex("dbo.InventoryAdjustmentLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.InventoryAdjustmentLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.InventoryAdjustmentLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.InventoryAdjustments", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.InventoryAdjustments", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.InventoryAdjustments", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.EstimateLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineOverrideItemAccountRefAccount_CompanyId", "EstimateLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineTaxCodeRefTaxCode_CompanyId", "EstimateLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineSalesTaxCodeRefSalesTaxCode_CompanyId", "EstimateLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineClassRefClass_CompanyId", "EstimateLineClassRefClass_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLinePriceLevelRefPriceLevel_CompanyId", "EstimateLinePriceLevelRefPriceLevel_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "EstimateLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineItemRefItem_CompanyId", "EstimateLineItemRefItem_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "EstimateLineGroupItemGroupRefItem_CompanyId", "EstimateLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.EstimateLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.Estimates", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Estimates", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Estimates", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.Estimates", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.Estimates", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.Estimates", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.Estimates", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.Estimates", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.Estimates", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.PayrollItemWages", new[] { "ExpenseAccountRefAccount_CompanyId", "ExpenseAccountRefAccount_ListID" });
            DropIndex("dbo.EmployeeEarnings", new[] { "PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_CompanyId", "PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_ListID" });
            DropIndex("dbo.EmployeeEarnings", new[] { "PayrollInfoClassRefClass_CompanyId", "PayrollInfoClassRefClass_ListID" });
            DropIndex("dbo.EmployeeEarnings", new[] { "BillingRateRefBillingRate_CompanyId", "BillingRateRefBillingRate_ListID" });
            DropIndex("dbo.EmployeeEarnings", new[] { "Employee_CompanyId", "Employee_ListID" });
            DropIndex("dbo.Employees", new[] { "PayrollInfoClassRefClass_CompanyId", "PayrollInfoClassRefClass_ListID" });
            DropIndex("dbo.Employees", new[] { "BillingRateRefBillingRate_CompanyId", "BillingRateRefBillingRate_ListID" });
            DropIndex("dbo.Employees", new[] { "Entity_CompanyId", "Entity_ListID" });
            DropIndex("dbo.DepositLines", new[] { "DepositLineClassRefClass_CompanyId", "DepositLineClassRefClass_ListID" });
            DropIndex("dbo.DepositLines", new[] { "DepositLinePaymentMethodRefPaymentMethod_CompanyId", "DepositLinePaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.DepositLines", new[] { "DepositLineAccountRefAccount_CompanyId", "DepositLineAccountRefAccount_ListID" });
            DropIndex("dbo.DepositLines", new[] { "DepositLineEntityRefEntity_CompanyId", "DepositLineEntityRefEntity_ListID" });
            DropIndex("dbo.DepositLines", new[] { "CashBackInfoAccountRefAccount_CompanyId", "CashBackInfoAccountRefAccount_ListID" });
            DropIndex("dbo.DepositLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.Deposits", new[] { "CashBackInfoAccountRefAccount_CompanyId", "CashBackInfoAccountRefAccount_ListID" });
            DropIndex("dbo.Deposits", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.DateDrivenTerms", new[] { "Terms_CompanyId", "Terms_ListID" });
            DropIndex("dbo.CustomFields", new[] { "EntityRefEntity_CompanyId", "EntityRefEntity_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.CreditMemoLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineOverrideItemAccountRefAccount_CompanyId", "CreditMemoLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineTaxCodeRefTaxCode_CompanyId", "CreditMemoLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineSalesTaxCodeRefSalesTaxCode_CompanyId", "CreditMemoLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineClassRefClass_CompanyId", "CreditMemoLineClassRefClass_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLinePriceLevelRefPriceLevel_CompanyId", "CreditMemoLinePriceLevelRefPriceLevel_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineItemRefItem_CompanyId", "CreditMemoLineItemRefItem_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CreditMemoLineGroupItemGroupRefItem_CompanyId", "CreditMemoLineGroupItemGroupRefItem_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.CreditMemoLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.CreditMemoes", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CreditCardCreditItemLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CreditCardCreditExpenseLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardCredits", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CreditCardCredits", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CreditCardChargeItemLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CreditCardChargeExpenseLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CreditCardCharges", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CreditCardCharges", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CreditCardCharges", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CreditCardCharges", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CheckItemLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CheckExpenseLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.CheckApplyCheckToTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.CheckApplyCheckToTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.CheckApplyCheckToTxns", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.CheckApplyCheckToTxns", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.CheckApplyCheckToTxns", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.Checks", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Checks", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Checks", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.Checks", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.Checks", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" });
            DropIndex("dbo.ChargeLinkedTxns", new[] { "OverrideItemAccountRefAccount_CompanyId", "OverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.ChargeLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.ChargeLinkedTxns", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.ChargeLinkedTxns", new[] { "OverrideUOMSetRefUnitOfMeasureSet_CompanyId", "OverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.ChargeLinkedTxns", new[] { "ItemRefItem_CompanyId", "ItemRefItem_ListID" });
            DropIndex("dbo.ChargeLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.Charges", new[] { "OverrideItemAccountRefAccount_CompanyId", "OverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.Charges", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" });
            DropIndex("dbo.Charges", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.Charges", new[] { "OverrideUOMSetRefUnitOfMeasureSet_CompanyId", "OverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.Charges", new[] { "ItemRefItem_CompanyId", "ItemRefItem_ListID" });
            DropIndex("dbo.Charges", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.BuildAssemblyComponentItemLines", new[] { "ComponentItemLineItemRefItem_CompanyId", "ComponentItemLineItemRefItem_ListID" });
            DropIndex("dbo.BuildAssemblyComponentItemLines", new[] { "ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId", "ItemInventoryAssemblyRefItemInventoryAssembly_ListID" });
            DropIndex("dbo.ItemInventoryAssemblies", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventoryAssemblies", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" });
            DropIndex("dbo.ItemInventoryAssemblies", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventoryAssemblies", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" });
            DropIndex("dbo.ItemInventoryAssemblies", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.ItemInventoryAssemblies", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.BuildAssemblies", new[] { "ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId", "ItemInventoryAssemblyRefItemInventoryAssembly_ListID" });
            DropIndex("dbo.BillToPays", new[] { "CreditToApplyAPAccountRefAccount_CompanyId", "CreditToApplyAPAccountRefAccount_ListID" });
            DropIndex("dbo.BillToPays", new[] { "BillToPayAPAccountRefAccount_CompanyId", "BillToPayAPAccountRefAccount_ListID" });
            DropIndex("dbo.BillToPays", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.BillPaymentCreditCardLines", new[] { "AppliedToTxnDiscountClassRefClass_CompanyId", "AppliedToTxnDiscountClassRefClass_ListID" });
            DropIndex("dbo.BillPaymentCreditCardLines", new[] { "AppliedToTxnDiscountAccountRefAccount_CompanyId", "AppliedToTxnDiscountAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCreditCardLines", new[] { "CreditCardAccountRefAccount_CompanyId", "CreditCardAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCreditCardLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCreditCardLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.BillPaymentCreditCards", new[] { "CreditCardAccountRefAccount_CompanyId", "CreditCardAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCreditCards", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCreditCards", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.BillPaymentCheckLines", new[] { "AppliedToTxnDiscountClassRefClass_CompanyId", "AppliedToTxnDiscountClassRefClass_ListID" });
            DropIndex("dbo.BillPaymentCheckLines", new[] { "AppliedToTxnDiscountAccountRefAccount_CompanyId", "AppliedToTxnDiscountAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCheckLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.BillPaymentCheckLines", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCheckLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentCheckLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.BillPaymentChecks", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.BillPaymentChecks", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentChecks", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillPaymentChecks", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" });
            DropIndex("dbo.BillingRateLines", new[] { "BillingRateLineItemRefItem_CompanyId", "BillingRateLineItemRefItem_ListID" });
            DropIndex("dbo.BillLinkedTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.BillLinkedTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.BillLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.BillLinkedTxns", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.Items", new[] { "TaxVendorRefVendor_CompanyId", "TaxVendorRefVendor_ListID" });
            DropIndex("dbo.Items", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.Items", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" });
            DropIndex("dbo.Items", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" });
            DropIndex("dbo.Items", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" });
            DropIndex("dbo.Items", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Items", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "ItemGroupLineItemGroupRefItem_CompanyId", "ItemGroupLineItemGroupRefItem_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillItemLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.BillExpenseLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.Bills", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Bills", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Bills", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.Bills", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" });
            DropIndex("dbo.Bills", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" });
            DropIndex("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "RefundFromAccountRefAccount_CompanyId", "RefundFromAccountRefAccount_ListID" });
            DropIndex("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.Vendors", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.Vendors", new[] { "SalesTaxReturnRefSalesTaxCode_CompanyId", "SalesTaxReturnRefSalesTaxCode_ListID" });
            DropIndex("dbo.Vendors", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Vendors", new[] { "BillingRateRefBillingRate_CompanyId", "BillingRateRefBillingRate_ListID" });
            DropIndex("dbo.Vendors", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.Vendors", new[] { "VendorTypeRefVendorType_CompanyId", "VendorTypeRefVendorType_ListID" });
            DropIndex("dbo.ItemSalesTaxes", new[] { "TaxVendorRefVendor_CompanyId", "TaxVendorRefVendor_ListID" });
            DropIndex("dbo.SalesReps", new[] { "SalesRepEntityRefEntity_CompanyId", "SalesRepEntityRefEntity_ListID" });
            DropIndex("dbo.Customers", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.Customers", new[] { "PriceLevelRefPriceLevel_CompanyId", "PriceLevelRefPriceLevel_ListID" });
            DropIndex("dbo.Customers", new[] { "JobTypeRefJobType_CompanyId", "JobTypeRefJobType_ListID" });
            DropIndex("dbo.Customers", new[] { "PreferredPaymentMethodRefPaymentMethod_CompanyId", "PreferredPaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.Customers", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" });
            DropIndex("dbo.Customers", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Customers", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropIndex("dbo.Customers", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" });
            DropIndex("dbo.Customers", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" });
            DropIndex("dbo.Customers", new[] { "CustomerTypeRefCustomerType_CompanyId", "CustomerTypeRefCustomerType_ListID" });
            DropIndex("dbo.Customers", new[] { "Entity_CompanyId", "Entity_ListID" });
            DropIndex("dbo.ARRefundCreditCards", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" });
            DropIndex("dbo.ARRefundCreditCards", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" });
            DropIndex("dbo.ARRefundCreditCards", new[] { "RefundFromAccountRefAccount_CompanyId", "RefundFromAccountRefAccount_ListID" });
            DropIndex("dbo.ARRefundCreditCards", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" });
            DropIndex("dbo.TaxCodes", new[] { "ItemSalesTaxRefSalesTaxCode_CompanyId", "ItemSalesTaxRefSalesTaxCode_ListID" });
            DropIndex("dbo.TaxCodes", new[] { "ItemPurchaseTaxRefSalesTaxCode_CompanyId", "ItemPurchaseTaxRefSalesTaxCode_ListID" });
            DropIndex("dbo.SalesTaxCodes", new[] { "CompanyId", "ItemSalesTaxRefListID" });
            DropIndex("dbo.SalesTaxCodes", new[] { "CompanyId", "ItemPurchaseTaxRefListID" });
            DropIndex("dbo.Accounts", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" });
            DropIndex("dbo.Accounts", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" });
            DropIndex("dbo.Accounts", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" });
            DropForeignKey("dbo.WorkersCompCodeRateHistories", new[] { "WorkersCompCode_CompanyId", "WorkersCompCode_ListID" }, "dbo.WorkersCompCodes");
            DropForeignKey("dbo.VendorCreditLinkedTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.VendorCreditLinkedTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.VendorCreditLinkedTxns", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.VendorCreditLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.VendorCreditItemLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.VendorCreditExpenseLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.VendorCredits", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.VendorCredits", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.VendorCredits", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.VendorCredits", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.VehicleMileages", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.VehicleMileages", new[] { "ItemRefItem_CompanyId", "ItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.VehicleMileages", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.VehicleMileages", new[] { "VehicleRefVehicle_CompanyId", "VehicleRefVehicle_ListID" }, "dbo.Vehicles");
            DropForeignKey("dbo.UnitOfMeasureSetDefaultUnits", new[] { "UnitOfMeasureSet_CompanyId", "UnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.UnitOfMeasureSetRelatedUnits", new[] { "UnitOfMeasureSet_CompanyId", "UnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.Transactions", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Transactions", new[] { "EntityRefEntity_CompanyId", "EntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.TimeTrackings", new[] { "PayrollItemWageRefPayrollItemWage_CompanyId", "PayrollItemWageRefPayrollItemWage_ListID" }, "dbo.PayrollItemWages");
            DropForeignKey("dbo.TimeTrackings", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.TimeTrackings", new[] { "ItemServiceRefItemService_CompanyId", "ItemServiceRefItemService_ListID" }, "dbo.ItemServices");
            DropForeignKey("dbo.TimeTrackings", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.TimeTrackings", new[] { "EntityRefEntity_CompanyId", "EntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.StandardTerms", new[] { "Terms_CompanyId", "Terms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.SpecialAccounts", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.SalesTaxPaymentCheckLines", new[] { "SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_CompanyId", "SalesTaxPaymentCheckLineItemSalesTaxRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesTaxPaymentCheckLines", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesTaxPaymentCheckLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.SalesTaxPaymentChecks", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesTaxPaymentChecks", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineOverrideItemAccountRefAccount_CompanyId", "SalesReceiptLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineTaxCodeRefTaxCode_CompanyId", "SalesReceiptLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineSalesTaxCodeRefSalesTaxCode_CompanyId", "SalesReceiptLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineClassRefClass_CompanyId", "SalesReceiptLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLinePriceLevelRefPriceLevel_CompanyId", "SalesReceiptLinePriceLevelRefPriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesReceiptLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineItemRefItem_CompanyId", "SalesReceiptLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesReceiptLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesReceiptLineGroupItemGroupRefItem_CompanyId", "SalesReceiptLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesReceiptLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.SalesReceipts", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesReceipts", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesReceipts", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesReceipts", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.SalesReceipts", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.SalesReceipts", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.SalesReceipts", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.SalesReceipts", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.SalesReceipts", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.SalesReceipts", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesReceipts", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesOrderLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineTaxCodeRefTaxCode_CompanyId", "SalesOrderLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId", "SalesOrderLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineClassRefClass_CompanyId", "SalesOrderLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLinePriceLevelRefPriceLevel_CompanyId", "SalesOrderLinePriceLevelRefPriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineItemRefItem_CompanyId", "SalesOrderLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "SalesOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesOrderLineGroupItemGroupRefItem_CompanyId", "SalesOrderLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.SalesOrderLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.SalesOrderLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.SalesOrderLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.SalesOrderLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.SalesOrderLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.SalesOrderLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesOrderLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.SalesOrders", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesOrders", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesOrders", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.SalesOrders", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.SalesOrders", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.SalesOrders", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.SalesOrders", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.SalesOrders", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.SalesOrders", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesOrders", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.SalesLines", new[] { "SalesLineTaxCodeRefTaxCode_CompanyId", "SalesLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesLines", new[] { "SalesLineSalesTaxCodeRefSalesTaxCode_CompanyId", "SalesLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesLines", new[] { "SalesLineClassRefClass_CompanyId", "SalesLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesLines", new[] { "SalesLineItemRefItem_CompanyId", "SalesLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.SalesLines", new[] { "SalesLineGroupItemGroupRefItem_CompanyId", "SalesLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.SalesLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.SalesLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.SalesLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.SalesLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.SalesLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.SalesLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.SalesLines", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.SalesLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.SalesLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.SalesLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.Sales", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Sales", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Sales", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Sales", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.Sales", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.Sales", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.Sales", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.Sales", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.Sales", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.Sales", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Sales", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.Sales", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.ReceivePaymentToDeposits", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.ReceivePaymentLines", new[] { "AppliedToTxnDiscountClassRefClass_CompanyId", "AppliedToTxnDiscountClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.ReceivePaymentLines", new[] { "AppliedToTxnDiscountAccountRefAccount_CompanyId", "AppliedToTxnDiscountAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ReceivePaymentLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ReceivePaymentLines", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.ReceivePaymentLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ReceivePaymentLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.ReceivePayments", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ReceivePayments", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.ReceivePayments", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ReceivePayments", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "ShipToEntityRefEntity_CompanyId", "ShipToEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.PurchaseOrderLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineOverrideItemAccountRefAccount_CompanyId", "PurchaseOrderLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineTaxCodeRefTaxCode_CompanyId", "PurchaseOrderLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_CompanyId", "PurchaseOrderLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineCustomerRefCustomer_CompanyId", "PurchaseOrderLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineClassRefClass_CompanyId", "PurchaseOrderLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "PurchaseOrderLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineItemRefItem_CompanyId", "PurchaseOrderLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "PurchaseOrderLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "PurchaseOrderLineGroupItemGroupRefItem_CompanyId", "PurchaseOrderLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "ShipToEntityRefEntity_CompanyId", "ShipToEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.PurchaseOrderLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.PurchaseOrders", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.PurchaseOrders", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.PurchaseOrders", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.PurchaseOrders", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.PurchaseOrders", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.PurchaseOrders", new[] { "ShipToEntityRefEntity_CompanyId", "ShipToEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.PurchaseOrders", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.PurchaseOrders", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.PriceLevelPerItems", new[] { "PriceLevelPerItemItemRefItem_CompanyId", "PriceLevelPerItemItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.PriceLevelPerItems", new[] { "PriceLevel_CompanyId", "PriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.Preferences", new[] { "SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_CompanyId", "SalesTaxPrefsDefaultNonTaxableSaleTCRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Preferences", new[] { "SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_CompanyId", "SalesTaxPrefsDefaultTaxableSaleTCRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Preferences", new[] { "SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_CompanyId", "SalesTaxPrefsDefaultItemSalesTaxRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Preferences", new[] { "SalesAndCustomersPrefsDeftShipMethRefShipMethod_CompanyId", "SalesAndCustomersPrefsDeftShipMethRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.Preferences", new[] { "PurchasesAndVendorsPrefDefaultDisARefAccount_CompanyId", "PurchasesAndVendorsPrefDefaultDisARefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Preferences", new[] { "MultiCurrencyPrefsHomeCurrencyRefCurrency_CompanyId", "MultiCurrencyPrefsHomeCurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.Preferences", new[] { "FinanceChargePrefsFinanceChargeAcctRefAccount_CompanyId", "FinanceChargePrefsFinanceChargeAcctRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.PayrollItemNonWages", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.PayrollItemNonWages", new[] { "ExpenseAccountRefAccount_CompanyId", "ExpenseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.OtherNames", new[] { "Entity_CompanyId", "Entity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.JournalEntryLines", new[] { "JournalLineClassRefClass_CompanyId", "JournalLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.JournalEntryLines", new[] { "JournalLineEntityRefEntity_CompanyId", "JournalLineEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.JournalEntryLines", new[] { "JournalLineAccountRefAccount_CompanyId", "JournalLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.JournalEntryDebitLines", new[] { "JournalDebitLineClassRefClass_CompanyId", "JournalDebitLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.JournalEntryDebitLines", new[] { "JournalDebitLineEntityRefEntity_CompanyId", "JournalDebitLineEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.JournalEntryDebitLines", new[] { "JournalDebitLineAccountRefAccount_CompanyId", "JournalDebitLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.JournalEntryCreditLines", new[] { "JournalCreditLineClassRefClass_CompanyId", "JournalCreditLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.JournalEntryCreditLines", new[] { "JournalCreditLineEntityRefEntity_CompanyId", "JournalCreditLineEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.JournalEntryCreditLines", new[] { "JournalCreditLineAccountRefAccount_CompanyId", "JournalCreditLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemServices", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemServices", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemServices", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemServices", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemServices", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemServices", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemSalesTaxGroupLines", new[] { "ItemSalesTaxRefSalesTaxCode_CompanyId", "ItemSalesTaxRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemSalesTaxGroupLines", new[] { "ItemSalesTaxGroup_CompanyId", "ItemSalesTaxGroup_ListID" }, "dbo.ItemSalesTaxGroups");
            DropForeignKey("dbo.ItemReceiptLinkedTxns", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptLinkedTxns", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "ItemGroupLineItemGroupRefItem_CompanyId", "ItemGroupLineItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptItemLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceiptExpenseLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemReceipts", new[] { "LiabilityAccountRefAccount_CompanyId", "LiabilityAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceipts", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemReceipts", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemPayments", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.ItemPayments", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemOtherCharges", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemOtherCharges", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemOtherCharges", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemOtherCharges", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemOtherCharges", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemNonInventories", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemNonInventories", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemNonInventories", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemNonInventories", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemNonInventories", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemNonInventories", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "ItemInventoryAssemblyLnItemInventoryRefItemInventory_CompanyId", "ItemInventoryAssemblyLnItemInventoryRefItemInventory_ListID" }, "dbo.ItemInventories");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemInventoryAssemblyLines", new[] { "ItemInventoryAssembly_CompanyId", "ItemInventoryAssembly_ListID" }, "dbo.ItemInventoryAssemblies");
            DropForeignKey("dbo.ItemInventories", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventories", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemInventories", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventories", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventories", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemInventories", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemGroupLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.ItemGroupLines", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemGroupLines", new[] { "ItemGroup_CompanyId", "ItemGroup_ListID" }, "dbo.ItemGroups");
            DropForeignKey("dbo.ItemGroups", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ItemFixedAssets", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemDiscounts", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemDiscounts", new[] { "TaxCodeRefSalesTaxCode_CompanyId", "TaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemDiscounts", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemAssembliesCanBuilds", new[] { "ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId", "ItemInventoryAssemblyRefItemInventoryAssembly_ListID" }, "dbo.ItemInventoryAssemblies");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.InvoiceLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineOverrideItemAccountRefAccount_CompanyId", "InvoiceLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineTaxCodeRefTaxCode_CompanyId", "InvoiceLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineSalesTaxCodeRefSalesTaxCode_CompanyId", "InvoiceLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineClassRefClass_CompanyId", "InvoiceLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLinePriceLevelRefPriceLevel_CompanyId", "InvoiceLinePriceLevelRefPriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "InvoiceLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineItemRefItem_CompanyId", "InvoiceLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "InvoiceLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.InvoiceLines", new[] { "InvoiceLineGroupItemGroupRefItem_CompanyId", "InvoiceLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.InvoiceLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.InvoiceLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.InvoiceLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.InvoiceLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.InvoiceLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.InvoiceLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.InvoiceLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.InvoiceLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.InvoiceLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.InvoiceLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.InvoiceLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.InvoiceLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.Invoices", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Invoices", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Invoices", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.Invoices", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.Invoices", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.Invoices", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.Invoices", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.Invoices", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.Invoices", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.Invoices", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Invoices", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.Invoices", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.InventoryAdjustmentLines", new[] { "InventoryAdjustmentLineItemRefItem_CompanyId", "InventoryAdjustmentLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.InventoryAdjustmentLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.InventoryAdjustmentLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.InventoryAdjustmentLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.InventoryAdjustments", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.InventoryAdjustments", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.InventoryAdjustments", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.EstimateLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineOverrideItemAccountRefAccount_CompanyId", "EstimateLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineTaxCodeRefTaxCode_CompanyId", "EstimateLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineSalesTaxCodeRefSalesTaxCode_CompanyId", "EstimateLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineClassRefClass_CompanyId", "EstimateLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLinePriceLevelRefPriceLevel_CompanyId", "EstimateLinePriceLevelRefPriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "EstimateLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineItemRefItem_CompanyId", "EstimateLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "EstimateLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.EstimateLines", new[] { "EstimateLineGroupItemGroupRefItem_CompanyId", "EstimateLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.EstimateLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.EstimateLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.EstimateLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.EstimateLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.EstimateLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.EstimateLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.EstimateLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.EstimateLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.EstimateLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.Estimates", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Estimates", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Estimates", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.Estimates", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.Estimates", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.Estimates", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.Estimates", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.Estimates", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.Estimates", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.PayrollItemWages", new[] { "ExpenseAccountRefAccount_CompanyId", "ExpenseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.EmployeeEarnings", new[] { "PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_CompanyId", "PayrollInfoEarningsPayrollItemWageRefPayrollItemWage_ListID" }, "dbo.PayrollItemWages");
            DropForeignKey("dbo.EmployeeEarnings", new[] { "PayrollInfoClassRefClass_CompanyId", "PayrollInfoClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.EmployeeEarnings", new[] { "BillingRateRefBillingRate_CompanyId", "BillingRateRefBillingRate_ListID" }, "dbo.BillingRates");
            DropForeignKey("dbo.EmployeeEarnings", new[] { "Employee_CompanyId", "Employee_ListID" }, "dbo.Employees");
            DropForeignKey("dbo.Employees", new[] { "PayrollInfoClassRefClass_CompanyId", "PayrollInfoClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.Employees", new[] { "BillingRateRefBillingRate_CompanyId", "BillingRateRefBillingRate_ListID" }, "dbo.BillingRates");
            DropForeignKey("dbo.Employees", new[] { "Entity_CompanyId", "Entity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.DepositLines", new[] { "DepositLineClassRefClass_CompanyId", "DepositLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.DepositLines", new[] { "DepositLinePaymentMethodRefPaymentMethod_CompanyId", "DepositLinePaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.DepositLines", new[] { "DepositLineAccountRefAccount_CompanyId", "DepositLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.DepositLines", new[] { "DepositLineEntityRefEntity_CompanyId", "DepositLineEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.DepositLines", new[] { "CashBackInfoAccountRefAccount_CompanyId", "CashBackInfoAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.DepositLines", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Deposits", new[] { "CashBackInfoAccountRefAccount_CompanyId", "CashBackInfoAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Deposits", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.DateDrivenTerms", new[] { "Terms_CompanyId", "Terms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.CustomFields", new[] { "EntityRefEntity_CompanyId", "EntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditMemoLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineOverrideItemAccountRefAccount_CompanyId", "CreditMemoLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineTaxCodeRefTaxCode_CompanyId", "CreditMemoLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineSalesTaxCodeRefSalesTaxCode_CompanyId", "CreditMemoLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineClassRefClass_CompanyId", "CreditMemoLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLinePriceLevelRefPriceLevel_CompanyId", "CreditMemoLinePriceLevelRefPriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "CreditMemoLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineItemRefItem_CompanyId", "CreditMemoLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "CreditMemoLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CreditMemoLineGroupItemGroupRefItem_CompanyId", "CreditMemoLineGroupItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.CreditMemoLines", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.CreditMemoLines", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.CreditMemoLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.CreditMemoLines", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.CreditMemoLines", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditMemoLines", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditMemoLines", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditMemoes", new[] { "CustomerTaxCodeRefTaxCode_CompanyId", "CustomerTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditMemoes", new[] { "CustomerSalesTaxCodeRefSalesTaxCode_CompanyId", "CustomerSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditMemoes", new[] { "CustomerMsgRefCustomerMsg_CompanyId", "CustomerMsgRefCustomerMsg_ListID" }, "dbo.CustomerMsgs");
            DropForeignKey("dbo.CreditMemoes", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.CreditMemoes", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.CreditMemoes", new[] { "ShipMethodRefShipMethod_CompanyId", "ShipMethodRefShipMethod_ListID" }, "dbo.ShipMethods");
            DropForeignKey("dbo.CreditMemoes", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.CreditMemoes", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.CreditMemoes", new[] { "TemplateRefTemplate_CompanyId", "TemplateRefTemplate_ListID" }, "dbo.Templates");
            DropForeignKey("dbo.CreditMemoes", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditMemoes", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditMemoes", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditCardCreditItemLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditCardCreditExpenseLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardCredits", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditCardCredits", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditCardChargeItemLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditCardChargeExpenseLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CreditCardCharges", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CreditCardCharges", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CreditCardCharges", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CreditCardCharges", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.CheckItemLines", new[] { "ItemGroupLineItemRefItem_CompanyId", "ItemGroupLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.CheckItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CheckItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CheckItemLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.CheckItemLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CheckItemLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CheckExpenseLines", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.CheckApplyCheckToTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.CheckApplyCheckToTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.CheckApplyCheckToTxns", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.CheckApplyCheckToTxns", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.CheckApplyCheckToTxns", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Checks", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Checks", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Checks", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.Checks", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.Checks", new[] { "AccountRefAccount_CompanyId", "AccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ChargeLinkedTxns", new[] { "OverrideItemAccountRefAccount_CompanyId", "OverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ChargeLinkedTxns", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.ChargeLinkedTxns", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ChargeLinkedTxns", new[] { "OverrideUOMSetRefUnitOfMeasureSet_CompanyId", "OverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.ChargeLinkedTxns", new[] { "ItemRefItem_CompanyId", "ItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.ChargeLinkedTxns", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.Charges", new[] { "OverrideItemAccountRefAccount_CompanyId", "OverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Charges", new[] { "ClassRefClass_CompanyId", "ClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.Charges", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Charges", new[] { "OverrideUOMSetRefUnitOfMeasureSet_CompanyId", "OverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.Charges", new[] { "ItemRefItem_CompanyId", "ItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.Charges", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.BuildAssemblyComponentItemLines", new[] { "ComponentItemLineItemRefItem_CompanyId", "ComponentItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.BuildAssemblyComponentItemLines", new[] { "ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId", "ItemInventoryAssemblyRefItemInventoryAssembly_ListID" }, "dbo.ItemInventoryAssemblies");
            DropForeignKey("dbo.ItemInventoryAssemblies", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventoryAssemblies", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ItemInventoryAssemblies", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventoryAssemblies", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ItemInventoryAssemblies", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.ItemInventoryAssemblies", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.BuildAssemblies", new[] { "ItemInventoryAssemblyRefItemInventoryAssembly_CompanyId", "ItemInventoryAssemblyRefItemInventoryAssembly_ListID" }, "dbo.ItemInventoryAssemblies");
            DropForeignKey("dbo.BillToPays", new[] { "CreditToApplyAPAccountRefAccount_CompanyId", "CreditToApplyAPAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillToPays", new[] { "BillToPayAPAccountRefAccount_CompanyId", "BillToPayAPAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillToPays", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.BillPaymentCreditCardLines", new[] { "AppliedToTxnDiscountClassRefClass_CompanyId", "AppliedToTxnDiscountClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.BillPaymentCreditCardLines", new[] { "AppliedToTxnDiscountAccountRefAccount_CompanyId", "AppliedToTxnDiscountAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCreditCardLines", new[] { "CreditCardAccountRefAccount_CompanyId", "CreditCardAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCreditCardLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCreditCardLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.BillPaymentCreditCards", new[] { "CreditCardAccountRefAccount_CompanyId", "CreditCardAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCreditCards", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCreditCards", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.BillPaymentCheckLines", new[] { "AppliedToTxnDiscountClassRefClass_CompanyId", "AppliedToTxnDiscountClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.BillPaymentCheckLines", new[] { "AppliedToTxnDiscountAccountRefAccount_CompanyId", "AppliedToTxnDiscountAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCheckLines", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.BillPaymentCheckLines", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCheckLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentCheckLines", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.BillPaymentChecks", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.BillPaymentChecks", new[] { "BankAccountRefAccount_CompanyId", "BankAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentChecks", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillPaymentChecks", new[] { "PayeeEntityRefEntity_CompanyId", "PayeeEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.BillingRateLines", new[] { "BillingRateLineItemRefItem_CompanyId", "BillingRateLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.BillLinkedTxns", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.BillLinkedTxns", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.BillLinkedTxns", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.BillLinkedTxns", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillLinkedTxns", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.Items", new[] { "TaxVendorRefVendor_CompanyId", "TaxVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.Items", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.Items", new[] { "DepositToAccountRefAccount_CompanyId", "DepositToAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "AssetAccountRefAccount_CompanyId", "AssetAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "PrefVendorRefVendor_CompanyId", "PrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.Items", new[] { "COGSAccountRefAccount_CompanyId", "COGSAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "IncomeAccountRefAccount_CompanyId", "IncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "SalesAndPurchasePrefVendorRefVendor_CompanyId", "SalesAndPurchasePrefVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.Items", new[] { "SalesAndPurchaseExpenseAccountRefAccount_CompanyId", "SalesAndPurchaseExpenseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "SalesAndPurchaseIncomeAccountRefAccount_CompanyId", "SalesAndPurchaseIncomeAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "SalesOrPurchaseAccountRefAccount_CompanyId", "SalesOrPurchaseAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Items", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Items", new[] { "UnitOfMeasureSetRefUnitOfMeasureSet_CompanyId", "UnitOfMeasureSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineOverrideItemAccountRefAccount_CompanyId", "ItemLineOverrideItemAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineTaxCodeRefTaxCode_CompanyId", "ItemLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ItemLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineClassRefClass_CompanyId", "ItemLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineCustomerRefCustomer_CompanyId", "ItemLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemLineOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemLineItemRefItem_CompanyId", "ItemLineItemRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_CompanyId", "ItemGroupOverrideUOMSetRefUnitOfMeasureSet_ListID" }, "dbo.UnitOfMeasureSets");
            DropForeignKey("dbo.BillItemLines", new[] { "ItemGroupLineItemGroupRefItem_CompanyId", "ItemGroupLineItemGroupRefItem_ListID" }, "dbo.Items");
            DropForeignKey("dbo.BillItemLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.BillItemLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.BillItemLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.BillItemLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillItemLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.BillExpenseLines", new[] { "ExpenseLineTaxCodeRefTaxCode_CompanyId", "ExpenseLineTaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.BillExpenseLines", new[] { "ExpenseLineSalesTaxCodeRefSalesTaxCode_CompanyId", "ExpenseLineSalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.BillExpenseLines", new[] { "ExpenseLineClassRefClass_CompanyId", "ExpenseLineClassRefClass_ListID" }, "dbo.Classes");
            DropForeignKey("dbo.BillExpenseLines", new[] { "ExpenseLineCustomerRefCustomer_CompanyId", "ExpenseLineCustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.BillExpenseLines", new[] { "ExpenseLineAccountRefAccount_CompanyId", "ExpenseLineAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillExpenseLines", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.BillExpenseLines", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.BillExpenseLines", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.BillExpenseLines", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.BillExpenseLines", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.Bills", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Bills", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Bills", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.Bills", new[] { "APAccountRefAccount_CompanyId", "APAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.Bills", new[] { "VendorRefVendor_CompanyId", "VendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "RefundFromAccountRefAccount_CompanyId", "RefundFromAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ARRefundCreditCardRefundAppliedToes", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.Vendors", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.Vendors", new[] { "SalesTaxReturnRefSalesTaxCode_CompanyId", "SalesTaxReturnRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Vendors", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Vendors", new[] { "BillingRateRefBillingRate_CompanyId", "BillingRateRefBillingRate_ListID" }, "dbo.BillingRates");
            DropForeignKey("dbo.Vendors", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.Vendors", new[] { "VendorTypeRefVendorType_CompanyId", "VendorTypeRefVendorType_ListID" }, "dbo.VendorTypes");
            DropForeignKey("dbo.ItemSalesTaxes", new[] { "TaxVendorRefVendor_CompanyId", "TaxVendorRefVendor_ListID" }, "dbo.Vendors");
            DropForeignKey("dbo.SalesReps", new[] { "SalesRepEntityRefEntity_CompanyId", "SalesRepEntityRefEntity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.Customers", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.Customers", new[] { "PriceLevelRefPriceLevel_CompanyId", "PriceLevelRefPriceLevel_ListID" }, "dbo.PriceLevels");
            DropForeignKey("dbo.Customers", new[] { "JobTypeRefJobType_CompanyId", "JobTypeRefJobType_ListID" }, "dbo.JobTypes");
            DropForeignKey("dbo.Customers", new[] { "PreferredPaymentMethodRefPaymentMethod_CompanyId", "PreferredPaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.Customers", new[] { "ItemSalesTaxRefItemSalesTax_CompanyId", "ItemSalesTaxRefItemSalesTax_ListID" }, "dbo.ItemSalesTaxes");
            DropForeignKey("dbo.Customers", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Customers", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Customers", new[] { "SalesRepRefSalesRep_CompanyId", "SalesRepRefSalesRep_ListID" }, "dbo.SalesReps");
            DropForeignKey("dbo.Customers", new[] { "TermsRefTerms_CompanyId", "TermsRefTerms_ListID" }, "dbo.Terms");
            DropForeignKey("dbo.Customers", new[] { "CustomerTypeRefCustomerType_CompanyId", "CustomerTypeRefCustomerType_ListID" }, "dbo.CustomerTypes");
            DropForeignKey("dbo.Customers", new[] { "Entity_CompanyId", "Entity_ListID" }, "dbo.Entities");
            DropForeignKey("dbo.ARRefundCreditCards", new[] { "PaymentMethodRefPaymentMethod_CompanyId", "PaymentMethodRefPaymentMethod_ListID" }, "dbo.PaymentMethods");
            DropForeignKey("dbo.ARRefundCreditCards", new[] { "ARAccountRefAccount_CompanyId", "ARAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ARRefundCreditCards", new[] { "RefundFromAccountRefAccount_CompanyId", "RefundFromAccountRefAccount_ListID" }, "dbo.Accounts");
            DropForeignKey("dbo.ARRefundCreditCards", new[] { "CustomerRefCustomer_CompanyId", "CustomerRefCustomer_ListID" }, "dbo.Customers");
            DropForeignKey("dbo.TaxCodes", new[] { "ItemSalesTaxRefSalesTaxCode_CompanyId", "ItemSalesTaxRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.TaxCodes", new[] { "ItemPurchaseTaxRefSalesTaxCode_CompanyId", "ItemPurchaseTaxRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesTaxCodes", new[] { "CompanyId", "ItemSalesTaxRefListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.SalesTaxCodes", new[] { "CompanyId", "ItemPurchaseTaxRefListID" }, "dbo.SalesTaxCodes");
            DropForeignKey("dbo.Accounts", new[] { "CurrencyRefCurrency_CompanyId", "CurrencyRefCurrency_ListID" }, "dbo.Currencies");
            DropForeignKey("dbo.Accounts", new[] { "TaxCodeRefTaxCode_CompanyId", "TaxCodeRefTaxCode_ListID" }, "dbo.TaxCodes");
            DropForeignKey("dbo.Accounts", new[] { "SalesTaxCodeRefSalesTaxCode_CompanyId", "SalesTaxCodeRefSalesTaxCode_ListID" }, "dbo.SalesTaxCodes");
            DropTable("dbo.WorkersCompCodeRateHistories");
            DropTable("dbo.WorkersCompCodes");
            DropTable("dbo.VendorCreditLinkedTxns");
            DropTable("dbo.VendorCreditItemLines");
            DropTable("dbo.VendorCreditExpenseLines");
            DropTable("dbo.VendorCredits");
            DropTable("dbo.VehicleMileages");
            DropTable("dbo.Vehicles");
            DropTable("dbo.UnitOfMeasureSetDefaultUnits");
            DropTable("dbo.UnitOfMeasureSetRelatedUnits");
            DropTable("dbo.TxnDeleteds");
            DropTable("dbo.Transactions");
            DropTable("dbo.ToDoes");
            DropTable("dbo.TimeTrackings");
            DropTable("dbo.StandardTerms");
            DropTable("dbo.SpecialItems");
            DropTable("dbo.SpecialAccounts");
            DropTable("dbo.SalesTaxPaymentCheckLines");
            DropTable("dbo.SalesTaxPaymentChecks");
            DropTable("dbo.SalesReceiptLines");
            DropTable("dbo.SalesReceipts");
            DropTable("dbo.SalesOrderLinkedTxns");
            DropTable("dbo.SalesOrderLines");
            DropTable("dbo.SalesOrders");
            DropTable("dbo.SalesLines");
            DropTable("dbo.Sales");
            DropTable("dbo.ReceivePaymentToDeposits");
            DropTable("dbo.ReceivePaymentLines");
            DropTable("dbo.ReceivePayments");
            DropTable("dbo.PurchaseOrderLinkedTxns");
            DropTable("dbo.PurchaseOrderLines");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.PriceLevelPerItems");
            DropTable("dbo.Preferences");
            DropTable("dbo.PayrollItemNonWages");
            DropTable("dbo.OtherNames");
            DropTable("dbo.ListDeleteds");
            DropTable("dbo.JournalEntryLines");
            DropTable("dbo.JournalEntryDebitLines");
            DropTable("dbo.JournalEntryCreditLines");
            DropTable("dbo.JournalEntries");
            DropTable("dbo.ItemSubtotals");
            DropTable("dbo.ItemServices");
            DropTable("dbo.ItemSalesTaxGroupLines");
            DropTable("dbo.ItemSalesTaxGroups");
            DropTable("dbo.ItemReceiptLinkedTxns");
            DropTable("dbo.ItemReceiptItemLines");
            DropTable("dbo.ItemReceiptExpenseLines");
            DropTable("dbo.ItemReceipts");
            DropTable("dbo.ItemPayments");
            DropTable("dbo.ItemOtherCharges");
            DropTable("dbo.ItemNonInventories");
            DropTable("dbo.ItemInventoryAssemblyLines");
            DropTable("dbo.ItemInventories");
            DropTable("dbo.ItemGroupLines");
            DropTable("dbo.ItemGroups");
            DropTable("dbo.ItemFixedAssets");
            DropTable("dbo.ItemDiscounts");
            DropTable("dbo.ItemAssembliesCanBuilds");
            DropTable("dbo.InvoiceLinkedTxns");
            DropTable("dbo.InvoiceLines");
            DropTable("dbo.Invoices");
            DropTable("dbo.InventoryAdjustmentLines");
            DropTable("dbo.InventoryAdjustments");
            DropTable("dbo.HostSupportedVersions");
            DropTable("dbo.HostMetaDatas");
            DropTable("dbo.Hosts");
            DropTable("dbo.EstimateLinkedTxns");
            DropTable("dbo.EstimateLines");
            DropTable("dbo.Estimates");
            DropTable("dbo.PayrollItemWages");
            DropTable("dbo.EmployeeEarnings");
            DropTable("dbo.Employees");
            DropTable("dbo.DepositLines");
            DropTable("dbo.Deposits");
            DropTable("dbo.DateDrivenTerms");
            DropTable("dbo.CustomFields");
            DropTable("dbo.CreditMemoLinkedTxns");
            DropTable("dbo.CreditMemoLines");
            DropTable("dbo.CustomerMsgs");
            DropTable("dbo.ShipMethods");
            DropTable("dbo.Templates");
            DropTable("dbo.CreditMemoes");
            DropTable("dbo.CreditCardCreditItemLines");
            DropTable("dbo.CreditCardCreditExpenseLines");
            DropTable("dbo.CreditCardCredits");
            DropTable("dbo.CreditCardChargeItemLines");
            DropTable("dbo.CreditCardChargeExpenseLines");
            DropTable("dbo.CreditCardCharges");
            DropTable("dbo.CompanyActivities");
            DropTable("dbo.Companies");
            DropTable("dbo.ClearedStatus");
            DropTable("dbo.CheckItemLines");
            DropTable("dbo.CheckExpenseLines");
            DropTable("dbo.CheckApplyCheckToTxns");
            DropTable("dbo.Checks");
            DropTable("dbo.ChargeLinkedTxns");
            DropTable("dbo.Charges");
            DropTable("dbo.BuildAssemblyComponentItemLines");
            DropTable("dbo.ItemInventoryAssemblies");
            DropTable("dbo.BuildAssemblies");
            DropTable("dbo.BillToPays");
            DropTable("dbo.BillPaymentCreditCardLines");
            DropTable("dbo.BillPaymentCreditCards");
            DropTable("dbo.BillPaymentCheckLines");
            DropTable("dbo.BillPaymentChecks");
            DropTable("dbo.BillingRateLines");
            DropTable("dbo.BillLinkedTxns");
            DropTable("dbo.UnitOfMeasureSets");
            DropTable("dbo.Items");
            DropTable("dbo.BillItemLines");
            DropTable("dbo.Classes");
            DropTable("dbo.BillExpenseLines");
            DropTable("dbo.Bills");
            DropTable("dbo.ARRefundCreditCardRefundAppliedToes");
            DropTable("dbo.PriceLevels");
            DropTable("dbo.JobTypes");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.BillingRates");
            DropTable("dbo.VendorTypes");
            DropTable("dbo.Vendors");
            DropTable("dbo.ItemSalesTaxes");
            DropTable("dbo.SalesReps");
            DropTable("dbo.Terms");
            DropTable("dbo.CustomerTypes");
            DropTable("dbo.Entities");
            DropTable("dbo.Customers");
            DropTable("dbo.ARRefundCreditCards");
            DropTable("dbo.AccountTaxLineInfoes");
            DropTable("dbo.Currencies");
            DropTable("dbo.TaxCodes");
            DropTable("dbo.SalesTaxCodes");
            DropTable("dbo.Accounts");
        }
    }
}
