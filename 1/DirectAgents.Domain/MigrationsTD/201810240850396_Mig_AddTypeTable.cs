namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AddTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("td.Strategy", "TypeId", c => c.Int());
            CreateIndex("td.Strategy", "TypeId");
            AddForeignKey("td.Strategy", "TypeId", "td.Type", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("td.Strategy", "TypeId", "td.Type");
            DropIndex("td.Strategy", new[] { "TypeId" });
            DropColumn("td.Strategy", "TypeId");
            DropTable("td.Type");
        }
    }
}
