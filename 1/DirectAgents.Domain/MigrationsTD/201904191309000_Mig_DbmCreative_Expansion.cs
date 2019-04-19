namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;
    
    public partial class Mig_DbmCreative_Expansion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.DbmCampaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(),
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
                        LineItemId = c.Int(),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Size = c.String(),
                        Type = c.String(),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.DbmLineItem", t => t.LineItemId)
                .Index(t => t.LineItemId);
            
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
                "td.DbmCreativeSummary",
                c => new
                    {
                        CreativeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.CreativeId, t.Date })
                .ForeignKey("td.DbmCreative", t => t.CreativeId, cascadeDelete: true)
                .Index(t => new { t.CreativeId, t.Date }, unique: true, name: "IX_CreativeIdAndDate");
            
            CreateTable(
                "td.DbmLineItemSummary",
                c => new
                    {
                        LineItemId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.LineItemId, t.Date })
                .ForeignKey("td.DbmLineItem", t => t.LineItemId, cascadeDelete: true)
                .Index(t => new { t.LineItemId, t.Date }, unique: true, name: "IX_LineItemIdAndDate");
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.DbmLineItemSummary", "LineItemId", "td.DbmLineItem");
            DropForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative");
            DropForeignKey("td.DbmCreative", "LineItemId", "td.DbmLineItem");
            DropForeignKey("td.DbmLineItem", "InsertionOrderId", "td.DbmInsertionOrder");
            DropForeignKey("td.DbmInsertionOrder", "CampaignId", "td.DbmCampaign");
            DropForeignKey("td.DbmCampaign", "AccountId", "td.Account");
            DropIndex("td.DbmLineItemSummary", "IX_LineItemIdAndDate");
            DropIndex("td.DbmCreativeSummary", "IX_CreativeIdAndDate");
            DropIndex("td.DbmInsertionOrder", new[] { "CampaignId" });
            DropIndex("td.DbmLineItem", new[] { "InsertionOrderId" });
            DropIndex("td.DbmCreative", new[] { "LineItemId" });
            DropIndex("td.DbmCampaign", new[] { "AccountId" });
            DropTable("td.DbmLineItemSummary");
            DropTable("td.DbmCreativeSummary");
            DropTable("td.DbmInsertionOrder");
            DropTable("td.DbmLineItem");
            DropTable("td.DbmCreative");
            DropTable("td.DbmCampaign");
        }
    }
}
