namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dtapicall : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DirectTrackApiCalls",
                c => new
                    {
                        Url = c.String(nullable: false, maxLength: 500),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.String(),
                        PointsUsed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Url);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DirectTrackApiCalls");
        }
    }
}
