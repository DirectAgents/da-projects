namespace DirectAgents.Domain.MigrationsRT
{
    using System;
    using System.Data.Entity.Migrations;
    using DirectAgents.Domain.Contexts;
    
    public partial class Mig_ProgTables : DbMigration
    {
        private const string indexCode = "IX_UQ_Code";
        private const string tableClientProg = RevTrackContext.adSchema + "." + RevTrackContext.tblClientProg;
        private const string tableVendorProg = RevTrackContext.adSchema + "." + RevTrackContext.tblVendorProg;
        private const string columnCode = "Code";

        public override void Up()
        {
            CreateTable(
                "ad.ClientProg",
                c => new
                    {
                        ClientId = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        MediaSpend = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MgmtFeePct = c.Decimal(nullable: false, precision: 10, scale: 5),
                        MarginPct = c.Decimal(nullable: false, precision: 10, scale: 5),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("ad.Client", t => t.ClientId)
                .Index(t => t.ClientId);
            Sql(string.Format(@"
                CREATE UNIQUE NONCLUSTERED INDEX {0}
                ON {1}({2})
                WHERE {2} IS NOT NULL;",
                indexCode, tableClientProg, columnCode));

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
                "ad.VendorProg",
                c => new
                    {
                        VendorId = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.VendorId)
                .ForeignKey("ad.Vendor", t => t.VendorId)
                .Index(t => t.VendorId);
            Sql(string.Format(@"
                CREATE UNIQUE NONCLUSTERED INDEX {0}
                ON {1}({2})
                WHERE {2} IS NOT NULL;",
                indexCode, tableVendorProg, columnCode));
        }
        
        public override void Down()
        {
            DropForeignKey("ad.VendorProg", "VendorId", "ad.Vendor");
            DropForeignKey("ad.ProgVendorBudgetInfo", "ClientId", "ad.ClientProg");
            DropForeignKey("ad.ProgVendorBudgetInfo", "VendorId", "ad.Vendor");
            DropForeignKey("ad.ProgVendorBudgetInfo", "ClientId", "ad.Client");
            DropForeignKey("ad.ClientProg", "ClientId", "ad.Client");
            DropForeignKey("ad.ProgBudgetInfo", "ClientId", "ad.ClientProg");
            DropForeignKey("ad.ProgBudgetInfo", "ClientId", "ad.Client");
            DropIndex(tableVendorProg, indexCode);
            DropIndex(tableClientProg, indexCode);
            DropIndex("ad.VendorProg", new[] { "VendorId" });
            DropIndex("ad.ProgVendorBudgetInfo", new[] { "VendorId" });
            DropIndex("ad.ProgVendorBudgetInfo", new[] { "ClientId" });
            DropIndex("ad.ProgBudgetInfo", new[] { "ClientId" });
            DropIndex("ad.ClientProg", new[] { "ClientId" });
            DropTable("ad.VendorProg");
            DropTable("ad.ProgVendorBudgetInfo");
            DropTable("ad.ProgBudgetInfo");
            DropTable("ad.ClientProg");
        }
    }
}
