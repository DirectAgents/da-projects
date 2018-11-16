namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AdExternalId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.AdExternalId",
                c => new
                    {
                        AdId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        ExternalId = c.String(),
                    })
                .PrimaryKey(t => new { t.AdId, t.TypeId })
                .ForeignKey("td.Ad", t => t.AdId, cascadeDelete: true)
                .ForeignKey("td.Type", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.AdId)
                .Index(t => t.TypeId);
            
            AddColumn("td.Ad", "AdSetId", c => c.Int());
            CreateIndex("td.Ad", "AdSetId");
            AddForeignKey("td.Ad", "AdSetId", "td.AdSet", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("td.AdExternalId", "TypeId", "td.Type");
            DropForeignKey("td.AdExternalId", "AdId", "td.Ad");
            DropForeignKey("td.Ad", "AdSetId", "td.AdSet");
            DropIndex("td.AdExternalId", new[] { "TypeId" });
            DropIndex("td.AdExternalId", new[] { "AdId" });
            DropIndex("td.Ad", new[] { "AdSetId" });
            DropColumn("td.Ad", "AdSetId");
            DropTable("td.AdExternalId");
        }
    }
}
