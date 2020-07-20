namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateReachCampaignEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.FbCampaignReachMetric",
                c => new
                    {
                        CampaignId = c.Int(nullable: false),
                        Period = c.String(nullable: false, maxLength: 128),
                        Reach = c.Int(nullable: false),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CampaignId, t.Period })
                .ForeignKey("td.FbCampaign", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
        }
        
        public override void Down()
        {
            DropForeignKey("td.FbCampaignReachMetric", "CampaignId", "td.FbCampaign");
            DropIndex("td.FbCampaignReachMetric", new[] { "CampaignId" });
            DropTable("td.FbCampaignReachMetric");
        }
    }
}
