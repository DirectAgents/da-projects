namespace Huggies.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HuggiesLeads");
        }
    }
}
