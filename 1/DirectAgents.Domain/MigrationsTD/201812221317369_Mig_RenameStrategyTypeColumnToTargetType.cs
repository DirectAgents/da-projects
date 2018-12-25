namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_RenameStrategyTypeColumnToTargetType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("td.Strategy", "TypeId", "td.Type");
            RenameColumn(table: "td.Strategy", name: "TypeId", newName: "TargetingTypeId");
            RenameIndex(table: "td.Strategy", name: "IX_TypeId", newName: "IX_TargetingTypeId");
            AddForeignKey("td.Strategy", "TargetingTypeId", "td.Type");
        }
        
        public override void Down()
        {
            DropForeignKey("td.Strategy", "TargetingTypeId", "td.Type");
            RenameIndex(table: "td.Strategy", name: "IX_TargetingTypeId", newName: "IX_TypeId");
            RenameColumn(table: "td.Strategy", name: "TargetingTypeId", newName: "TypeId");
            AddForeignKey("td.Strategy", "TypeId", "td.Type");
        }
    }
}
