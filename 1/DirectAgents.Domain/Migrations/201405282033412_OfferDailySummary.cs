namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferDailySummary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "cake.OfferDailySummary",
                c => new
                    {
                        OfferId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Views = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        Conversions = c.Int(nullable: false),
                        Paid = c.Int(nullable: false),
                        Sellable = c.Int(nullable: false),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OfferId, t.Date });
            
        }
        
        public override void Down()
        {
            DropTable("cake.OfferDailySummary");
        }
    }
}
