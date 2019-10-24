namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_FacebookAddReachMetric : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.FbAdSetSummary", "Reach", c => c.Int(nullable: false));
            AddColumn("td.FbAdSummary", "Reach", c => c.Int(nullable: false));
            AddColumn("td.FbCampaignSummary", "Reach", c => c.Int(nullable: false));
            AddColumn("td.FbDailySummary", "Reach", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("td.FbDailySummary", "Reach");
            DropColumn("td.FbCampaignSummary", "Reach");
            DropColumn("td.FbAdSummary", "Reach");
            DropColumn("td.FbAdSetSummary", "Reach");
        }
    }
}
