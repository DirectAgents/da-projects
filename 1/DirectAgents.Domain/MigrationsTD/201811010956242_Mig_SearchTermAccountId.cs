namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_SearchTermAccountId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("td.SearchTerm", "KeywordId", "td.Keyword");
            DropIndex("td.SearchTerm", new[] { "KeywordId" });
            AddColumn("td.SearchTerm", "AccountId", c => c.Int(nullable: false));
            AlterColumn("td.SearchTerm", "KeywordId", c => c.Int());
            CreateIndex("td.SearchTerm", "KeywordId");
            CreateIndex("td.SearchTerm", "AccountId");
            AddForeignKey("td.SearchTerm", "AccountId", "td.Account", "Id", cascadeDelete: true);
            AddForeignKey("td.SearchTerm", "KeywordId", "td.Keyword", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("td.SearchTerm", "KeywordId", "td.Keyword");
            DropForeignKey("td.SearchTerm", "AccountId", "td.Account");
            DropIndex("td.SearchTerm", new[] { "AccountId" });
            DropIndex("td.SearchTerm", new[] { "KeywordId" });
            AlterColumn("td.SearchTerm", "KeywordId", c => c.Int(nullable: false));
            DropColumn("td.SearchTerm", "AccountId");
            CreateIndex("td.SearchTerm", "KeywordId");
            AddForeignKey("td.SearchTerm", "KeywordId", "td.Keyword", "Id", cascadeDelete: true);
        }
    }
}
