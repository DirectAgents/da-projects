namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCurrencyTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "CostCurrency", c => c.String());
            AlterColumn("dbo.Campaigns", "RevenueCurrency", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "RevenueCurrency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Campaigns", "CostCurrency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
