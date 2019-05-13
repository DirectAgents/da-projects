namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;
    
    public partial class FacebookCreativeExpansion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.FbActionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true, name: "CodeIndex");
            
            CreateTable(
                "td.FbAdAction",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdId = c.Int(nullable: false),
                        ActionTypeId = c.Int(nullable: false),
                        PostClick = c.Int(nullable: false),
                        PostView = c.Int(nullable: false),
                        PostClickVal = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewVal = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickAttrWindow = c.String(),
                        ViewAttrWindow = c.String(),
                    })
                .PrimaryKey(t => new { t.Date, t.AdId, t.ActionTypeId })
                .ForeignKey("td.FbActionType", t => t.ActionTypeId, cascadeDelete: true)
                .ForeignKey("td.FbAd", t => t.AdId, cascadeDelete: true)
                .Index(t => t.AdId)
                .Index(t => t.ActionTypeId);
            
            CreateTable(
                "td.FbAd",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        AdSetId = c.Int(),
                        CreativeId = c.Int(),
                        Name = c.String(),
                        ExternalId = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.FbAdSet", t => t.AdSetId)
                .ForeignKey("td.FbCreative", t => t.CreativeId)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AdSetId)
                .Index(t => t.CreativeId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.FbAdSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CampaignId = c.Int(),
                        Name = c.String(),
                        ExternalId = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.FbCampaign", t => t.CampaignId)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.FbCampaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ExternalId = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.FbCreative",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThumbnailUrl = c.String(),
                        ImageUrl = c.String(),
                        Title = c.String(),
                        Body = c.String(),
                        Name = c.String(),
                        ExternalId = c.String(),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.FbAdSetAction",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdSetId = c.Int(nullable: false),
                        ActionTypeId = c.Int(nullable: false),
                        PostClick = c.Int(nullable: false),
                        PostView = c.Int(nullable: false),
                        PostClickVal = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewVal = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickAttrWindow = c.String(),
                        ViewAttrWindow = c.String(),
                    })
                .PrimaryKey(t => new { t.Date, t.AdSetId, t.ActionTypeId })
                .ForeignKey("td.FbActionType", t => t.ActionTypeId, cascadeDelete: true)
                .ForeignKey("td.FbAdSet", t => t.AdSetId, cascadeDelete: true)
                .Index(t => t.AdSetId)
                .Index(t => t.ActionTypeId);
            
            CreateTable(
                "td.FbAdSetSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdSetId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdSetId })
                .ForeignKey("td.FbAdSet", t => t.AdSetId, cascadeDelete: true)
                .Index(t => t.AdSetId);
            
            CreateTable(
                "td.FbAdSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdId })
                .ForeignKey("td.FbAd", t => t.AdId, cascadeDelete: true)
                .Index(t => t.AdId);
            
            CreateTable(
                "td.FbCampaignAction",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        ActionTypeId = c.Int(nullable: false),
                        PostClick = c.Int(nullable: false),
                        PostView = c.Int(nullable: false),
                        PostClickVal = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewVal = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickAttrWindow = c.String(),
                        ViewAttrWindow = c.String(),
                    })
                .PrimaryKey(t => new { t.Date, t.CampaignId, t.ActionTypeId })
                .ForeignKey("td.FbActionType", t => t.ActionTypeId, cascadeDelete: true)
                .ForeignKey("td.FbCampaign", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.ActionTypeId);
            
            CreateTable(
                "td.FbCampaignSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        CampaignId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.CampaignId })
                .ForeignKey("td.FbCampaign", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "td.FbDailySummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        PostClickRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        PostViewRev = c.Decimal(nullable: false, precision: 18, scale: 6),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AccountId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.FbCampaignSummary", "CampaignId", "td.FbCampaign");
            DropForeignKey("td.FbCampaignAction", "CampaignId", "td.FbCampaign");
            DropForeignKey("td.FbCampaignAction", "ActionTypeId", "td.FbActionType");
            DropForeignKey("td.FbAdSummary", "AdId", "td.FbAd");
            DropForeignKey("td.FbAdSetSummary", "AdSetId", "td.FbAdSet");
            DropForeignKey("td.FbAdSetAction", "AdSetId", "td.FbAdSet");
            DropForeignKey("td.FbAdSetAction", "ActionTypeId", "td.FbActionType");
            DropForeignKey("td.FbAdAction", "AdId", "td.FbAd");
            DropForeignKey("td.FbAd", "AccountId", "td.Account");
            DropForeignKey("td.FbAd", "CreativeId", "td.FbCreative");
            DropForeignKey("td.FbCreative", "AccountId", "td.Account");
            DropForeignKey("td.FbAd", "AdSetId", "td.FbAdSet");
            DropForeignKey("td.FbAdSet", "AccountId", "td.Account");
            DropForeignKey("td.FbAdSet", "CampaignId", "td.FbCampaign");
            DropForeignKey("td.FbCampaign", "AccountId", "td.Account");
            DropForeignKey("td.FbAdAction", "ActionTypeId", "td.FbActionType");
            DropIndex("td.FbCampaignSummary", new[] { "CampaignId" });
            DropIndex("td.FbCampaignAction", new[] { "ActionTypeId" });
            DropIndex("td.FbCampaignAction", new[] { "CampaignId" });
            DropIndex("td.FbAdSummary", new[] { "AdId" });
            DropIndex("td.FbAdSetSummary", new[] { "AdSetId" });
            DropIndex("td.FbAdSetAction", new[] { "ActionTypeId" });
            DropIndex("td.FbAdSetAction", new[] { "AdSetId" });
            DropIndex("td.FbCreative", new[] { "AccountId" });
            DropIndex("td.FbCampaign", new[] { "AccountId" });
            DropIndex("td.FbAdSet", new[] { "AccountId" });
            DropIndex("td.FbAdSet", new[] { "CampaignId" });
            DropIndex("td.FbAd", new[] { "AccountId" });
            DropIndex("td.FbAd", new[] { "CreativeId" });
            DropIndex("td.FbAd", new[] { "AdSetId" });
            DropIndex("td.FbAdAction", new[] { "ActionTypeId" });
            DropIndex("td.FbAdAction", new[] { "AdId" });
            DropIndex("td.FbActionType", "CodeIndex");
            DropTable("td.FbDailySummary");
            DropTable("td.FbCampaignSummary");
            DropTable("td.FbCampaignAction");
            DropTable("td.FbAdSummary");
            DropTable("td.FbAdSetSummary");
            DropTable("td.FbAdSetAction");
            DropTable("td.FbCreative");
            DropTable("td.FbCampaign");
            DropTable("td.FbAdSet");
            DropTable("td.FbAd");
            DropTable("td.FbAdAction");
            DropTable("td.FbActionType");
        }
    }
}
