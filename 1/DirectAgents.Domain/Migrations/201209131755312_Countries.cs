namespace DirectAgents.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Countries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryCode = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Countries");
        }
    }
}
