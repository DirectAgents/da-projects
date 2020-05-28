namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatchedResultEntity : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.buyma_handbags");
            CreateTable(
                "dbo.MatchingResult",
                c => new
                    {
                        BuymaId = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(),
                        ProductDescription = c.String(),
                        MatchedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BuymaId);

            AlterColumn("dbo.buyma_handbags", "index", c => c.Long(nullable: false));
            AlterColumn("dbo.buyma_handbags", "buyma_id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.buyma_handbags", "buyma_id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.buyma_handbags");
            AlterColumn("dbo.buyma_handbags", "buyma_id", c => c.String());
            AlterColumn("dbo.buyma_handbags", "index", c => c.Long(nullable: false, identity: true));
            DropTable("dbo.MatchingResult");
            AddPrimaryKey("dbo.buyma_handbags", "index");
        }
    }
}
