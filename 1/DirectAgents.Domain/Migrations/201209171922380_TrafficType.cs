namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrafficType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrafficTypes",
                c => new
                    {
                        TrafficTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TrafficTypeId);
            
            CreateTable(
                "dbo.CampaignTrafficTypes",
                c => new
                    {
                        Pid = c.Int(nullable: false),
                        TrafficTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pid, t.TrafficTypeId })
                .ForeignKey("dbo.Campaigns", t => t.Pid, cascadeDelete: false)
                .ForeignKey("dbo.TrafficTypes", t => t.TrafficTypeId, cascadeDelete: false)
                .Index(t => t.Pid)
                .Index(t => t.TrafficTypeId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CampaignTrafficTypes", new[] { "TrafficTypeId" });
            DropIndex("dbo.CampaignTrafficTypes", new[] { "Pid" });
            DropForeignKey("dbo.CampaignTrafficTypes", "TrafficTypeId", "dbo.TrafficTypes");
            DropForeignKey("dbo.CampaignTrafficTypes", "Pid", "dbo.Campaigns");
            DropTable("dbo.CampaignTrafficTypes");
            DropTable("dbo.TrafficTypes");
        }
    }
}
