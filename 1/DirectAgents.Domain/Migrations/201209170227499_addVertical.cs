namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVertical : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "Vertical", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "Vertical");
        }
    }
}
