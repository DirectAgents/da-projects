namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewProductEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.buyma_product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Brand = c.String(),
                        BuymaId = c.String(),
                        BuymaImageUrl = c.String(),
                        ImageUrl = c.String(),
                        Index = c.Int(nullable: false),
                        OldTitle = c.String(),
                        RelatedSert = c.String(),
                        SerItemUrl = c.String(),
                        SerpUrl = c.String(),
                        SrSubTitle = c.String(),
                        SrTitle = c.String(),
                        UId = c.Int(nullable: false),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.buyma_product");
        }
    }
}
