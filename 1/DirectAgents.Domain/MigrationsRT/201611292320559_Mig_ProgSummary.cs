namespace DirectAgents.Domain.MigrationsRT
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_ProgSummary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "rt.ProgExtraItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ProgCampaignId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Description = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Revenue = c.Decimal(nullable: false, precision: 14, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("rt.ProgCampaign", t => t.ProgCampaignId, cascadeDelete: true)
                .ForeignKey("rt.Vendor", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.ProgCampaignId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "rt.ProgSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        ProgCampaignId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.ProgCampaignId, t.VendorId })
                .ForeignKey("rt.ProgCampaign", t => t.ProgCampaignId, cascadeDelete: true)
                .ForeignKey("rt.Vendor", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.ProgCampaignId)
                .Index(t => t.VendorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("rt.ProgSummary", "VendorId", "rt.Vendor");
            DropForeignKey("rt.ProgSummary", "ProgCampaignId", "rt.ProgCampaign");
            DropForeignKey("rt.ProgExtraItem", "VendorId", "rt.Vendor");
            DropForeignKey("rt.ProgExtraItem", "ProgCampaignId", "rt.ProgCampaign");
            DropIndex("rt.ProgSummary", new[] { "VendorId" });
            DropIndex("rt.ProgSummary", new[] { "ProgCampaignId" });
            DropIndex("rt.ProgExtraItem", new[] { "VendorId" });
            DropIndex("rt.ProgExtraItem", new[] { "ProgCampaignId" });
            DropTable("rt.ProgSummary");
            DropTable("rt.ProgExtraItem");
        }
    }
}
