namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_KeywordAndSearchTermSummaries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.KeywordSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        KeywordId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.KeywordId })
                .ForeignKey("td.Keyword", t => t.KeywordId, cascadeDelete: true)
                .Index(t => t.KeywordId);
            
            CreateTable(
                "td.SearchTermSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        SearchTermId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        AllClicks = c.Int(nullable: false),
                        PostClickConv = c.Int(nullable: false),
                        PostViewConv = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.SearchTermId })
                .ForeignKey("td.SearchTerm", t => t.SearchTermId, cascadeDelete: true)
                .Index(t => t.SearchTermId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.SearchTermSummary", "SearchTermId", "td.SearchTerm");
            DropForeignKey("td.KeywordSummary", "KeywordId", "td.Keyword");
            DropIndex("td.SearchTermSummary", new[] { "SearchTermId" });
            DropIndex("td.KeywordSummary", new[] { "KeywordId" });
            DropTable("td.SearchTermSummary");
            DropTable("td.KeywordSummary");
        }
    }
}
