namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_Keyword : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.Keyword",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccountId = c.Int(nullable: false),
                        StrategyId = c.Int(),
                        AdSetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdSet", t => t.AdSetId)
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("td.Strategy", t => t.StrategyId)
                .Index(t => t.AccountId)
                .Index(t => t.StrategyId)
                .Index(t => t.AdSetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.Keyword", "StrategyId", "td.Strategy");
            DropForeignKey("td.Keyword", "AccountId", "td.Account");
            DropForeignKey("td.Keyword", "AdSetId", "td.AdSet");
            DropIndex("td.Keyword", new[] { "AdSetId" });
            DropIndex("td.Keyword", new[] { "StrategyId" });
            DropIndex("td.Keyword", new[] { "AccountId" });
            DropTable("td.Keyword");
        }
    }
}
