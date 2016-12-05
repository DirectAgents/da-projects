namespace DirectAgents.Domain.MigrationsRT
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_ProgBudgetInfos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "rt.ProgBudgetInfo",
                c => new
                    {
                        ProgCampaignId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => new { t.ProgCampaignId, t.Date })
                .ForeignKey("rt.ProgCampaign", t => t.ProgCampaignId, cascadeDelete: true)
                .Index(t => t.ProgCampaignId);
            
            CreateTable(
                "rt.ProgVendorBudgetInfo",
                c => new
                    {
                        ProgCampaignId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => new { t.ProgCampaignId, t.VendorId, t.Date })
                .ForeignKey("rt.ProgCampaign", t => t.ProgCampaignId, cascadeDelete: true)
                .ForeignKey("rt.Vendor", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.ProgCampaignId)
                .Index(t => t.VendorId);
            
            AddColumn("rt.ProgCampaign", "MediaSpend", c => c.Decimal(nullable: false, precision: 14, scale: 2));
            AddColumn("rt.ProgCampaign", "MgmtFeePct", c => c.Decimal(nullable: false, precision: 10, scale: 5));
            AddColumn("rt.ProgCampaign", "MarginPct", c => c.Decimal(nullable: false, precision: 10, scale: 5));
        }
        
        public override void Down()
        {
            DropForeignKey("rt.ProgVendorBudgetInfo", "VendorId", "rt.Vendor");
            DropForeignKey("rt.ProgVendorBudgetInfo", "ProgCampaignId", "rt.ProgCampaign");
            DropForeignKey("rt.ProgBudgetInfo", "ProgCampaignId", "rt.ProgCampaign");
            DropIndex("rt.ProgVendorBudgetInfo", new[] { "VendorId" });
            DropIndex("rt.ProgVendorBudgetInfo", new[] { "ProgCampaignId" });
            DropIndex("rt.ProgBudgetInfo", new[] { "ProgCampaignId" });
            DropColumn("rt.ProgCampaign", "MarginPct");
            DropColumn("rt.ProgCampaign", "MgmtFeePct");
            DropColumn("rt.ProgCampaign", "MediaSpend");
            DropTable("rt.ProgVendorBudgetInfo");
            DropTable("rt.ProgBudgetInfo");
        }
    }
}
