namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTrackingPointSummaryPK : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("td.AdfTrackingPointSummary");
            AddPrimaryKey("td.AdfTrackingPointSummary", new[] { "Date", "TrackingPointId", "MediaTypeId", "LineItemId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("td.AdfTrackingPointSummary");
            AddPrimaryKey("td.AdfTrackingPointSummary", new[] { "Date", "TrackingPointId", "MediaTypeId" });
        }
    }
}
