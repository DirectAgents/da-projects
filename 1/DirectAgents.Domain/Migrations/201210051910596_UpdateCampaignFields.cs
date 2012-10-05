namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCampaignFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "Restrictions", c => c.String());
            AddColumn("dbo.Campaigns", "Budget", c => c.String());
            AddColumn("dbo.Campaigns", "PassedInfo", c => c.String());
            AlterColumn("dbo.Campaigns", "CampaignCap", c => c.String());
            DropColumn("dbo.Campaigns", "BannedNetworks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "BannedNetworks", c => c.String());
            AlterColumn("dbo.Campaigns", "CampaignCap", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Campaigns", "PassedInfo");
            DropColumn("dbo.Campaigns", "Budget");
            DropColumn("dbo.Campaigns", "Restrictions");
        }
    }
}
