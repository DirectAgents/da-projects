namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.buyma_handbags",
                c => new
                {
                    index = c.Long(nullable: false, identity: true),
                    level_0 = c.Long(nullable: false),
                    brand = c.String(),
                    buyma_id = c.String(),
                    buyma_image_url = c.String(),
                    image_url = c.String(),
                    old_title = c.String(),
                    p_related_serp = c.String(),
                    SerItemUrl = c.String(),
                    serp_url = c.String(),
                    sr_sub_title = c.String(),
                    sr_title = c.String(),
                    uid = c.Double(nullable: false),
                    category = c.String(),
                })
                .PrimaryKey(t => t.index);

        }
        
        public override void Down()
        {
            DropTable("dbo.buyma_handbags");
        }
    }
}
