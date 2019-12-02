namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_DbmAddIdForSummaryTables : DbMigration
    {
        private const string LineItemIndexName = "IX_LineItemIdAndDate";
        private const string CreativeIndexName = "IX_CreativeIdAndDate";

        public override void Up()
        {
            DropForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative");
            DropForeignKey("td.DbmLineItemSummary", "LineItemId", "td.DbmLineItem");
            DropIndex("td.DbmCreativeSummary", CreativeIndexName);
            DropIndex("td.DbmLineItemSummary", LineItemIndexName);
            DropPrimaryKey("td.DbmCreativeSummary");
            DropPrimaryKey("td.DbmLineItemSummary");
            AddColumn("td.DbmCreativeSummary", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("td.DbmLineItemSummary", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("td.DbmCreativeSummary", "CreativeId", c => c.Int(nullable: false));
            AlterColumn("td.DbmCreativeSummary", "Country", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("td.DbmLineItemSummary", "LineItemId", c => c.Int(nullable: false));
            AlterColumn("td.DbmLineItemSummary", "Country", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("td.DbmLineItemSummary", "FloodlightActivityName", c => c.String(nullable: false, maxLength: 256));
            AddPrimaryKey("td.DbmCreativeSummary", "Id");
            AddPrimaryKey("td.DbmLineItemSummary", "Id");
            Sql($@"CREATE NONCLUSTERED INDEX [{LineItemIndexName}] ON [td].[DbmLineItemSummary] ([LineItemId]) INCLUDE ([Date])");
            Sql($@"CREATE NONCLUSTERED INDEX [{CreativeIndexName}] ON [td].[DbmCreativeSummary] ([CreativeId]) INCLUDE ([Date])");
            AddForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative", "Id");
            AddForeignKey("td.DbmLineItemSummary", "LineItemId", "td.DbmLineItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("td.DbmLineItemSummary", "LineItemId", "td.DbmLineItem");
            DropForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative");
            DropIndex("td.DbmLineItemSummary", LineItemIndexName);
            DropIndex("td.DbmCreativeSummary", CreativeIndexName);
            DropPrimaryKey("td.DbmLineItemSummary");
            DropPrimaryKey("td.DbmCreativeSummary");
            AlterColumn("td.DbmLineItemSummary", "FloodlightActivityName", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("td.DbmLineItemSummary", "Country", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("td.DbmLineItemSummary", "LineItemId", c => c.Int(nullable: false));
            AlterColumn("td.DbmCreativeSummary", "Country", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("td.DbmCreativeSummary", "CreativeId", c => c.Int(nullable: false));
            DropColumn("td.DbmLineItemSummary", "Id");
            DropColumn("td.DbmCreativeSummary", "Id");
            AddPrimaryKey("td.DbmLineItemSummary", new[] { "LineItemId", "Date", "Country", "FloodlightActivityName" });
            AddPrimaryKey("td.DbmCreativeSummary", new[] { "CreativeId", "Date", "Country" });
            Sql($@"CREATE NONCLUSTERED INDEX [{LineItemIndexName}] ON [td].[DbmLineItemSummary] ([LineItemId]) INCLUDE ([Date])");
            Sql($@"CREATE NONCLUSTERED INDEX [{CreativeIndexName}] ON [td].[DbmCreativeSummary] ([CreativeId]) INCLUDE ([Date])");
            AddForeignKey("td.DbmLineItemSummary", "LineItemId", "td.DbmLineItem", "Id", cascadeDelete: true);
            AddForeignKey("td.DbmCreativeSummary", "CreativeId", "td.DbmCreative", "Id", cascadeDelete: true);
        }
    }
}
