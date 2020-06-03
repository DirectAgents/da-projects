namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendPrimaryKeyRokuSummary : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("td.RokuSummary");
            AddPrimaryKey("td.RokuSummary", new[] { "Id", "ExtractingDate" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("td.RokuSummary");
            AddPrimaryKey("td.RokuSummary", "Id");
        }
    }
}
