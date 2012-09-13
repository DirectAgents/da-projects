namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "ImageUrl");
        }
    }
}
