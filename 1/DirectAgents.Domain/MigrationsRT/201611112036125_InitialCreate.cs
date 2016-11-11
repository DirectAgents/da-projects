namespace DirectAgents.Domain.MigrationsRT
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ad.ClientProg",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("ad.Client", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "ad.ProgBudgetInfo",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => new { t.ClientId, t.Date })
                .ForeignKey("ad.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("ad.ClientProg", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "ad.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true, name: "CodeIndex");
            
            CreateTable(
                "ad.ProgVendorBudgetInfo",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => new { t.ClientId, t.VendorId, t.Date })
                .ForeignKey("ad.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("ad.Vendor", t => t.VendorId, cascadeDelete: true)
                .ForeignKey("ad.ClientProg", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "ad.Vendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true, name: "CodeIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("ad.ProgVendorBudgetInfo", "ClientId", "ad.ClientProg");
            DropForeignKey("ad.ProgVendorBudgetInfo", "VendorId", "ad.Vendor");
            DropForeignKey("ad.ProgVendorBudgetInfo", "ClientId", "ad.Client");
            DropForeignKey("ad.ClientProg", "ClientId", "ad.Client");
            DropForeignKey("ad.ProgBudgetInfo", "ClientId", "ad.ClientProg");
            DropForeignKey("ad.ProgBudgetInfo", "ClientId", "ad.Client");
            DropIndex("ad.Vendor", "CodeIndex");
            DropIndex("ad.ProgVendorBudgetInfo", new[] { "VendorId" });
            DropIndex("ad.ProgVendorBudgetInfo", new[] { "ClientId" });
            DropIndex("ad.Client", "CodeIndex");
            DropIndex("ad.ProgBudgetInfo", new[] { "ClientId" });
            DropIndex("ad.ClientProg", new[] { "ClientId" });
            DropTable("ad.Vendor");
            DropTable("ad.ProgVendorBudgetInfo");
            DropTable("ad.Client");
            DropTable("ad.ProgBudgetInfo");
            DropTable("ad.ClientProg");
        }
    }
}
