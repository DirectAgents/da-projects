namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MobileLP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "MobileLP", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "MobileLP");
        }
    }
}
