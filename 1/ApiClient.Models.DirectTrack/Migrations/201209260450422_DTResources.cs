namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DTResources : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DirectTrackResources",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DirectTrackResources");
        }
    }
}
