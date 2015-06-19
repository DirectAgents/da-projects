namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AdRoll : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "adr.Advertisable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Eid = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "adr.AdvertisableStat",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdvertisableId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        Conversions = c.Int(nullable: false),
                        Spend = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdvertisableId })
                .ForeignKey("adr.Advertisable", t => t.AdvertisableId, cascadeDelete: true)
                .Index(t => t.AdvertisableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("adr.AdvertisableStat", "AdvertisableId", "adr.Advertisable");
            DropIndex("adr.AdvertisableStat", new[] { "AdvertisableId" });
            DropTable("adr.AdvertisableStat");
            DropTable("adr.Advertisable");
        }
    }
}
