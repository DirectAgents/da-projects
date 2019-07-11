namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_DbmCountry : DbMigration
    {
        private const string CountryDefaultValue = "Unknown";

        public override void Up()
        {
            DropPrimaryKey("td.DbmCreativeSummary");
            DropPrimaryKey("td.DbmLineItemSummary");
            AddColumn("td.DbmCreativeSummary", "Country", c => c.String(nullable: false, maxLength: 128, defaultValue: CountryDefaultValue));
            AddColumn("td.DbmLineItemSummary", "Country", c => c.String(nullable: false, maxLength: 128, defaultValue: CountryDefaultValue));
            AddPrimaryKey("td.DbmCreativeSummary", new[] { "CreativeId", "Date", "Country" });
            AddPrimaryKey("td.DbmLineItemSummary", new[] { "LineItemId", "Date", "Country" });
        }

        public override void Down()
        {
            DropPrimaryKey("td.DbmLineItemSummary");
            DropPrimaryKey("td.DbmCreativeSummary");
            DropColumn("td.DbmLineItemSummary", "Country");
            DropColumn("td.DbmCreativeSummary", "Country");
            AddPrimaryKey("td.DbmLineItemSummary", new[] { "LineItemId", "Date" });
            AddPrimaryKey("td.DbmCreativeSummary", new[] { "CreativeId", "Date" });
        }
    }
}
