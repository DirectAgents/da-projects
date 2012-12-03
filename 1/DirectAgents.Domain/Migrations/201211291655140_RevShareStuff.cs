namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevShareStuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "DefaultPriceFormat", c => c.String());
            AddColumn("dbo.Campaigns", "RevenueIsPercentage", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "RevenueIsPercentage");
            DropColumn("dbo.Campaigns", "DefaultPriceFormat");
        }
    }
}
