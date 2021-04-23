namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_VcdInvHealthNewMetrics : DbMigration
    {
        public override void Up()
        {
            AddColumn("td.VcdAnalytic", "SellThroughRate", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("td.VcdAnalytic", "OpenPurchaseOrderQuantity", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            DropColumn("td.VcdAnalytic", "OpenPurchaseOrderQuantity");
            DropColumn("td.VcdAnalytic", "SellThroughRate");
        }
    }
}
