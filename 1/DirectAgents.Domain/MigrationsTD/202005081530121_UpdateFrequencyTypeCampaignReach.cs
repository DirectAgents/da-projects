namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFrequencyTypeCampaignReach : DbMigration
    {
        public override void Up()
        {
            AlterColumn("td.FbCampaignReachMetric", "Frequency", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("td.FbCampaignReachMetric", "Frequency", c => c.Int(nullable: false));
        }
    }
}
