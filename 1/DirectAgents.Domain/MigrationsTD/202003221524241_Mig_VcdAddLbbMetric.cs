namespace DirectAgents.Domain.MigrationsTD
{
    using System.Data.Entity.Migrations;

    public partial class Mig_VcdAddLbbMetric : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.VcdAnalytic", "LBB", c => c.Decimal(precision: 18, scale: 6));
        }

        public override void Down()
        {
            DropColumn("td.VcdAnalytic", "LBB");
        }
    }
}
