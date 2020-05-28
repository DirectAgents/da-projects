namespace DirectAgents.Domain.MigrationsMP
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatebuymatable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.buyma_handbags", name: "SerItemUrl", newName: "ser_item_url");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.buyma_handbags", name: "ser_item_url", newName: "SerItemUrl");
        }
    }
}
