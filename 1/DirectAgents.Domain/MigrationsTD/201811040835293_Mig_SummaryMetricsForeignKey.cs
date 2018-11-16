namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_SummaryMetricsForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("td.AdSetSummaryMetric", new[] { "Date", "AdSetId" });
            CreateIndex("td.DailySummaryMetric", new[] { "Date", "AccountId" });
            CreateIndex("td.KeywordSummaryMetric", new[] { "Date", "KeywordId" });
            CreateIndex("td.SearchTermSummaryMetric", new[] { "Date", "SearchTermId" });
            CreateIndex("td.StrategySummaryMetric", new[] { "Date", "StrategyId" });
            CreateIndex("td.AdSummaryMetric", new[] { "Date", "TDadId" });
            AddForeignKey("td.AdSetSummaryMetric", new[] { "Date", "AdSetId" }, "td.AdSetSummary", new[] { "Date", "AdSetId" });
            AddForeignKey("td.DailySummaryMetric", new[] { "Date", "AccountId" }, "td.DailySummary", new[] { "Date", "AccountId" });
            AddForeignKey("td.KeywordSummaryMetric", new[] { "Date", "KeywordId" }, "td.KeywordSummary", new[] { "Date", "KeywordId" });
            AddForeignKey("td.SearchTermSummaryMetric", new[] { "Date", "SearchTermId" }, "td.SearchTermSummary", new[] { "Date", "SearchTermId" });
            AddForeignKey("td.StrategySummaryMetric", new[] { "Date", "StrategyId" }, "td.StrategySummary", new[] { "Date", "StrategyId" });
            AddForeignKey("td.AdSummaryMetric", new[] { "Date", "TDadId" }, "td.AdSummary", new[] { "Date", "TDadId" });
        }
        
        public override void Down()
        {
            DropForeignKey("td.AdSummaryMetric", new[] { "Date", "TDadId" }, "td.AdSummary");
            DropForeignKey("td.StrategySummaryMetric", new[] { "Date", "StrategyId" }, "td.StrategySummary");
            DropForeignKey("td.SearchTermSummaryMetric", new[] { "Date", "SearchTermId" }, "td.SearchTermSummary");
            DropForeignKey("td.KeywordSummaryMetric", new[] { "Date", "KeywordId" }, "td.KeywordSummary");
            DropForeignKey("td.DailySummaryMetric", new[] { "Date", "AccountId" }, "td.DailySummary");
            DropForeignKey("td.AdSetSummaryMetric", new[] { "Date", "AdSetId" }, "td.AdSetSummary");
            DropIndex("td.AdSummaryMetric", new[] { "Date", "TDadId" });
            DropIndex("td.StrategySummaryMetric", new[] { "Date", "StrategyId" });
            DropIndex("td.SearchTermSummaryMetric", new[] { "Date", "SearchTermId" });
            DropIndex("td.KeywordSummaryMetric", new[] { "Date", "KeywordId" });
            DropIndex("td.DailySummaryMetric", new[] { "Date", "AccountId" });
            DropIndex("td.AdSetSummaryMetric", new[] { "Date", "AdSetId" });
        }
    }
}
