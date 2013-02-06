namespace Accounting.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyFiles",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.CompanyId);
            
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
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ListID })
                .ForeignKey("dbo.Entities", t => new { t.Entity_CompanyId, t.Entity_ListID })
                .Index(t => new { t.Entity_CompanyId, t.Entity_ListID });
            
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "Entity_CompanyId", "Entity_ListID" });
            DropForeignKey("dbo.Customers", new[] { "Entity_CompanyId", "Entity_ListID" }, "dbo.Entities");
            DropTable("dbo.Entities");
            DropTable("dbo.Customers");
            DropTable("dbo.CompanyFiles");
        }
    }
}
