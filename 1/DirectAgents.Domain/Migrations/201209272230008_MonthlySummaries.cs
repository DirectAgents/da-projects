namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MonthlySummaries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MonthlySummaries",
                c => new
                    {
                        MonthlySummaryId = c.Int(nullable: false, identity: true),
                        pid = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        clicks = c.Int(nullable: false),
                        conversions = c.Int(nullable: false),
                        paid = c.Int(nullable: false),
                        sellable = c.Int(nullable: false),
                        cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        revenue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MonthlySummaryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MonthlySummaries");
        }
    }
}
