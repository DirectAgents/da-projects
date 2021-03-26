namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_FixQuarterlyTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "td.VRepeatPurchaseBehaviorQuaterlyProduct", newName: "VRepeatPurchaseBehaviorQuarterlyProduct");
        }
        
        public override void Down()
        {
            RenameTable(name: "td.VRepeatPurchaseBehaviorQuarterlyProduct", newName: "VRepeatPurchaseBehaviorQuaterlyProduct");
        }
    }
}
