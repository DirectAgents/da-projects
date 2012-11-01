namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addessID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectTrackResources", "AccessId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectTrackResources", "AccessId");
        }
    }
}
