namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VcdAddItemComparisonAndAlternativePurchaseReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.VAlternativePurchaseProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        No1PurchasedAsin = c.String(),
                        No1PurchasedProductTitle = c.String(),
                        No1PurchasedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No2PurchasedAsin = c.String(),
                        No2PurchasedProductTitle = c.String(),
                        No2PurchasedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No3PurchasedAsin = c.String(),
                        No3PurchasedProductTitle = c.String(),
                        No3PurchasedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No4PurchasedAsin = c.String(),
                        No4PurchasedProductTitle = c.String(),
                        No4PurchasedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No5PurchasedAsin = c.String(),
                        No5PurchasedProductTitle = c.String(),
                        No5PurchasedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VItemComparisonProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        No1ComparedAsin = c.String(),
                        No1ComparedProductTitle = c.String(),
                        No1ComparedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No2ComparedAsin = c.String(),
                        No2ComparedProductTitle = c.String(),
                        No2ComparedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No3ComparedAsin = c.String(),
                        No3ComparedProductTitle = c.String(),
                        No3ComparedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No4ComparedAsin = c.String(),
                        No4ComparedProductTitle = c.String(),
                        No4ComparedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        No5ComparedAsin = c.String(),
                        No5ComparedProductTitle = c.String(),
                        No5ComparedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            DropForeignKey("td.VItemComparisonProduct", "AccountId", "td.Account");
            DropForeignKey("td.VAlternativePurchaseProduct", "AccountId", "td.Account");
            DropIndex("td.VItemComparisonProduct", new[] { "AccountId" });
            DropIndex("td.VAlternativePurchaseProduct", new[] { "AccountId" });
            DropTable("td.VItemComparisonProduct");
            DropTable("td.VAlternativePurchaseProduct");
        }
    }
}
