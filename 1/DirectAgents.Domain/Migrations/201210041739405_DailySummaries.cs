namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DailySummaries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailySummaries",
                c => new
                    {
                        Pid = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Clicks = c.Int(nullable: false),
                        Conversions = c.Int(nullable: false),
                        Paid = c.Int(nullable: false),
                        Sellable = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.Pid, t.Date })
                .ForeignKey("dbo.Campaigns", t => t.Pid, cascadeDelete: true)
                .Index(t => t.Pid);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DailySummaries", new[] { "Pid" });
            DropForeignKey("dbo.DailySummaries", "Pid", "dbo.Campaigns");
            DropTable("dbo.DailySummaries");
        }
    }
}
