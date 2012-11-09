namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceConfig : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceConfigs");
        }
    }
}
