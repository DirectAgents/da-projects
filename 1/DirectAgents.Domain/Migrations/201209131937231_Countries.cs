namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Countries : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Campaigns", "Countries", "CountryCodes");

            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryCode = c.String(nullable: false, maxLength: 10),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryCode);
            
            CreateTable(
                "dbo.CampaignCountries",
                c => new
                    {
                        Pid = c.Int(nullable: false),
                        CountryCode = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => new { t.Pid, t.CountryCode })
                .ForeignKey("dbo.Campaigns", t => t.Pid, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.CountryCode, cascadeDelete: true)
                .Index(t => t.Pid)
                .Index(t => t.CountryCode);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CampaignCountries", new[] { "CountryCode" });
            DropIndex("dbo.CampaignCountries", new[] { "Pid" });
            DropForeignKey("dbo.CampaignCountries", "CountryCode", "dbo.Countries");
            DropForeignKey("dbo.CampaignCountries", "Pid", "dbo.Campaigns");
            DropTable("dbo.CampaignCountries");
            DropTable("dbo.Countries");

            RenameColumn("dbo.Campaigns", "CountryCodes", "Countries");
        }
    }
}
