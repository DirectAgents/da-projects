namespace DirectAgents.Domain.MigrationsTD
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRokuSummaryEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "td.RokuSummary",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrderName = c.String(),
                        Types = c.String(),
                        NumberOfLineItems = c.Int(nullable: false),
                        FlightDates = c.String(),
                        Stats = c.String(),
                        Spend = c.String(),
                        Budget = c.String(),
                        OrderDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("td.RokuSummary");
        }
    }
}
