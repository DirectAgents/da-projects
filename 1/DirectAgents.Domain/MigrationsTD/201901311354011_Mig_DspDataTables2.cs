namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_DspDataTables2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.DspAdvertiser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspAdvertiserDailyMetricValues",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdvertiserId = c.Int(nullable: false),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickThroughs = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEvents = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        DPV = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ATC = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Purchase = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdvertiserId })
                .ForeignKey("td.DspAdvertiser", t => t.AdvertiserId, cascadeDelete: true)
                .Index(t => t.AdvertiserId);
            
            CreateTable(
                "td.DspCreative",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertiserReportId = c.String(),
                        AdvertiserName = c.String(),
                        OrderReportId = c.String(),
                        OrderName = c.String(),
                        LineItemReportId = c.String(),
                        LineItemName = c.String(),
                        ReportId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspCreativeDailyMetricValues",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CreativeId = c.Int(nullable: false),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickThroughs = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEvents = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        DPV = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ATC = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Purchase = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.CreativeId })
                .ForeignKey("td.DspCreative", t => t.CreativeId, cascadeDelete: true)
                .Index(t => t.CreativeId);
            
            CreateTable(
                "td.DspLineItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertiserReportId = c.String(),
                        AdvertiserName = c.String(),
                        OrderReportId = c.String(),
                        OrderName = c.String(),
                        ReportId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspLineDailyMetricValues",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        LineItemId = c.Int(nullable: false),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickThroughs = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEvents = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        DPV = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ATC = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Purchase = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.LineItemId })
                .ForeignKey("td.DspLineItem", t => t.LineItemId, cascadeDelete: true)
                .Index(t => t.LineItemId);
            
            CreateTable(
                "td.DspOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertiserReportId = c.String(),
                        AdvertiserName = c.String(),
                        ReportId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspOrderMetricValues",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        OrderId = c.Int(nullable: false),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickThroughs = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEvents = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        TotalPixelEventsClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        DPV = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ATC = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Purchase = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseViews = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PurchaseClicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.OrderId })
                .ForeignKey("td.DspOrder", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.DspOrderMetricValues", "OrderId", "td.DspOrder");
            DropForeignKey("td.DspOrder", "AccountId", "td.Account");
            DropForeignKey("td.DspLineDailyMetricValues", "LineItemId", "td.DspLineItem");
            DropForeignKey("td.DspLineItem", "AccountId", "td.Account");
            DropForeignKey("td.DspCreativeDailyMetricValues", "CreativeId", "td.DspCreative");
            DropForeignKey("td.DspCreative", "AccountId", "td.Account");
            DropForeignKey("td.DspAdvertiserDailyMetricValues", "AdvertiserId", "td.DspAdvertiser");
            DropForeignKey("td.DspAdvertiser", "AccountId", "td.Account");
            DropIndex("td.DspOrderMetricValues", new[] { "OrderId" });
            DropIndex("td.DspOrder", new[] { "AccountId" });
            DropIndex("td.DspLineDailyMetricValues", new[] { "LineItemId" });
            DropIndex("td.DspLineItem", new[] { "AccountId" });
            DropIndex("td.DspCreativeDailyMetricValues", new[] { "CreativeId" });
            DropIndex("td.DspCreative", new[] { "AccountId" });
            DropIndex("td.DspAdvertiserDailyMetricValues", new[] { "AdvertiserId" });
            DropIndex("td.DspAdvertiser", new[] { "AccountId" });
            DropTable("td.DspOrderMetricValues");
            DropTable("td.DspOrder");
            DropTable("td.DspLineDailyMetricValues");
            DropTable("td.DspLineItem");
            DropTable("td.DspCreativeDailyMetricValues");
            DropTable("td.DspCreative");
            DropTable("td.DspAdvertiserDailyMetricValues");
            DropTable("td.DspAdvertiser");
        }
    }
}
