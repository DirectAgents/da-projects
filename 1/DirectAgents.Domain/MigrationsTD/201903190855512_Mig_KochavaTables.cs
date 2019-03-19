namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_KochavaTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.KochavaItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        AppName = c.String(),
                        Date = c.DateTime(nullable: false),
                        NetworkName = c.String(),
                        EventName = c.String(),
                        CampaignId = c.String(),
                        CreativeId = c.String(),
                        SiteId = c.String(),
                        AdGroupId = c.String(),
                        Keyword = c.String(),
                        DeviceId = c.String(),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.KochavaItem", "AccountId", "td.Account");
            DropIndex("td.KochavaItem", new[] { "AccountId" });
            DropTable("td.KochavaItem");
        }
    }
}
