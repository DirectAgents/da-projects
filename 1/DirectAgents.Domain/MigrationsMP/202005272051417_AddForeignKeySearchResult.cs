namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeySearchResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchingResult", "SearchResultId", c => c.Int());
            CreateIndex("dbo.MatchingResult", "SearchResultId");
            AddForeignKey("dbo.MatchingResult", "SearchResultId", "dbo.buyma_product", "Id");
            DropColumn("dbo.MatchingResult", "SearchResult");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MatchingResult", "SearchResult", c => c.Long(nullable: false));
            DropForeignKey("dbo.MatchingResult", "SearchResultId", "dbo.buyma_product");
            DropIndex("dbo.MatchingResult", new[] { "SearchResultId" });
            DropColumn("dbo.MatchingResult", "SearchResultId");
        }
    }
}
