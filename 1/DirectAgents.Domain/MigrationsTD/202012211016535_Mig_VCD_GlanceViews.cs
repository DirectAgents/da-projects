namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VCD_GlanceViews : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.VcdAnalytic", "GlanceViews", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("td.VcdAnalytic", "GlanceViews");
        }
    }
}
