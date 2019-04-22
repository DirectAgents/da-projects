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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.YamLine", t => t.LineId, cascadeDelete: true)
                .Index(t => t.LineId);
            
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
            DropForeignKey("td.YamCreativeSummary", "CreativeId", "td.YamCreative");
            DropForeignKey("td.YamCreative", "AccountId", "td.Account");
            DropForeignKey("td.YamAdSummary", "AdId", "td.YamAd");
            DropForeignKey("td.YamAd", "LineId", "td.YamLine");
            DropForeignKey("td.YamLine", "CampaignId", "td.YamCampaign");
            DropForeignKey("td.YamCampaign", "AccountId", "td.Account");
            DropIndex("td.YamPixelSummary", new[] { "PixelId" });
            DropIndex("td.YamPixel", new[] { "AccountId" });
            DropIndex("td.YamCreativeSummary", new[] { "CreativeId" });
            DropIndex("td.YamCreative", new[] { "AccountId" });
            DropIndex("td.YamAdSummary", new[] { "AdId" });
            DropIndex("td.YamCampaign", new[] { "AccountId" });
            DropIndex("td.YamLine", new[] { "CampaignId" });
            DropIndex("td.YamAd", new[] { "LineId" });
            DropTable("td.YamPixelSummary");
            DropTable("td.YamPixel");
            DropTable("td.YamCreativeSummary");
            DropTable("td.YamCreative");
            DropTable("td.YamAdSummary");
            DropTable("td.YamCampaign");
            DropTable("td.YamLine");
            DropTable("td.YamAd");
        }
    }
}
