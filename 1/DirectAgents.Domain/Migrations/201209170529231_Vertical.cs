namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vertical : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Verticals",
                c => new
                    {
                        VerticalId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.VerticalId);
            
            AddColumn("dbo.Campaigns", "Vertical_VerticalId", c => c.Int());
            AddForeignKey("dbo.Campaigns", "Vertical_VerticalId", "dbo.Verticals", "VerticalId");
            CreateIndex("dbo.Campaigns", "Vertical_VerticalId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Campaigns", new[] { "Vertical_VerticalId" });
            DropForeignKey("dbo.Campaigns", "Vertical_VerticalId", "dbo.Verticals");
            DropColumn("dbo.Campaigns", "Vertical_VerticalId");
            DropTable("dbo.Verticals");
        }
    }
}
