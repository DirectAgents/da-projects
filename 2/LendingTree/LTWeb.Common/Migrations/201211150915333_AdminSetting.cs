namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Key = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdminSettings");
        }
    }
}
