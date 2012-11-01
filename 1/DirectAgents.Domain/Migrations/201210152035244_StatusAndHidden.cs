namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusAndHidden : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Status",
                c => new
                {
                    StatusId = c.Int(nullable: false),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.StatusId);

            AddColumn("dbo.Campaigns", "StatusId", c => c.Int());
            AddForeignKey("dbo.Campaigns", "StatusId", "dbo.Status", "StatusId");
            CreateIndex("dbo.Campaigns", "StatusId");
            AddColumn("dbo.Campaigns", "Hidden", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Hidden");
            DropIndex("dbo.Campaigns", new[] { "StatusId" });
            DropForeignKey("dbo.Campaigns", "StatusId", "dbo.Status");
            DropColumn("dbo.Campaigns", "StatusId");
            DropTable("dbo.Status");
        }
    }
}
