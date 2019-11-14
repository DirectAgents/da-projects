namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_AdformNewDbTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "td.AdfMediaType",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "td.AdfDailySummary",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AccountId = c.Int(nullable: false),
                    MediaTypeId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Impressions = c.Int(nullable: false),
                    UniqueImpressions = c.Int(nullable: false),
                    Clicks = c.Int(nullable: false),
                    PostClickConv = c.Int(nullable: false),
                    PostViewConv = c.Int(nullable: false),
                    PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                    PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                    Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    ConversionsAll = c.Int(nullable: false),
                    ConversionsConvType1Clicks = c.Int(nullable: false),
                    ConversionsConvType2Clicks = c.Int(nullable: false),
                    ConversionsConvType3Clicks = c.Int(nullable: false),
                    ConversionsConvType1Impressions = c.Int(nullable: false),
                    ConversionsConvType2Impressions = c.Int(nullable: false),
                    ConversionsConvType3Impressions = c.Int(nullable: false),
                    SalesAll = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType1Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType2Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType3Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType1Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType2Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType3Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("td.AdfMediaType", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.MediaTypeId);

            CreateTable(
                    "td.AdfCampaign",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);

            CreateTable(
                "td.AdfCampaignSummary",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CampaignId = c.Int(nullable: false),
                    MediaTypeId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Impressions = c.Int(nullable: false),
                    UniqueImpressions = c.Int(nullable: false),
                    Clicks = c.Int(nullable: false),
                    PostClickConv = c.Int(nullable: false),
                    PostViewConv = c.Int(nullable: false),
                    PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                    PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                    Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    ConversionsAll = c.Int(nullable: false),
                    ConversionsConvType1Clicks = c.Int(nullable: false),
                    ConversionsConvType2Clicks = c.Int(nullable: false),
                    ConversionsConvType3Clicks = c.Int(nullable: false),
                    ConversionsConvType1Impressions = c.Int(nullable: false),
                    ConversionsConvType2Impressions = c.Int(nullable: false),
                    ConversionsConvType3Impressions = c.Int(nullable: false),
                    SalesAll = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType1Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType2Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType3Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType1Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType2Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType3Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdfCampaign", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("td.AdfMediaType", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.MediaTypeId);

            CreateTable(
                    "td.AdfLineItem",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(nullable: false),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdfCampaign", t => t.CampaignId)
                .Index(t => t.CampaignId);

            CreateTable(
                "td.AdfLineItemSummary",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    LineItemId = c.Int(nullable: false),
                    MediaTypeId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Impressions = c.Int(nullable: false),
                    UniqueImpressions = c.Int(nullable: false),
                    Clicks = c.Int(nullable: false),
                    PostClickConv = c.Int(nullable: false),
                    PostViewConv = c.Int(nullable: false),
                    PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                    PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                    Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    ConversionsAll = c.Int(nullable: false),
                    ConversionsConvType1Clicks = c.Int(nullable: false),
                    ConversionsConvType2Clicks = c.Int(nullable: false),
                    ConversionsConvType3Clicks = c.Int(nullable: false),
                    ConversionsConvType1Impressions = c.Int(nullable: false),
                    ConversionsConvType2Impressions = c.Int(nullable: false),
                    ConversionsConvType3Impressions = c.Int(nullable: false),
                    SalesAll = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType1Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType2Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType3Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType1Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType2Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    SalesConvType3Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdfLineItem", t => t.LineItemId, cascadeDelete: true)
                .ForeignKey("td.AdfMediaType", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.LineItemId)
                .Index(t => t.MediaTypeId);

            CreateTable(
                "td.AdfBanner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineItemId = c.Int(nullable: false),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdfLineItem", t => t.LineItemId)
                .Index(t => t.LineItemId);

            CreateTable(
                "td.AdfBannerSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerId = c.Int(nullable: false),
                        MediaTypeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Impressions = c.Int(nullable: false),
                        UniqueImpressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ConversionsAll = c.Int(nullable: false),
                        ConversionsConvType1Clicks = c.Int(nullable: false),
                        ConversionsConvType2Clicks = c.Int(nullable: false),
                        ConversionsConvType3Clicks = c.Int(nullable: false),
                        ConversionsConvType1Impressions = c.Int(nullable: false),
                        ConversionsConvType2Impressions = c.Int(nullable: false),
                        ConversionsConvType3Impressions = c.Int(nullable: false),
                        SalesAll = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SalesConvType1Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SalesConvType2Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SalesConvType3Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SalesConvType1Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SalesConvType2Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        SalesConvType3Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdfBanner", t => t.BannerId, cascadeDelete: true)
                .ForeignKey("td.AdfMediaType", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.BannerId)
                .Index(t => t.MediaTypeId);
        }

        public override void Down()
        {
            DropForeignKey("td.AdfBannerSummary", "MediaTypeId", "td.AdfMediaType");
            DropForeignKey("td.AdfBannerSummary", "BannerId", "td.AdfBanner");
            DropForeignKey("td.AdfBanner", "LineItemId", "td.AdfLineItem");
            DropForeignKey("td.AdfLineItemSummary", "MediaTypeId", "td.AdfMediaType");
            DropForeignKey("td.AdfLineItemSummary", "LineItemId", "td.AdfLineItem");
            DropForeignKey("td.AdfLineItem", "CampaignId", "td.AdfCampaign");
            DropForeignKey("td.AdfCampaignSummary", "MediaTypeId", "td.AdfMediaType");
            DropForeignKey("td.AdfCampaignSummary", "CampaignId", "td.AdfCampaign");
            DropForeignKey("td.AdfCampaign", "AccountId", "td.Account");
            DropForeignKey("td.AdfDailySummary", "MediaTypeId", "td.AdfMediaType");
            DropForeignKey("td.AdfDailySummary", "AccountId", "td.Account");
            DropIndex("td.AdfBannerSummary", new[] { "MediaTypeId" });
            DropIndex("td.AdfBannerSummary", new[] { "BannerId" });
            DropIndex("td.AdfBanner", new[] { "LineItemId" });
            DropIndex("td.AdfLineItemSummary", new[] { "MediaTypeId" });
            DropIndex("td.AdfLineItemSummary", new[] { "LineItemId" });
            DropIndex("td.AdfLineItem", new[] { "CampaignId" });
            DropIndex("td.AdfCampaignSummary", new[] { "MediaTypeId" });
            DropIndex("td.AdfCampaignSummary", new[] { "CampaignId" });
            DropIndex("td.AdfCampaign", new[] { "AccountId" });
            DropIndex("td.AdfDailySummary", new[] { "MediaTypeId" });
            DropIndex("td.AdfDailySummary", new[] { "AccountId" });
            DropTable("td.AdfBannerSummary");
            DropTable("td.AdfBanner");
            DropTable("td.AdfLineItemSummary");
            DropTable("td.AdfLineItem");
            DropTable("td.AdfCampaignSummary");
            DropTable("td.AdfCampaign");
            DropTable("td.AdfDailySummary");
            DropTable("td.AdfMediaType");
        }
    }
}
