namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmCountryCodes : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Campaigns", "CountryCodes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "CountryCodes", c => c.String());
        }
    }
}
