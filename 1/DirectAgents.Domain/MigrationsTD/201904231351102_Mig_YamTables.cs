namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_YamTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.YamAd",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Name = c.String(),
                        LineId = c.Int(nullable: false),
                        CreativeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.YamCreative", t => t.CreativeId)
                .ForeignKey("td.YamLine", t => t.LineId)
                .Index(t => t.LineId)
                .Index(t => t.CreativeId);
            
            CreateTable(
                "td.YamCreative",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.YamLine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Name = c.String(),
                        CampaignId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.YamCampaign", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "td.YamCampaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.YamAdSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        ClickThroughConversion = c.Int(nullable: false),
                        ViewThroughConversion = c.Int(nullable: false),
                        ConversionValue = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserSpending = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdId })
                .ForeignKey("td.YamAd", t => t.AdId, cascadeDelete: true)
                .Index(t => t.AdId);
            
            CreateTable(
                "td.YamCampaignSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        ClickThroughConversion = c.Int(nullable: false),
                        ViewThroughConversion = c.Int(nullable: false),
                        ConversionValue = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserSpending = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.CampaignId })
                .ForeignKey("td.YamCampaign", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "td.YamCreativeSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CreativeId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        ClickThroughConversion = c.Int(nullable: false),
                        ViewThroughConversion = c.Int(nullable: false),
                        ConversionValue = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserSpending = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.CreativeId })
                .ForeignKey("td.YamCreative", t => t.CreativeId, cascadeDelete: true)
                .Index(t => t.CreativeId);
            
            CreateTable(
                "td.YamDailySummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        ClickThroughConversion = c.Int(nullable: false),
                        ViewThroughConversion = c.Int(nullable: false),
                        ConversionValue = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserSpending = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AccountId })
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.YamLineSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        LineId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        ClickThroughConversion = c.Int(nullable: false),
                        ViewThroughConversion = c.Int(nullable: false),
                        ConversionValue = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserSpending = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.LineId })
                .ForeignKey("td.YamLine", t => t.LineId, cascadeDelete: true)
                .Index(t => t.LineId);
            
            CreateTable(
                "td.YamPixel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.YamPixelSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        PixelId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        ClickThroughConversion = c.Int(nullable: false),
                        ViewThroughConversion = c.Int(nullable: false),
                        ConversionValue = c.Decimal(nullable: false, precision: 18, scale: 6),
                        AdvertiserSpending = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.PixelId })
                .ForeignKey("td.YamPixel", t => t.PixelId, cascadeDelete: true)
                .Index(t => t.PixelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.YamPixelSummary", "PixelId", "td.YamPixel");
            DropForeignKey("td.YamPixel", "AccountId", "td.Account");
            DropForeignKey("td.YamLineSummary", "LineId", "td.YamLine");
            DropForeignKey("td.YamDailySummary", "AccountId", "td.Account");
            DropForeignKey("td.YamCreativeSummary", "CreativeId", "td.YamCreative");
            DropForeignKey("td.YamCampaignSummary", "CampaignId", "td.YamCampaign");
            DropForeignKey("td.YamAdSummary", "AdId", "td.YamAd");
            DropForeignKey("td.YamLine", "CampaignId", "td.YamCampaign");
            DropForeignKey("td.YamCampaign", "AccountId", "td.Account");
            DropForeignKey("td.YamAd", "LineId", "td.YamLine");
            DropForeignKey("td.YamAd", "CreativeId", "td.YamCreative");
            DropForeignKey("td.YamCreative", "AccountId", "td.Account");
            DropIndex("td.YamPixelSummary", new[] { "PixelId" });
            DropIndex("td.YamPixel", new[] { "AccountId" });
            DropIndex("td.YamLineSummary", new[] { "LineId" });
            DropIndex("td.YamDailySummary", new[] { "AccountId" });
            DropIndex("td.YamCreativeSummary", new[] { "CreativeId" });
            DropIndex("td.YamCampaignSummary", new[] { "CampaignId" });
            DropIndex("td.YamAdSummary", new[] { "AdId" });
            DropIndex("td.YamCampaign", new[] { "AccountId" });
            DropIndex("td.YamLine", new[] { "CampaignId" });
            DropIndex("td.YamCreative", new[] { "AccountId" });
            DropIndex("td.YamAd", new[] { "CreativeId" });
            DropIndex("td.YamAd", new[] { "LineId" });
            DropTable("td.YamPixelSummary");
            DropTable("td.YamPixel");
            DropTable("td.YamLineSummary");
            DropTable("td.YamDailySummary");
            DropTable("td.YamCreativeSummary");
            DropTable("td.YamCampaignSummary");
            DropTable("td.YamAdSummary");
            DropTable("td.YamCampaign");
            DropTable("td.YamLine");
            DropTable("td.YamCreative");
            DropTable("td.YamAd");
        }
    }
}
