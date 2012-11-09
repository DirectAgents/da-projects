namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Leads : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        RequestContent = c.String(),
                        ResponseContent = c.String(),
                        Key = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leads");
        }
    }
}
