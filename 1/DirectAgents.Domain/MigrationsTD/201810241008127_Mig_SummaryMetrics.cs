namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_SummaryMetrics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.AdSetSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AdSetId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AdSetId })
                .ForeignKey("td.AdSet", t => t.AdSetId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.AdSetId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.DailySummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.AccountId })
                .ForeignKey("td.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.KeywordSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        KeywordId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.KeywordId })
                .ForeignKey("td.Keyword", t => t.KeywordId, cascadeDelete: true)
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .Index(t => t.KeywordId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.SearchTermSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        SearchTermId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.SearchTermId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.SearchTerm", t => t.SearchTermId, cascadeDelete: true)
                .Index(t => t.SearchTermId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.StrategySummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        StrategyId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.StrategyId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.Strategy", t => t.StrategyId, cascadeDelete: true)
                .Index(t => t.StrategyId)
                .Index(t => t.MetricTypeId);
            
            CreateTable(
                "td.AdSummaryMetric",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        TDadId = c.Int(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => new { t.Date, t.TDadId })
                .ForeignKey("td.MetricType", t => t.MetricTypeId, cascadeDelete: true)
                .ForeignKey("td.Ad", t => t.TDadId, cascadeDelete: true)
                .Index(t => t.TDadId)
                .Index(t => t.MetricTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.AdSummaryMetric", "TDadId", "td.Ad");
            DropForeignKey("td.AdSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.StrategySummaryMetric", "StrategyId", "td.Strategy");
            DropForeignKey("td.StrategySummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.SearchTermSummaryMetric", "SearchTermId", "td.SearchTerm");
            DropForeignKey("td.SearchTermSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.KeywordSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.KeywordSummaryMetric", "KeywordId", "td.Keyword");
            DropForeignKey("td.DailySummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.DailySummaryMetric", "AccountId", "td.Account");
            DropForeignKey("td.AdSetSummaryMetric", "MetricTypeId", "td.MetricType");
            DropForeignKey("td.AdSetSummaryMetric", "AdSetId", "td.AdSet");
            DropIndex("td.AdSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.AdSummaryMetric", new[] { "TDadId" });
            DropIndex("td.StrategySummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.StrategySummaryMetric", new[] { "StrategyId" });
            DropIndex("td.SearchTermSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.SearchTermSummaryMetric", new[] { "SearchTermId" });
            DropIndex("td.KeywordSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.KeywordSummaryMetric", new[] { "KeywordId" });
            DropIndex("td.DailySummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.DailySummaryMetric", new[] { "AccountId" });
            DropIndex("td.AdSetSummaryMetric", new[] { "MetricTypeId" });
            DropIndex("td.AdSetSummaryMetric", new[] { "AdSetId" });
            DropTable("td.AdSummaryMetric");
            DropTable("td.StrategySummaryMetric");
            DropTable("td.SearchTermSummaryMetric");
            DropTable("td.KeywordSummaryMetric");
            DropTable("td.DailySummaryMetric");
            DropTable("td.AdSetSummaryMetric");
        }
    }
}
