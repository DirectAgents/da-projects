namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmTrafficTypeNvarchar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campaigns", "TrafficType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "TrafficType", c => c.String());
        }
    }
}
