namespace ApiClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DailySummaryMultiKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DailySummaries", new[] { "DailySummaryId" });
            AddPrimaryKey("dbo.DailySummaries", new[] { "offer_id", "date" });
            DropColumn("dbo.DailySummaries", "DailySummaryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DailySummaries", "DailySummaryId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.DailySummaries", new[] { "offer_id", "date" });
            AddPrimaryKey("dbo.DailySummaries", "DailySummaryId");
        }
    }
}
