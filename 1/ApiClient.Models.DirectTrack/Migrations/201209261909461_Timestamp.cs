namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Timestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectTrackResources", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DirectTrackResources", "Name", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DirectTrackResources", "Name", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.DirectTrackResources", "Timestamp");
        }
    }
}
