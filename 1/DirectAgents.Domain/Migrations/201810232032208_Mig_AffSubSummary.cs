namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AffSubSummary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "cake.AffSub",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AffiliateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("cake.Affiliate", t => t.AffiliateId, cascadeDelete: true)
                .Index(t => t.AffiliateId);
            
            CreateTable(
                "cake.AffSubSummary",
                c => new
                    {
                        AffSubId = c.Int(nullable: false),
                        OfferId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Views = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        Conversions = c.Decimal(nullable: false, precision: 16, scale: 6),
                        Paid = c.Decimal(nullable: false, precision: 16, scale: 6),
                        Sellable = c.Decimal(nullable: false, precision: 16, scale: 6),
                        Revenue = c.Decimal(nullable: false, precision: 19, scale: 4),
                        Cost = c.Decimal(nullable: false, precision: 19, scale: 4),
                    })
                .PrimaryKey(t => new { t.AffSubId, t.OfferId, t.Date })
                .ForeignKey("cake.AffSub", t => t.AffSubId, cascadeDelete: true)
                .ForeignKey("cake.Offer", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.AffSubId)
                .Index(t => t.OfferId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("cake.AffSubSummary", "OfferId", "cake.Offer");
            DropForeignKey("cake.AffSubSummary", "AffSubId", "cake.AffSub");
            DropForeignKey("cake.AffSub", "AffiliateId", "cake.Affiliate");
            DropIndex("cake.AffSubSummary", new[] { "OfferId" });
            DropIndex("cake.AffSubSummary", new[] { "AffSubId" });
            DropIndex("cake.AffSub", new[] { "AffiliateId" });
            DropTable("cake.AffSubSummary");
            DropTable("cake.AffSub");
        }
    }
}
