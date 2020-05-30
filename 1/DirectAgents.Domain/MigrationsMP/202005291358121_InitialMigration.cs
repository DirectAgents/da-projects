namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.buyma_source",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    level_0 = c.Long(nullable: false),
                    brand = c.String(),
                    buyma_id = c.String(),
                    buyma_image_url = c.String(),
                    image_url = c.String(),
                    index = c.Long(nullable: false),
                    old_title = c.String(),
                    p_related_serp = c.String(),
                    ser_item_url = c.String(),
                    serp_url = c.String(),
                    sr_sub_title = c.String(),
                    sr_title = c.String(),
                    uid = c.Double(nullable: false),
                    category = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.buyma_matching_result",
                c => new
                    {
                        BuymaId = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(),
                        ProductDescription = c.String(),
                        MatchedDate = c.DateTime(nullable: false),
                        SearchResultId = c.Int(),
                    })
                .PrimaryKey(t => t.BuymaId)
                .ForeignKey("dbo.buyma_source", t => t.SearchResultId)
                .Index(t => t.SearchResultId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.buyma_matching_result", "SearchResultId", "dbo.buyma_source");
            DropIndex("dbo.buyma_matching_result", new[] { "SearchResultId" });
            DropTable("dbo.buyma_matching_result");
            DropTable("dbo.buyma_source");
        }
    }
}
