namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeadPosts", "PurchasePrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LeadPosts", "DownPayment", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LeadPosts", "PropertyCity", c => c.String());
            AlterColumn("dbo.LeadPosts", "PropertyApproximateValue", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LeadPosts", "EstimatedMortgageBalance", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LeadPosts", "CashOut", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LeadPosts", "MonthlyPayment", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeadPosts", "MonthlyPayment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LeadPosts", "CashOut", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LeadPosts", "EstimatedMortgageBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LeadPosts", "PropertyApproximateValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.LeadPosts", "PropertyCity");
            DropColumn("dbo.LeadPosts", "DownPayment");
            DropColumn("dbo.LeadPosts", "PurchasePrice");
        }
    }
}
