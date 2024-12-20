namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;
    
    public partial class Mig_DbmCreativeExpansion : DbMigration
    {
        private const string LineItemIndexName = "IX_LineItemIdAndDate";
        private const string CreativeIndexName = "IX_CreativeIdAndDate";

        public override void Up()
        {
            CreateTable(
                "td.DbmCampaign",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AdvertiserId = c.Int(),
                    ExternalId = c.String(),
                    Name = c.String(),
                    AccountId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.DbmAdvertiser", t => t.AdvertiserId)
                .Index(t => t.AdvertiserId)
                .Index(t => t.AccountId);

            CreateTable(
                "td.DbmAdvertiser",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CurrencyCode = c.String(),
                    ExternalId = c.String(),
                    Name = c.String(),
                    AccountId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);

            CreateTable(
                "td.DbmCreative",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AdvertiserId = c.Int(),
                    Height = c.Int(),
                    Width = c.Int(),
                    Size = c.String(),
                    Type = c.String(),
                    ExternalId = c.String(),
                    Name = c.String(),
                    AccountId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.DbmAdvertiser", t => t.AdvertiserId)
                .Index(t => t.AdvertiserId)
                .Index(t => t.AccountId);

            CreateTable(
                "td.DbmCreativeSummary",
                c => new
                {
                    CreativeId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Revenue = c.Decimal(nullable: false, precision: 18, scale: 6),
                    Impressions = c.Int(nullable: false),
                    Clicks = c.Int(nullable: false),
                    PostClickConversions = c.Int(nullable: false),
                    PostViewConversions = c.Int(nullable: false),
                    CMPostClickRevenue = c.Decimal(nullable: false, precision: 18, scale: 6),
                    CMPostViewRevenue = c.Decimal(nullable: false, precision: 18, scale: 6),
                })
                .PrimaryKey(t => new { t.CreativeId, t.Date })
                .ForeignKey("td.DbmCreative", t => t.CreativeId, cascadeDelete: true);

            CreateTable(
                "td.DbmInsertionOrder",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CampaignId = c.Int(),
                    ExternalId = c.String(),
                    Name = c.String(),
                    AccountId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.DbmCampaign", t => t.CampaignId)
                .Index(t => t.CampaignId)
                .Index(t => t.AccountId);

            CreateTable(
                "td.DbmLineItem",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InsertionOrderId = c.Int(),
                    Type = c.String(),
                    Status = c.String(),
                    ExternalId = c.String(),
                    Name = c.String(),
                    AccountId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .ForeignKey("td.DbmInsertionOrder", t => t.InsertionOrderId)
                .Index(t => t.InsertionOrderId)
                .Index(t => t.AccountId);

            CreateTable(
                "td.DbmLineItemSummary",
                c => new
                {
                    LineItemId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Revenue = c.Decimal(nullable: false, precision: 18, scale: 6),
                    Impressions = c.Int(nullable: false),
                    Clicks = c.Int(nullable: false),
                    PostClickConversions = c.Int(nullable: false),
                    PostViewConversions = c.Int(nullable: false),
                    CMPostClickRevenue = c.Decimal(nullable: false, precision: 18, scale: 6),
                    CMPostViewRevenue = c.Decimal(nullable: false, precision: 18, scale: 6),
                })
                .PrimaryKey(t => new { t.LineItemId, t.Date })
                .ForeignKey("td.DbmLineItem", t => t.LineItemId, cascadeDelete: true);

            Sql($@"CREATE NONCLUSTERED INDEX [{LineItemIndexName}] ON [td].[DbmLineItemSummary] ([LineItemId]) INCLUDE ([Date])");
            Sql($@"CREATE NONCLUSTERED INDEX [{CreativeIndexName}] ON [td].[DbmCreativeSummary] ([CreativeId]) INCLUDE ([Date])");

        }

        public override void Down()
        {
            DropForeignKey("td.DbmLineItemSummary", "LineItemId", "td.DbmLineItem");
            DropForeignKey("td.DbmLineItem", "InsertionOrderId", "td.DbmInsertionOrder");
            DropForeignKey("td.DbmLineItem", "AccountId", "td.Account");
            DropForeignKey("td.DbmInsertionOrder", "CampaignId", "td.DbmCampaign");
            DropForeignKey("td.DbmInsertionOrder", "AccountId", "td.Account");
            DropForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative");
            DropForeignKey("td.DbmCreative", "AdvertiserId", "td.DbmAdvertiser");
            DropForeignKey("td.DbmCreative", "AccountId", "td.Account");
            DropForeignKey("td.DbmCampaign", "AdvertiserId", "td.DbmAdvertiser");
            DropForeignKey("td.DbmAdvertiser", "AccountId", "td.Account");
            DropForeignKey("td.DbmCampaign", "AccountId", "td.Account");
            DropIndex("td.DbmLineItemSummary", LineItemIndexName);
            DropIndex("td.DbmLineItem", new[] { "AccountId" });
            DropIndex("td.DbmLineItem", new[] { "InsertionOrderId" });
            DropIndex("td.DbmInsertionOrder", new[] { "AccountId" });
            DropIndex("td.DbmInsertionOrder", new[] { "CampaignId" });
            DropIndex("td.DbmCreativeSummary", CreativeIndexName);
            DropIndex("td.DbmCreative", new[] { "AccountId" });
            DropIndex("td.DbmCreative", new[] { "AdvertiserId" });
            DropIndex("td.DbmAdvertiser", new[] { "AccountId" });
            DropIndex("td.DbmCampaign", new[] { "AccountId" });
            DropIndex("td.DbmCampaign", new[] { "AdvertiserId" });
            DropTable("td.DbmLineItemSummary");
            DropTable("td.DbmLineItem");
            DropTable("td.DbmInsertionOrder");
            DropTable("td.DbmCreativeSummary");
            DropTable("td.DbmCreative");
            DropTable("td.DbmAdvertiser");
            DropTable("td.DbmCampaign");
        }
    }
}
