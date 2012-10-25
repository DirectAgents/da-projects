namespace ApiClient.Models.DirectTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clickThurisDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CumulativeCampaignStats", "clickthru", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CumulativeCampaignStats", "clickthru", c => c.Int(nullable: false));
        }
    }
}
