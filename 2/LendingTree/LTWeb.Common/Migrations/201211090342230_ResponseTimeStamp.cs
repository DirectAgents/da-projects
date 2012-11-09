namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResponseTimeStamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leads", "ResponseTimestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leads", "ResponseTimestamp");
        }
    }
}
