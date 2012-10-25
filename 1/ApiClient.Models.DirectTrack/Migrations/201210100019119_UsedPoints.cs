namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsedPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectTrackResources", "PointsUsed", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectTrackResources", "PointsUsed");
        }
    }
}
