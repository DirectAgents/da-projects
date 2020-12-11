namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig_AdfTrackingPoint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.AdfTrackingPoint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineItemId = c.Int(nullable: false),
                        ExternalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("td.AdfLineItem", t => t.LineItemId, cascadeDelete: true)
                .Index(t => t.LineItemId);
            
            CreateTable(
                "td.AdfTrackingPointSummary",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        TrackingPointId = c.Int(nullable: false),
                        MediaTypeId = c.Int(nullable: false),
                        Impressions = c.Int(nullable: false),
                        Clicks = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickConversionsConvTypeAll = c.Int(nullable: false),
                        ClickConversionsConvType1 = c.Int(nullable: false),
                        ClickConversionsConvType2 = c.Int(nullable: false),
                        ClickConversionsConvType3 = c.Int(nullable: false),
                        ImpressionConversionsConvTypeAll = c.Int(nullable: false),
                        ImpressionConversionsConvType1 = c.Int(nullable: false),
                        ImpressionConversionsConvType2 = c.Int(nullable: false),
                        ImpressionConversionsConvType3 = c.Int(nullable: false),
                        ClickSalesConvTypeAll = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickSalesConvType1 = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickSalesConvType2 = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ClickSalesConvType3 = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ImpressionSalesConvTypeAll = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ImpressionSalesConvType1 = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ImpressionSalesConvType2 = c.Decimal(nullable: false, precision: 18, scale: 6),
                        ImpressionSalesConvType3 = c.Decimal(nullable: false, precision: 18, scale: 6),
                        UniqueImpressions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.TrackingPointId, t.MediaTypeId })
                .ForeignKey("td.AdfMediaType", t => t.MediaTypeId, cascadeDelete: true)
                .ForeignKey("td.AdfTrackingPoint", t => t.TrackingPointId, cascadeDelete: true)
                .Index(t => t.TrackingPointId)
                .Index(t => t.MediaTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("td.AdfTrackingPointSummary", "TrackingPointId", "td.AdfTrackingPoint");
            DropForeignKey("td.AdfTrackingPointSummary", "MediaTypeId", "td.AdfMediaType");
            DropForeignKey("td.AdfTrackingPoint", "LineItemId", "td.AdfLineItem");
            DropIndex("td.AdfTrackingPointSummary", new[] { "MediaTypeId" });
            DropIndex("td.AdfTrackingPointSummary", new[] { "TrackingPointId" });
            DropIndex("td.AdfTrackingPoint", new[] { "LineItemId" });
            DropTable("td.AdfTrackingPointSummary");
            DropTable("td.AdfTrackingPoint");
        }
    }
}
