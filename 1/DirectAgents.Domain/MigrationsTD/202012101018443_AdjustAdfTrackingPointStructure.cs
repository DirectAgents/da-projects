namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustAdfTrackingPointStructure : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("td.AdfTrackingPoint", "LineItemId", "td.AdfLineItem");
            DropIndex("td.AdfTrackingPoint", new[] { "LineItemId" });
            AddColumn("td.AdfTrackingPointSummary", "LineItemId", c => c.Int(nullable: false));
            CreateIndex("td.AdfTrackingPointSummary", "LineItemId");
            AddForeignKey("td.AdfTrackingPointSummary", "LineItemId", "td.AdfLineItem", "Id", cascadeDelete: true);
            DropColumn("td.AdfTrackingPoint", "LineItemId");
        }
        
        public override void Down()
        {
            AddColumn("td.AdfTrackingPoint", "LineItemId", c => c.Int(nullable: false));
            DropForeignKey("td.AdfTrackingPointSummary", "LineItemId", "td.AdfLineItem");
            DropIndex("td.AdfTrackingPointSummary", new[] { "LineItemId" });
            DropColumn("td.AdfTrackingPointSummary", "LineItemId");
            CreateIndex("td.AdfTrackingPoint", "LineItemId");
            AddForeignKey("td.AdfTrackingPoint", "LineItemId", "td.AdfLineItem", "Id", cascadeDelete: true);
        }
    }
}
