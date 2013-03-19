namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeadPosts", "Test", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeadPosts", "Test");
        }
    }
}
