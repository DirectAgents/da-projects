namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_VcdAddRepOosMetrics : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.VcdAnalytic", "RepOos", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "RepOosPercentOfTotal", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "RepOosPriorPeriodPercentChange", c => c.Decimal(precision: 18, scale: 6));
        }

        public override void Down()
        {
            DropColumn("td.VcdAnalytic", "RepOosPriorPeriodPercentChange");
            DropColumn("td.VcdAnalytic", "RepOosPercentOfTotal");
            DropColumn("td.VcdAnalytic", "RepOos");
        }
    }
}
