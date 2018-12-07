namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AddCampaignType : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.Strategy", "CampaignType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("td.Strategy", "CampaignType");
        }
    }
}
