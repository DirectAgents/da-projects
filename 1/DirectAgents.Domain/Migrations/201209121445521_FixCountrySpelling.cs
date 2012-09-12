namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCountrySpelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "Countries", c => c.String());
            DropColumn("dbo.Campaigns", "Coutries");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "Coutries", c => c.String());
            DropColumn("dbo.Campaigns", "Countries");
        }
    }
}
