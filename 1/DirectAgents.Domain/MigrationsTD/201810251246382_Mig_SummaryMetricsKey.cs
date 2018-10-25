namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_SummaryMetricsKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("td.AdSetSummaryMetric");
            DropPrimaryKey("td.DailySummaryMetric");
            DropPrimaryKey("td.KeywordSummaryMetric");
            DropPrimaryKey("td.SearchTermSummaryMetric");
            DropPrimaryKey("td.StrategySummaryMetric");
            DropPrimaryKey("td.AdSummaryMetric");
            AddPrimaryKey("td.AdSetSummaryMetric", new[] { "Date", "AdSetId", "MetricTypeId" });
            AddPrimaryKey("td.DailySummaryMetric", new[] { "Date", "AccountId", "MetricTypeId" });
            AddPrimaryKey("td.KeywordSummaryMetric", new[] { "Date", "KeywordId", "MetricTypeId" });
            AddPrimaryKey("td.SearchTermSummaryMetric", new[] { "Date", "SearchTermId", "MetricTypeId" });
            AddPrimaryKey("td.StrategySummaryMetric", new[] { "Date", "StrategyId", "MetricTypeId" });
            AddPrimaryKey("td.AdSummaryMetric", new[] { "Date", "TDadId", "MetricTypeId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("td.AdSummaryMetric");
            DropPrimaryKey("td.StrategySummaryMetric");
            DropPrimaryKey("td.SearchTermSummaryMetric");
            DropPrimaryKey("td.KeywordSummaryMetric");
            DropPrimaryKey("td.DailySummaryMetric");
            DropPrimaryKey("td.AdSetSummaryMetric");
            AddPrimaryKey("td.AdSummaryMetric", new[] { "Date", "TDadId" });
            AddPrimaryKey("td.StrategySummaryMetric", new[] { "Date", "StrategyId" });
            AddPrimaryKey("td.SearchTermSummaryMetric", new[] { "Date", "SearchTermId" });
            AddPrimaryKey("td.KeywordSummaryMetric", new[] { "Date", "KeywordId" });
            AddPrimaryKey("td.DailySummaryMetric", new[] { "Date", "AccountId" });
            AddPrimaryKey("td.AdSetSummaryMetric", new[] { "Date", "AdSetId" });
        }
    }
}
