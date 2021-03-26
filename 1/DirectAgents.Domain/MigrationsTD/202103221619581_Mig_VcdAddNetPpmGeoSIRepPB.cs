namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VcdAddNetPpmGeoSIRepPB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.VGeographicSalesInsightsProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        Region = c.String(),
                        State = c.String(),
                        City = c.String(),
                        Zip = c.String(),
                        Author = c.String(),
                        Isbn13 = c.String(),
                        ShippedRevenuePriorPeriodPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedRevenuePriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedRevenuePercentOfTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedCogsPriorPeriodPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedCogsPriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedCogsPercentOfTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedUnitsPriorPeriodPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedUnitsPriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippedUnitsPercentOfTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AverageShippedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AverageShippedPricePriorPeriodPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AverageShippedPricePriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VNetPpmMonthlyProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        Subcategory = c.String(),
                        NetPpm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetPpmPriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VNetPpmWeeklyProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        Subcategory = c.String(),
                        NetPpm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetPpmPriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VNetPpmYearlyProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        Subcategory = c.String(),
                        NetPpm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetPpmPriorYearPercentChange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VRepeatPurchaseBehaviorMonthlyProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        UniqueCustomers = c.Int(nullable: false),
                        RepeatPurchaseRevenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RepeatPurchaseRevenuePriorPeriod = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "td.VRepeatPurchaseBehaviorQuarterlyProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asin = c.String(),
                        UniqueCustomers = c.Int(nullable: false),
                        RepeatPurchaseRevenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RepeatPurchaseRevenuePriorPeriod = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.VRepeatPurchaseBehaviorQuarterlyProduct", "AccountId", "td.Account");
            DropForeignKey("td.VRepeatPurchaseBehaviorMonthlyProduct", "AccountId", "td.Account");
            DropForeignKey("td.VNetPpmYearlyProduct", "AccountId", "td.Account");
            DropForeignKey("td.VNetPpmWeeklyProduct", "AccountId", "td.Account");
            DropForeignKey("td.VNetPpmMonthlyProduct", "AccountId", "td.Account");
            DropForeignKey("td.VGeographicSalesInsightsProduct", "AccountId", "td.Account");
            DropIndex("td.VRepeatPurchaseBehaviorQuarterlyProduct", new[] { "AccountId" });
            DropIndex("td.VRepeatPurchaseBehaviorMonthlyProduct", new[] { "AccountId" });
            DropIndex("td.VNetPpmYearlyProduct", new[] { "AccountId" });
            DropIndex("td.VNetPpmWeeklyProduct", new[] { "AccountId" });
            DropIndex("td.VNetPpmMonthlyProduct", new[] { "AccountId" });
            DropIndex("td.VGeographicSalesInsightsProduct", new[] { "AccountId" });
            DropTable("td.VRepeatPurchaseBehaviorQuarterlyProduct");
            DropTable("td.VRepeatPurchaseBehaviorMonthlyProduct");
            DropTable("td.VNetPpmYearlyProduct");
            DropTable("td.VNetPpmWeeklyProduct");
            DropTable("td.VNetPpmMonthlyProduct");
            DropTable("td.VGeographicSalesInsightsProduct");
        }
    }
}
