namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartOver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Pid = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        PayableAction = c.String(),
                        TrafficType = c.String(),
                        Link = c.String(),
                        CostCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RevenueCurrency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImportantDetails = c.String(),
                        BannedNetworks = c.String(),
                        CampaignCap = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Countries = c.String(),
                        ScrubPolicy = c.String(),
                        EomNotes = c.String(),
                    })
                .PrimaryKey(t => t.Pid);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PersonId);
            
            CreateTable(
                "dbo.CampaignAccountManagers",
                c => new
                    {
                        Pid = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pid, t.PersonId })
                .ForeignKey("dbo.Campaigns", t => t.Pid, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Pid)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.CampaignAdManagers",
                c => new
                    {
                        Pid = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pid, t.PersonId })
                .ForeignKey("dbo.Campaigns", t => t.Pid, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.Pid)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CampaignAdManagers", new[] { "PersonId" });
            DropIndex("dbo.CampaignAdManagers", new[] { "Pid" });
            DropIndex("dbo.CampaignAccountManagers", new[] { "PersonId" });
            DropIndex("dbo.CampaignAccountManagers", new[] { "Pid" });
            DropForeignKey("dbo.CampaignAdManagers", "PersonId", "dbo.People");
            DropForeignKey("dbo.CampaignAdManagers", "Pid", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignAccountManagers", "PersonId", "dbo.People");
            DropForeignKey("dbo.CampaignAccountManagers", "Pid", "dbo.Campaigns");
            DropTable("dbo.CampaignAdManagers");
            DropTable("dbo.CampaignAccountManagers");
            DropTable("dbo.People");
            DropTable("dbo.Campaigns");
        }
    }
}
