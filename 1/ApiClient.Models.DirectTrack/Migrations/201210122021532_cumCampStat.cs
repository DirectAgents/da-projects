namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cumCampStat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CumulativeCampaignStats",
                c => new
                    {
                        Url = c.String(nullable: false, maxLength: 500),
                        impressions = c.Int(nullable: false),
                        contextualImpressions = c.Int(nullable: false),
                        clicks = c.Int(nullable: false),
                        clickthru = c.Int(nullable: false),
                        leads = c.Int(nullable: false),
                        signups = c.Double(nullable: false),
                        numSales = c.Int(nullable: false),
                        saleAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        numSubSales = c.Int(nullable: false),
                        subSaleAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        theyGet = c.Decimal(nullable: false, precision: 18, scale: 2),
                        weGet = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EPC = c.Double(nullable: false),
                        revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        currency = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.Url)
                .ForeignKey("dbo.DirectTrackApiCalls", t => t.Url)
                .Index(t => t.Url);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CumulativeCampaignStats", new[] { "Url" });
            DropForeignKey("dbo.CumulativeCampaignStats", "Url", "dbo.DirectTrackApiCalls");
            DropTable("dbo.CumulativeCampaignStats");
        }
    }
}
