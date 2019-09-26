namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_DbmFloodlightActivityName : DbMigration
    {
        private const string FloodlightActivityNameDefaultValue = "Unknown";

        public override void Up()
        {
            DropPrimaryKey("td.DbmLineItemSummary");
            AddColumn("td.DbmLineItemSummary", "FloodlightActivityName", c => c.String(nullable: false, maxLength: 256, defaultValue: FloodlightActivityNameDefaultValue));
            AddPrimaryKey("td.DbmLineItemSummary", new[] { "LineItemId", "Date", "Country", "FloodlightActivityName" });
        }

        public override void Down()
        {
            DropPrimaryKey("td.DbmLineItemSummary");
            DropColumn("td.DbmLineItemSummary", "FloodlightActivityName");
            AddPrimaryKey("td.DbmLineItemSummary", new[] { "LineItemId", "Date", "Country" });
        }
    }
}