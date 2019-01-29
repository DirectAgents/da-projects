namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_DspDataTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.DspAdvertiser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspAdvertiserSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdvertiserId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdvertiserId, t.MetricTypeId })
                .ForeignKey("td.DspAdvertiser", t => t.AdvertiserId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.AdvertiserId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.DspCreative",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineItemId = c.Int(),
                        ExternalId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.DspLineItem", t => t.LineItemId)
                .Index(t => t.LineItemId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspLineItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        ExternalId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.DspOrder", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdvertiserId = c.Int(),
                        ExternalId = c.String(),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.DspAdvertiser", t => t.AdvertiserId)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AdvertiserId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.DspCreativeSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CreativeId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.CreativeId, t.MetricTypeId })
                .ForeignKey("td.DspCreative", t => t.CreativeId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.CreativeId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.DspLineItemSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        LineItemId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.LineItemId, t.MetricTypeId })
                .ForeignKey("td.DspLineItem", t => t.LineItemId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.LineItemId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.DspOrderSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        OrderId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.OrderId, t.MetricTypeId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.DspOrder", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.MetricTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.DspOrderSummaryMetric", "OrderId", "td.DspOrder");
            DropForeignKey("td.DspOrderSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.DspLineItemSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.DspLineItemSummaryMetric", "LineItemId", "td.DspLineItem");
            DropForeignKey("td.DspCreativeSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.DspCreativeSummaryMetric", "CreativeId", "td.DspCreative");
            DropForeignKey("td.DspCreative", "LineItemId", "td.DspLineItem");
            DropForeignKey("td.DspLineItem", "OrderId", "td.DspOrder");
            DropForeignKey("td.DspOrder", "AccountId", "td.Account");
            DropForeignKey("td.DspOrder", "AdvertiserId", "td.DspAdvertiser");
            DropForeignKey("td.DspLineItem", "AccountId", "td.Account");
            DropForeignKey("td.DspCreative", "AccountId", "td.Account");
            DropForeignKey("td.DspAdvertiserSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.DspAdvertiserSummaryMetric", "AdvertiserId", "td.DspAdvertiser");
            DropForeignKey("td.DspAdvertiser", "AccountId", "td.Account");
            DropIndex("td.DspOrderSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.DspOrderSummaryMetric", new[] { "OrderId" });
            DropIndex("td.DspLineItemSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.DspLineItemSummaryMetric", new[] { "LineItemId" });
            DropIndex("td.DspCreativeSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.DspCreativeSummaryMetric", new[] { "CreativeId" });
            DropIndex("td.DspOrder", new[] { "AccountId" });
            DropIndex("td.DspOrder", new[] { "AdvertiserId" });
            DropIndex("td.DspLineItem", new[] { "AccountId" });
            DropIndex("td.DspLineItem", new[] { "OrderId" });
            DropIndex("td.DspCreative", new[] { "AccountId" });
            DropIndex("td.DspCreative", new[] { "LineItemId" });
            DropIndex("td.DspAdvertiserSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.DspAdvertiserSummaryMetric", new[] { "AdvertiserId" });
            DropIndex("td.DspAdvertiser", new[] { "AccountId" });
            DropTable("td.DspOrderSummaryMetric");
            DropTable("td.DspLineItemSummaryMetric");
            DropTable("td.DspCreativeSummaryMetric");
            DropTable("td.DspOrder");
            DropTable("td.DspLineItem");
            DropTable("td.DspCreative");
            DropTable("td.DspAdvertiserSummaryMetric");
            DropTable("td.DspAdvertiser");
        }
    }
}
