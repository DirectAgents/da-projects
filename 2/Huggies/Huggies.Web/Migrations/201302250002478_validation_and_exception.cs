namespace Huggies.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation_and_exception : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Zip = c.String(),
                        Ethnicity = c.String(),
                        FirstChild = c.Boolean(nullable: false),
                        Language = c.String(),
                        Gender = c.String(),
                        DueDate = c.DateTime(),
                        AffiliateId = c.Int(nullable: false),
                        IpAddress = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Success = c.Boolean(nullable: false),
                        ValidationErrors = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.HuggiesLeads");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HuggiesLeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Zip = c.String(),
                        Ethnicity = c.String(),
                        FirstChild = c.Boolean(nullable: false),
                        Language = c.String(),
                        Gender = c.String(),
                        DueDate = c.DateTime(),
                        AffiliateId = c.Int(nullable: false),
                        IpAddress = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Success = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Leads");
        }
    }
}
