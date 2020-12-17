namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VCD_GlanceViews : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.VcdAnalytic", "GlanceViews", c => c.String());
            AddColumn("td.VProduct", "GlanceViews", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("td.VProduct", "GlanceViews");
            DropColumn("td.VcdAnalytic", "GlanceViews");
        }
    }
}
