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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.DbmAdvertiser", t => t.AdvertiserId)
                .Index(t => t.AdvertiserId);
            
            CreateTable(
                "td.DbmAdvertiser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(),
                        Currency = c.String(),
                        ExternalId = c.String(),
                        Name = c.String(),
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
                        Height = c.String(),
                        Width = c.String(),
                        Size = c.String(),
                        Type = c.String(),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.DbmAdvertiser", t => t.AdvertiserId)
                .Index(t => t.AdvertiserId);
            
            CreateTable(
                "td.DbmCreativeSummary",
                c => new
                    {
                        CreativeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostClickConv = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewConv = c.Decimal(nullable: false, precision: 18, scale: 6),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.DbmCampaign", t => t.CampaignId)
                .Index(t => t.CampaignId);
            
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.DbmInsertionOrder", t => t.InsertionOrderId)
                .Index(t => t.InsertionOrderId);
            
            CreateTable(
                "td.DbmLineItemSummary",
                c => new
                    {
                        LineItemId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Impressions = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Clicks = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostClickConv = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewConv = c.Decimal(nullable: false, precision: 18, scale: 6),
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
            DropForeignKey("td.DbmInsertionOrder", "CampaignId", "td.DbmCampaign");
            DropForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative");
            DropForeignKey("td.DbmCreative", "AdvertiserId", "td.DbmAdvertiser");
            DropForeignKey("td.DbmCampaign", "AdvertiserId", "td.DbmAdvertiser");
            DropForeignKey("td.DbmAdvertiser", "AccountId", "td.Account");
            DropIndex("td.DbmLineItemSummary", LineItemIndexName);
            DropIndex("td.DbmLineItem", new[] { "InsertionOrderId" });
            DropIndex("td.DbmInsertionOrder", new[] { "CampaignId" });
            DropIndex("td.DbmCreativeSummary", CreativeIndexName);
            DropIndex("td.DbmCreative", new[] { "AdvertiserId" });
            DropIndex("td.DbmAdvertiser", new[] { "AccountId" });
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
