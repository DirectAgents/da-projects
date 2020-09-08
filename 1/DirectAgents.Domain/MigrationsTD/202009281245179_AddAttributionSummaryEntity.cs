namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttributionSummaryEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.AttributionSummary",
                c => new
                    {
                        AccountId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        AdvertiserName = c.String(nullable: false, maxLength: 128),
                        CampaignName = c.String(nullable: false, maxLength: 128),
                        AdGroupName = c.String(nullable: false, maxLength: 128),
                        CreativeName = c.String(nullable: false, maxLength: 128),
                        Clicks = c.Int(nullable: false),
                        DetailedPageViewsClicks = c.Int(nullable: false),
                        AddToCartClicks = c.Int(nullable: false),
                        Purchases = c.Int(nullable: false),
                        UnitsSold = c.Int(nullable: false),
                        Sales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDetailedPageViewsClicks = c.Int(nullable: false),
                        TotalAddToCartClicks = c.Int(nullable: false),
                        TotalPurchases = c.Int(nullable: false),
                        TotalUnitsSold = c.Int(nullable: false),
                        TotalSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.AccountId, t.Date, t.AdvertiserName, t.CampaignName, t.AdGroupName, t.CreativeName })
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.AttributionSummary", "AccountId", "td.Account");
            DropIndex("td.AttributionSummary", new[] { "AccountId" });
            DropTable("td.AttributionSummary");
        }
    }
}
