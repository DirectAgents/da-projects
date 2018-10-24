namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_SearchTerm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.SearchTerm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Query = c.String(),
                        KeywordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.Keyword", t => t.KeywordId, cascadeDelete: true)
                .Index(t => t.KeywordId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.SearchTerm", "KeywordId", "td.Keyword");
            DropIndex("td.SearchTerm", new[] { "KeywordId" });
            DropTable("td.SearchTerm");
        }
    }
}
