namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SourceOfRequest_LendingTreeAffiliatePartnerCode = c.String(),
                        SourceOfRequest_LendingTreeAffiliateUserName = c.String(),
                        SourceOfRequest_LendingTreeAffiliatePassword = c.String(),
                        SourceOfRequest_LendingTreeAffiliateEsourceID = c.String(),
                        SourceOfRequest_LendingTreeAffiliateBrand = c.String(),
                        SourceOfRequest_LendingTreeAffiliateFormVersion = c.String(),
                        SourceOfRequest_VisitorIPAddress = c.String(),
                        SourceOfRequest_VisitorURL = c.String(),
                        SourceOfRequest_TreeSessionID = c.String(),
                        SourceOfRequest_TreeComputerID = c.String(),
                        SourceOfRequest_V1stCookie = c.String(),
                        SourceOfRequest_LTLOptin = c.Int(nullable: false),
                        SourceOfRequest_AffiliateSiteID = c.String(),
                        PostUrl = c.String(),
                        Name = c.String(),
                        Key = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        RequestContent = c.String(),
                        ResponseContent = c.String(),
                        ResponseTimestamp = c.DateTime(nullable: false),
                        AppId = c.String(),
                        AffiliateId = c.String(),
                        IPAddress = c.String(),
                        IsError = c.Int(nullable: false),
                        Key = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdminSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Key = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeadPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoanType = c.String(),
                        PropertyState = c.String(),
                        CreditRating = c.String(),
                        PropertyType = c.String(),
                        PropertyUse = c.String(),
                        PropertyZip = c.String(),
                        PropertyApproximateValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimatedMortgageBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CashOut = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MonthlyPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankruptcyDischarged = c.String(),
                        ForeclosureDischarged = c.String(),
                        IsVetran = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        ApplicantZipCode = c.String(),
                        Email = c.String(),
                        DOB = c.String(),
                        HomePhone = c.String(),
                        WorkPhone = c.String(),
                        SSN = c.String(),
                        AffiliateSiteID = c.String(),
                        VisitorIPAddress = c.String(),
                        VisitorURL = c.String(),
                        SourceID = c.String(),
                        Username = c.String(),
                        AppID = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LeadPosts");
            DropTable("dbo.AdminSettings");
            DropTable("dbo.Leads");
            DropTable("dbo.ServiceConfigs");
        }
    }
}
