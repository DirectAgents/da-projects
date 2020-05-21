namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatchTypeSearchTerm : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.SearchTerm", "MediaType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("td.SearchTerm", "MediaType");
        }
    }
}
