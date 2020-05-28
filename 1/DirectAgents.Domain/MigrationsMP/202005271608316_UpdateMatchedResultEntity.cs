namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMatchedResultEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchingResult", "SearchResult", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MatchingResult", "SearchResult");
        }
    }
}
