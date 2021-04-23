namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VcdAddMarketBasketAnalysisReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.VMarketBasketAnalysisProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        N1PurchasedAsin = c.String(),
                        N1PurchasedTitle = c.String(),
                        N1Combination = c.Decimal(nullable: false, precision: 18, scale: 2),
                        N2PurchasedAsin = c.String(),
                        N2PurchasedTitle = c.String(),
                        N2Combination = c.Decimal(nullable: false, precision: 18, scale: 2),
                        N3PurchasedAsin = c.String(),
                        N3PurchasedTitle = c.String(),
                        N3Combination = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.VMarketBasketAnalysisProduct", "AccountId", "td.Account");
            DropIndex("td.VMarketBasketAnalysisProduct", new[] { "AccountId" });
            DropTable("td.VMarketBasketAnalysisProduct");
        }
    }
}
