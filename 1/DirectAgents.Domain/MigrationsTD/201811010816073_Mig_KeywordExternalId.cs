namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_KeywordExternalId : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.Keyword", "ExternalId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("td.Keyword", "ExternalId");
        }
    }
}
