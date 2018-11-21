namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_ExtendKeywordAndSearchTermSummaries : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.KeywordSummary", "PostClickRev", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("td.KeywordSummary", "PostViewRev", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            RenameColumn("td.SearchTerm", "Query", "Name");
            AddColumn("td.SearchTermSummary", "PostClickRev", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("td.SearchTermSummary", "PostViewRev", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            RenameColumn("td.SearchTerm", "Name", "Query");
            DropColumn("td.SearchTermSummary", "PostViewRev");
            DropColumn("td.SearchTermSummary", "PostClickRev");
            DropColumn("td.KeywordSummary", "PostViewRev");
            DropColumn("td.KeywordSummary", "PostClickRev");
        }
    }
}
