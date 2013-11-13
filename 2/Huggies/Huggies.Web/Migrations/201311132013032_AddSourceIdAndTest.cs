namespace Huggies.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceIdAndTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leads", "SourceId", c => c.Int(nullable: false));
            AddColumn("dbo.Leads", "Test", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leads", "Test");
            DropColumn("dbo.Leads", "SourceId");
        }
    }
}
