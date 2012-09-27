namespace ApiClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DailySummary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailySummaries",
                c => new
                    {
                        DailySummaryId = c.Int(nullable: false, identity: true),
                        offer_id = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        views = c.Int(nullable: false),
                        clicks = c.Int(nullable: false),
                        click_thru = c.Decimal(nullable: false, precision: 18, scale: 2),
                        conversions = c.Int(nullable: false),
                        paid = c.Int(nullable: false),
                        sellable = c.Int(nullable: false),
                        conversion_rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cpl = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        rpt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        profit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        epc = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DailySummaryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DailySummaries");
        }
    }
}
